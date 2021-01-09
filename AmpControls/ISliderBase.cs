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
        /// Gets or sets the current value with fractional value included.
        /// </summary>
        /// <value>The current value with fractional value included.</value>
        [Browsable(false)]
        double CurrentValueFractional { get; set; }

        /// <summary>
        /// Occurs when the slider value has changed.
        /// </summary>
        [Category("Behaviour")]
        [Description("An event which occurs when the slider value has changed.")]
        event EventHandler<SliderValueChangedEventArgs> ValueChanged;
    }
}
