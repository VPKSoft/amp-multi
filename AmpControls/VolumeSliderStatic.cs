using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AmpControls
{
    [DefaultEvent(nameof(ValueChanged))]
    public partial class VolumeSliderStatic : UserControl, ISliderBase
    {
        public VolumeSliderStatic()
        {
            InitializeComponent();
        }

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
