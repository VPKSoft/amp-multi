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
using Eto.Drawing;
using EtoForms.Controls.Custom.EventArguments;
using EtoForms.Controls.Custom.Interfaces.BaseClasses;
using FluentIcons.Resources.Filled;

namespace EtoForms.Controls.Custom;

/// <summary>
/// A slider control with customizable graphics to display and adjust position.
/// Implements the <see cref="SliderBase" />
/// </summary>
/// <seealso cref="SliderBase" />
public class PositionSlider : SliderBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PositionSlider"/> class.
    /// </summary>
    public PositionSlider()
    {
        SliderImageSvg = Size16.ic_fluent_star_16_filled;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PositionSlider"/> class.
    /// <param name="valueChanged">Event handler for the <see cref="SliderBase.ValueChanged"/> event.</param>
    /// </summary>
    public PositionSlider(EventHandler<ValueChangedEventArgs> valueChanged) : this()
    {
        ValueChanged += valueChanged;
    }

    /// <inheritdoc cref="SliderBase.PaintControl"/>
    protected override void PaintControl(Graphics graphics, RectangleF paintRectangle)
    {
        graphics.FillRectangle(BackgroundColor, paintRectangle);
        graphics.DrawImage(SliderImage, new PointF(sliderPadding.Left, sliderPadding.Top));

        var width = sliderPadding.Size.Width < sliderWidth ? sliderPadding.Size.Width : sliderWidth;
        var left = (float)(currentValue / (maximum - minimum)) * (MouseArea.Width + width / 2f);
        left += width / 2f;

        graphics.DrawImage(SliderMarkImage, new PointF(left, 0));
    }
}