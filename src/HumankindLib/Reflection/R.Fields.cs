using System.Reflection;
using Amplitude.Mercury.AI;
using System;
using Amplitude.Mercury.AI.Brain;
using Amplitude.Mercury.UI.Windows;
using Amplitude.Mercury.UI;

namespace AnN3x.HumankindLib.Reflection;

public partial class R
{
    public static readonly FieldInfo AIPlayerByEmpireIndex = GetField<AIController>("aiPlayerByEmpireIndex");
    public static readonly FieldInfo ControlledEmpire = GetField<AIPlayer>("controlledEmpire");
    public static readonly FieldInfo AllGameWindows = GetField<WindowsManager>("allGameWindows");
    public static readonly FieldInfo DataUtils = typeof(Utils).GetField("DataUtils", Flags);
}
