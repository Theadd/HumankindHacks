using System;
using Amplitude.Framework.Presentation;
using Amplitude.Mercury.Presentation;
using Amplitude.Framework.Game;
using AnN3x.ModdingLib;

namespace AnN3x.HumankindLib;

internal enum ViewType
{
    Loading,
    OutGame,
    InGame,
    MapEditor
}

internal enum ChangeType
{
    ViewChange,
    GameChange
}

public partial class HumankindGame
{
    private static ViewType View { get; set; } = ViewType.Loading;
    private static GameChangeAction GameState { get; set; } = GameChangeAction.Shutdown;

    private static void OnViewChanged(object sender, ViewChangedEventArgs eventArgs) =>
        ApplyViewType(eventArgs.View);

    private static void OnGameChanged(object sender, GameChangeEventArgs e) => ApplyGameState(e.Action);

    private static void ApplyViewType(IView view)
    {
        try
        {
            View = view switch
            {
                InGameView => ViewType.InGame,
                OutGameView => ViewType.OutGame,
                EmptyView => ViewType.Loading,
                MapEditorView => ViewType.MapEditor,
                Amplitude.Framework.Presentation.View => ViewType.Loading,
                _ => throw new ArgumentOutOfRangeException(nameof(view))
            };

            OnGameViewChanged(ChangeType.ViewChange);
        }
        catch (Exception e)
        {
            Loggr.Log(e);
        }
    }

    private static void ApplyGameState(GameChangeAction newGameState)
    {
        GameState = newGameState;

        OnGameViewChanged(ChangeType.GameChange);
    }

    private static void OnGameViewChanged(ChangeType changeType)
    {
        Loggr.Debug(
            $"#[{changeType.ToString()}] GameState = {GameState.ToString()}, View = (ViewType: {View.ToString()}, HasFocus: {ViewService.View.HasFocus}, IsActive: {ViewService.View.IsActive})");
    }
}
