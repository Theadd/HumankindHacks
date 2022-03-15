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
    
    /*private void SyncMovingArmies()
    {
        for (var i = MovingArmies.Count() - 1; i >= 0; i--)
        {
            var movingArmy = MovingArmies[i];

            if (ArmyUtils.ArmyMovementRatio(movingArmy.Army) <= 0.5f)
                Amplitude.Mercury.Sandbox.SandboxManager.PostOrder(movingArmy.Order, movingArmy.EmpireIndex);
            else if (!ArmyUtils.IsRunning(movingArmy.Army))
            {
                movingArmy.SkipOneTurn();
                MovingArmies.Remove(movingArmy);
            }
        }
    }
    
    public void AddToEndlessMovingArmies(Army army)
    {
        if (!MovingArmies.Any(item => item.Army.EntityGUID == army.EntityGUID))
        {
            if (IsArmySelectedInGame(army) && TryGetArmyCursor(out ArmyCursor armyCursor))
            {
                if (army.EntityGUID == armyCursor.EntityGUID && ArmyUtils.IsRunning(army))
                {
                    if (armyCursor.SelectedUnitCount != army.Units.Length)
                        armyCursor.SelectAll();

                    EndlessMovingArmy movingArmy = new EndlessMovingArmy() {
                        Order = new OrderChangeMovementRatio(
                            armyCursor.EntityGUID, 
                            armyCursor.SelectedUnits.Select(guid => guid).ToArray(), 
                            1.0f
                        ),
                        EmpireIndex = (int) Snapshots.ArmyCursorSnapshot.PresentationData.EmpireIndex,
                        Army = army
                    };
                    MovingArmies.Add(movingArmy);
                }
            }
        }
    }

    public bool IsArmySelectedInGame(Army army) => army.EntityGUID == ArmyCursorEntityGUID;

    public static bool TryGetArmyCursor(out ArmyCursor armyCursor) => (
        (armyCursor = Presentation.PresentationCursorController.CurrentCursor as ArmyCursor) != null 
        && Snapshots.ArmyCursorSnapshot != null);
    */

    public static List<int> EmpireIndicesLeft { get; private set; } = new List<int>();
    public static int TakeUpTo { get; private set; }
    public static IEnumerable<Empire> Empires { get; private set; } = new List<Empire>();

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
            EmpireIndicesLeft = Empires.Select((e, i) => i).ToList();
            TakeUpTo = (int)Mathf.Ceil((float)EmpireIndicesLeft.Count /
                                  (float)Config.EndlessMoving.LoopIterationsPerCollectionOfEmpires);
        }
        EmpireIndicesLeft.Shuffle();
    }

    public static void Run()
    {
        if (EmpireIndicesLeft.Count == 0)
            Reset();

        var someEmpires = EmpireIndicesLeft.Take(TakeUpTo);
        EmpireIndicesLeft = EmpireIndicesLeft.Skip(TakeUpTo).ToList();
        
        foreach (var empireIndex in someEmpires)
        {
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
        foreach (var army in armies)
        {
            if (ArmyMovementRatio(army) < 1.0f)
            {
                Amplitude.Mercury.Sandbox.SandboxManager.PostOrder(
                    new OrderChangeMovementRatio(
                        army.SimulationEntityGUID,
                        army.Units.Select(u => (SimulationEntityGUID) u.SimulationEntityGUID).ToArray(),
                        1.0f
                    ), army.EmpireIndex);
            }
            else if (!IsRunning(army))
            {
                if (Config.EndlessMoving.SkipOneTurn)
                    SkipOneTurn(army);
            }
        }
    }
    
    public static float ArmyMovementRatio(Army army) => (float) army.PathfindContext.MovementRatio;

    public static bool IsRunning(Army army) => army.GoToActionStatus == Army.ActionStatus.Running;
    
    public static void SkipOneTurn(Army army) => 
        Amplitude.Mercury.Sandbox.SandboxManager.PostOrder((Order) new OrderChangeEntityAwakeState() {
            EntityGuid = army.SimulationEntityGUID, AwakeState = AwakeState.SkipOneTurn
        }, army.EmpireIndex);
}
