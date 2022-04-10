using System.Globalization;
using UnityEngine;

namespace AnN3x.CoreLib;

public static class ColorsEx
{
    /// <summary>
    /// Converts Color to 6-digit RGB hex code (without # symbol) when Color has no alpha applied.
    /// Otherwise, to 8-digit RGBA hex code.
    /// Eg, RGBA(1, 0, 0, 1) -> FF0000
    /// Eg, RGBA(1, 0, 0, .5f) -> FF000080
    /// </summary>
    public static string ToHex(this Color color)
    {
        byte r = (byte) Mathf.Clamp(Mathf.RoundToInt(color.r * 255f), 0, 255);
        byte g = (byte) Mathf.Clamp(Mathf.RoundToInt(color.g * 255f), 0, 255);
        byte b = (byte) Mathf.Clamp(Mathf.RoundToInt(color.b * 255f), 0, 255);
        byte a = (byte) Mathf.Clamp(Mathf.RoundToInt(color.a * 255f), 0, 255);

        return a == 255 ? $"{r:X2}{g:X2}{b:X2}" : $"{r:X2}{g:X2}{b:X2}{a:X2}";
    }

    /// <summary>
    /// Assumes the string is a 6-digit or 8-digit RGB/RGBA Hex color code (with optional leading #) which it will parse into a UnityEngine.Color.
    /// </summary>
    public static Color ToColor(this string _string) =>
        _string.Length >= 8 ? ToColorRGBA(_string) : ToColorRGB(_string);

    /// <summary>
    /// Assumes the string is a 6-digit RGB Hex color code (with optional leading #) which it will parse into a UnityEngine.Color.
    /// Eg, FF0000 -> RGBA(1,0,0,1)
    /// </summary>
    public static Color ToColorRGB(string _string)
    {
        _string = _string.Replace("#", "");

        if (_string.Length != 6)
            return Color.magenta;

        var r = byte.Parse(_string.Substring(0, 2), NumberStyles.HexNumber);
        var g = byte.Parse(_string.Substring(2, 2), NumberStyles.HexNumber);
        var b = byte.Parse(_string.Substring(4, 2), NumberStyles.HexNumber);

        var color = new Color
        {
            r = (float) (r / (decimal) 255),
            g = (float) (g / (decimal) 255),
            b = (float) (b / (decimal) 255),
            a = 1
        };

        return color;
    }

    /// <summary>
    /// Assumes the string is a 8-digit RGBA Hex color code (with optional leading #) which it will parse into a UnityEngine.Color.
    /// Eg, FF000080 -> RGBA(1, 0, 0, .5f)
    /// </summary>
    public static Color ToColorRGBA(string _string)
    {
        _string = _string.Replace("#", "");

        if (_string.Length != 8)
            return Color.magenta;

        var r = byte.Parse(_string.Substring(0, 2), NumberStyles.HexNumber);
        var g = byte.Parse(_string.Substring(2, 2), NumberStyles.HexNumber);
        var b = byte.Parse(_string.Substring(4, 2), NumberStyles.HexNumber);
        var a = byte.Parse(_string.Substring(6, 2), NumberStyles.HexNumber);

        var color = new Color
        {
            r = (float) (r / (decimal) 255),
            g = (float) (g / (decimal) 255),
            b = (float) (b / (decimal) 255),
            a = (float) (a / (decimal) 255)
        };

        return color;
    }
}
