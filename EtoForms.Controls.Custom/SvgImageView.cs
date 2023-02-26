#region License
/*
MIT License

Copyright(c) 2023 Petteri Kautonen

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

using System;
using System.Linq;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom.SvgColorization;
using EtoForms.Controls.Custom.Utilities;

namespace EtoForms.Controls.Custom;

/// <summary>
/// A control to view an SVG image.
/// Implements the <see cref="Eto.Forms.Drawable" />
/// </summary>
/// <seealso cref="Eto.Forms.Drawable" />
public class SvgImageView : Drawable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SvgImageView"/> class.
    /// </summary>
    /// <param name="svgImageBytes">The SVG image data bytes.</param>
    /// <param name="svgColor">The color (fill/stroke) of the SVG.</param>
    public SvgImageView(byte[] svgImageBytes, Color svgColor)
    {
        SvgImageData = svgImageBytes;
        Paint += SvgImageView_Paint;
        SvgFillColor = SvgStrokeColor = svgColor;
    }

    private void SvgImageView_Paint(object? sender, PaintEventArgs e)
    {
        PaintImageView(e.Graphics, e.ClipRectangle);
    }

    /// <summary>
    /// Draws the SVG image to the specified graphics.
    /// </summary>
    /// <param name="graphics">The graphics to draw on to.</param>
    /// <param name="drawArea">The drawing area rectangle.</param>
    private void PaintImageView(Graphics graphics, RectangleF drawArea)
    {
        var size = (int)Math.Min(drawArea.Width, drawArea.Height);

        if (svgImageData?.Length > 0)
        {
            var svgData = SvgColorize.FromBytes(svgImageData)
                .ColorizeElementsFill(SvgElement.All, svgFillColor.ToString())
                .ColorizeElementsStroke(SvgElement.All, SvgStrokeColor.ToString());

            if (size > 0)
            {
                var image = SvgToImage.ImageFromSvg(svgData.ToBytes(), new Size(size, size));
                graphics.DrawImage(image, (drawArea.Left) / 2, (drawArea.Top) / 2);
            }
        }
    }

    /// <summary>
    /// Gets or sets the color of the SVG fill.
    /// </summary>
    /// <value>The color of the SVG fill.</value>
    public Color SvgFillColor
    {
        get => svgFillColor;

        set
        {
            if (svgFillColor != value)
            {
                svgFillColor = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Gets or sets the color of the SVG stroke.
    /// </summary>
    /// <value>The color of the SVG stroke.</value>
    public Color SvgStrokeColor
    {
        get => svgStrokeColor;

        set
        {
            if (svgStrokeColor != value)
            {
                svgStrokeColor = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Gets or sets the SVG image data bytes.
    /// </summary>
    /// <value>The SVG image data.</value>
    public byte[] SvgImageData
    {
        get => svgImageData ?? Array.Empty<byte>();

        set
        {
            if (svgImageData == null || !value.SequenceEqual(svgImageData))
            {
                svgImageData = value;
                Invalidate();
            }
        }
    }

    private byte[]? svgImageData;
    private Color svgFillColor;
    private Color svgStrokeColor;
}