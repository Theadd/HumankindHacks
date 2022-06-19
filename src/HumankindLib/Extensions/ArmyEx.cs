using System.Collections.Generic;
using System.Linq;
using Amplitude.Mercury;
using Amplitude.Mercury.Interop.AI.Data;
using Amplitude.Mercury.Interop;
using Amplitude.Mercury.Simulation;
using AnN3x.HumankindLib.Reflection;
using SimulationEntityGUID = Amplitude.Mercury.Simulation.SimulationEntityGUID;
using Army = Amplitude.Mercury.Interop.AI.Entities.Army;
using Territory = Amplitude.Mercury.Interop.AI.Entities.Territory;

namespace AnN3x.HumankindLib;

public static class ArmyEx
{
    public static Amplitude.Mercury.Simulation.Army GetSimulationEntity(this Army self) =>
        Amplitude.Mercury.Sandbox.Sandbox.SimulationEntityRepository
            .TryGetSimulationEntity<Amplitude.Mercury.Simulation.Army>(
                new SimulationEntityGUID(self.EntityGUID), out Amplitude.Mercury.Simulation.Army army)
            ? army
            : default;

//    public static ArmyGoToAction GetArmyGoToAction(this Army self) =>
//        Amplitude.Mercury.Sandbox.Sandbox.ActionController.GetActionFor<ArmyGoToAction, Army>(
//            self.GetSimulationEntity());

    public static int GetDistanceTo(this Army self, int tileIndex) =>
        WorldPosition.GetTileIndexDistance(self.TileIndex, tileIndex);

    public static Territory GetTerritory(this Army self) =>
        Amplitude.Mercury.Interop.AI.Snapshots.World.Territories[self.TerritoryIndex];

    public static TerritoryInfo GetTerritoryInfo(this Army self) =>
        Amplitude.Mercury.Interop.AI.Snapshots.World.Territories[self.TerritoryIndex].Info;

    public static IEnumerable<Army> GetAdjacentArmies(this Army self) =>
        Amplitude.Mercury.Interop.AI.Snapshots.World.Tiles[self.TileIndex].AdjacentTiles
            .Select(adj => Amplitude.Mercury.Interop.AI.Snapshots.World.Tiles[adj.TileIndex].Army)
            .Where(army => army != null);

    public static AwakeState GetAwakeState(this Amplitude.Mercury.Simulation.Army self) =>
        (AwakeState) R.AwakeState.GetValue(self);

    public static void SetMovementRatio(this Army army, float movementRatio) =>
        Amplitude.Mercury.Sandbox.SandboxManager.PostOrder(
            new OrderChangeMovementRatio(
                army.SimulationEntityGUID,
                army.Units.Select(u => (SimulationEntityGUID) u.SimulationEntityGUID).ToArray(),
                movementRatio
            ), army.EmpireIndex);

    public static float GetMovementRatio(this Army army) => (float) army.PathfindContext.MovementRatio;

    public static void RefillMovementPoints(this Army army,
        Amplitude.Mercury.Simulation.Army simulationEntity)
    {
        army.SetMovementRatio(.95f);
        Amplitude.Mercury.Sandbox.Sandbox.SimulationEntityRepository.SetSynchronizationDirty(
            (ISimulationEntityWithSynchronization) simulationEntity);
    }

    public static bool IsRunning(this Army army) => army.GoToAction.Status != Army.ActionStatus.None;

    public static bool IsRunningWaitingForFinishTurn(this Army army) =>
        army.GoToAction.Status == Army.ActionStatus.WaitingForFinishTurn;

    public static bool IsIdle(this Army army) => army.State == ArmyState.Idle;

    public static void SkipOneTurn(this Army army) =>
        Amplitude.Mercury.Sandbox.SandboxManager.PostOrder((Order) new OrderChangeEntityAwakeState()
        {
            EntityGuid = army.SimulationEntityGUID, AwakeState = AwakeState.SkipOneTurn
        }, army.EmpireIndex);

    public static bool IsAwake(this Army army) =>
        (army.GetSimulationEntity()?.GetAwakeState() ?? AwakeState.Sleep) == AwakeState.Awake;

    public static bool IsAwake(this Amplitude.Mercury.Simulation.Army army) =>
        (army?.GetAwakeState() ?? AwakeState.Sleep) == AwakeState.Awake;
}
