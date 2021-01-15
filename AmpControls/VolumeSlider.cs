#region License
/*
MIT License

Copyright(c) 2021 Petteri Kautonen

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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AmpControls
{
    /// <summary>
    /// A scalable volume slider control for windows forms application.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// Implements the <see cref="AmpControls.ISliderBase" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    /// <seealso cref="AmpControls.ISliderBase" />
    [DefaultEvent(nameof(ValueChanged))]
    public partial class VolumeSlider : UserControl, ISliderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeSlider"/> class.
        /// </summary>
        public VolumeSlider()
        {
            InitializeComponent();
            base.DoubleBuffered = true;
        }

        // the "main" area of the control as it is custom-drawn..
        private void pnSlider_Paint(object sender, PaintEventArgs e)
        {
            bool ValidRectangle() => e.ClipRectangle.Width > 0 && e.ClipRectangle.Height > 0;

            // some times a zero-sized rectangle caused a crash..
            if (!ValidRectangle())
            {
                return;
            }

            // set the SmoothingMode property to smooth the line..
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using var backgroundBrush = new SolidBrush(BackColor);

            e.Graphics.FillRectangle(backgroundBrush, e.ClipRectangle);


            using var brush = new LinearGradientBrush(e.ClipRectangle, ColorMinimum, ColorMaximum,
                LinearGradientMode.Horizontal);

            e.Graphics.FillPolygon(brush, new[]
            {
                new PointF(e.ClipRectangle.X + imageHalf, e.ClipRectangle.Bottom),
                new PointF(e.ClipRectangle.Right - imageHalf, e.ClipRectangle.Bottom),
                new PointF(e.ClipRectangle.Right - imageHalf, e.ClipRectangle.Y),
            });

            var width = e.ClipRectangle.Width - imageWidth;

            var sliderPoint = (MaximumValue - CurrentValue) * (float) width / MaximumValue;

            var rectWidth = width - (int) sliderPoint;

            var rectangle = new Rectangle(e.ClipRectangle.X + rectWidth, e.ClipRectangle.Y,
                e.ClipRectangle.Width - rectWidth, e.ClipRectangle.Bottom);

            e.Graphics.FillRectangle(backgroundBrush, rectangle);

            rectWidth -= imageHalf;

            if (rectWidth < 0)
            {
                rectWidth = 0;
            }

            if (imageSliderTracker != null)
            {
                rectangle = new Rectangle(rectWidth, e.ClipRectangle.Y,
                    ImageSliderTracker.Width, e.ClipRectangle.Height);

                e.Graphics.DrawImage(ImageSliderTracker, rectangle);
            }
        }

        private int currentValue = 50;

        /// <summary>
        /// Gets or sets the current value of the slider.
        /// </summary>
        /// <value>The current value of the slider.</value>
        public int CurrentValue
        {
            get => currentValue;

            set
            {
                if (value == currentValue)
                {
                    pnSlider.Invalidate();
                    return;
                }

                currentValue = value;
                if ((int)CurrentValueFractional != value)
                {
                    CurrentValueFractional = value;
                }
                ValueChanged?.Invoke(this, new SliderValueChangedEventArgs(this));
                pnSlider.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the left side volume image.
        /// </summary>
        /// <value>The left side volume image.</value>
        [Category("Appearance")]
        [Description("The left side volume image.")]
        public Image ImageVolumeLeft
        {
            get => pnMainVolumeLeft.BackgroundImage;
            set => pnMainVolumeLeft.BackgroundImage = value;
        }

        /// <summary>
        /// Gets or sets the right side volume image.
        /// </summary>
        /// <value>The right side volume image.</value>
        [Category("Appearance")]
        [Description("The right side volume image.")]
        public Image ImageVolumeRight
        {
            get => pnMainVolumeRight.BackgroundImage;
            set => pnMainVolumeRight.BackgroundImage = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the left image should be visible.
        /// </summary>
        /// <value><c>true</c> if the left image should be visible; otherwise, <c>false</c>.</value>
        [Category("Appearance")]
        [Description("A value indicating if the left image should be visible.")]
        public bool LeftImageVisible
        {
            get => tlpVolumeSlider.ColumnStyles[0].Width > 0;
            set
            {
                tlpVolumeSlider.ColumnStyles[0].Width = value ? 32 : 0;

                pnMainVolumeLeft.Visible = value;
                pnSlider.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the right image should be visible.
        /// </summary>
        /// <value><c>true</c> if the right image should be visible; otherwise, <c>false</c>.</value>
        [Category("Appearance")]
        [Description("A value indicating if the left right should be visible.")]
        public bool RightImageVisible
        {
            get => tlpVolumeSlider.ColumnStyles[2].Width > 0;
            set
            {
                tlpVolumeSlider.ColumnStyles[2].Width = value ? 32 : 0;

                pnMainVolumeRight.Visible = value;
                pnSlider.Invalidate();
            }
        }

        private Image imageSliderTracker = Properties.Resources.volume_slide_2;
        private int imageHalf;
        private int imageWidth;

        /// <summary>
        /// Gets or sets the image to use as a volume slider.
        /// </summary>
        /// <value>The image to use as a volume slider.</value>
        [Category("Appearance")]
        [Description("The image to use as a volume slider.")]
        public Image ImageSliderTracker
        {
            get => imageSliderTracker;

            set
            {
                imageSliderTracker = value;
                pnSlider.Invalidate();

                if (imageSliderTracker != null)
                {
                    imageHalf = (int) (value.Width / 2d);
                    imageWidth = value.Width;
                    imageSliderTracker = value;
                    pnSlider.Invalidate();
                }
            }
        }

        private Color colorMinimum = Color.Yellow;

        /// <summary>
        /// Gets or sets the color indicating the minimum value for the slider.
        /// </summary>
        /// <value>The color indicating the minimum value for the slider.</value>
        [Category("Appearance")]
        [Description("The color indicating the minimum value for the slider.")]
        public Color ColorMinimum
        {
            get => colorMinimum;
            set
            {
                colorMinimum = value;
                pnSlider.Invalidate();
            }
        }

        private Color colorMaximum = Color.OrangeRed;


        /// <summary>
        /// Gets or sets the color indicating the maximum value for the slider.
        /// </summary>
        /// <value>The color indicating the maximum value for the slider.</value>
        [Category("Appearance")]
        [Description("The color indicating the maximum value for the slider.")]
        public Color ColorMaximum
        {
            get => colorMaximum;
            set
            {
                colorMaximum = value;
                pnSlider.Invalidate();
            }
        }

        private int minimumValue;

        /// <summary>
        /// Gets or sets the minimum value of the slider.
        /// </summary>
        /// <value>The minimum value of the slider.</value>
        public int MinimumValue
        {
            get => minimumValue;
            set
            {
                minimumValue = value;
                pnSlider.Invalidate();
            }
        }

        private int maximumValue = 100;

        /// <summary>
        /// Gets or sets the maximum value of the slider.
        /// </summary>
        /// <value>The maximum value of the slider.</value>
        public int MaximumValue
        {
            get => maximumValue;
            set
            {
                maximumValue = value;
                pnSlider.Invalidate();
            }
        }

        private double currentValueFractional = 50;

        /// <summary>
        /// Gets or sets the current value with fractional value included.
        /// </summary>
        /// <value>The current value with fractional value included.</value>
        public double CurrentValueFractional
        {
            get => currentValueFractional;
            set
            {
                currentValueFractional = value;
                CurrentValue = (int) value;
                pnSlider.Invalidate();
            }
        }

        /// <summary>
        /// Occurs when the slider value has changed.
        /// </summary>
        public event EventHandler<SliderValueChangedEventArgs> ValueChanged;

        /// <summary>
        /// Gets or set the value indicating whether the mouse button is down over the volume slider area.
        /// </summary>
        /// <value>The value indicating whether the mouse button is down over the volume slider area.</value>
        private bool MouseIsDown { get; set; }

        private void pnSlider_MouseDown(object sender, MouseEventArgs e)
        {
            MouseIsDown = true;
        }

        private void pnSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseIsDown && e.X >= pnSlider.ClientRectangle.X && e.X < pnSlider.ClientRectangle.Width - imageHalf)
            {
                if (e.Button == MouseButtons.Right)
                {
                    CurrentValue = MaximumValue / 2;
                    CurrentValueFractional = MaximumValue / 2.0;
                }
                else
                {
                    var width = pnSlider.ClientRectangle.Width - imageWidth;
                    CurrentValue = MaximumValue - (int)((width - (double)e.X) * MaximumValue / width);
                    CurrentValueFractional = MaximumValue - (width - (double) e.X) * MaximumValue / width;
                }
            }
        }

        private void pnSlider_MouseUp(object sender, MouseEventArgs e)
        {
            MouseIsDown = false;
        }

        private void pnSlider_MouseLeave(object sender, EventArgs e)
        {
            MouseIsDown = false;
        }

        private void pnMainVolume_Click(object sender, EventArgs e)
        {
            CurrentValue = sender.Equals(pnMainVolumeLeft) ? MinimumValue : MaximumValue;
        }
    }
}
