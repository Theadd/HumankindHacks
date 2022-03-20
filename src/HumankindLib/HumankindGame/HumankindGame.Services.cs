using Amplitude.Framework;
using Amplitude.Framework.Presentation;
using Amplitude.Framework.Session;
using Amplitude.Mercury.UI;
using Amplitude.Mercury.UI.Windows;

namespace AnN3x.HumankindLib;

public partial class HumankindGame
{
    public static IViewService ViewService { get; private set; }
    public static Amplitude.Framework.Game.IGameService GameService { get; private set; }
    public static IWindowsService WindowsService => Services.GetService<IWindowsService>();
    public static ISessionService SessionService => Services.GetService<ISessionService>();
}
