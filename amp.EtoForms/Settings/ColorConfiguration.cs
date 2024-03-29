﻿#region License
/*
MIT License

Copyright(c) 2023 Petteri Kautonen

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

using amp.EtoForms.Settings.AttributeClasses;
using VPKSoft.ApplicationSettingsJson;
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global, the NuGet VPKSoft.ApplicationSettingsJson requires get<-->set-properties

namespace amp.EtoForms.Settings;

/// <summary>
/// A color configuration class for the amp#.
/// Implements the <see cref="ApplicationJsonSettings" />
/// </summary>
/// <seealso cref="ApplicationJsonSettings" />
// ReSharper disable once ClassNeverInstantiated.Global, this is instantiated by the VPKSoft.ApplicationSettingsJson 
public class ColorConfiguration : ApplicationJsonSettings
{
    ///// <summary>
    ///// Gets or sets the color of the window background.
    ///// </summary>
    ///// <value>The color of the window background.</value>
    //[Settings]
    //public string? WindowBackgroundColor { get; set; }

    ///// <summary>
    ///// Gets or sets the color of the window text.
    ///// </summary>
    ///// <value>The color of the window text.</value>
    //[Settings]
    //public string? WindowTextColor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the default color of the button image.
    /// </summary>
    /// <value>The default color of the button image.</value>
    [Settings(Default = "#4682B4")] // SteelBlue
    public string ButtonImageDefaultColor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color of the disabled button image.
    /// </summary>
    /// <value>The color of the disabled button image.</value>
    [Settings(Default = "#B6BCB6")] // Gray-ish
    [NoSyncColor]
    public string DisabledButtonImageColor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color of the menu item image.
    /// </summary>
    /// <value>The color of the menu item image.</value>
    [Settings(Default = "#4682B4")] // SteelBlue
    public string MenuItemImageColor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the alternate color of the menu item image.
    /// </summary>
    /// <value>The alternate color of the menu item image.</value>
    [Settings(Default = "#008080")] // Teal
    public string MenuItemImageAlternateColor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color of the play-pause button play state.
    /// </summary>
    /// <value>The color of the play-pause button play state.</value>
    [Settings(Default = "#800080")] // Purple
    public string PlayButtonPlayColor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color of the play-pause button pause state.
    /// </summary>
    /// <value>The color of the play-pause button pause state.</value>
    [Settings(Default = "#800080")] // Purple
    public string PlayButtonPauseColor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color of the jump to previous button.
    /// </summary>
    /// <value>The color of the jump to previous button.</value>
    [Settings(Default = "#008080")] // Teal
    public string PreviousButtonColor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color of the jump to next button.
    /// </summary>
    /// <value>The color of the jump to next button.</value>
    [Settings(Default = "#008080")] // Teal
    public string NextButtonColor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color of the show queue button.
    /// </summary>
    /// <value>The color of the show queue button.</value>
    [Settings(Default = "#502D16")] // Brown
    public string QueueButtonColor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color of the shuffle button.
    /// </summary>
    /// <value>The color of the shuffle button.</value>
    [Settings(Default = "#D4AA00")] // Orange-ish
    public string ShuffleButtonColor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color of the repeat button.
    /// </summary>
    /// <value>The color of the repeat button.</value>
    [Settings(Default = "#FF5555")] // Pink-ish
    public string RepeatButtonColor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color of the stack queue button.
    /// </summary>
    /// <value>The color of the stack queue button.</value>
    [Settings(Default = "#000080")] // Navy
    public string StackQueueButtonColor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color speaker main volume image.
    /// </summary>
    /// <value>The color speaker main volume image.</value>
    [Settings(Default = "#4682B4")] // SteelBlue
    public string ColorSpeakerMainVolume { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color main volume slider.
    /// </summary>
    /// <value>The color main volume slider.</value>
    [Settings(Default = "#6495ED")] // CornflowerBlue
    public string ColorMainVolumeSlider { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color main volume value indicator.
    /// </summary>
    /// <value>The color main volume value indicator.</value>
    [Settings(Default = "#000080")] // Navy
    public string ColorMainVolumeValueIndicator { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color speaker track volume image.
    /// </summary>
    /// <value>The color speaker track volume image.</value>
    [Settings(Default = "#4682B4")] // SteelBlue
    public string ColorSpeakerTrackVolume { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color track volume slider.
    /// </summary>
    /// <value>The color track volume slider.</value>
    [Settings(Default = "#008080")] // Teal
    public string ColorTrackVolumeSlider { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color track volume value indicator.
    /// </summary>
    /// <value>The color track volume value indicator.</value>
    [Settings(Default = "#000080")] // Navy
    public string ColorTrackVolumeValueIndicator { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color position slider.
    /// </summary>
    /// <value>The color position slider.</value>
    [Settings(Default = "#008080")] // Teal
    public string ColorPositionSlider { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color position slider value indicator.
    /// </summary>
    /// <value>The color position slider value indicator.</value>
    [Settings(Default = "#000080")] // Navy
    public string ColorPositionSliderValueIndicator { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color rating slider.
    /// </summary>
    /// <value>The color rating slider.</value>
    [Settings(Default = "#FFA500")] // Orange
    public string ColorRatingSlider { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color of the rating column image.
    /// </summary>
    /// <value>The color rating slider.</value>
    [Settings(Default = "#FFA500")] // Orange
    public string ColorRatingPlaylist { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color of the rating column image when the rating is undefined.
    /// </summary>
    /// <value>The color rating slider  when the rating is undefined.</value>
    [Settings(Default = "#B6BCB6")] // Gray-ish
    public string ColorRatingPlaylistUndefined { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color rating slider value indicator.
    /// </summary>
    /// <value>The color rating slider value indicator.</value>
    [Settings(Default = "#000080")] // Navy
    public string ColorRatingSliderValueIndicator { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color rating slider value indicator for undefined rating.
    /// </summary>
    /// <value>The color rating slider value indicator for undefined rating.</value>
    [Settings(Default = "#B6BCB6")] // Gray-ish
    public string ColorRatingSliderValueIndicatorUndefined { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color of the music note.
    /// </summary>
    /// <value>The color of the music note.</value>
    [Settings(Default = "#FFA500")] // Orange
    public string TheMusicNoteColor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color of the clear search button.
    /// </summary>
    /// <value>The color of the clear search button.</value>
    [Settings(Default = "#8B0000")] // DarkRed
    public string ClearSearchButtonColor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color tool button image.
    /// </summary>
    /// <value>The color tool button image.</value>
    [Settings(Default = "#4682B4")] // SteelBlue
    public string ColorToolButtonImage { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the color of a missing track image.
    /// </summary>
    /// <value>The color of a missing track image.</value>
    [Settings(Default = "#008080")] // Teal
    public string ColorTrackMissingImage { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the start color of left audio level bar.
    /// </summary>
    /// <value>The start color of left audio level bar.</value>
    [Settings(Default = "#4682B4")] // SteelBlue
    public string ColorLevelBarLeftStart { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the end color of left audio level bar.
    /// </summary>
    /// <value>The end color of left audio level bar.</value>
    [Settings(Default = "#000080")] // Navy
    public string ColorLevelBarLeftEnd { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the background color of left audio level bar.
    /// </summary>
    /// <value>The background color of left audio level bar.</value>
    [Settings(Default = "#000000")] // Black
    [NoSyncColor]
    public string ColorLevelBarLeftBackground { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the start color of right audio level bar.
    /// </summary>
    /// <value>The start color of right audio level bar.</value>
    [Settings(Default = "#4682B4")] // SteelBlue
    public string ColorLevelBarRightStart { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the end color of right audio level bar.
    /// </summary>
    /// <value>The end color of right audio level bar.</value>
    [Settings(Default = "#000080")] // Navy
    public string ColorLevelBarRightEnd { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the background color of right audio level bar.
    /// </summary>
    /// <value>The background color of right audio level bar.</value>
    [Settings(Default = "#000000")] // Black
    [NoSyncColor]
    public string ColorLevelBarRightBackground { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the background color for the <see cref="global::EtoForms.SpectrumVisualizer.SpectrumVisualizer"/> control.
    /// </summary>
    /// <value>The background color for the <see cref="global::EtoForms.SpectrumVisualizer.SpectrumVisualizer"/> control.</value>
    [Settings(Default = "#000000")] // Black
    [NoSyncColor]
    public string ColorSpectrumVisualizerBackground { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the colors for the <see cref="global::EtoForms.SpectrumVisualizer.SpectrumVisualizer"/> spectrum visualizer channels.
    /// </summary>
    /// <value>The colors spectrum visualizer channels.</value>
    [Settings(Default = new[] { "#191970", "#B0C4DE", "#FF00FF", "#9370DB", "#32CD32", "#00FF00", "#00BFFF", "#87CEFA", })]
    public string[] ColorsSpectrumVisualizerChannels { get; set; } = Array.Empty<string>();
}