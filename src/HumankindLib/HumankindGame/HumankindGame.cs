using System;
using AnN3x.HumankindLib.Reflection;
using System.Linq;
using Amplitude.Framework.Game;
using Amplitude.Mercury.AI.Brain;
using Amplitude.Mercury.Interop;
using Amplitude.Mercury.Interop.AI;
using Amplitude.Mercury.Interop.AI.Entities;
using Amplitude.Mercury.Sandbox;
using Snapshots = Amplitude.Mercury.Interop.Snapshots;

namespace AnN3x.HumankindLib;

public partial class HumankindGame
{
    public static bool IsInGame => View == ViewType.InGame && GameState == GameChangeAction.Started;
    public static bool IsOutGame => View == ViewType.OutGame && GameState == GameChangeAction.Shutdown;
    public static bool IsLoadingGame => View == ViewType.Loading && GameState is GameChangeAction.Starting or GameChangeAction.Started;

    public static bool IsUILockedByEndTurn => Amplitude.Mercury.Presentation.Presentation
        .PresentationUIController is { IsUILockedByEndTurn: true };
    public static int Turn => Amplitude.Mercury.Interop.AI.Snapshots.Game?.Turn ?? 0;
    
    public static IAIPlayer[] GetIAIPlayers() => 
        (IAIPlayer[]) R.AIPlayerByEmpireIndex.GetValue(Sandbox.AIController);
    
    public static Empire[] GetAllEmpireEntities() => GetIAIPlayers().Select(player => 
        (Empire) R.ControlledEmpire.GetValue((AIPlayer) player)).ToArray();
    
    public static void CenterCameraAt(int tileIndex) => 
        Amplitude.Mercury.Presentation.Presentation.PresentationCameraController.CenterCameraAt(tileIndex);

    public static bool GameplayFogOfWar
    {
        get => Snapshots.GameSnapshot?.PresentationData?.IsGameplayFogOfWarEnabled ?? false;
        set => SandboxManager.PostOrder(new OrderEnableFogOfWar { Enable = value });
    }

    public static bool PresentationFogOfWar
    {
        get => Snapshots.GameSnapshot?.PresentationData?.IsPresentationFogOfWarEnabled ?? false;
        set => Snapshots.GameSnapshot?.SetFogOfWarEnabled(value);
    }

    public static int LocalEmpireIndex => Snapshots.GameSnapshot.PresentationData.LocalEmpireInfo.EmpireIndex;

    public static MajorEmpire[] MajorEmpires => Amplitude.Mercury.Interop.AI.Snapshots.Game?.MajorEmpires ??
                                                Array.Empty<MajorEmpire>();
}
