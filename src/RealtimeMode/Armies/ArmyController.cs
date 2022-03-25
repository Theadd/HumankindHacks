using System;
using System.Collections.Generic;
using System.Linq;
using Amplitude.Mercury;
using Amplitude.Mercury.Interop;
using Amplitude.Mercury.Simulation;
using AnN3x.HumankindLib;
using AnN3x.ModdingLib;
using UnityEngine;
using Army = Amplitude.Mercury.Interop.AI.Entities.Army;
using Empire = Amplitude.Mercury.Interop.AI.Entities.Empire;

namespace AnN3x.RealtimeMode.Armies;

public class ArmyController
{
    public static List<int> EmpireIndicesLeft { get; private set; } = new List<int>();
    public static int TakeUpTo { get; private set; }
    public static IEnumerable<Empire> Empires { get; private set; } = new List<Empire>();
    public static int SkippedCyclesToMinor { get; private set; } = 0;
    public static bool IsProcessingMinorEmpires { get; private set; } = false;

    private static void Reset()
    {
        Empires = HumankindGame.GetAllEmpireEntities();
        if (!Config.EndlessMoving.OnAllEmpires)
        {
            EmpireIndicesLeft = new List<int>() { HumankindGame.LocalEmpireIndex };
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
        var now = DateTime.Now;
        Loggr.Log(
            @$"%RED%[ARMY CONTROLLER - RESET CYCLE]%DEFAULT% @ {now.Second}.{now.Millisecond}s {(IsProcessingMinorEmpires ? "PROCESSING MINOR EMPIRES" : "")}
    Empires = {EmpireIndicesLeft.Count}, MajorEmpires = {Amplitude.Mercury.Sandbox.Sandbox.NumberOfMajorEmpires}, TakeUpTo = {TakeUpTo}
    [{string.Join(", ", EmpireIndicesLeft.Where(i => IsProcessingMinorEmpires || (!IsProcessingMinorEmpires && i < Amplitude.Mercury.Sandbox.Sandbox.NumberOfMajorEmpires)).Select(i => "" + i))}]",
            ConsoleColor.Yellow);
    }

    public static void Run()
    {
        if (EmpireIndicesLeft.Count == 0)
            Reset();

        var someEmpires = EmpireIndicesLeft.Take(TakeUpTo);
        EmpireIndicesLeft = EmpireIndicesLeft.Skip(TakeUpTo).ToList();

        foreach (var empireIndex in someEmpires)
        {
            if (!IsProcessingMinorEmpires &&
                empireIndex < Amplitude.Mercury.Sandbox.Sandbox.NumberOfMajorEmpires)
                continue;

            try
            {
                var armies = Empires.ElementAt(empireIndex).Armies;
                if (armies != null && armies.Length > 0)
                {
                    if (armies[0].EmpireIndex != empireIndex)
                        throw new Exception(
                            "Current EmpireIndex and Army's EmpireIndex are expected to be the same.");

                    KeepArmiesMoving(armies);
                }
            }
            catch (Exception e)
            {
                Loggr.Log(e);
            }
        }
    }

    private static void KeepArmiesMoving(IEnumerable<Army> armies)
    {
        // TODO: if (army.EmpireIndex < Amplitude.Mercury.Sandbox.Sandbox.NumberOfMajorEmpires)
        foreach (var army in armies)
        {
            if (IsIdle(army))
            {
                if (GetArmyMovementRatio(army) < .1f)
                {
                    SetArmyMovementRatio(army, 1f);
                }
                else if (!IsRunning(army))
                {
                    if (Config.EndlessMoving.SkipOneTurn)
                        SkipOneTurn(army);
                }
            }
        }
    }

    public static void SetArmyMovementRatio(Army army, float movementRatio) =>
        Amplitude.Mercury.Sandbox.SandboxManager.PostOrder(
            new OrderChangeMovementRatio(
                army.SimulationEntityGUID,
                army.Units.Select(u => (SimulationEntityGUID) u.SimulationEntityGUID).ToArray(),
                movementRatio
            ), army.EmpireIndex);

    public static float GetArmyMovementRatio(Army army) => (float) army.PathfindContext.MovementRatio;

    public static bool IsRunning(Army army) => army.GoToActionStatus == Army.ActionStatus.Running;
    public static bool IsIdle(Army army) => army.State == ArmyState.Idle;

    public static void SkipOneTurn(Army army) =>
        Amplitude.Mercury.Sandbox.SandboxManager.PostOrder((Order) new OrderChangeEntityAwakeState()
        {
            EntityGuid = army.SimulationEntityGUID, AwakeState = AwakeState.SkipOneTurn
        }, army.EmpireIndex);
}
