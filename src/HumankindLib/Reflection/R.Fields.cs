using System.Reflection;
using Amplitude.Mercury.AI;
using System;
using Amplitude.Mercury.AI.Brain;

namespace AnN3x.HumankindLib.Reflection;

public partial class R
{
    public static readonly FieldInfo AIPlayerByEmpireIndex = GetField<AIController>("aiPlayerByEmpireIndex");
    public static readonly FieldInfo ControlledEmpire = GetField<AIPlayer>("controlledEmpire");
}
