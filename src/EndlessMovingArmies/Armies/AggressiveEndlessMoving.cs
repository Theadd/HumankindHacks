using System;
using System.Collections.Generic;
using System.Linq;
using Amplitude.Framework;
using Amplitude.Mercury;
using Amplitude.Mercury.Interop;
using Amplitude.Mercury.Simulation;
using Amplitude.Mercury.UI;
using AnN3x.EndlessMovingArmies.Core;
using AnN3x.HumankindLib;
using AnN3x.ModdingLib;
using UnityEngine;
using Army = Amplitude.Mercury.Interop.AI.Entities.Army;
using Empire = Amplitude.Mercury.Interop.AI.Entities.Empire;

namespace AnN3x.EndlessMovingArmies.Armies;

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
        var threshold = isControlledByHuman ? 1f : .2f;

        foreach (var army in armies)
        {
            if (army.IsIdle())
            {
                var simulationEntity = army.GetSimulationEntity();
                var isAwake = (simulationEntity?.GetAwakeState() ?? AwakeState.Sleep) == AwakeState.Awake;
                var isRunning = army.IsRunning();
                var movementRatio = army.GetMovementRatio();
                var otherFlags = isControlledByHuman || (!isRunning);

                if (isAwake && movementRatio < threshold && otherFlags)
                {
                    if (isControlledByHuman)
                    {
                        army.SetMovementRatio(1f);
                    }
                    else
                    {
                        if (IsProcessingMinorEmpires)
                        {
                            army.SetMovementRatio(1f);
                        }
                        else
                        {
                            var hasAdjacentHumanArmies = army.GetAdjacentArmies()
                                .Any(other => ControlledByHuman.Contains(other.EmpireIndex));

                            if (hasAdjacentHumanArmies && army.GetMovementRatio() > 0)
                            {
                                army.SetMovementRatio(0);
                                Amplitude.Mercury.Sandbox.Sandbox.SimulationEntityRepository
                                    .SetSynchronizationDirty(
                                        (ISimulationEntityWithSynchronization) simulationEntity);
                                Loggr.Log(
                                    $"LOCKED ARMY OF EMPIRE {army.EmpireIndex} DUE TO ADJACENT HUMAN CONTROLLED ARMY",
                                    ConsoleColor.Red);
                            }
                            else if (!hasAdjacentHumanArmies)
                            {
                                army.SetMovementRatio(1f);
                            }
                        }
                    }
                }
                else if (!isRunning)
                {
                    if (AreMandatoriesActive && isAwake)
                        army.SkipOneTurn();
                }
            }
        }
    }
}
