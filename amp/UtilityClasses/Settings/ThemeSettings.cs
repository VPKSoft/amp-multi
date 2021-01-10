﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using ReaLTaiizor.Helper;
using ReaLTaiizor.Interface.Crown;
using VPKSoft.Utils;
using VPKSoft.Utils.XmlSettingsMisc;
using Image = System.Drawing.Image;

namespace amp.UtilityClasses.Settings
{
    /// <summary>
    /// A class to store user theme settings.
    /// </summary>
    public class ThemeSettings: XmlSettings
    {
        /// <summary>
        /// Gets or sets the name of the default theme file.
        /// </summary>
        /// <value>The name of the default theme file.</value>
        internal static string DefaultFileName { get; set;
        } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            Assembly.GetExecutingAssembly()?.GetName().Name ?? @"amp#") + "\\" +
            ((Assembly.GetExecutingAssembly()?.GetName().Name ?? @"amp#") + @"_theme.xml");

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeSettings"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file containing the theme settings.</param>
        public ThemeSettings(string fileName)
        {
            RequestTypeConverter += themeSettings_RequestTypeConverter;
            Load(fileName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeSettings"/> class.
        /// </summary>
        /// <param name="theme">The theme to use as a base for the class colors.</param>
        public ThemeSettings(ITheme theme)
        {
            RequestTypeConverter += themeSettings_RequestTypeConverter;
            GreyBackground = theme.Colors.GreyBackground;
            HeaderBackground = theme.Colors.HeaderBackground;
            BlueBackground = theme.Colors.BlueBackground;
            DarkBlueBackground = theme.Colors.DarkBlueBackground;
            DarkBackground = theme.Colors.DarkBackground;
            LightBackground = theme.Colors.LightBackground;
            LighterBackground = theme.Colors.LighterBackground;
            LightestBackground = theme.Colors.LightestBackground;
            LightBorder = theme.Colors.LightBorder;
            MediumBackground = theme.Colors.MediumBackground;
            DarkBorder = theme.Colors.DarkBorder;
            LightText = theme.Colors.LightText;
            DisabledText = theme.Colors.DisabledText;
            BlueHighlight = theme.Colors.BlueHighlight;
            BlueSelection = theme.Colors.BlueSelection;
            GreyHighlight = theme.Colors.GreyHighlight;
            GreySelection = theme.Colors.GreySelection;
            DarkGreySelection = theme.Colors.DarkGreySelection;
            DarkBlueBorder = theme.Colors.DarkBlueBorder;
            LightBlueBorder = theme.Colors.LightBlueBorder;
            ActiveControl = theme.Colors.ActiveControl;
            PlaybackPrevious = Properties.Resources.amp_back;
            PlaybackNext = Properties.Resources.amp_forward;
            PlaybackPause = Properties.Resources.amp_pause;
            PlaybackPlay = Properties.Resources.amp_play;
            PlaybackShuffle = Properties.Resources.amp_shuffle;
            PlaybackRepeat = Properties.Resources.amp_repeat;
            PlaybackStackQueue = Properties.Resources.stack;
            PlaybackShowQueue = Properties.Resources.amp_queue;
            SongStars = Properties.Resources.stars;
            PlaybackSongVolumeStart = Properties.Resources.volume_small;
            PlaybackSongVolumeEnd = Properties.Resources.volume_high;
            PlaybackMainVolumeStart = Properties.Resources.volume_small;
            PlaybackMainVolumeEnd = Properties.Resources.volume_high;
            PlaybackSongVolumeTracker = Properties.Resources.volume_slide_2;
            PlaybackMainVolumeTracker = Properties.Resources.volume_slide_2;
        }

        /// <summary>
        /// Gets the default light theme.
        /// </summary>
        /// <value>The default light theme.</value>
        public static ThemeSettings DefaultThemeLight => new ThemeSettings(new CrownHelper.LightTheme());

        /// <summary>
        /// Gets the default dark theme.
        /// </summary>
        /// <value>The default dark theme.</value>
        public static ThemeSettings DefaultThemeDark => new ThemeSettings(new CrownHelper.DarkTheme());

        private void themeSettings_RequestTypeConverter(object sender, RequestTypeConverterEventArgs e)
        {
            if (e.TypeToConvert == typeof(Color))
            {
                e.TypeConverter = new ColorConverter();
            }
        }

        #region Colors
        /// <summary>
        /// Gets or sets the grey background.
        /// </summary>
        /// <value>The grey background.</value>
        [IsSetting]
        public Color GreyBackground { get; set; } = Color.FromArgb(220, 223, 225);

        /// <summary>
        /// Gets or sets the header background.
        /// </summary>
        /// <value>The header background.</value>
        [IsSetting]
        public Color HeaderBackground { get; set; } = Color.FromArgb(210, 213, 215);

        /// <summary>
        /// Gets or sets the blue background.
        /// </summary>
        /// <value>The blue background.</value>
        [IsSetting]
        public Color BlueBackground { get; set; } = Color.FromArgb(200, 203, 205);

        /// <summary>
        /// Gets or sets the dark blue background.
        /// </summary>
        /// <value>The dark blue background.</value>
        [IsSetting]
        public Color DarkBlueBackground { get; set; } = Color.FromArgb(190, 193, 195);

        /// <summary>
        /// Gets or sets the dark background.
        /// </summary>
        /// <value>The dark background.</value>
        [IsSetting]
        public Color DarkBackground { get; set; } = Color.FromArgb(225, 228, 231);

        /// <summary>
        /// Gets or sets the medium background.
        /// </summary>
        /// <value>The medium background.</value>
        [IsSetting]
        public Color MediumBackground { get; set; } = Color.FromArgb(215, 218, 221);

        /// <summary>
        /// Gets or sets the light background.
        /// </summary>
        /// <value>The light background.</value>
        [IsSetting]
        public Color LightBackground { get; set; } = Color.FromArgb(192, 193, 194);

        /// <summary>
        /// Gets or sets the lighter background.
        /// </summary>
        /// <value>The lighter background.</value>
        [IsSetting]
        public Color LighterBackground { get; set; } = Color.FromArgb(182, 183, 184);

        /// <summary>
        /// Gets or sets the lightest background.
        /// </summary>
        /// <value>The lightest background.</value>
        [IsSetting]
        public Color LightestBackground { get; set; } = Color.FromArgb(128, 128, 128);

        /// <summary>
        /// Gets or sets the light border.
        /// </summary>
        /// <value>The light border.</value>
        [IsSetting]
        public Color LightBorder { get; set; } = Color.FromArgb(201, 201, 201);

        /// <summary>
        /// Gets or sets the dark border.
        /// </summary>
        /// <value>The dark border.</value>
        [IsSetting]
        public Color DarkBorder { get; set; } = Color.FromArgb(171, 171, 171);

        /// <summary>
        /// Gets or sets the light text.
        /// </summary>
        /// <value>The light text.</value>
        [IsSetting]
        public Color LightText { get; set; } = Color.FromArgb(50, 50, 50);

        /// <summary>
        /// Gets or sets the disabled text.
        /// </summary>
        /// <value>The disabled text.</value>
        [IsSetting]
        public Color DisabledText { get; set; } = Color.FromArgb(113, 113, 113);

        /// <summary>
        /// Gets or sets the blue highlight.
        /// </summary>
        /// <value>The blue highlight.</value>
        [IsSetting]
        public Color BlueHighlight { get; set; } = Color.DodgerBlue;

        /// <summary>
        /// Gets or sets the blue selection.
        /// </summary>
        /// <value>The blue selection.</value>
        [IsSetting]
        public Color BlueSelection { get; set; } = Color.FromArgb(75, 110, 175);

        /// <summary>
        /// Gets or sets the grey highlight.
        /// </summary>
        /// <value>The grey highlight.</value>
        [IsSetting]
        public Color GreyHighlight { get; set; } = Color.FromArgb(182, 188, 182);

        /// <summary>
        /// Gets or sets the grey selection.
        /// </summary>
        /// <value>The grey selection.</value>
        [IsSetting]
        public Color GreySelection { get; set; } = Color.FromArgb(160, 160, 160);

        /// <summary>
        /// Gets or sets the dark grey selection.
        /// </summary>
        /// <value>The dark grey selection.</value>
        [IsSetting]
        public Color DarkGreySelection { get; set; } = Color.FromArgb(202, 202, 202);

        /// <summary>
        /// Gets or sets the dark blue border.
        /// </summary>
        /// <value>The dark blue border.</value>
        [IsSetting]
        public Color DarkBlueBorder { get; set; } = Color.FromArgb(171, 181, 198);

        /// <summary>
        /// Gets or sets the light blue border.
        /// </summary>
        /// <value>The light blue border.</value>
        [IsSetting]
        public Color LightBlueBorder { get; set; } = Color.FromArgb(206, 217, 114);

        /// <summary>
        /// Gets or sets the active control.
        /// </summary>
        /// <value>The active control.</value>
        [IsSetting]
        public Color ActiveControl { get; set; } = Color.FromArgb(159, 178, 196);

        /// <summary>
        /// Gets or sets the start color of the main volume slider.
        /// </summary>
        /// <value>The start color of the main volume slider.</value>
        [IsSetting]
        public Color MainVolumeStartColor { get; set; } = Color.Yellow;

        /// <summary>
        /// Gets or sets the end color of the main volume slider.
        /// </summary>
        /// <value>The end color of the main volume slider.</value>
        [IsSetting]
        public Color MainVolumeEndColor { get; set; } = Color.OrangeRed;

        /// <summary>
        /// Gets or sets the start color of the per-song volume slider.
        /// </summary>
        /// <value>The start color of the per-song volume slider.</value>
        [IsSetting]
        public Color SongVolumeStartColor { get; set; } = Color.FromArgb(110, 21, 164);

        /// <summary>
        /// Gets or sets the end color of the per-song volume slider.
        /// </summary>
        /// <value>The end color of the per-song volume slider.</value>
        [IsSetting]
        public Color SongVolumeEndColor { get; set; } = Color.FromArgb(249, 1, 7);
        #endregion

        #region ImageHelpers
        /// <summary>
        /// Sets a property value as base64 encoded string with the specified property name and an <see cref="Image"/> instance.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="propertyName">Name of the property.</param>
        private void FromImage(Image image, string propertyName)
        {
            var propertyInfo = GetType().GetProperty(propertyName);

            if (image == null)
            {
                propertyInfo?.SetValue(this, null);
                return;
            }

            using var memoryStream = new MemoryStream();

            image.Save(memoryStream, ImageFormat.Png);

            var value = Convert.ToBase64String(memoryStream.ToArray());

            propertyInfo?.SetValue(this, value);
        }

        /// <summary>
        /// Converts the specified base64 encoded string property value to an <see cref="Image"/> instance.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Image.</returns>
        private Image ToImage(string propertyName)
        {
            var propertyInfo = GetType().GetProperty(propertyName);

            string imageData = (string)propertyInfo?.GetValue(this);

            if (imageData == null)
            {
                return null;
            }

            return Image.FromStream(new MemoryStream(Convert.FromBase64String(imageData)));
        }
        #endregion

        #region ImageData
        /// <summary>
        /// Gets or sets the playback previous button image data as a base64 encoded string.
        /// </summary>
        /// <value>The playback previous button image data as a base64 encoded string.</value>
        [IsSetting]
        public string PlaybackPreviousImageData { get; set; } 

        /// <summary>
        /// Gets or sets the playback next button image data as a base64 encoded string.
        /// </summary>
        /// <value>The playback next button image data as a base64 encoded string.</value>
        [IsSetting]
        public string PlaybackNextImageData { get; set; }

        /// <summary>
        /// Gets or sets the playback pause button image data as a base64 encoded string.
        /// </summary>
        /// <value>The playback pause button image data as a base64 encoded string.</value>
        [IsSetting]
        public string PlaybackPauseImageData { get; set; }        

        /// <summary>
        /// Gets or sets the playback play button image data as a base64 encoded string.
        /// </summary>
        /// <value>The playback play button image data as a base64 encoded string.</value>
        [IsSetting]
        public string PlaybackPlayImageData { get; set; }

        /// <summary>
        /// Gets or sets the playback shuffle button image data as a base64 encoded string.
        /// </summary>
        /// <value>The playback shuffle button image data as a base64 encoded string.</value>
        [IsSetting]
        public string PlaybackShuffleImageData { get; set; }

        /// <summary>
        /// Gets or sets the playback repeat button image data as a base64 encoded string.
        /// </summary>
        /// <value>The playback repeat button image data as a base64 encoded string.</value>
        [IsSetting]
        public string PlaybackRepeatImageData { get; set; }

        /// <summary>
        /// Gets or sets the playback stack queue button image data as a base64 encoded string.
        /// </summary>
        /// <value>The playback stack queue button image data as a base64 encoded string.</value>
        [IsSetting]
        public string PlaybackStackQueueImageData { get; set; } 

        /// <summary>
        /// Gets or sets the playback show queue button image data as a base64 encoded string.
        /// </summary>
        /// <value>The playback show queue button image data as a base64 encoded string.</value>
        [IsSetting]
        public string PlaybackShowQueueImageData { get; set; }   

        /// <summary>
        /// Gets or sets the data for an image for a slider for giving stars to a song as a base64 encoded string.
        /// </summary>
        /// <value>The data for an image for a slider for giving stars to a song as a base64 encoded string.</value>
        [IsSetting]
        public string SongStarsImageData { get; set; }

        /// <summary>
        /// Gets or sets the playback song volume slider's left side button image data as a base64 encoded string.
        /// </summary>
        /// <value>The playback song volume slider's left side button image data as a base64 encoded string.</value>
        [IsSetting]
        public string PlaybackSongVolumeStartImageData { get; set; }

        /// <summary>
        /// Gets or sets the playback song volume slider's right side button image data as a base64 encoded string.
        /// </summary>
        /// <value>The playback song volume slider's right side button image data as a base64 encoded string.</value>
        [IsSetting]
        public string PlaybackSongVolumeEndImageData { get; set; }

        /// <summary>
        /// Gets or sets the playback main volume slider's left side button image data as a base64 encoded string.
        /// </summary>
        /// <value>The playback main volume slider's left side button image data as a base64 encoded string.</value>
        [IsSetting]
        public string PlaybackMainVolumeStartImageData { get; set; }     

        /// <summary>
        /// Gets or sets the playback main volume slider's right side button image data as a base64 encoded string.
        /// </summary>
        /// <value>The playback main volume slider's right side button image data as a base64 encoded string.</value>
        [IsSetting]
        public string PlaybackMainVolumeEndImageData { get; set; }

        /// <summary>
        /// Gets or sets the playback song volume slider's tracker image data as a base64 encoded string.
        /// </summary>
        /// <value>The playback song volume slider's tracker image data as a base64 encoded string.</value>
        [IsSetting]
        public string PlaybackSongVolumeTrackerImageData { get; set; }

        /// <summary>
        /// Gets or sets the playback main volume slider's tracker image data as a base64 encoded string.
        /// </summary>
        /// <value>The playback main volume slider's tracker image data as a base64 encoded string.</value>
        [IsSetting]
        public string PlaybackMainVolumeTrackerImageData { get; set; }   
        #endregion

        #region Images        
        /// <summary>
        /// Gets or sets the image for playback previous button.
        /// </summary>
        /// <value>The image for playback previous button.</value>
        public Image PlaybackPrevious 
        {
            get => ToImage(nameof(PlaybackPreviousImageData));
            set => FromImage(value, nameof(PlaybackPreviousImageData));
        }

        /// <summary>
        /// Gets or sets the image for playback next button.
        /// </summary>
        /// <value>The image for playback next button.</value>
        public Image PlaybackNext
        {
            get => ToImage(nameof(PlaybackNextImageData));
            set => FromImage(value, nameof(PlaybackNextImageData));
        }

        /// <summary>
        /// Gets or sets the image for playback pause button.
        /// </summary>
        /// <value>The image for playback pause button.</value>
        public Image PlaybackPause
        {
            get => ToImage(nameof(PlaybackPauseImageData));
            set => FromImage(value, nameof(PlaybackPauseImageData));
        }

        /// <summary>
        /// Gets or sets the image for playback play button.
        /// </summary>
        /// <value>The image for playback play button.</value>
        public Image PlaybackPlay
        {
            get => ToImage(nameof(PlaybackPlayImageData));
            set => FromImage(value, nameof(PlaybackPlayImageData));
        }

        /// <summary>
        /// Gets or sets the image for playback shuffle button.
        /// </summary>
        /// <value>The image for playback shuffle button.</value>
        public Image PlaybackShuffle
        {
            get => ToImage(nameof(PlaybackShuffleImageData));
            set => FromImage(value, nameof(PlaybackShuffleImageData));
        }

        /// <summary>
        /// Gets or sets the image for playback repeat button.
        /// </summary>
        /// <value>The image for playback repeat button.</value>
        public Image PlaybackRepeat
        {
            get => ToImage(nameof(PlaybackRepeatImageData));
            set => FromImage(value, nameof(PlaybackRepeatImageData));
        }

        /// <summary>
        /// Gets or sets the image for playback stack queue button.
        /// </summary>
        /// <value>The image for playback stack queue button.</value>
        public Image PlaybackStackQueue
        {
            get => ToImage(nameof(PlaybackStackQueueImageData));
            set => FromImage(value, nameof(PlaybackStackQueueImageData));
        }

        /// <summary>
        /// Gets or sets the image for playback show queue button.
        /// </summary>
        /// <value>The image for playback show queue button.</value>
        public Image PlaybackShowQueue
        {
            get => ToImage(nameof(PlaybackShowQueueImageData));
            set => FromImage(value, nameof(PlaybackShowQueueImageData));
        }

        /// <summary>
        /// Gets or sets the image for a slider for giving stars to a song.
        /// </summary>
        /// <value>The image for a slider for giving stars to a song.</value>
        public Image SongStars
        {
            get => ToImage(nameof(SongStarsImageData));
            set => FromImage(value, nameof(SongStarsImageData));
        }

        /// <summary>
        /// Gets or sets the image for playback song volume slider's left side button.
        /// </summary>
        /// <value>The image for playback song volume slider's left side button.</value>
        public Image PlaybackSongVolumeStart
        {
            get => ToImage(nameof(PlaybackSongVolumeStartImageData));
            set => FromImage(value, nameof(PlaybackSongVolumeStartImageData));
        }

        /// <summary>
        /// Gets or sets the image for playback song volume slider's right side button.
        /// </summary>
        /// <value>The image for playback song volume slider's right side button.</value>
        public Image PlaybackSongVolumeEnd
        {
            get => ToImage(nameof(PlaybackSongVolumeEndImageData));
            set => FromImage(value, nameof(PlaybackSongVolumeEndImageData));
        }

        /// <summary>
        /// Gets or sets the image for playback main volume slider's left side button.
        /// </summary>
        /// <value>The image for playback main volume slider's left side button.</value>
        public Image PlaybackMainVolumeStart
        {
            get => ToImage(nameof(PlaybackMainVolumeStartImageData));
            set => FromImage(value, nameof(PlaybackMainVolumeStartImageData));
        }

        /// <summary>
        /// Gets or sets the image for playback main volume slider's right side button.
        /// </summary>
        /// <value>The image for playback main volume slider's right side button.</value>
        public Image PlaybackMainVolumeEnd
        {
            get => ToImage(nameof(PlaybackMainVolumeEndImageData));
            set => FromImage(value, nameof(PlaybackMainVolumeEndImageData));
        }

        /// <summary>
        /// Gets or sets the image for playback song volume slider's tracker.
        /// </summary>
        /// <value>The image for playback song volume slider's tracker.</value>
        public Image PlaybackSongVolumeTracker
        {
            get => ToImage(nameof(PlaybackSongVolumeTrackerImageData));
            set => FromImage(value, nameof(PlaybackSongVolumeTrackerImageData));
        }

        /// <summary>
        /// Gets or sets the image for playback main volume slider's tracker.
        /// </summary>
        /// <value>The image for playback main volume slider's tracker.</value>
        public Image PlaybackMainVolumeTracker
        {
            get => ToImage(nameof(PlaybackMainVolumeTrackerImageData));
            set => FromImage(value, nameof(PlaybackMainVolumeTrackerImageData));
        }
        #endregion
    }
}
