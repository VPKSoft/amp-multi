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
using System.Linq;
using Eto.Drawing;
using EtoForms.Controls.Custom.EventArguments;
using EtoForms.Controls.Custom.Interfaces.BaseClasses;
using EtoForms.Controls.Custom.Properties;
using EtoForms.Controls.Custom.Utilities;
using FluentIcons.Resources.Filled;

namespace EtoForms.Controls.Custom;

/// <summary>
/// A volume slider control for <see cref="Eto.Forms"/>.
/// Implements the <see cref="SliderBase" />
/// </summary>
/// <seealso cref="SliderBase" />
public class VolumeSlider : SliderBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VolumeSlider"/> class.
    /// </summary>
    public VolumeSlider()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VolumeSlider"/> class.
    /// <param name="valueChanged">Event handler for the <see cref="SliderBase.ValueChanged"/> event.</param>
    /// </summary>
    public VolumeSlider(EventHandler<ValueChangedEventArgs> valueChanged) : this()
    {
        ValueChanged += valueChanged;
    }

    #region PrivateFields
    private Color colorSpeaker = Colors.SteelBlue;
    private byte[]? speakerImageSvg;
    private Image? speakerImage;
    #endregion

    #region PublicProperties
    /// <summary>
    /// Gets or sets the speaker image color.
    /// </summary>
    /// <value>The speaker image color.</value>
    public Color ColorSpeaker
    {
        get => colorSpeaker;

        set
        {
            if (value != colorSpeaker)
            {
                colorSpeaker = value;
                speakerImage = null;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Gets or sets the SVG image for a speaker.
    /// </summary>
    /// <value>The SVG image for a speaker.</value>
    public byte[] SpeakerImageSvg
    {
        get => speakerImageSvg ?? Size16.ic_fluent_speaker_2_16_filled;

        set
        {
            if (speakerImageSvg?.SequenceEqual(value) != true)
            {
                speakerImageSvg = value;
                speakerImage = null;
                Invalidate();
            }
        }
    }
    #endregion

    #region PrivateProperties    
    /// <inheritdoc cref="SliderBase.SquareSize"/>
    protected override Size SquareSize
    {
        get
        {
            var wh = Math.Min(Width, Height);
            return new Size(wh, wh);
        }
    }

    private Image SpeakerImage
    {
        get
        {
            if (speakerImage == null)
            {
                speakerImage = EtoHelpers.ImageFromSvg(colorSpeaker,
                    speakerImageSvg ?? Size16.ic_fluent_speaker_2_16_filled, SquareSize);
            }

            return speakerImage;
        }
    }
    #endregion

    #region PrivateMethods    
    /// <summary>
    /// Paints the control.
    /// </summary>
    /// <param name="graphics">The graphics.</param>
    /// <param name="paintRectangle">The paint rectangle.</param>
    protected override void PaintControl(Graphics graphics, RectangleF paintRectangle)
    {
        graphics.FillRectangle(BackgroundColor, paintRectangle);
        var wh = Math.Min(Width, Height);
        graphics.DrawImage(SpeakerImage, PointF.Empty);
        graphics.DrawImage(SliderImage, new PointF(wh + sliderPadding.Left, sliderPadding.Top));

        var width = sliderPadding.Size.Width < sliderWidth ? sliderPadding.Size.Width : sliderWidth;
        var left = wh + (float)(currentValue / (maximum - minimum)) * (MouseArea.Width + width / 2f);
        left += width / 2f;

        graphics.DrawImage(SliderMarkImage, new PointF(left, 0));
    }
    #endregion

    #region InternalEvents
    internal override void Slider_SizeChanged(object? sender, EventArgs e)
    {
        var sizeChanged = previousSize.Equals(base.Size);
        if (sizeChanged)
        {
            speakerImage?.Dispose();
            speakerImage = EtoHelpers.ImageFromSvg(Colors.SteelBlue,
                Size16.ic_fluent_speaker_2_16_filled, SquareSize);

            sliderImage?.Dispose();
            sliderImage = EtoHelpers.ImageFromSvg(Colors.Teal, Resources.volume_slider, RestAreaSize);

            CreateSliderMark();
        }
    }
    #endregion
}