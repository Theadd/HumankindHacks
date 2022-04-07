using System;
using System.Collections;
using UnityEngine;
using Amplitude.Framework.Overlay;

namespace AnN3x.UI
{
    public abstract class PopupToolWindow : PopupWindow, IToolWindow
    {
        public PopupToolWindow() => IsDraggable = true;

        public string TypeName { get; set; } = null;

        private bool _forceWriteAsVisible = false;

        protected virtual void OnApplicationQuit() => OnWritePlayerPreferences();

        protected override void OnBecomeInvisible()
        {
            base.OnBecomeInvisible();
            
            if (UsePlayerPrefs)
                OnWritePlayerPreferences();
        }

        public virtual void SetWindowPosition(float x, float y)
        {
            float posX = Math.Min(x, Screen.width - 50f);
            float posY = Math.Min(y, Screen.height - 50f);

            var r = GetWindowRect();
            SetWindowRect(new Rect(posX, posY, r.width, r.height));
        }

        protected override void OnBecomeVisible()
        {
            if (UsePlayerPrefs)
            {
                OnReadPlayerPreferences();
                PlayerPrefs.SetInt(GetPlayerPrefKey("IsVisible"), 1);
            }
            
            base.OnBecomeVisible();
        }

        protected override void OnDrawWindowClientArea(int instanceId) { }
        
        protected override IEnumerator Start()
        {
            PopupToolWindow popupWindow = this;
            
            yield return (object) base.Start();
            if (UsePlayerPrefs)
            {
                var playerPrefKey = popupWindow.GetPlayerPrefKey("IsVisible");
                if (PlayerPrefs.HasKey(playerPrefKey) && PlayerPrefs.GetInt(playerPrefKey) != 0)
                    popupWindow.ShowWindow(true);
            }
        }

        public virtual string GetPlayerPrefKey(string key)
        {
            if (TypeName == null)
                TypeName = GetType().Name;

            return $"{Config.ToolWindowKeyPrefix}.{TypeName}.{key}";
        }

        public static bool WasVisible<T>() where T : PopupToolWindow
        {
            return PlayerPrefs.GetInt($"{Config.ToolWindowKeyPrefix}.{typeof(T).Name}.IsVisible", 0) == 1;
        }


        public virtual void OnWritePlayerPreferences()
        {
            if (!UsePlayerPrefs) return;
            
            PlayerPrefs.SetInt(GetPlayerPrefKey("IsVisible"), _forceWriteAsVisible || IsVisible ? 1 : 0);
            PlayerPrefs.SetFloat(GetPlayerPrefKey("X"), GetWindowRect().x);
            PlayerPrefs.SetFloat(GetPlayerPrefKey("Y"), GetWindowRect().y);
        }

        public virtual void OnReadPlayerPreferences()
        {
            if (!UsePlayerPrefs) return;
            
            string prefKeyX = GetPlayerPrefKey("X");
            string prefKeyY = GetPlayerPrefKey("Y");
            
            if (RestoreLastWindowPosition && PlayerPrefs.HasKey(prefKeyX) && PlayerPrefs.HasKey(prefKeyY))
                SetWindowPosition(PlayerPrefs.GetFloat(prefKeyX), PlayerPrefs.GetFloat(prefKeyY));
        }
        
        public abstract Rect GetWindowRect();
        public abstract void SetWindowRect(Rect rect);
        public abstract bool ShouldBeVisible { get; }
        public abstract bool RestoreLastWindowPosition { get; }
        public abstract bool UsePlayerPrefs { get; }
        
        private static T Open<T>() where T : PopupToolWindow
        {
            var window = Plugin.GetGameObject()?.GetComponent<T>() ?? Plugin.GetGameObject()?.AddComponent<T>();
            
            if (window != null)
                window.ShowWindow(true);
            
            return (T) window;
        }
        
        public static void Open<T>(Action<T> callback) where T : PopupToolWindow
        {
            if (!UIController.IsGUILoaded)
                UIController.OnGUIHasLoaded += () => callback.Invoke(Open<T>());
            else
                callback.Invoke(Open<T>());
        }

        public virtual void Close(bool saveVisibilityStateBeforeClosing = false)
        {
            if (saveVisibilityStateBeforeClosing && IsVisible)
                _forceWriteAsVisible = true;
            
            ShowWindow(false);
            Destroy(this);
        }
    }
}
