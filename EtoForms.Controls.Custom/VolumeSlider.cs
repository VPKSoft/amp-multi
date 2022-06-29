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
using Eto.Forms;
using EtoForms.Controls.Custom.EventArguments;
using EtoForms.Controls.Custom.Properties;
using EtoForms.Controls.Custom.Utilities;

namespace EtoForms.Controls.Custom;

/// <summary>
/// A volume slider control for <see cref="Eto.Forms"/>.
/// Implements the <see cref="Drawable" />
/// </summary>
/// <seealso cref="Drawable" />
public class VolumeSlider : Drawable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VolumeSlider"/> class.
    /// </summary>
    public VolumeSlider()
    {
        Paint += VolumeSlider_Paint;
        SizeChanged += VolumeSlider_SizeChanged;
        MouseDown += VolumeSlider_MouseDown;
        MouseUp += VolumeSlider_MouseUp;
        MouseLeave += VolumeSlider_MouseUp;
        MouseMove += OnMouseMove;
        previousSize = base.Size;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VolumeSlider"/> class.
    /// <param name="valueChanged">Event handler for the <see cref="ValueChanged"/> event.</param>
    /// </summary>
    public VolumeSlider(EventHandler<ValueChangedEventArgs> valueChanged) : this()
    {
        ValueChanged += valueChanged;
    }

    #region PrivateFields
    private double minimum;
    private double maximum = 100;
    private double currentValue;
    private Padding sliderPadding = new(10, 5);
    private bool mouseDown;
    private int sliderWidth = 10;
    private Color colorSpeaker = Colors.SteelBlue;
    private Color colorSlider = Colors.Teal;
    private Color colorSliderMarker = Colors.Navy;
    private byte[]? speakerImageSvg;
    private byte[]? sliderImageSvg;
    private byte[]? sliderMarkerImageSvg;
    private Size previousSize;
    private Image? speakerImage;
    private Image? sliderImage;
    private Image? sliderMarkImage;
    #endregion

    /// <summary>
    /// Occurs when the <see cref="Value"/> property value has been changed.
    /// </summary>
    public event EventHandler<ValueChangedEventArgs>? ValueChanged;

    #region PublicProperties
    /// <summary>
    /// Gets or sets the minimum value for the slider.
    /// </summary>
    /// <value>The minimum value for the slider.</value>
    public double Minimum
    {
        get => minimum;

        set
        {
            if (Math.Abs(value - minimum) > Globals.FloatingPointTolerance)
            {
                if (maximum < value)
                {
                    maximum = value + maximum - minimum;
                }

                minimum = value;
                if (minimum < currentValue)
                {
                    currentValue = minimum;
                }

                Invalidate();
            }
        }
    }

    /// <summary>
    /// Gets or sets the maximum value for the slider.
    /// </summary>
    /// <value>The maximum value for the slider.</value>
    public double Maximum
    {
        get => maximum;

        set
        {
            if (Math.Abs(maximum - value) > Globals.FloatingPointTolerance)
            {
                if (value < minimum)
                {
                    minimum -= maximum - minimum;
                    if (currentValue > maximum)
                    {
                        currentValue = maximum;
                    }

                    currentValue = value;
                }

                maximum = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether suspend raising the <see cref="ValueChanged"/> event.
    /// </summary>
    /// <value><c>true</c> if suspend the <see cref="ValueChanged"/> event invocation; otherwise, <c>false</c>.</value>
    public bool SuspendEventInvocation { get; set; }

    /// <summary>
    /// Gets or sets the current slider position value.
    /// </summary>
    /// <value>The current slider position value.</value>
    public double Value
    {
        get => currentValue;

        set
        {
            if (Math.Abs(currentValue - value) > Globals.FloatingPointTolerance)
            {
                if (value < minimum)
                {
                    currentValue = minimum;
                }

                if (value > maximum)
                {
                    currentValue = maximum;
                }

                currentValue = value;

                Invalidate();

                if (!SuspendEventInvocation)
                {
                    ValueChanged?.Invoke(this, new ValueChangedEventArgs { Value = Value, });
                }
            }
        }
    }

    /// <summary>
    /// Gets or sets the slider padding.
    /// </summary>
    /// <value>The slider padding.</value>
    public Padding SliderPadding
    {
        get => sliderPadding;

        set
        {
            if (!sliderPadding.Equals(value))
            {
                sliderPadding = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Gets or sets the width of the value slider.
    /// </summary>
    /// <value>The width of the value slider.</value>
    public int SliderWidth
    {
        get => sliderWidth;

        set
        {
            if (value != sliderWidth)
            {
                sliderWidth = value;
                CreateSliderMark();
                Invalidate();
            }
        }
    }

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
    /// Gets or sets the slider image color.
    /// </summary>
    /// <value>The slider image color.</value>
    public Color ColorSlider
    {
        get => colorSlider;

        set
        {
            if (value != colorSlider)
            {
                colorSlider = value;
                sliderImage = null;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Gets or sets the slider marker image color.
    /// </summary>
    /// <value>The slider marker image color.</value>
    public Color ColorSliderMarker
    {
        get => colorSliderMarker;

        set
        {
            if (value != colorSliderMarker)
            {
                colorSliderMarker = value;
                sliderMarkImage = null;
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
        get => speakerImageSvg ?? FluentIcons.Resources.Filled.Size16.ic_fluent_speaker_2_16_filled;

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

    /// <summary>
    /// Gets or sets the SVG image for the slide area.
    /// </summary>
    /// <value>The SVG image for the slide area.</value>
    public byte[] SliderImageSvg
    {
        get => sliderImageSvg ?? Resources.volume_slider;

        set
        {
            if (sliderImageSvg?.SequenceEqual(value) != true)
            {
                sliderImageSvg = value;
                sliderImage = null;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Gets or sets the SVG image for the slider marker.
    /// </summary>
    /// <value>The SVG image for the slider marker.</value>
    public byte[] SliderMarkerImageSvg
    {
        get => sliderMarkerImageSvg ?? Resources.slider_mark;

        set
        {
            if (sliderMarkerImageSvg?.SequenceEqual(value) != true)
            {
                sliderMarkerImageSvg = value;
                sliderMarkImage = null;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// Gets or sets the color of the background.
    /// </summary>
    /// <value>The color of the background.</value>
    /// <remarks>Note that on some platforms (e.g. Mac), setting the background color of a control can change the performance
    /// characteristics of the control and its children, since it must enable layers to do so.</remarks>
    public new Color BackgroundColor
    {
        get => base.BackgroundColor;

        set
        {
            if (value != base.BackgroundColor)
            {
                base.BackgroundColor = value;
                Invalidate();
            }
        }
    }
    #endregion

    #region PrivateProperties
    private RectangleF MouseArea =>
        new(SquareSize.Width + sliderPadding.Left, sliderPadding.Top, RestAreaSize.Width,
            RestAreaSize.Height);

    private Size SquareSize
    {
        get
        {
            var wh = Math.Min(Width, Height);
            return new Size(wh, wh);
        }
    }

    private Size RestAreaSize
    {
        get
        {
            var wh = Math.Min(Width, Height);
            return new Size(Width - wh - sliderPadding.Size.Width, Height - sliderPadding.Size.Height);
        }
    }

    private Image SpeakerImage
    {
        get
        {
            if (speakerImage == null)
            {
                speakerImage = EtoHelpers.ImageFromSvg(colorSpeaker,
                    speakerImageSvg ?? FluentIcons.Resources.Filled.Size16.ic_fluent_speaker_2_16_filled, SquareSize);
            }

            return speakerImage;
        }
    }

    private Image SliderImage
    {
        get
        {
            if (sliderImage == null)
            {
                sliderImage = EtoHelpers.ImageFromSvg(colorSlider,
                    Resources.volume_slider, RestAreaSize);
            }

            return sliderImage;
        }
    }

    private Image SliderMarkImage
    {
        get
        {
            if (sliderMarkImage == null)
            {
                CreateSliderMark();
            }

            return sliderMarkImage!;
        }
    }
    #endregion

    #region PrivateMethods
    private void CreateSliderMark()
    {
        var width = sliderPadding.Size.Width < sliderWidth ? sliderPadding.Size.Width : sliderWidth;

        sliderMarkImage =
            EtoHelpers.ImageFromSvg(colorSliderMarker, Resources.slider_mark, new Size(width, Height));
    }

    private void PaintControl(Graphics graphics, RectangleF paintRectangle)
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
    private void OnMouseMove(object? sender, MouseEventArgs e)
    {
        UpdateValueFromPoint(e.Location);
    }

    private void UpdateValueFromPoint(PointF location)
    {
        if (mouseDown && MouseArea.Contains(location))
        {
            var value = (location.X - MouseArea.Left) / MouseArea.Width * (maximum - minimum);
            Value = value;
        }
    }

    private void VolumeSlider_MouseUp(object? sender, MouseEventArgs e)
    {
        mouseDown = false;
    }

    private void VolumeSlider_MouseDown(object? sender, MouseEventArgs e)
    {
        mouseDown = e.Buttons == MouseButtons.Primary;
        UpdateValueFromPoint(e.Location);
    }

    private void VolumeSlider_SizeChanged(object? sender, EventArgs e)
    {
        var sizeChanged = previousSize.Equals(base.Size);
        if (sizeChanged)
        {
            speakerImage?.Dispose();
            speakerImage = EtoHelpers.ImageFromSvg(Colors.SteelBlue,
                FluentIcons.Resources.Filled.Size16.ic_fluent_speaker_2_16_filled, SquareSize);

            sliderImage?.Dispose();
            sliderImage = EtoHelpers.ImageFromSvg(Colors.Teal, Resources.volume_slider, RestAreaSize);

            CreateSliderMark();
        }
    }

    private void VolumeSlider_Paint(object? sender, PaintEventArgs e)
    {
        PaintControl(e.Graphics, e.ClipRectangle);
    }
    #endregion
}