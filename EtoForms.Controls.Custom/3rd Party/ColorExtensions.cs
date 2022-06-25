#region License
/*
MIT License

Copyright(c) 2022 Petteri Kautonen

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion

// Actual copyright, CC BY 4.0 (C): https://github.com/sanjayatpilcrow/SharpSnippets/blob/master/POCs/POCs/Sanjay/SharpSnippets/Drawing/ColorExtensions.cs
// https://github.com/sanjayatpilcrow
// https://sharpsnippets.wordpress.com/author/sharpsnippets/
// https://github.com/sanjayatpilcrow/SharpSnippets
// ReSharper disable All
using System;
using Eto.Drawing;

#pragma warning disable CS1591
namespace POCs.Sanjay.SharpSnippets.Drawing;

public static class ColorExtensions
{
    static Random _randomizer = new Random();
    public static Color GetContrast(this Color Source, bool PreserveOpacity)
    {
        Color inputColor = Source;
        //if RGB values are close to each other by a diff less than 10%, then if RGB values are lighter side, decrease the blue by 50% (eventually it will increase in conversion below), if RBB values are on darker side, decrease yellow by about 50% (it will increase in conversion)
        byte avgColorValue = (byte)((Source.R + Source.G + Source.B) / 3);
        int diff_r = Math.Abs(Source.Rb - avgColorValue);
        int diff_g = Math.Abs(Source.Gb - avgColorValue);
        int diff_b = Math.Abs(Source.Bb - avgColorValue);
        if (diff_r < 20 && diff_g < 20 && diff_b < 20) //The color is a shade of gray
        {
            if (avgColorValue < 123) //color is dark
            {
                inputColor = Color.FromArgb(220, 230, 50, Source.Ab);
            }
            else
            {
                inputColor = Color.FromArgb(255, 255, 50, Source.Ab);
            }
        }
        byte sourceAlphaValue = (byte)Source.Ab;
        if (!PreserveOpacity)
        {
            sourceAlphaValue = Math.Max((byte)Source.Ab, (byte)127); //We don't want contrast color to be more than 50% transparent ever.
        }
        RGB rgb = new RGB { R = inputColor.Rb, G = inputColor.Gb, B = inputColor.Bb, };
        HSB hsb = ConvertToHSB(rgb);
        hsb.H = hsb.H < 180 ? hsb.H + 180 : hsb.H - 180;
        //_hsb.B = _isColorDark ? 240 : 50; //Added to create dark on light, and light on dark
        rgb = ConvertToRGB(hsb);
        return Color.FromArgb((int)rgb.R, (int)rgb.G, (int)rgb.B, (int)sourceAlphaValue);
    }

    #region Code from MSDN
    internal static RGB ConvertToRGB(HSB hsb)
    {
        // Following code is taken as it is from MSDN. See link below.
        // By: <a href="http://blogs.msdn.com/b/codefx/archive/2012/02/09/create-a-color-picker-for-windows-phone.aspx" title="MSDN" target="_blank">Yi-Lun Luo</a>
        double chroma = hsb.S * hsb.B;
        double hue2 = hsb.H / 60;
        double x = chroma * (1 - Math.Abs(hue2 % 2 - 1));
        double r1 = 0d;
        double g1 = 0d;
        double b1 = 0d;
        if (hue2 >= 0 && hue2 < 1)
        {
            r1 = chroma;
            g1 = x;
        }
        else if (hue2 >= 1 && hue2 < 2)
        {
            r1 = x;
            g1 = chroma;
        }
        else if (hue2 >= 2 && hue2 < 3)
        {
            g1 = chroma;
            b1 = x;
        }
        else if (hue2 >= 3 && hue2 < 4)
        {
            g1 = x;
            b1 = chroma;
        }
        else if (hue2 >= 4 && hue2 < 5)
        {
            r1 = x;
            b1 = chroma;
        }
        else if (hue2 >= 5 && hue2 <= 6)
        {
            r1 = chroma;
            b1 = x;
        }
        double m = hsb.B - chroma;
        return new RGB()
        {
            R = r1 + m,
            G = g1 + m,
            B = b1 + m,
        };
    }
    internal static HSB ConvertToHSB(RGB rgb)
    {
        // Following code is taken as it is from MSDN. See link below.
        // By: <a href="http://blogs.msdn.com/b/codefx/archive/2012/02/09/create-a-color-picker-for-windows-phone.aspx" title="MSDN" target="_blank">Yi-Lun Luo</a>
        double r = rgb.R;
        double g = rgb.G;
        double b = rgb.B;

        double max = Max(r, g, b);
        double min = Min(r, g, b);
        double chroma = max - min;
        double hue2 = 0d;
        if (chroma != 0)
        {
            if (max == r)
            {
                hue2 = (g - b) / chroma;
            }
            else if (max == g)
            {
                hue2 = (b - r) / chroma + 2;
            }
            else
            {
                hue2 = (r - g) / chroma + 4;
            }
        }
        double hue = hue2 * 60;
        if (hue < 0)
        {
            hue += 360;
        }
        double brightness = max;
        double saturation = 0;
        if (chroma != 0)
        {
            saturation = chroma / brightness;
        }
        return new HSB()
        {
            H = hue,
            S = saturation,
            B = brightness,
        };
    }
    private static double Max(double d1, double d2, double d3)
    {
        if (d1 > d2)
        {
            return Math.Max(d1, d3);
        }
        return Math.Max(d2, d3);
    }
    private static double Min(double d1, double d2, double d3)
    {
        if (d1 < d2)
        {
            return Math.Min(d1, d3);
        }
        return Math.Min(d2, d3);
    }
    internal struct RGB
    {
        internal double R;
        internal double G;
        internal double B;
    }
    internal struct HSB
    {
        internal double H;
        internal double S;
        internal double B;
    }
    #endregion //Code from MSDN
}