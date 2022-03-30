using System;
using System.Collections;
using AnN3x.EndlessMovingArmies.UI;
using BepInEx;
using AnN3x.ModdingLib;
using AnN3x.ModdingLib.Core;
using UnityEngine;

namespace AnN3x.EndlessMovingArmies
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin, IPluginLauncher
    {
        public static GameObject MovingArmiesGameObject { get; private set; }
        public static EndlessArmyMover ArmyMover { get; private set; }

        private void Awake()
        {
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

            ArmyMover = MovingArmiesGameObject.AddComponent<EndlessArmyMover>();
            // RealtimeModeGameObject.SetActive(true);

            Loggr.Debug($"{PluginInfo.PLUGIN_GUID} successfully loaded.");
        }

        private void LateUpdate()
        {
            if (EndlessMovingArmies.Config.Runtime.ShowUIKey.IsDown())
            {
                Loggr.Log("ShowUIKey.IsDown() in Update()", ConsoleColor.Green);
                Launch();
            }
        }

        private void OnDestroy()
        {
            MovingArmiesGameObject.SetActive(false);
            Initializer.Unload();
            Destroy(ArmyMover);
            Destroy(MovingArmiesGameObject);
            Loggr.Debug($"{PluginInfo.PLUGIN_GUID} successfully unloaded.");
        }

        public void Launch() => ModalMessage.Show();
    }
}
