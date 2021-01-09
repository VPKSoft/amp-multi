using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpControls
{
    /// <summary>
    /// Event arguments for the <see cref="ISliderBase.ValueChanged"/> event.
    /// Implements the <see cref="System.EventArgs" />
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class SliderValueChangedEventArgs: EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SliderValueChangedEventArgs"/> class.
        /// </summary>
        public SliderValueChangedEventArgs()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SliderValueChangedEventArgs"/> class.
        /// </summary>
        /// <param name="slider">An object instance implementing the <see cref="ISliderBase"/> interface.</param>
        public SliderValueChangedEventArgs(ISliderBase slider)
        {
            CurrentValue = slider.CurrentValue;
            MinimumValue = slider.MinimumValue;
            MaximumValue = slider.MaximumValue;
            CurrentValueFractional = slider.CurrentValueFractional;
        }

        /// <summary>
        /// Gets or sets the current value with fractional value included.
        /// </summary>
        /// <value>The current value with fractional value included.</value>
        public double CurrentValueFractional { get; set; }

        /// <summary>
        /// Gets or sets the current value of the slider.
        /// </summary>
        /// <value>The current value of the slider.</value>
        public int CurrentValue { get; set; }

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
    }
}
