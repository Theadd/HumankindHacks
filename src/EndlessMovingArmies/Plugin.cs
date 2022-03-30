using System;
using System.Collections;
using AnN3x.EndlessMovingArmies.UI;
using BepInEx;
using AnN3x.ModdingLib;
using AnN3x.ModdingLib.Core;
using UnityEngine;
using AnN3x.EndlessMovingArmies.Core;

namespace AnN3x.EndlessMovingArmies
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME,
        PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin, IPluginLauncher
    {
        public static GameObject MovingArmiesGameObject { get; private set; }
        public static Core.EndlessMovingArmies EndlessMovingArmiesInstance { get; private set; }

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

            EndlessMovingArmiesInstance = MovingArmiesGameObject.AddComponent<Core.EndlessMovingArmies>();
            // RealtimeModeGameObject.SetActive(true);

            Loggr.Debug($"{PluginInfo.PLUGIN_GUID} successfully loaded.");
        }

        private void LateUpdate()
        {
            if (Core.Config.Runtime.ShowUIKey.IsDown())
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
