using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AmpControls
{
    [DefaultEvent(nameof(ValueChanged))]
    public partial class VolumeSlider : UserControl, ISliderBase
    {
        public VolumeSlider()
        {
            InitializeComponent();
            base.DoubleBuffered = true;
        }

        private void pnSlider_Paint(object sender, PaintEventArgs e)
        {
            // set the SmoothingMode property to smooth the line..
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using var backgroundBrush = new SolidBrush(BackColor);

            e.Graphics.FillRectangle(backgroundBrush, e.ClipRectangle);


            using var brush = new LinearGradientBrush(e.ClipRectangle, ColorMinimum, ColorMaximum, LinearGradientMode.Horizontal);

            e.Graphics.FillPolygon(brush, new[]
            {
                new PointF(e.ClipRectangle.X + imageHalf, e.ClipRectangle.Bottom),
                new PointF(e.ClipRectangle.Right - imageHalf, e.ClipRectangle.Bottom),
                new PointF(e.ClipRectangle.Right - imageHalf, e.ClipRectangle.Y),
            });

            var width = e.ClipRectangle.Width - imageWidth;

            var sliderPoint = (MaximumValue - CurrentValue) * (float) width / MaximumValue;

            var rectWidth = width - (int) sliderPoint;

            var rectangle = new Rectangle(e.ClipRectangle.X + rectWidth + imageHalf, e.ClipRectangle.Y,
                e.ClipRectangle.Width - rectWidth - imageHalf, e.ClipRectangle.Bottom);

            e.Graphics.FillRectangle(backgroundBrush, rectangle);

            sliderPoint += ImageSliderTracker.Width / 2f;
            rectWidth = e.ClipRectangle.Width - (int) sliderPoint;
            var positionMultiplier = ((double)(CurrentValue - MaximumValue) / MaximumValue);
//            rectWidth += (int)(positionMultiplier * imageHalf);
            rectWidth -= imageHalf;

            rectangle = new Rectangle(rectWidth, e.ClipRectangle.Y,
                ImageSliderTracker.Width, e.ClipRectangle.Height);

            e.Graphics.DrawImage(ImageSliderTracker, rectangle);
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
                if (imageSliderTracker != null)
                {
                    imageHalf = (int) (value.Width / 2d);
                    imageWidth = value.Width;
                    imageSliderTracker = value;
                    pnSlider.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the color indicating the minimum value for the slider.
        /// </summary>
        /// <value>The color indicating the minimum value for the slider.</value>
        [Category("Appearance")]
        [Description("The color indicating the minimum value for the slider.")]
        public Color ColorMinimum { get; set; } = Color.Yellow;

        /// <summary>
        /// Gets or sets the color indicating the maximum value for the slider.
        /// </summary>
        /// <value>The color indicating the maximum value for the slider.</value>
        [Category("Appearance")]
        [Description("The color indicating the maximum value for the slider.")]
        public Color ColorMaximum { get; set; } = Color.OrangeRed;

        /// <summary>
        /// Gets or sets the minimum value of the slider.
        /// </summary>
        /// <value>The minimum value of the slider.</value>
        public int MinimumValue { get; set; } = 0;

        /// <summary>
        /// Gets or sets the maximum value of the slider.
        /// </summary>
        /// <value>The maximum value of the slider.</value>
        public int MaximumValue { get; set; } = 100;

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
            if (MouseIsDown && e.X >= pnSlider.ClientRectangle.X + imageHalf && e.X < pnSlider.ClientRectangle.Width - imageHalf)
            {
                var width = pnSlider.ClientRectangle.Width;
                CurrentValue = MaximumValue - (int)((width - (double)(e.X + imageHalf)) * MaximumValue / width);
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
