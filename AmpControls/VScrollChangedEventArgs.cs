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

namespace AmpControls
{
    /// <summary>
    /// A class that is passed to the ListBoxExtension's VScrollChanged event.
    /// </summary>
    public class VScrollChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The current minimum value of the scroll bar position. Always 0.
        /// </summary>
        public int Minimum { get; set; }

        /// <summary>
        /// The current maximum value for the scroll bar position.
        /// </summary>
        public int Maximum { get; set; }

        /// <summary>
        /// The current value for the scroll bar position.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Indicates if the event was raised internally from the control or the VScrollPosition property's value was changed by code.
        /// </summary>
        public bool FromControl { get; set; }
    }
}
