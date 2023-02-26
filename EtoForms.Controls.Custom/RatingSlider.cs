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
using EtoForms.Controls.Custom.Interfaces.BaseClasses;
using EtoForms.Controls.Custom.Properties;
using EtoForms.Controls.Custom.Utilities;
using FluentIcons.Resources.Filled;

namespace EtoForms.Controls.Custom;

/// <summary>
/// A slider control to give a rating to something.
/// Implements the <see cref="SliderBase" />
/// </summary>
/// <seealso cref="SliderBase" />
public class RatingSlider : SliderBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RatingSlider"/> class.
    /// </summary>
    public RatingSlider()
    {
        sliderImageSvg = Size16.ic_fluent_star_16_filled;
        ColorSlider = Colors.Orange;
    }

    private bool isRatingDefined = true;
    private byte[]? sliderImageUndefinedSvg;
    private Image? sliderImageUndefined;
    private Color colorSliderUndefined = Colors.Orange;


    /// <summary>
    /// Gets or sets the undefined value slider image color.
    /// </summary>
    /// <value>The undefined value slider image color.</value>
    public Color ColorSliderUndefined
    {
        get => colorSliderUndefined;

        set
        {
            if (value != colorSliderUndefined)
            {
                colorSliderUndefined = value;
                sliderImageUndefined = null;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is rating defined.
    /// </summary>
    /// <value><c>true</c> if this instance is rating defined; otherwise, <c>false</c>.</value>
    public bool IsRatingDefined 
    { 
        get => isRatingDefined;
        set
        {
            if (value != isRatingDefined)
            {
                isRatingDefined = value;
                Invalidate();
            }
        }
    }

    internal Image SliderImageUndefined
    {
        get
        {
            if (sliderImageUndefined == null)
            {
                sliderImageUndefined = EtoHelpers.ImageFromSvg(colorSliderUndefined,
                    SliderImageUndefinedSvg, RestAreaSize);
            }

            return sliderImageUndefined;
        }
    }


    /// <summary>
    /// Gets or sets the SVG image for the slider area when the value is indicated as <see cref="IsRatingDefined"/>=<c>false</c>.
    /// </summary>
    /// <value>The SVG image for the slide area when the value is undefined.</value>
    public byte[] SliderImageUndefinedSvg
    {
        get => sliderImageUndefinedSvg ?? Resources.line_horizontal;

        set
        {
            if (sliderImageUndefinedSvg?.SequenceEqual(value) != true)
            {
                sliderImageUndefinedSvg = value;
                sliderImageUndefined = null;
                Invalidate();
            }
        }
    }

    /// <inheritdoc cref="SliderBase.PaintControl"/>
    protected override void PaintControl(Graphics graphics, RectangleF paintRectangle)
    {
        var wh = Math.Min(Width - sliderPadding.Size.Width, Height - sliderPadding.Size.Height);
        var drawRect = new Size(wh, wh);

        var sliderColor = isRatingDefined ? ColorSlider : ColorSliderUndefined;
        var imageSvg = isRatingDefined ? SliderImageSvg : SliderImageUndefinedSvg;

        using var drawImage = EtoHelpers.ImageFromSvg(sliderColor, imageSvg, drawRect);
        graphics.FillRectangle(BackgroundColor, paintRectangle);

        var drawCount = (int)Math.Ceiling(((double)Width - sliderPadding.Size.Width) / wh);

        var width = sliderPadding.Size.Width;
        var left = (float)(currentValue / (maximum - minimum)) * (MouseArea.Width);
        left += width / 2f;


        graphics.SetClip(new RectangleF(0, 0, left + 1, Height));

        for (var i = 0; i < drawCount; i++)
        {
            graphics.DrawImage(drawImage, new PointF(i * wh + sliderPadding.Left, sliderPadding.Top));
        }


        graphics.DrawLine(ColorSliderMarker, new PointF(left, 0), new PointF(left, Height));
    }
}