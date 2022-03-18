using System;
using Amplitude.Framework;
using Amplitude.Framework.Presentation;
using Amplitude.Mercury.UI;

namespace AnN3x.HumankindLib;

public partial class HumankindGame
{
    internal static void Initialize()
    {
        ViewService = Services.GetService<IViewService>();
        ViewService.ViewChange += OnViewChanged;
        UIService = Services.GetService<IUIService>();
        UIService.UIStateChange += OnUIStateChanged;
        Amplitude.Framework.Game.IGameService gameService = Services.GetService<Amplitude.Framework.Game.IGameService>();
        gameService.GameChange += new EventHandler<Amplitude.Framework.Game.GameChangeEventArgs>(OnGameChanged);
    }

    internal static void Unload()
    {
        ViewService.ViewChange -= OnViewChanged;
        UIService.UIStateChange -= OnUIStateChanged;
        Amplitude.Framework.Game.IGameService gameService = Services.GetService<Amplitude.Framework.Game.IGameService>();
        gameService.GameChange -= new EventHandler<Amplitude.Framework.Game.GameChangeEventArgs>(OnGameChanged);
    }
}
