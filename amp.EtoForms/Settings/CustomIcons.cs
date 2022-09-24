#region License
/*
MIT License

Copyright(c) 2022 Petteri Kautonen

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

using VPKSoft.ApplicationSettingsJson;

namespace amp.EtoForms.Settings;

/// <summary>
/// Custom icon settings for the software.
/// Implements the <see cref="ApplicationJsonSettings" />
/// </summary>
/// <seealso cref="ApplicationJsonSettings" />
public class CustomIcons : ApplicationJsonSettings
{
    /// <summary>
    /// Gets or sets the add music files icon image data.
    /// </summary>
    /// <value>The add music files icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? AddMusicFiles { get; set; }

    /// <summary>
    /// Gets or sets the album icon image data.
    /// </summary>
    /// <value>The album icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? Album { get; set; }

    /// <summary>
    /// Gets or sets the track information icon image data.
    /// </summary>
    /// <value>The track information icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? TrackInformation { get; set; }

    /// <summary>
    /// Gets or sets the quit application icon image data.
    /// </summary>
    /// <value>The quit application icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? QuitApplication { get; set; }

    /// <summary>
    /// Gets or sets the save current queue icon image data.
    /// </summary>
    /// <value>The save current queue icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? SaveCurrentQueue { get; set; }

    /// <summary>
    /// Gets or sets the saved queues icon image data.
    /// </summary>
    /// <value>The saved queues icon icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? SavedQueues { get; set; }

    /// <summary>
    /// Gets or sets the clear queue icon image data.
    /// </summary>
    /// <value>The clear queue icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? ClearQueue { get; set; }

    /// <summary>
    /// Gets or sets the scramble queue icon image data.
    /// </summary>
    /// <value>The scramble queue icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? ScrambleQueue { get; set; }

    /// <summary>
    /// Gets or sets the stash queue icon image data.
    /// </summary>
    /// <value>The stash queue icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? StashQueue { get; set; }

    /// <summary>
    /// Gets or sets the pop stashed queue icon image data.
    /// </summary>
    /// <value>The pop stashed queue icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? PopStashedQueue { get; set; }

    /// <summary>
    /// Gets or sets the settings icon image data.
    /// </summary>
    /// <value>The settings icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? Settings { get; set; }

    /// <summary>
    /// Gets or sets the color settings icon image data.
    /// </summary>
    /// <value>The color settings icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? ColorSettings { get; set; }

    /// <summary>
    /// Gets or sets the icon settings icon image data.
    /// </summary>
    /// <value>The icon settings icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? IconSettings { get; set; }

    /// <summary>
    /// Gets or sets the update track metadata icon image data.
    /// </summary>
    /// <value>The update track icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? UpdateTrackMetadata { get; set; }

    /// <summary>
    /// Gets or sets the help about icon image data.
    /// </summary>
    /// <value>The help about icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? HelpAbout { get; set; }

    /// <summary>
    /// Gets or sets the help icon image data.
    /// </summary>
    /// <value>The help icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? Help { get; set; }

    /// <summary>
    /// Gets or sets the check new version icon image data.
    /// </summary>
    /// <value>The check new version icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? CheckNewVersion { get; set; }

    /// <summary>
    /// Gets or sets the album lookup icon image data.
    /// </summary>
    /// <value>The album lookup icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? AlbumLookup { get; set; }

    /// <summary>
    /// Gets or sets the main volume slider icon image data.
    /// </summary>
    /// <value>The main volume slider icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? MainVolumeSlider { get; set; }

    /// <summary>
    /// Gets or sets the main volume slider speaker icon image data.
    /// </summary>
    /// <value>The main volume slider speaker icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? MainVolumeSliderSpeaker { get; set; }

    /// <summary>
    /// Gets or sets the track volume slider icon image data.
    /// </summary>
    /// <value>The track volume slider icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? TrackVolumeSlider { get; set; }

    /// <summary>
    /// Gets or sets the track volume slider speaker icon image data.
    /// </summary>
    /// <value>The track volume slider speaker icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? TrackVolumeSliderSpeaker { get; set; }

    /// <summary>
    /// Gets or sets the rating icon image data.
    /// </summary>
    /// <value>The rating icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? Rating { get; set; }

    /// <summary>
    /// Gets or sets the position slider icon image data.
    /// </summary>
    /// <value>The position slider icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? PositionSlider { get; set; }

    /// <summary>
    /// Gets or sets the clear search icon image data.
    /// </summary>
    /// <value>The clear search icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? ClearSearch { get; set; }

    /// <summary>
    /// Gets or sets the previous track icon image data.
    /// </summary>
    /// <value>The previous track icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? PreviousTrack { get; set; }

    /// <summary>
    /// Gets or sets the play icon image data icon image data.
    /// </summary>
    /// <value>The play icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? Play { get; set; }

    /// <summary>
    /// Gets or sets the pause icon image data.
    /// </summary>
    /// <value>The pause icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? Pause { get; set; }

    /// <summary>
    /// Gets or sets the next track icon image data.
    /// </summary>
    /// <value>The next track icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? NextTrack { get; set; }

    /// <summary>
    /// Gets or sets the show queue icon image data.
    /// </summary>
    /// <value>The show queue icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? ShowQueue { get; set; }

    /// <summary>
    /// Gets or sets the shuffle icon image data.
    /// </summary>
    /// <value>The shuffle icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? Shuffle { get; set; }

    /// <summary>
    /// Gets or sets the repeat icon image data.
    /// </summary>
    /// <value>The repeat icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? Repeat { get; set; }

    /// <summary>
    /// Gets or sets the stack queue icon image data.
    /// </summary>
    /// <value>The stack queue icon image data.</value>
    [Settings(Default = null)]
    public CustomIcon? StackQueue { get; set; }
}