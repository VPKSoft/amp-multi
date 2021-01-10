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
