using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AnN3x.ModdingLib
{
    internal class Colors
    {
        public static Colorable[] Values =
        {
            new Colorable("#F0F8FF0C"),
            new Colorable("#FAEBD7"),
            new Colorable(Color.cyan.ToString())
        };

        public static Colorable AliceBlue => Values[0];
        public static Colorable AntiqueWhite => Values[1];
        public static Colorable Cyan => Values[2];

    }

    public readonly struct Colorable
    {
        private readonly string hex;
        // private readonly Color rgb;

        public Colorable(string color)
        {
            hex = color;
            // rgb = color.ToColor();
        }

        public static implicit operator string(Colorable c) => c.hex;
        public static explicit operator Colorable(string s) => new Colorable(s);

        public override string ToString() => hex;

    }

}
