using System;
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

        private void Awake()
        {
            Initializer.Setup();

            RealtimeModeGameObject = new GameObject("RealtimeMode");
            RealtimeModeGameObject.transform.parent = gameObject.transform;
            RealtimeModeGameObject.SetActive(false);
            RealtimeModeComponentInstance = RealtimeModeGameObject.AddComponent<RealtimeModeComponent>();

            Loggr.Log($"{PluginInfo.PLUGIN_GUID} successfully loaded.", System.ConsoleColor.Gray);
        }

        private void Start()
        {
            RealtimeModeGameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            RealtimeModeGameObject.SetActive(false);
            Destroy(RealtimeModeComponentInstance);
            Destroy(RealtimeModeGameObject);
            Loggr.Log($"{PluginInfo.PLUGIN_GUID} successfully unloaded.", System.ConsoleColor.Gray);
        }
    }
}
