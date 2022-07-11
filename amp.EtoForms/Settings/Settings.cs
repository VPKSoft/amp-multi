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

using amp.Playback.Interfaces;
using VPKSoft.ApplicationSettingsJson;

namespace amp.EtoForms.Settings;

/// <summary>
/// Settings for the amp# software.
/// Implements the <see cref="ApplicationJsonSettings" />
/// Implements the <see cref="IBiasedRandomSettings" />
/// </summary>
/// <seealso cref="ApplicationJsonSettings" />
/// <seealso cref="IBiasedRandomSettings" />
public class Settings : ApplicationJsonSettings, IBiasedRandomSettings
{
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
    /// Gets or sets a value indicating whether to use biased randomization with randomizing songs.
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
    public double BiasedRandomizedCount { get; set; } = -1;

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
    public double BiasedSkippedCount { get; set; } = -1;

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
    /// <summary>
    /// A flag indicating whether the quiet hours is enabled in the settings.
    /// </summary>
    [Settings]
    public bool QuietHours { get; set; }

    /// <summary>
    /// A value indicating the quiet hour starting time if the <see cref="QuietHours"/> is enabled.
    /// </summary>
    [Settings(Default = "08:00")]
    public string QuietHoursFrom { get; set; } = string.Empty;

    /// <summary>
    /// A value indicating the quiet hour ending time if the <see cref="QuietHours"/> is enabled.
    /// </summary>
    [Settings(Default = "23:00")]
    public string QuietHoursTo { get; set; } = string.Empty;

    /// <summary>
    /// A value indicating whether to pause the playback at a quiet hour in case if the <see cref="QuietHours"/> is enabled.
    /// </summary>
    public bool QuietHoursPause { get; set; }

    /// <summary>
    /// A value indicating a volume decrease in percentage if the <see cref="QuietHours"/> is enabled.
    /// </summary>
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
    /// Gets or sets the selected album reference identifier.
    /// </summary>
    /// <value>The selected album reference identifier.</value>
    [Settings(Default = 1L)]
    public long SelectedAlbum { get; set; }

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
    [Settings(Default = 30.0)]
    public double StackQueueRandomPercentage { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to automatically hide the album track image view if no image exists.
    /// </summary>
    /// <value><c>true</c> if to automatically hide the album track image view if no image exists; otherwise, <c>false</c>.</value>
    [Settings(Default = true)]
    public bool AutoHideEmptyAlbumImage { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether display playlist column headers.
    /// </summary>
    /// <value><c>true</c> if display playlist column headers; otherwise, <c>false</c>.</value>
    [Settings(Default = true)]
    public bool DisplayPlaylistHeader { get; set; }
    #endregion
}