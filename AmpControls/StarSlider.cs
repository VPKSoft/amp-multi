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
using System.Windows.Forms;

namespace AmpControls
{
    /// <summary>
    /// A limited slider control to allow a user to specify amount of stars (=points) to something within a scale of zero to five.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// Implements the <see cref="AmpControls.ISliderBase" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    /// <seealso cref="AmpControls.ISliderBase" />
    [DefaultEvent(nameof(ValueChanged))]
    public partial class StarSlider : UserControl, ISliderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StarSlider"/> class.
        /// </summary>
        public StarSlider()
        {
            InitializeComponent();
            Size = new Size(176, 35);
        }

        // a user changes the per-song based volume; set the volume and update it to the database..
        private void pnStars0_MouseClick(object sender, MouseEventArgs e)
        {
            var multiply = Width / 2d;
            currentValueFractional = MaximumValue / 2d;

            if (sender == pnStars0)
            {
                currentValueFractional = currentValueFractional * (e.X / multiply);
                CurrentValue = (int) currentValueFractional;
                pnStars1.Left = e.X;
            }
            else if (sender == pnStars1)
            {
                if (pnStars1 != null)
                {
                    pnStars1.Left += e.X;
                    currentValueFractional = currentValueFractional * (pnStars1.Left / multiply);
                    CurrentValue = (int) (currentValueFractional);
                }
            }
        }

        private int currentValue;

        /// <summary>
        /// Gets or sets the image representing stars.
        /// </summary>
        /// <value>The the image representing stars.</value>
        [Category("Appearance")]
        [Description("The the image representing stars.")]
        public Image ImageStars
        {
            get => pnStars0.BackgroundImage;
            set => pnStars0.BackgroundImage = value;
        }

        /// <summary>
        /// Gets or sets the current value of the slider.
        /// </summary>
        /// <value>The current value of the slider.</value>
        public int CurrentValue { 
            get => currentValue;
            set
            {
                pnStars1.Left = (int)((double)value / MaximumValue * Width);

                if ((int) CurrentValueFractional != value)
                {
                    currentValueFractional = value;
                }

                this.currentValue = value;
                ValueChanged?.Invoke(this, new SliderValueChangedEventArgs(this));
            }
        }

        /// <summary>
        /// Gets or sets the minimum value of the slider.
        /// </summary>
        /// <value>The minimum value of the slider.</value>
        public int MinimumValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum value of the slider.
        /// </summary>
        /// <value>The maximum value of the slider.</value>
        public int MaximumValue { get; set; }

        // a field for the CurrentValueFractional property..
        private double currentValueFractional;

        /// <summary>
        /// Gets the current value with fractional value included.
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

        // ensure no scaling..
        private void StarSlider_SizeChanged(object sender, EventArgs e)
        {
            SizeChanged -= StarSlider_SizeChanged;
            Size = new Size(176, 35);
            SizeChanged += StarSlider_SizeChanged;
        }
    }
}
