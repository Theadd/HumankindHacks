using System;
using Amplitude.Framework;
using Amplitude.Framework.Game;
using Amplitude.Framework.Presentation;
using Amplitude.Mercury.UI;
using AnN3x.CoreLib;

namespace AnN3x.HumankindLib;

public partial class HumankindGame
{
    internal static void Initialize()
    {
        if ((ViewService = Services.GetService<IViewService>()) is null)
            throw new NullReferenceException(nameof(ViewService));

        ViewService.ViewChange += OnViewChanged;
        if (ViewService.View is not null)
            ApplyViewType(ViewService.View);

        if ((GameService = Services.GetService<Amplitude.Framework.Game.IGameService>()) is null)
            throw new NullReferenceException(nameof(GameService));

        GameService.GameChange +=
            new EventHandler<Amplitude.Framework.Game.GameChangeEventArgs>(OnGameChanged);
        try
        {
            if (Amplitude.Mercury.Presentation.Presentation.HasBeenStarted)
                GameState = GameChangeAction.Started;
        }
        catch (Exception e)
        {
            Loggr.Log(e);
        }
    }

    internal static void Unload()
    {
        if (ViewService is not null)
            ViewService.ViewChange -= OnViewChanged;

        if (GameService is not null)
            GameService.GameChange -=
                new EventHandler<Amplitude.Framework.Game.GameChangeEventArgs>(OnGameChanged);
    }
}
