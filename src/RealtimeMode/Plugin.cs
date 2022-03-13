using BepInEx;
// using SharedLib;
using UnityEngine;

namespace AnN3x.RealtimeMode
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin ... {PluginInfo.PLUGIN_GUID} is loaded! ############################################ ");
            Logger.LogInfo($"Color.Yellow = {Color.yellow.ToString()}");
            /*Logger.LogInfo($"Colors.AliceBlue = {Colors.AliceBlue}");
            Logger.LogInfo($"Color.Yellow = {Color.yellow.ToString()}");
            Logger.LogInfo($"Colors.Cyan = {Colors.Cyan}");
            Logger.LogInfo($"RedColorString = {RedColorString()}");*/
        }

        private void OnDestroy()
        {
            Logger.LogInfo($"\n###################################################");
            Logger.LogInfo($"\tUNLOADING {PluginInfo.PLUGIN_GUID}");
            Logger.LogInfo($"###################################################\n");
        }
    }
}
