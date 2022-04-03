using UnityEngine;

namespace AnN3x.StyledGUI
{
    public static partial class Styles
    {
        private static GUIStyle _alertLabelStyle;

        public static GUIStyle AlertLabelStyle => _alertLabelStyle ??= new GUIStyle(Graphics.DefaultSkin.FindStyle("Text"))
        {
            margin = new RectOffset(24, 24, 16, 16),
            padding = new RectOffset(12, 8, 8, 8),
            border = new RectOffset(8, 2, 8, 8),
            overflow = new RectOffset(0, 0, 0, 0),
            // alignment = TextAnchor.MiddleCenter,
            font = null,
            fontSize = 12,
            normal = new GUIStyleState()
            {
                background = Graphics.DefaultSkin.textArea.hover.background,
                textColor = Color.white
            },
        };
        
    }
}
