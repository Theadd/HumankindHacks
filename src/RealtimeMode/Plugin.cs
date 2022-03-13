using BepInEx;
using AnN3x.ModdingLib;

namespace AnN3x.RealtimeMode
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Initializer.Setup();

            Loggr.Log($"{PluginInfo.PLUGIN_GUID} successfully loaded.", System.ConsoleColor.Gray);
        }

        private void OnDestroy()
        {
            Loggr.Log($"{PluginInfo.PLUGIN_GUID} successfully unloaded.", System.ConsoleColor.Gray);
        }
    }
}
