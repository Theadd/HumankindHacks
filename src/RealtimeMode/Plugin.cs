using System.Collections;
using BepInEx;
using AnN3x.ModdingLib;
using UnityEngine;

namespace AnN3x.RealtimeMode
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static GameObject RealtimeModeGameObject { get; private set; }
        public static RealtimeModeComponent RealtimeModeComponentInstance { get; private set; }
        // public static Plugin Instance { get; private set; }

        private void Awake()
        {
            // Instance = this;
            RealtimeModeGameObject = new GameObject("RealtimeMode");
            RealtimeModeGameObject.transform.parent = gameObject.transform;
            RealtimeModeGameObject.SetActive(false);
        }

        IEnumerator Start()
        {
            Initializer.Setup();

            Loggr.Debug("BEGIN INITIALIZING...");
            
            while(!Initializer.Initialize())
            {
                yield return new WaitForSeconds(.1f);
                Loggr.Debug("...");
            }

            Loggr.Debug("END INITIALIZING...");
            
            RealtimeModeComponentInstance = RealtimeModeGameObject.AddComponent<RealtimeModeComponent>();
            // RealtimeModeGameObject.SetActive(true);
            
            Loggr.Debug($"{PluginInfo.PLUGIN_GUID} successfully loaded.");
        }

        private void OnDestroy()
        {
            RealtimeModeGameObject.SetActive(false);
            Initializer.Unload();
            Destroy(RealtimeModeComponentInstance);
            Destroy(RealtimeModeGameObject);
            Loggr.Debug($"{PluginInfo.PLUGIN_GUID} successfully unloaded.");
        }
    }
}
