using System;
using System.Collections.Generic;
using System.Linq;
using Amplitude.Framework;
using Amplitude.Mercury;
using Amplitude.Mercury.Interop;
using Amplitude.Mercury.Simulation;
using Amplitude.Mercury.UI;
using AnN3x.HumankindLib;
using AnN3x.CoreLib;
using UnityEngine;
using Army = Amplitude.Mercury.Interop.AI.Entities.Army;
using Empire = Amplitude.Mercury.Interop.AI.Entities.Empire;

namespace AnN3x.EndlessMovingArmies.MovingArmies;

public class AggressiveEndlessMoving
{
    public static List<int> EmpireIndicesLeft { get; private set; } = new List<int>();
    public static int TakeUpTo { get; private set; }
    public static IEnumerable<Empire> Empires { get; private set; } = new List<Empire>();
    public static int SkippedCyclesToMinor { get; private set; } = 0;
    public static bool IsProcessingMinorEmpires { get; private set; } = false;
    public static int[] ControlledByHuman { get; private set; }
    public static bool AreMandatoriesActive { get; private set; } = true;
    public static bool WasLockedByEndTurn { get; private set; } = true;

    private static void Reset()
    {
        Empires = HumankindGame.GetAllEmpireEntities();
        ControlledByHuman = HumankindGame.MajorEmpires
            .Where(e => e.IsControlledByHuman)
            .Select(h => h.EmpireIndex)
            .ToArray();

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
            if (SkippedCyclesToMinor >= Config.EndlessMoving.CyclesToSkipBeforeProcessingMinorEmpires)
            {
                IsProcessingMinorEmpires = true;
                SkippedCyclesToMinor = 0;
            }
            else
            {
                IsProcessingMinorEmpires = false;
                SkippedCyclesToMinor++;
            }

            EmpireIndicesLeft = Empires.Select((e, i) => i).ToList();
            TakeUpTo = (int) Mathf.Ceil((float) EmpireIndicesLeft.Count /
                                        (float) Config.EndlessMoving.LoopIterationsPerCollectionOfEmpires);
        }

        EmpireIndicesLeft.Shuffle();
    }

    public static void Run(bool isLockedByEndTurn)
    {
        if (isLockedByEndTurn)
        {
            WasLockedByEndTurn = true;
            return;
        }

        if (WasLockedByEndTurn)
        {
            WasLockedByEndTurn = false;
            EmpireIndicesLeft.Clear();
        }

        if (EmpireIndicesLeft.Count == 0)
            Reset();

        var someEmpires = EmpireIndicesLeft.Take(TakeUpTo);
        EmpireIndicesLeft = EmpireIndicesLeft.Skip(TakeUpTo).ToList();

        foreach (var empireIndex in someEmpires)
        {
            if (!IsProcessingMinorEmpires &&
                empireIndex >= Amplitude.Mercury.Sandbox.Sandbox.NumberOfMajorEmpires)
                continue;

            try
            {
                if (Empires.ElementAt(empireIndex).Armies is { Length: > 0 } armies)
                {
                    if (armies[0].EmpireIndex != empireIndex)
                        throw new Exception(
                            "Current EmpireIndex and Army's EmpireIndex are expected to be the same.");

                    KeepArmiesMoving(armies, ControlledByHuman.Contains(empireIndex));
                }
            }
            catch (Exception e)
            {
                Loggr.Log(e);
            }
        }
    }

    private static void KeepArmiesMoving(IEnumerable<Army> armies, bool isControlledByHuman)
    {
        var threshold = isControlledByHuman ? .8f : .15f;
        var processFurtherActions = false;
        var empireIndex = 0;

        foreach (var army in armies)
        {
            if (!army.IsIdle()) continue;
            var simulationEntity = army.GetSimulationEntity();
            var isAwake = (simulationEntity?.GetAwakeState() ?? AwakeState.Sleep) == AwakeState.Awake;
            if (!isAwake) continue;
            var isRunning = army.IsRunning();
            var movementRatio = army.GetMovementRatio();


            if (isControlledByHuman && army.AutoExplore)
            {
                if (!isRunning || army.IsRunningWaitingForFinishTurn())
                {
                    army.RefillMovementPoints(simulationEntity);
                    processFurtherActions = true;
                    empireIndex = army.EmpireIndex;
                }
                else if (movementRatio <= .2f)
                {
                    army.RefillMovementPoints(simulationEntity);
                }
            }
            else if (army.IsRunningWaitingForFinishTurn())
            {
                army.RefillMovementPoints(simulationEntity);
                processFurtherActions = true;
                empireIndex = army.EmpireIndex;
            }
            else if ((movementRatio <= threshold && isRunning) ||
                     (!isControlledByHuman && movementRatio <= threshold))
            {
                army.RefillMovementPoints(simulationEntity);
            }
            else if (isControlledByHuman && !isRunning && AreMandatoriesActive &&
                     movementRatio is < .99f and > 0)
            {
                army.SkipOneTurn();
            }
        }

        if (processFurtherActions)
            Amplitude.Mercury.Sandbox.Sandbox.ActionController.FurtherForEmpire(empireIndex);
    }
}
