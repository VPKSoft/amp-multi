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

using Eto.Drawing;

namespace amp.EtoForms.Classes;

/// <summary>
/// A class for user defined custom icons for the software GUI.
/// </summary>
public class IconData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IconData"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="description">The description.</param>
    /// <param name="image">The image.</param>
    public IconData(string name, string description, Image image)
    {
        Name = name;
        Description = description;
        Image = image;
    }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the image.
    /// </summary>
    /// <value>The image.</value>
    public Image Image { get; set; }

    /// <summary>
    /// Gets or sets the SVG data.
    /// </summary>
    /// <value>The SVG data.</value>
    public byte[] SvgData { get; set; } = Array.Empty<byte>();

    /// <summary>
    /// Gets or sets the color for the icon to override the color setting value.
    /// </summary>
    /// <value>The color for the icon to override the color setting value.</value>
    public string? OverrideColor { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the color of the SVG will not be adjusted.
    /// </summary>
    /// <value><c>true</c> if the color of the SVG is kept as is; otherwise, <c>false</c>.</value>
    public bool NoColorChange { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether icon represented by this instance is user customized.
    /// </summary>
    /// <value><c>true</c> if the icon represented by this instance is user customized; otherwise, <c>false</c>.</value>
    public bool IsCustom { get; set; }

    /// <summary>
    /// Gets or sets the display size of the image.
    /// </summary>
    /// <value>The display size of the image.</value>
    public Size ImageDisplaySize { get; set; }
}