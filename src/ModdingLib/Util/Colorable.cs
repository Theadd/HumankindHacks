using UnityEngine;

namespace AnN3x.ModdingLib;

public readonly struct Colorable
{
    private readonly string hex;
    private readonly Color rgb;

    public Colorable(string color)
    {
        hex = color;
        rgb = color.ToColor();
    }
    
    public Colorable(Color color)
    {
        rgb = color;
        hex = "#" + color.ToHex();
    }

    public Colorable gamma => new Colorable(rgb.gamma);
    public Colorable grayscale => new Colorable(new Color(rgb.grayscale, rgb.grayscale, rgb.grayscale));
    public Colorable linear => new Colorable(rgb.linear);
    
    // Color AlphaMultiplied(float multiplier) => new Color(this.r, this.g, this.b, this.a * multiplier);
    
    public static implicit operator string(Colorable c) => c.hex;
    public static explicit operator Colorable(string s) => new Colorable(s);
    public static implicit operator Color(Colorable c) => c.rgb;
    public static explicit operator Colorable(Color c) => new Colorable(c);

    public override string ToString() => hex;
}
