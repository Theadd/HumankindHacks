
using UnityEngine;

namespace AnN3x.UI
{
    public interface IToolWindow
    {
        Rect GetWindowRect();
        void SetWindowRect(Rect rect);
        bool ShouldBeVisible { get; }
        bool RestoreLastWindowPosition { get; }
        void OnWritePlayerPreferences();
        void OnReadPlayerPreferences();
        void Close(bool saveVisibilityStateBeforeClosing = false);
    }
}
