using System;
using System.Collections.Generic;
using System.Linq;
using Amplitude.Framework;
using Amplitude.Mercury;
using Amplitude.Mercury.Interop;
using Amplitude.Mercury.UI;
using AnN3x.HumankindLib;
using AnN3x.ModdingLib;
using UnityEngine;
using Army = Amplitude.Mercury.Interop.AI.Entities.Army;
using Empire = Amplitude.Mercury.Interop.AI.Entities.Empire;

namespace AnN3x.EndlessMovingArmies.MovingArmies;

public class StandardEndlessMoving
{
    public static List<int> EmpireIndicesLeft { get; private set; } = new List<int>();
    public static int TakeUpTo { get; private set; }
    public static IEnumerable<Empire> Empires { get; private set; } = new List<Empire>();
    public static int PrimarySkippedCycles { get; private set; } = 0;
    public static bool IsPrimaryBeingProcessed { get; private set; } = false;
    public static int[] ControlledByHuman { get; private set; }
    public static int IdleLoopsLeft { get; private set; } = 0;
    public static bool CycleEnded { get; private set; } = true;
    public static bool AreMandatoriesActive { get; private set; } = true;
    public static int CycleModIndex { get; private set; } = 0;
    public static bool WasUILockedByEndTurn { get; private set; } = false;

    private static void Reset()
    {
        Empires = HumankindGame.GetAllEmpireEntities();
        ControlledByHuman = HumankindGame.MajorEmpires
            .Where(e => e.IsControlledByHuman)
            .Select(h => h.EmpireIndex)
            .ToArray();
        CycleModIndex = (CycleModIndex + 1) % 4;

        if (Services.GetService<IUIService>() is UIManager uiManager)
            AreMandatoriesActive = uiManager.ActivateMandatories;

        if (!Config.EndlessMoving.OnAllEmpires)
        {
            EmpireIndicesLeft = Config.EndlessMoving.IncludeOtherEmpiresControlledByHuman
                ? ControlledByHuman.ToList()
                : new List<int>() { HumankindGame.LocalEmpireIndex };
            TakeUpTo = 1;
        }
        else
        {
            if (PrimarySkippedCycles >= Config.EndlessMoving.PrimaryCyclesToSkip)
            {
                IsPrimaryBeingProcessed = true;
                PrimarySkippedCycles = 0;
            }
            else
            {
                IsPrimaryBeingProcessed = false;
                PrimarySkippedCycles++;
            }

            EmpireIndicesLeft = Empires.Select((e, i) => i).ToList();
            TakeUpTo = (int) Mathf.Ceil((float) EmpireIndicesLeft.Count /
                                        (float) Config.EndlessMoving.LoopIterationsPerCollectionOfEmpires);
        }

        EmpireIndicesLeft.Shuffle();
        CycleEnded = false;
    }
    
    public static void Run(bool isLockedByEndTurn)
    {
        if (IsValidRun(isLockedByEndTurn))
            DoRun();
    }
    
    private static bool IsValidRun(bool isLockedByEndTurn)
    {
        if (isLockedByEndTurn)
        {
            WasUILockedByEndTurn = true;
            return false;
        }

        if (WasUILockedByEndTurn)
        {
            WasUILockedByEndTurn = false;
            IdleLoopsLeft = 0;
            CycleEnded = true;
            EmpireIndicesLeft.Clear();
        }
        
        if (IdleLoopsLeft-- > 0)
            return false;

        if (!CycleEnded && EmpireIndicesLeft.Count == 0)
        {
            CycleEnded = true;
            IdleLoopsLeft = Config.EndlessMoving.IdleLoopsBetweenCycles;
            return false;
        }

        if (CycleEnded && EmpireIndicesLeft.Count == 0)
            Reset();

        return true;
    }

    private static void DoRun()
    {
        var someEmpires = EmpireIndicesLeft.Take(TakeUpTo);
        EmpireIndicesLeft = EmpireIndicesLeft.Skip(TakeUpTo).ToList();

        foreach (var empireIndex in someEmpires)
        {
            if (!IsPrimaryBeingProcessed &&
                empireIndex >= Amplitude.Mercury.Sandbox.Sandbox.NumberOfMajorEmpires)
                continue;

            try
            {
                if (Empires.ElementAt(empireIndex).Armies is { Length: > 0 } armies)
                    if (ControlledByHuman.Contains(empireIndex))
                        KeepHumanControlledArmiesMoving(armies, empireIndex);
                    else
                        KeepAIControlledArmiesMoving(armies, empireIndex);
            }
            catch (Exception e)
            {
                Loggr.Log(e);
            }
        }
    }

    private static void KeepHumanControlledArmiesMoving(IEnumerable<Army> armies, int empireIndex)
    {
        var processFurtherActions = false;
        var autoExploreArmyIndex = 0;
        
        foreach (var army in armies)
        {
            if (!army.IsIdle()) continue;

            var simulationEntity = army.GetSimulationEntity();
            var isAwake = (simulationEntity?.GetAwakeState() ?? AwakeState.Sleep) == AwakeState.Awake;
            var isRunning = army.IsRunning();
            var movementRatio = army.GetMovementRatio();
            var movementPointsLeft = (int) (movementRatio * army.MovementSpeed);

            if (isAwake)
            {
                if (army.AutoExplore)
                {
                    if (movementRatio <= 0.5f && autoExploreArmyIndex % 4 == CycleModIndex)
                        army.RefillMovementPoints(simulationEntity);

                    autoExploreArmyIndex++;
                }
                else
                {
                    // NO AutoExplore
                    if (army.IsRunningWaitingForFinishTurn())
                    {
                        // An active ArmyGoToAction waiting for next turn,
                        // dealt with the FurtherForEmpire call below
                        army.RefillMovementPoints(simulationEntity);
                        processFurtherActions = true;
                    }
                    else if (movementPointsLeft == 0 && isRunning)
                    {
                        // An active ArmyGoToAction and no more movement points left, used to avoid armies
                        // not moving between refilling movement points from WaitingForFinishTurn above.
                        army.RefillMovementPoints(simulationEntity);
                    }
                    else if (!isRunning && AreMandatoriesActive && movementRatio < .99f)
                    {
                        army.SkipOneTurn();
                    }
                }
            }
        }
        
        if (processFurtherActions)
            Amplitude.Mercury.Sandbox.Sandbox.ActionController.FurtherForEmpire(empireIndex);
    }

    private static void KeepAIControlledArmiesMoving(IReadOnlyList<Army> armies, int empireIndex)
    {
        var isMajorEmpire = empireIndex < Amplitude.Mercury.Sandbox.Sandbox.NumberOfMajorEmpires;
        var processFurtherActions = false;

        for (var i = 0; i < armies.Count; i++)
        {
            var army = armies[i];

            var isSecondaryBeingProcessed = !isMajorEmpire || i % 4 == CycleModIndex;

            if (!army.IsIdle()) continue;
            
            var isRunning = army.IsRunning();
            var movementRatio = army.GetMovementRatio();
            var movementPointsLeft = (int) (movementRatio * army.MovementSpeed);

            if (army.IsRunningWaitingForFinishTurn())
            {
                // An active ArmyGoToAction waiting for next turn,
                // dealt with the FurtherForEmpire call below
                var simulationEntity = army.GetSimulationEntity();
                if (simulationEntity.IsAwake())
                    army.RefillMovementPoints(simulationEntity);
                processFurtherActions = true;
            }
            else if (movementPointsLeft == 0 && !isRunning && isSecondaryBeingProcessed)
            {
                // An active ArmyGoToAction and no more movement points left, used to avoid armies
                // not moving between refilling movement points from WaitingForFinishTurn above.
                var simulationEntity = army.GetSimulationEntity();
                if (simulationEntity.IsAwake())
                    army.RefillMovementPoints(simulationEntity);
            }
        }

        if (processFurtherActions)
            Amplitude.Mercury.Sandbox.Sandbox.ActionController.FurtherForEmpire(empireIndex);
    }
}
