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

using System;
using Eto.Drawing;
using EtoForms.Controls.Custom.Interfaces.BaseClasses;
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

    /// <inheritdoc cref="SliderBase.PaintControl"/>
    protected override void PaintControl(Graphics graphics, RectangleF paintRectangle)
    {
        var wh = Math.Min(Width - sliderPadding.Size.Width, Height - sliderPadding.Size.Height);
        var drawRect = new Size(wh, wh);

        using var drawImage = EtoHelpers.ImageFromSvg(ColorSlider, SliderImageSvg, drawRect);
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