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
    /// A limited slider control to allow a user to specify a volume for a single audio file.
    /// Implements the <see cref="System.Windows.Forms.UserControl" />
    /// Implements the <see cref="AmpControls.ISliderBase" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    /// <seealso cref="AmpControls.ISliderBase" />
    [DefaultEvent(nameof(ValueChanged))]
    public partial class VolumeSliderStatic : UserControl, ISliderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeSliderStatic"/> class.
        /// </summary>
        public VolumeSliderStatic()
        {
            InitializeComponent();
        }

        // ensure no scaling..
        private void VolumeSliderStatic_SizeChanged(object sender, EventArgs e)
        {
            SizeChanged -= VolumeSliderStatic_SizeChanged;
            Size = new Size(100, 35);
            SizeChanged += VolumeSliderStatic_SizeChanged;
        }

        private int currentValue;

        /// <summary>
        /// Gets or sets the current value of the slider.
        /// </summary>
        /// <value>The current value of the slider.</value>
        public int CurrentValue
        {
            get => currentValue;

            set
            {
                currentValue = value;

                if ((int) currentValueFractional != value)
                {
                    currentValueFractional = value;
                }

                if ((int) CurrentValueFractional != value)
                {
                    currentValueFractional = value;
                }

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
        /// Gets or sets the image representing the audio volume.
        /// </summary>
        /// <value>The image representing the audio volume.</value>
        [Category("Appearance")]
        [Description("The image representing the audio volume.")]
        public Image ImageVolume
        {
            get => pnVol1.BackgroundImage;
            set => pnVol1.BackgroundImage = value;
        }

        /// <summary>
        /// Occurs when the slider value has changed.
        /// </summary>
        public event EventHandler<SliderValueChangedEventArgs> ValueChanged;

        private void pnVol1_MouseClick(object sender, MouseEventArgs e)
        {
            currentValueFractional = 1;
            if (sender == pnVol1)
            {
                pnVol2.Left = e.X;
                currentValueFractional = MaximumValue * (e.X / (double)Width);
                CurrentValue = (int) currentValueFractional;
            }
            else if (sender == pnVol2)
            {
                if (pnVol2 != null)
                {
                    pnVol2.Left += e.X;
                    currentValueFractional = MaximumValue * (pnVol2.Left / (double)Width);
                    CurrentValue = (int) currentValueFractional;
                }
            }
        }
    }
}
