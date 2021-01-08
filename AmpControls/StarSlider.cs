using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AmpControls
{
    [DefaultEvent(nameof(ValueChanged))]
    public partial class StarSlider : UserControl, ISliderBase
    {
        public StarSlider()
        {
            InitializeComponent();
            Size = new Size(176, 35);
        }

        // a user changes the per-song based volume; set the volume and update it to the database..
        private void pnStars0_MouseClick(object sender, MouseEventArgs e)
        {
            var multiply = Width / 2d;
            var value = MaximumValue / 2d;

            if (sender == pnStars0)
            {
                CurrentValue = (int) (value * (e.X / multiply));
                pnStars1.Left = e.X;
            }
            else if (sender == pnStars1)
            {
                if (pnStars1 != null)
                {
                    pnStars1.Left += e.X;
                    CurrentValue = (int) (value * (pnStars1.Left / multiply));
                }
            }
        }

        private int currentValue;

        /// <summary>
        /// Gets or sets the current value of the slider.
        /// </summary>
        /// <value>The current value of the slider.</value>
        public int CurrentValue { 
            get => currentValue;
            set
            {
                pnStars1.Left = (int)((double)value / MaximumValue * Width);
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

        /// <summary>
        /// Occurs when the slider value has changed.
        /// </summary>
        public event EventHandler<SliderValueChangedEventArgs> ValueChanged;

        private void StarSlider_SizeChanged(object sender, EventArgs e)
        {
            SizeChanged -= StarSlider_SizeChanged;
            Size = new Size(176, 35);
            SizeChanged += StarSlider_SizeChanged;
        }
    }
}
