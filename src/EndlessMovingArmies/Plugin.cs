using System.Collections;
using AnN3x.EndlessMovingArmies.UI;
using BepInEx;
using AnN3x.CoreLib.Plugin;
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

            while (!Initializer.Initialize())
                yield return new WaitForSeconds(.1f);

            ArmyMover = MovingArmiesGameObject.AddComponent<EndlessArmyMover>();
        }

        private void LateUpdate()
        {
            if (EndlessMovingArmies.Config.Runtime.ShowUIKey.IsDown()) 
                Launch();
        }

        private void OnDestroy()
        {
            MovingArmiesGameObject.SetActive(false);
            Initializer.Unload();
            Destroy(ArmyMover);
            Destroy(MovingArmiesGameObject);
        }

        public void Launch() => ModalMessage.Show();
    }
}
