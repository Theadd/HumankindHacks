using UnityEngine;

namespace AnN3x.StyledGUI
{
    public static partial class Styles
    {
        private static GUIStyle _unicodeSymbolTextStyle;
        private static GUIStyle _toggleCollapsibleSectionStyle;
        private static GUIStyle _unicodeSymbolButtonStyle;
        private static GUIStyle _unicodeSymbolToolbarStyle;
        private static GUIStyle _unicodeIconStyle;
        private static GUIStyle _unicodeLinkStyle;
        private static GUIStyle _unicodeLabelStyle;
        private static GUIStyle _bgSectionHeaderStyle;
        private static GUIStyle _inlineUnicodeButtonStyle;

        public static GUIStyle DefaultLinkStyle => Graphics.DefaultSkin.FindStyle("Link");

        public static GUIStyle UnicodeSymbolTextStyle => _unicodeSymbolTextStyle ??=
            new GUIStyle(Graphics.DefaultSkin.FindStyle("Text"))
            {
                font = Graphics.UnicodeSymbolsFont,
                fontSize = 24
            };

        public static GUIStyle CollapsibleSectionToggleStyle => _toggleCollapsibleSectionStyle ??=
            new GUIStyle(DefaultLinkStyle)
            {
                font = Graphics.UnicodeSymbolsFont,
                name = "CollapsibleSectionToggle",
                fontSize = 16,
                fontStyle = FontStyle.Normal,
                margin = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(2, 0, 3, 3),
                normal = new GUIStyleState()
                {
                    background = DefaultLinkStyle.normal.background,
                    textColor = Color.white
                },
                onNormal = new GUIStyleState()
                {
                    background = DefaultLinkStyle.normal.background,
                    textColor = WhitePearlTextColor
                },
                onHover = new GUIStyleState()
                {
                    background = DefaultLinkStyle.normal.background,
                    // background = StyledGUI.Graphics.BlackTexture,
                    textColor = GoldTextColor
                },
                hover = new GUIStyleState()
                {
                    background = DefaultLinkStyle.normal.background,
                    textColor = GoldTextColor
                },
            };

        public static GUIStyle UnicodeSymbolButtonStyle => _unicodeSymbolButtonStyle ??= new GUIStyle(Graphics
            .DefaultSkin
            .button)
        {
            font = Graphics.UnicodeSymbolsFont,
            fontSize = 13,
            alignment = TextAnchor.MiddleCenter,
            padding = new RectOffset(0, 0, 0, 0)
        };

        public static GUIStyle UnicodeSymbolToolbarStyle => _unicodeSymbolToolbarStyle ??=
            new GUIStyle(UnicodeSymbolButtonStyle)
            {
                fixedWidth = 22f,
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 4, 4)
            };

        public static GUIStyle UnicodeIconStyle => _unicodeIconStyle ??=
            new GUIStyle(Graphics.DefaultSkin.FindStyle("Text"))
            {
                font = Graphics.UnicodeSymbolsFont,
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter,
                padding = new RectOffset(0, 0, 0, 0),
                /*normal = new GUIStyleState()
                    {
                        background = Graphics.BlackTexture,
                        textColor = Color.white
                    },
                    onNormal = new GUIStyleState()
                    {
                        background = Graphics.BlackTexture,
                        textColor = Color.white
                    }*/
            };

        public static GUIStyle UnicodeLinkStyle => _unicodeLinkStyle ??= new GUIStyle(DefaultLinkStyle)
        {
            alignment = TextAnchor.MiddleCenter,
            font = Graphics.UnicodeSymbolsFont,
            fontSize = 16,
            margin = new RectOffset(0, 0, 4, 0),
            padding = new RectOffset(8, 8, 0, 0),
            fixedHeight = 26f,
            normal = new GUIStyleState()
            {
                background = DefaultLinkStyle.normal.background,
                // background = Utils.BlackTexture,
                textColor = Color.white
            }
        };

        public static GUIStyle UnicodeLabelStyle => _unicodeLabelStyle ??= new GUIStyle(DefaultLinkStyle)
        {
            alignment = TextAnchor.MiddleCenter,
            // alignment = TextAnchor.LowerCenter,
            font = Graphics.UnicodeSymbolsFont,
            fontSize = 16,
            margin = new RectOffset(0, 0, 0, 0),
            padding = new RectOffset(8, 8, 0, 0),
            fixedHeight = 32f,
            normal = new GUIStyleState()
            {
                background = DefaultLinkStyle.normal.background,
                // background = Utils.BlackTexture,
                textColor = Color.white
            }
        };

        public static GUIStyle SectionHeaderToggleAreaStyle => _bgSectionHeaderStyle ??= new GUIStyle(Graphics
            .DefaultSkin
            .FindStyle("Link"))
        {
            alignment = TextAnchor.MiddleRight,
            font = Graphics
                .UnicodeSymbolsFont,
            fontSize = 12,
            margin = new RectOffset(0, 0, 0, 0),
            padding = new RectOffset(0, 0, 0, 0),
            border = new RectOffset(0, 0, 0, 0),
            overflow = new RectOffset(0, 0, 0, 0),
            // fixedHeight = 26f,
            normal = new GUIStyleState()
            {
                background = Alpha65WhitePixel,
                textColor = WhiteTextColor
            },
            hover = new GUIStyleState()
            {
                background = Alpha80WhitePixel,
                textColor = BlueTextColor
            }
        };

        public static GUIStyle InlineUnicodeButtonStyle => _inlineUnicodeButtonStyle ??=
            new GUIStyle(DefaultLinkStyle)
            {
                font = Graphics.UnicodeSymbolsFont,
                name = "InlineUnicodeButton",
                fontSize = 16,
                fontStyle = FontStyle.Normal,
                margin = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(0, 0, 0, 0),
                stretchWidth = false,
                stretchHeight = false,
                alignment = TextAnchor.MiddleCenter,
                normal = new GUIStyleState()
                {
                    background = DefaultLinkStyle.normal.background,
                    textColor = Color.white
                },
                onNormal = new GUIStyleState()
                {
                    background = DefaultLinkStyle.normal.background,
                    textColor = BlueTextColor
                },
                onHover = new GUIStyleState()
                {
                    background = Alpha35WhitePixel,
                    textColor = Color.white
                },
                hover = new GUIStyleState()
                {
                    background = Alpha35WhitePixel,
                    textColor = BlueTextColor
                },
                active = new GUIStyleState()
                {
                    background = DefaultLinkStyle.normal.background,
                    textColor = BlueTextColor
                },
            };
    }
}
