using System;
using System.Collections;
using Amplitude.Framework.Overlay;
using Amplitude.Mercury.UI;
using Amplitude.UI;
using Amplitude.UI.Interactables;
using UnityEngine;

namespace AnN3x.UI
{
    [RequireComponent(typeof(UITransform))]
    [RequireComponent(typeof(UIButton))]
    public class UIOverlay : MonoBehaviour
    {
        public UIButton Control { get; protected set; }

        public UITransform UITransform { get; protected set; }

        public PopupWindow Target { get; set; }

        public static bool DEBUG_DRAW_OVERLAY { get; set; } = false;

        private IEnumerator _resync;
        private float _syncInterval = 0.1f;
        private Rect _lastRect = Rect.zero;
        private static GameObject _container;
        public bool IsVisibleSelf { get; private set; } = true;

        public static GameObject Container
        {
            get
            {
                if (_container == null)
                {
                    var parentContainer = GameObject.Find("/WindowsRoot/SystemOverlays");
                    var count = parentContainer.transform.childCount;

                    for (int i = count - 1; i >= 0; i--)
                    {
                        var child = parentContainer.transform.GetChild(i);

                        if (child.name == Config.UIOverlayName)
                        {
                            Destroy(child.gameObject);
                        }
                    }

                    _container = new GameObject(Config.UIOverlayName);
                    _container.transform.parent = parentContainer != null ? parentContainer.transform : null;
                    _container.AddComponent<UITransform>();
                }

                return _container;
            }
        }

        public static UIOverlay Find(string uuid)
        {
            var t = Container.transform.Find(uuid);

            return (t == null) ? Create(uuid) : t.GetComponent<UIOverlay>();
        }

        public Rect ApplyRelativeResolution(Rect rect)
        {
            Rect uiRect = UITransform.Parent.Parent.GlobalRect;

            return new Rect(
                (uiRect.width * rect.x) / Screen.width,
                (uiRect.height * rect.y) / Screen.height,
                (uiRect.width * rect.width) / Screen.width,
                (uiRect.height * rect.height) / Screen.height
            );
        }

        public void Sync(PopupWindow target)
        {
            Target = target;

            if (Target == null || !Target.IsVisible)
            {
                _syncInterval = -1f;

                if (_resync != null)
                    StopCoroutine(_resync);

                if (gameObject != null && gameObject.transform != null)
                    gameObject.transform.parent = null;

                Destroy(gameObject);

                return;
            }

            var rect = ApplyRelativeResolution(target.GetWindowPosition());
            _lastRect = rect;
            UITransform.X = rect.x;
            UITransform.Y = rect.y;
            UITransform.Width = rect.width;
            UITransform.Height = rect.height;
            UITransform.VisibleSelf = true;
            IsVisibleSelf = true;
        }

        private IEnumerator Resync()
        {
            while (_syncInterval != -1f)
            {
                if (Target != null)
                {
                    var targetVisible = Target.IsWindowVisible();

                    if (targetVisible)
                    {
                        if (!ApplyRelativeResolution(Target.GetWindowPosition()).Equals(_lastRect))
                        {
                            _syncInterval = 0.1f;
                            Sync(Target);
                        }
                        else
                        {
                            _syncInterval = 1.0f;
                        }

                        if (!IsVisibleSelf)
                        {
                            UITransform.VisibleSelf = true;
                            IsVisibleSelf = true;
                        }
                    }
                    else if (targetVisible != IsVisibleSelf)
                    {
                        UITransform.VisibleSelf = false;
                        IsVisibleSelf = false;
                        _syncInterval = 0.8f;
                    }
                }

                yield return new WaitForSeconds(_syncInterval);
            }
        }

        protected static UIOverlay Create(string uuid)
        {
            var go = new GameObject(uuid);
            go.transform.parent = Container.transform;
            var overlay = go.AddComponent<UIOverlay>();
            overlay.Setup();

            return overlay;
        }

        protected void Setup()
        {
            UITransform = GetComponent<UITransform>();
            Control = GetComponent<UIButton>();
            Control.LoadIfNecessary();

            if (DEBUG_DRAW_OVERLAY && gameObject.GetComponent<SquircleBackgroundWidget>() == null)
            {
                var canvas = gameObject.AddComponent<SquircleBackgroundWidget>();
                canvas.BackgroundColor = Color.clear;
                canvas.OuterBorderColor = Color.green;
                canvas.BorderColor = Color.clear;
                canvas.CornerRadius = 0f;
            }
        }

        public void OnMouseEventHandler(IUIControl control, Vector2 coords)
        {
            if (Target != null)
            {
                _syncInterval = 0.1f;
                Sync(Target);
            }
        }

        public static void Unload()
        {
            var c = GameObject.Find("/WindowsRoot/SystemOverlays/" + Config.UIOverlayName);
            if (c != null)
            {
                c.transform.parent = null;
                Destroy(c);
                _container = null;
            }
        }

        void Start()
        {
            _resync = Resync();
            Setup();
            StartCoroutine(_resync);
        }
    }

    public static class UIWindowExtensions
    {
        public static void SyncUIOverlay(this PopupWindow target, Action priorAction = null)
        {
            try
            {
                priorAction?.Invoke();
                UIOverlay.Find("UIOverlay" + target.GetInstanceID()).Sync(target);
            }
            catch (Exception)
            {
                // Ignore
            }
        }

        public static Rect GetWindowPosition(this PopupWindow target)
        {
            if (target is IToolWindow customTarget)
            {
                return customTarget.GetWindowRect();
            }

            return target.WindowPosition;
        }

        public static bool IsWindowVisible(this PopupWindow target)
        {
            if (target is IToolWindow customTarget)
            {
                return customTarget.ShouldBeVisible && target.IsVisible &&
                       !FloatingToolWindow.HideAllGUITools;
            }

            return target.IsVisible;
        }
    }
}
