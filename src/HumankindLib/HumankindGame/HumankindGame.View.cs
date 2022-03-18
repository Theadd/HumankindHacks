using System;
using Amplitude.Framework.Presentation;
using Amplitude.Mercury.Presentation;
using Amplitude.Mercury.UI;
using Amplitude.Framework.Game;
using AnN3x.ModdingLib;

namespace AnN3x.HumankindLib;

internal enum ViewType
{
    Loading,    //EmptyView
    OutGame,
    InGame,
    MapEditor,
    ShuttingDown
}

internal enum ChangeType
{
    ViewChange,
    UIStateChange,
    GameChange
}

public partial class HumankindGame
{
    private static ViewType View { get; set; } = ViewType.Loading;
    private static UIState State { get; set; } = UIState.Undefined;
    private static GameChangeAction GameState { get; set; } = GameChangeAction.Shutdown;

    private static void OnViewChanged(object sender, ViewChangedEventArgs eventArgs)
    {
        ApplyViewType(eventArgs.View);
    }

    private static void ApplyViewType(IView view)
    {
        View = view switch
        {
            /*EmptyView emptyView => throw new NotImplementedException(),
            InGameView inGameView => throw new NotImplementedException(),
            MapEditorView mapEditorView => throw new NotImplementedException(),
            OutGameView outGameView => throw new NotImplementedException(),
            View view1 => throw new NotImplementedException(),*/
            InGameView => ViewType.InGame,
            OutGameView => ViewType.OutGame,
            EmptyView => ViewType.Loading,
            MapEditorView => ViewType.MapEditor,
            _ => throw new ArgumentOutOfRangeException(nameof(view))
        };

        if (State != UIService.UIState)
            State = UIService.UIState;
        
        OnViewOrUIStateChanged(ChangeType.ViewChange);
    }
    
    private static void OnUIStateChanged(UIState newState)
    {
        ApplyUIState(newState);
    }
    
    private static void ApplyUIState(UIState newState)
    {
        State = newState;
        
        if (newState is UIState.ShuttingDown or UIState.Shutdown)
            View = ViewType.ShuttingDown;

        OnViewOrUIStateChanged(ChangeType.UIStateChange);
    }
    
    private static void OnGameChanged(object sender, GameChangeEventArgs e)
    {
        ApplyGameState(e.Action);
    }
    
    private static void ApplyGameState(GameChangeAction newGameState)
    {
        GameState = newGameState;

        OnViewOrUIStateChanged(ChangeType.GameChange);
    }

    private static void OnViewOrUIStateChanged(ChangeType changeType)
    {
        Loggr.Log($"#[%DARKMAGENTA%{changeType.ToString()}%DEFAULT%] GameState = {GameState.ToString()}, UIState = {State.ToString()}, View = (ViewType: {View.ToString()}, Name: {ViewService.View.Name}, HasFocus: {ViewService.View.HasFocus}, IsActive: {ViewService.View.IsActive})", ConsoleColor.Gray);
    }
}
