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

using amp.EtoForms.Settings.Enumerations;
using amp.Playback.Interfaces;
using ManagedBass.FftSignalProvider;
using VPKSoft.ApplicationSettingsJson;

namespace amp.EtoForms.Settings;

/// <summary>
/// Settings for the amp# software.
/// Implements the <see cref="ApplicationJsonSettings" />
/// Implements the <see cref="IBiasedRandomSettings" />
/// </summary>
/// <seealso cref="ApplicationJsonSettings" />
/// <seealso cref="IBiasedRandomSettings" />
// ReSharper disable once ClassNeverInstantiated.Global, yes it is instantiated via activator.
public class Settings : ApplicationJsonSettings, IBiasedRandomSettings, IQuietHourSettings
{
    /// <summary>
    /// Gets or sets the value indicating whether the previous database migration should be queried from the user.
    /// </summary>
    [Settings(Default = true)]
    public bool MigrateDatabase { get; set; }

    #region BiasedRandomization
    /// <inheritdoc cref="IBiasedRandomSettings.ApplyFrom"/>
    public void ApplyFrom(IBiasedRandomSettings settings)
    {
        BiasedRandomSettingsBase.ApplyFromTo(settings, this);
    }

    /// <inheritdoc cref="IBiasedRandomSettings.ApplyTo"/>
    public void ApplyTo(IBiasedRandomSettings settings)
    {
        BiasedRandomSettingsBase.ApplyFromTo(this, settings);
    }

    /// <summary>
    /// Gets or sets a value indicating whether to use biased randomization with randomizing audio tracks.
    /// </summary>
    [Settings(Default = true)]
    public bool BiasedRandom { get; set; }

    /// <summary>
    /// Gets or sets the tolerance for biased randomization.
    /// </summary>
    [Settings(Default = 10)]
    public double Tolerance { get; set; }

    /// <summary>
    /// Gets or sets the value for randomization with biased rating.
    /// </summary>        
    [Settings(Default = 50)]
    public double BiasedRating { get; set; }

    /// <summary>
    /// Gets or sets the value for randomization with biased played count.
    /// </summary>
    [Settings(Default = -1)]
    public double BiasedPlayedCount { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the biased rating in randomization is enabled.
    /// </summary>
    [Settings]
    public bool BiasedRatingEnabled
    {
        get => BiasedRating >= 0;

        set
        {
            if (!value)
            {
                BiasedRating = -1;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the biased played count in randomization is enabled.
    /// </summary>
    [Settings]
    public bool BiasedPlayedCountEnabled
    {
        get => BiasedPlayedCount >= 0;

        set
        {
            if (!value)
            {
                BiasedPlayedCount = -1;
            }
        }
    }

    /// <summary>
    /// Gets or sets the value for randomization with biased randomized count.
    /// </summary>
    [Settings(Default = -1)]
    public double BiasedRandomizedCount { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the biased randomized count in randomization is enabled.
    /// </summary>
    [Settings]
    public bool BiasedRandomizedCountEnabled
    {
        get => BiasedRandomizedCount >= 0;

        set
        {
            if (!value)
            {
                BiasedRandomizedCount = -1;
            }
        }
    }

    /// <summary>
    /// Gets or sets the value for randomization with biased skipped count.
    /// </summary>
    [Settings(Default = -1)]
    public double BiasedSkippedCount { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the biased skipped count in randomization is enabled.
    /// </summary>
    [Settings]
    public bool BiasedSkippedCountEnabled
    {
        get => BiasedSkippedCount >= 0;

        set
        {
            if (!value)
            {
                BiasedSkippedCount = -1;
            }
        }
    }
    #endregion

    #region QuietHours
    /// <inheritdoc cref="IQuietHourSettings.QuietHours"/>
    [Settings]
    public bool QuietHours { get; set; }

    /// <inheritdoc cref="IQuietHourSettings.QuietHoursFrom"/>
    [Settings(Default = "23:00")]
    public string QuietHoursFrom { get; set; } = string.Empty;

    /// <inheritdoc cref="IQuietHourSettings.QuietHoursTo"/>
    [Settings(Default = "08:00")]
    public string QuietHoursTo { get; set; } = string.Empty;

    /// <inheritdoc cref="IQuietHourSettings.QuietHoursPause"/>
    public bool QuietHoursPause { get; set; }

    /// <inheritdoc cref="IQuietHourSettings.QuietHoursVolumePercentage"/>
    [Settings(Default = 30)]
    public double QuietHoursVolumePercentage { get; set; }
    #endregion

    #region Misc
    /// <summary>
    /// Gets or sets the master volume for audio playback.
    /// </summary>
    /// <value>The master volume.</value>
    [Settings(Default = 0.15)]
    public double MasterVolume { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to automatically check for updates upon application startup.
    /// </summary>
    /// <value><c>true</c> if to automatically check for updates upon application startup; otherwise, <c>false</c>.</value>
    [Settings(Default = false)]
    public bool AutoCheckUpdates { get; set; }

    /// <summary>
    /// Gets or sets the locale.
    /// </summary>
    /// <value>The locale.</value>
    [Settings(Default = "en")]
    public string Locale { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the stack queue random percentage.
    /// </summary>
    /// <value>The stack queue random percentage.</value>
    [Settings(Default = 30)]
    public int StackQueueRandomPercentage { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to display the audio levels (Right/Left).
    /// </summary>
    /// <value><c>true</c> if to display the audio levels; otherwise, <c>false</c>.</value>
    [Settings(Default = false)]
    public bool DisplayAudioLevels { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to visualize the audio levels horizontally.
    /// </summary>
    /// <value><c>true</c> if to visualize the audio levels horizontally; otherwise, <c>false</c>.</value>
    [Settings(Default = false)]
    public bool AudioLevelsHorizontal { get; set; }

    /// <summary>
    /// Gets or sets the version data to not to check updates for.
    /// </summary>
    /// <value>The version data to not to check updates for.</value>
    [Settings(Default = "")]
    public string ForgetVersionUpdate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the help folder for the software.
    /// </summary>
    /// <value>The help folder.</value>
    [Settings(Default = "")]
    public string HelpFolder { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the first queue finish action.
    /// </summary>
    /// <value>The first queue finish action.</value>
    [Settings(Default = QueueFinishActionType.None)]
    public QueueFinishActionType QueueFinishActionFirst { get; set; }

    /// <summary>
    /// Gets or sets the second queue finish action.
    /// </summary>
    /// <value>The first second finish action.</value>
    [Settings(Default = QueueFinishActionType.None)]
    public QueueFinishActionType QueueFinishActionSecond { get; set; }
    #endregion

    #region AudioVisualization
    /// <summary>
    /// Gets or sets a value indicating whether to display audio visualization when the music is playing.
    /// </summary>
    /// <value><c>true</c> if to display audio visualization; otherwise, <c>false</c>.</value>
    [Settings(Default = true)]
    public bool DisplayAudioVisualization { get; set; }

    /// <summary>
    /// Gets or sets the FFT window function.
    /// </summary>
    /// <value>The FFT window function.</value>
    [Settings(Default = (int)WindowType.Hanning)]
    public int FftWindow { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to visualize the audio in bar mode.
    /// </summary>
    /// <value><c>true</c> if to visualize the audio in bar mode; otherwise, <c>false</c>.</value>
    [Settings(Default = true)]
    public bool AudioVisualizationBars { get; set; }
    #endregion

    #region Visual
    /// <summary>
    /// Gets or sets a value indicating whether display playlist column headers.
    /// </summary>
    /// <value><c>true</c> if display playlist column headers; otherwise, <c>false</c>.</value>
    [Settings(Default = true)]
    public bool DisplayPlaylistHeader { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to automatically hide the album track image view if no image exists.
    /// </summary>
    /// <value><c>true</c> if to automatically hide the album track image view if no image exists; otherwise, <c>false</c>.</value>
    [Settings(Default = true)]
    public bool AutoHideEmptyAlbumImage { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to display track image window.
    /// </summary>
    /// <value><c>true</c> if to display track image window; otherwise, <c>false</c>.</value>
    [Settings(Default = true)]
    public bool ShowAlbumImage { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to display the rating column.
    /// </summary>
    /// <value><c>true</c> if to display the rating column; otherwise, <c>false</c>.</value>
    [Settings(Default = true)]
    public bool DisplayRatingColumn { get; set; }

    #endregion

    #region SearchSettings
    /// <summary>
    /// Gets or sets a value indicating whether use FuzzyWuzzy algorithm in the search.
    /// </summary>
    /// <value><c>true</c> if FuzzyWuzzy algorithm with the search; otherwise, <c>false</c>.</value>
    [Settings(Default = false)]
    public bool UseFuzzyWuzzySearch { get; set; }

    /// <summary>
    /// Gets or sets the FuzzyWuzzy search tolerance.
    /// </summary>
    /// <value>The FuzzyWuzzy search tolerance.</value>
    [Settings(Default = 70)]
    public int FuzzyWuzzyTolerance { get; set; }

    /// <summary>
    /// Gets or sets the FuzzyWuzzy maximum result amount.
    /// </summary>
    /// <value>The FuzzyWuzzy maximum result amount.</value>
    [Settings(Default = 50)]
    public int FuzzyWuzzyMaxResults { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to use FuzzyWuzzy algorithm always when searching for tracks.
    /// </summary>
    /// <value><c>true</c> if to use FuzzyWuzzy algorithm always; otherwise, <c>false</c>.</value>
    [Settings(Default = false)]
    public bool FuzzyWuzzyAlwaysOn { get; set; }
    #endregion

    #region Runtime    
    /// <summary>
    /// Gets or sets the playback retry count.
    /// </summary>
    /// <value>The playback retry count.</value>
    [Settings(Default = 20)]
    public int PlaybackRetryCount { get; set; }
    #endregion

    #region TrackNaming
    /// <summary>
    /// Gets or sets the track name formula.
    /// </summary>
    /// <value>The track name formula.</value>
    [Settings(Default = "    {@Ar - }{@Al - }{(@Tn) }{@Tl}")]
    public string TrackNameFormula { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the track name formula for a renamed track.
    /// </summary>
    /// <value>The track name formula for a renamed track.</value>
    [Settings(Default = "    {@R}")]
    public string TrackNameFormulaRenamed { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether to fall back to the track file name if the generated title contains no letters.
    /// </summary>
    /// <value><c>true</c> if to fall back to the track file name if the generated title contains no letters; otherwise, <c>false</c>.</value>
    [Settings(Default = true)]
    public bool TrackNamingFallbackToFileNameWhenNoLetters { get; set; }

    /// <summary>
    /// Gets or sets the minimum length of the track naming title before using the file name as fall back.
    /// </summary>
    /// <value>The minimum length of the track naming title.</value>
    [Settings(Default = 5)]
    public int TrackNamingMinimumTitleLength { get; set; }
    #endregion

    #region PassiveSettings
    /// <summary>
    /// Gets or sets the selected album reference identifier.
    /// </summary>
    /// <value>The selected album reference identifier.</value>
    [Settings(Default = 1L)]
    public long SelectedAlbum { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to hide the add files menu items which do not add tracks to an album.
    /// </summary>
    /// <value><c>true</c> if to hide the menu items to add files to a database only; otherwise, <c>false</c>.</value>
    [Settings(Default = true)]
    // ReSharper disable once UnusedAutoPropertyAccessor.Global, don't remove yet, might be used in the settings dialog (?)
    public bool HideAddFilesToNonAlbum { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the audio and rating controls are expanded.
    /// </summary>
    /// <value><c>true</c> if the audio and rating controls are expanded; otherwise, <c>false</c>.</value>
    [Settings(Default = false)]
    public bool AudioAndRatingControlsExpanded { get; set; }
    #endregion
}