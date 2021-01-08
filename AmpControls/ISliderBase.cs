using System;
using System.ComponentModel;

namespace AmpControls
{
    /// <summary>
    /// An interface for different slider properties.
    /// </summary>
    public interface ISliderBase
    {
        /// <summary>
        /// Gets or sets the current value of the slider.
        /// </summary>
        /// <value>The current value of the slider.</value>
        [Category("Behaviour")]
        [Description("The current value of the slider.")]
        int CurrentValue { get; set; }

        /// <summary>
        /// Gets or sets the minimum value of the slider.
        /// </summary>
        /// <value>The minimum value of the slider.</value>
        [Category("Behaviour")]
        [Description("The minimum value of the slider.")]
        int MinimumValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum value of the slider.
        /// </summary>
        /// <value>The maximum value of the slider.</value>
        [Category("Behaviour")]
        [Description("The maximum value of the slider.")]
        int MaximumValue { get; set; }

        /// <summary>
        /// Occurs when the slider value has changed.
        /// </summary>
        [Category("Behaviour")]
        [Description("An event which occurs when the slider value has changed.")]
        event EventHandler<SliderValueChangedEventArgs> ValueChanged;
    }
}
