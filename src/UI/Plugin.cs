using BepInEx;
using UnityEngine;

namespace AnN3x.UI
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private static AssetLoader _assets;

        private static bool _uiReady;

        public static AssetLoader Assets =>
            _assets = _assets ?? new AssetLoader()
            {
                Assembly = typeof(Plugin).Assembly,
                ManifestResourceName = "AnN3x.UI.Resources.ann3x-shared-resources"
            };

        public static Plugin Instance { get; private set; }
        public static GameObject GetGameObject() => Instance != null ? Instance.gameObject : null;

        private void Awake()
        {
            Instance = this;
        }

        private void LateUpdate()
        {
            if (_uiReady) return;

            if (GameObject.Find("/WindowsRoot/SystemOverlays") != null)
            {
                _uiReady = true;
                UIController.Initialize();
            }
        }

        private void OnDestroy()
        {
            Assets.Unload(true);
            Destroy(gameObject);
        }
    }
}
