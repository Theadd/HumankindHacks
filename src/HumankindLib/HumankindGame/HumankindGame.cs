using AnN3x.HumankindLib.Reflection;
using System.Linq;
using Amplitude.Mercury.AI.Brain;
using Amplitude.Mercury.Interop.AI;
using Amplitude.Mercury.Interop.AI.Entities;
using Amplitude.Mercury.Sandbox;

namespace AnN3x.HumankindLib;

public class HumankindGame
{
    public static IAIPlayer[] GetIAIPlayers() => 
        (IAIPlayer[]) R.AIPlayerByEmpireIndex.GetValue(Sandbox.AIController);
    
    public static Empire[] GetAllEmpireEntities() => GetIAIPlayers().Select(player => 
        (Empire) R.ControlledEmpire.GetValue((AIPlayer) player)).ToArray();
    
    public static int LocalEmpireIndex => (int)Amplitude.Mercury.Interop.Snapshots.GameSnapshot.PresentationData.LocalEmpireInfo.EmpireIndex;
}
