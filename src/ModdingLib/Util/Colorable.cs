using UnityEngine;

namespace AnN3x.ModdingLib;

public readonly struct Colorable
{
    private readonly string hex;
    private readonly Color rgb;

    public Colorable(string color)
    {
        hex = color.Replace("#", "");;
        rgb = color.ToColor();
    }
    
    public Colorable(Color color)
    {
        rgb = color;
        hex = color.ToHex();
    }

    public Colorable(Colorable other, float alpha = 1f)
    {
        byte a = (byte)Mathf.Clamp(Mathf.RoundToInt(alpha * 255f), 0, 255);
        
        rgb = new Color(other.rgb.r, other.rgb.g, other.rgb.b, Mathf.Clamp01(alpha));
        hex = other.hex.Substring(0, 6) + $"{a:X2}";
    }

    public Colorable gamma => new Colorable(rgb.gamma);
    public Colorable grayscale => new Colorable(new Color(rgb.grayscale, rgb.grayscale, rgb.grayscale));
    public Colorable linear => new Colorable(rgb.linear);
    public Colorable alpha(float a) => new Colorable(this, a);

    public static implicit operator string(Colorable c) => c.ToString();
    public static explicit operator Colorable(string s) => new Colorable(s);
    public static implicit operator Color(Colorable c) => c.rgb;
    public static explicit operator Colorable(Color c) => new Colorable(c);

    public override string ToString() => Colors.PrefixWithHash ? $"#{hex}" : hex;
}
