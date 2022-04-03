﻿using AnN3x.ModdingLib;
using UnityEngine;

namespace AnN3x.StyledGUI
{
    public static partial class Styled
    {

        public static void Alert(string message) => Alert(message, Colors.DeepSkyBlue);

        public static void Alert(string message, Colorable color)
        {
            var prev = GUI.backgroundColor;
            GUI.backgroundColor = color;
            GUILayout.Label($"<color={color}>{message}</color>", Styles.AlertLabelStyle);
            GUI.backgroundColor = prev;
        }
    }
}
