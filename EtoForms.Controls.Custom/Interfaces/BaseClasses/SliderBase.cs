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

namespace EtoForms.Controls.Custom.Interfaces.BaseClasses;

/// <summary>
/// A base class for custom slider controls.
/// Implements the <see cref="Drawable" />
/// </summary>
/// <seealso cref="Drawable" />
public abstract class SliderBase : Drawable
{
    #region PrivateFields
    // ReSharper disable four times InconsistentNaming
    internal double minimum;
    internal double maximum = 100;
    internal double currentValue;
    internal Padding sliderPadding = new(10, 5);
    private bool mouseDown;
    internal int sliderWidth = 10;
    private Color colorSlider = Colors.Teal;
    private Color colorSliderMarker = Colors.Navy;
    internal byte[]? sliderImageSvg;
    private byte[]? sliderMarkerImageSvg;
    internal Size previousSize;
    internal Image? sliderImage;
    private Image? sliderMarkImage;
    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="SliderBase"/> class.
    /// </summary>
    protected SliderBase()
    {
        Paint += Slider_Paint;
        SizeChanged += Slider_SizeChanged;
        MouseDown += Slider_MouseDown;
        MouseUp += Slider_MouseUp;
        MouseLeave += Slider_MouseUp;
        MouseMove += OnMouseMove;
        previousSize = base.Size;
    }

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
            if (mouseDown)
            {
                return;
            }

            UpdateValue(value);
        }
    }

    private void UpdateValue(double value)
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
    internal RectangleF MouseArea =>
        new(SquareSize.Width + sliderPadding.Left, sliderPadding.Top, RestAreaSize.Width,
            RestAreaSize.Height);

    /// <summary>
    /// Gets the size of and additional space taking square area.
    /// </summary>
    protected virtual Size SquareSize => new(0, 0);

    internal Size RestAreaSize
    {
        get
        {
            var wh = Math.Min(SquareSize.Width, SquareSize.Height);
            return new Size(Width - wh - sliderPadding.Size.Width, Height - sliderPadding.Size.Height);
        }
    }

    internal Image SliderImage
    {
        get
        {
            if (sliderImage == null)
            {
                sliderImage = EtoHelpers.ImageFromSvg(colorSlider,
                    SliderImageSvg, RestAreaSize);
            }

            return sliderImage;
        }
    }

    internal Image SliderMarkImage
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

    #region InternalMethods
    internal virtual void CreateSliderMark()
    {
        var width = sliderPadding.Size.Width < sliderWidth ? sliderPadding.Size.Width : sliderWidth;

        sliderMarkImage =
            EtoHelpers.ImageFromSvg(colorSliderMarker, Resources.slider_mark, new Size(width, Height));
    }

    /// <summary>
    /// Paints the control.
    /// </summary>
    /// <param name="graphics">The graphics.</param>
    /// <param name="paintRectangle">The paint rectangle.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    protected abstract void PaintControl(Graphics graphics, RectangleF paintRectangle);
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
            UpdateValue(value);
        }
    }

    private void Slider_MouseUp(object? sender, MouseEventArgs e)
    {
        mouseDown = false;
    }

    private void Slider_MouseDown(object? sender, MouseEventArgs e)
    {
        mouseDown = e.Buttons == MouseButtons.Primary;
        UpdateValueFromPoint(e.Location);
    }

    internal virtual void Slider_SizeChanged(object? sender, EventArgs e)
    {
        var sizeChanged = !previousSize.Equals(base.Size);
        if (sizeChanged)
        {
            previousSize = base.Size;
            sliderImage?.Dispose();
            sliderImage = EtoHelpers.ImageFromSvg(colorSlider, SliderImageSvg, RestAreaSize);

            CreateSliderMark();
        }
    }

    private void Slider_Paint(object? sender, PaintEventArgs e)
    {
        PaintControl(e.Graphics, e.ClipRectangle);
    }
    #endregion
}