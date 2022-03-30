using System;
using System.Collections;
using BepInEx;
using AnN3x.ModdingLib;
using AnN3x.ModdingLib.Core;
using AnN3x.RealtimeMode.UI;
using UnityEngine;

namespace AnN3x.RealtimeMode
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin, IPluginLauncher
    {
        public static GameObject MovingArmiesGameObject { get; private set; }
        public static EndlessMovingArmies EndlessMovingArmiesInstance { get; private set; }

        private void Awake()
        {
            // Instance = this;
            MovingArmiesGameObject = new GameObject("EndlessMovingArmies");
            MovingArmiesGameObject.transform.parent = gameObject.transform;
            MovingArmiesGameObject.SetActive(false);
        }

        IEnumerator Start()
        {
            Initializer.Setup();

            Loggr.Debug("BEGIN INITIALIZING...");

            while (!Initializer.Initialize())
            {
                yield return new WaitForSeconds(.1f);
                Loggr.Debug("...");
            }

            Loggr.Debug("END INITIALIZING...");

            EndlessMovingArmiesInstance = MovingArmiesGameObject.AddComponent<EndlessMovingArmies>();
            // RealtimeModeGameObject.SetActive(true);

            Loggr.Debug($"{PluginInfo.PLUGIN_GUID} successfully loaded.");
        }

        private void LateUpdate()
        {
            if (AnN3x.RealtimeMode.Config.Runtime.ShowUIKey.IsDown())
            {
                Loggr.Log("ShowUIKey.IsDown() in Update()", ConsoleColor.Green);
                Launch();
            }
        }

        private void OnDestroy()
        {
            MovingArmiesGameObject.SetActive(false);
            Initializer.Unload();
            Destroy(EndlessMovingArmiesInstance);
            Destroy(MovingArmiesGameObject);
            Loggr.Debug($"{PluginInfo.PLUGIN_GUID} successfully unloaded.");
        }

        public void Launch() => ModalMessage.Show();
    }
}
