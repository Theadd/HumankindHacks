using UnityEngine;

namespace AnN3x.StyledGUI
{
    public static partial class Styles
    {
        private static GUIStyle _treeRowBgStyle;
        private static GUIStyle _treeInlineTextStyle;

        // public static GUIStyle DefaultLinkStyle => StyledGUIUtility.DefaultSkin.FindStyle("Link");

        public static GUIStyle TreeBackgroundRowStyle => _treeRowBgStyle ??= new GUIStyle(SectionHeaderToggleAreaStyle)
        {
            normal = new GUIStyleState()
            {
                background = DefaultLinkStyle.normal.background,
                textColor = Color.white
            },
            hover = new GUIStyleState()
            {
                background = RowHoverPixel,
                textColor = Color.white
            },
        };
        
        public static GUIStyle TreeInlineTextStyle => _treeInlineTextStyle ??= new GUIStyle(Graphics.DefaultSkin.FindStyle("Text"))
        {
            // font = StyledGUIUtility.DefaultSkin.label.font,
            // fontSize = 13,
            font = null,
            fontSize = 12,
            alignment = TextAnchor.MiddleLeft,
            padding = new RectOffset(4, 0, 0, 0),
            margin = new RectOffset(0, 0, 0, 0),
            stretchWidth = false,
            stretchHeight = false,
            wordWrap = false,
            clipping = TextClipping.Clip,
            name = "TreeInlineText",
            normal = new GUIStyleState()
            {
                background = null,
                textColor = WhiteTextColor
            }
        };
    }
}
