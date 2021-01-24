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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpControls
{
    /// <summary>
    /// Provides access to the images used with the class controls.
    /// </summary>
    public class Images
    {
        /// <summary>
        /// Gets the image representing a high audio volume.
        /// </summary>
        /// <value>Gets the image representing a high audio volume.</value>
        public static Image VolumeHigh => Properties.Resources.volume_high;

        /// <summary>
        /// Gets the image representing a low audio volume.
        /// </summary>
        /// <value>Gets the image representing a low audio volume.</value>
        public static Image VolumeLow => Properties.Resources.volume_small;

        /// <summary>
        /// Gets the image representing a volume slider.
        /// </summary>
        /// <value>The image representing a volume slider.</value>
        public static Image VolumeSlider => Properties.Resources.volume_slide_2;

        /// <summary>
        /// Gets the image representing stars.
        /// </summary>
        /// <value>The image representing stars.</value>
        public Image ImageStars => Properties.Resources.stars;

        /// <summary>
        /// Gets the image representing the audio volume.
        /// </summary>
        /// <value>The image representing the audio volume.</value>
        public Image ImageVolume => Properties.Resources.volume_slider;
    }
}
