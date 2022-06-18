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

using System.Globalization;

namespace amp.EtoForms.Utilities.SvgColorization;

/// <summary>
/// A struct representing a HTML-styled SVG color.
/// </summary>
public readonly struct SvgColor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SvgColor"/> struct.
    /// </summary>
    /// <param name="color">The color in hexadecimal representation, i.e. '#FF557E'.</param>
    public SvgColor(string color)
    {
        color = color.TrimStart('#').Trim();

        // Remove the alpha channel information.
        if (color.Length == 8)
        {
            color = color[2..];
        }

        R = byte.Parse(color[..2], NumberStyles.HexNumber);
        G = byte.Parse(color.Substring(2, 2), NumberStyles.HexNumber);
        B = byte.Parse(color.Substring(4, 2), NumberStyles.HexNumber);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SvgColor"/> struct.
    /// </summary>
    /// <param name="r">The red color component.</param>
    /// <param name="g">The green color component.</param>
    /// <param name="b">The blue color component.</param>
    public SvgColor(int r, int g, int b)
    {
        R = (byte)r;
        G = (byte)g;
        B = (byte)b;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SvgColor"/> struct.
    /// </summary>
    /// <param name="value">The object value which string representation is used to create a new <see cref="SvgColor"/> struct.</param>
    public SvgColor(object value) : this(value.ToString() ?? "#000000")
    {

    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="SvgColor"/>.
    /// </summary>
    /// <param name="value">The color in hexadecimal representation, i.e. '#FF557E'.</param>
    /// <returns>A new instance to a <see cref="SvgColor"/> class.</returns>
    public static implicit operator SvgColor(string value)
    {
        return new SvgColor(value);
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="System.ValueTuple{T1,T2,T3}"/> to <see cref="SvgColor"/>.
    /// </summary>
    /// <param name="value">The reg, green and blue color components.</param>
    /// <returns>A new instance to a <see cref="SvgColor"/> class.</returns>
    public static implicit operator SvgColor((int r, int g, int b) value)
    {
        var (r, g, b) = value;
        return new SvgColor(r, g, b);
    }

    /// <summary>
    /// Gets the red color component.
    /// </summary>
    /// <value>The red color.</value>
    public byte R { get; }

    /// <summary>
    /// Gets the green color component.
    /// </summary>
    /// <value>The green color component.</value>
    public byte G { get; }

    /// <summary>
    /// Gets the blue color component.
    /// </summary>
    /// <value>The blue color component.</value>
    public byte B { get; }

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
    {
        return $"#{R:X2}{G:X2}{B:X2}";
    }
}