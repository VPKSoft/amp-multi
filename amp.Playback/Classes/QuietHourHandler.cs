#region License
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

using amp.Playback.Enumerations;
using amp.Playback.Interfaces;
using amp.Shared.Interfaces;

namespace amp.Playback.Classes;

/// <summary>
/// Setting data for audio playback adjustment during a quiet hour time span.
/// Implements the <see cref="IQuietHourSettings" />
/// </summary>
/// <seealso cref="IQuietHourSettings" />
public class QuietHourHandler<TAudioTrack, TAlbumTrack, TAlbum> where TAudioTrack : IAudioTrack where TAlbum : IAlbum where TAlbumTrack : IAlbumTrack<TAudioTrack, TAlbum>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QuietHourHandler{TAudioTrack, TAlbumTrack, TAlbum}"/> class.
    /// </summary>
    /// <param name="settings">The settings.</param>
    public QuietHourHandler(IQuietHourSettings settings)
    {
        this.settings = settings;
    }

    /// <summary>
    /// Gets a value indicating whether the quiet hour should be enabled at current <see cref="DateTime"/>.
    /// </summary>
    /// <value><c>true</c> if quit hour should be enabled; otherwise, <c>false</c>.</value>
    public bool IsQuietHour => IsQuietHourTime(DateTime.Now);

    /// <summary>
    /// Determines whether the specified <see cref="DateTime"/> is in the defined quiet hour time span.
    /// </summary>
    /// <param name="value">The <see cref="DateTime"/> value to check for.</param>
    /// <returns><c>true</c> if the specified <see cref="DateTime"/> is in the defined quiet hour time span; otherwise, <c>false</c>.</returns>
    public bool IsQuietHourTime(DateTime value)
    {
        if (!settings.QuietHours)
        {
            return false;
        }

        var startTime = TimeOnly.ParseExact(settings.QuietHoursFrom, "HH':'mm");
        var endTime = TimeOnly.ParseExact(settings.QuietHoursTo, "HH':'mm");
        var start = new DateTime(value.Year, value.Month, value.Day, startTime.Hour, startTime.Minute, 0);
        var end = new DateTime(value.Year, value.Month, value.Day, endTime.Hour, endTime.Minute, 0);
        if (start > end)
        {
            end = end.AddDays(1);
        }

        var startMinus = start.AddDays(-1);
        var endMinus = end.AddDays(-1);

        var startPlus = start.AddDays(1);
        var endPlus = end.AddDays(1);

        if (value >= startMinus && value < endMinus)
        {
            return true;
        }

        if (value >= startPlus && value < endPlus)
        {
            return true;
        }

        if (value >= start && value < end)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Gets the quiet hour times.
    /// </summary>
    /// <value>The quiet hour times.</value>
    public (DateTime start, DateTime end) QuietHourTimes
    {
        get
        {
            var startTime = TimeOnly.ParseExact(settings.QuietHoursFrom, "HH':'mm");
            var endTime = TimeOnly.ParseExact(settings.QuietHoursTo, "HH':'mm");
            var now = DateTime.Now;
            var start = new DateTime(now.Year, now.Month, now.Day, startTime.Hour, startTime.Minute, 0);
            var end = new DateTime(now.Year, now.Month, now.Day, endTime.Hour, endTime.Minute, 0);

            if (start < end)
            {
                end = end.AddDays(1);
            }

            if (start < now && end < now)
            {
                start = start.AddDays(1);
                end = end.AddDays(1);
            }

            return (start, end);
        }
    }

    private readonly IQuietHourSettings settings;

    /// <summary>
    /// Sets the quiet hour if <see cref="IQuietHourSettings.QuietHours"/> is set to <c>true</c> and
    /// the time between <see cref="IQuietHourSettings.QuietHoursFrom"/> and
    /// <see cref="IQuietHourSettings.QuietHoursTo"/> matches the current time.
    /// </summary>
    /// <param name="playbackManager">The playback manager.</param>
    /// <returns><c>true</c> if the quiet hour was changed for the <paramref name="playbackManager"/>, <c>false</c> otherwise.</returns>
    public async Task<bool> SetQuietHour(PlaybackManager<TAudioTrack, TAlbumTrack, TAlbum>? playbackManager)
    {
        if (!settings.QuietHours || playbackManager == null)
        {
            return false;
        }

        var times = QuietHourTimes;

        var now = DateTime.Now;
        var start = times.start;
        var end = times.end;


        if (now > start && now < end)
        {
            quietHoursSet = true;
            quietHoursPreviousPlaying = false;
            quietHoursPreviousVolume = 0;

            if (settings.QuietHoursPause)
            {
                if (playbackManager.PlaybackState == PlaybackState.Playing)
                {
                    quietHoursPreviousPlaying = true;
                    playbackManager.Pause();
                }
                return true;
            }

            if (settings.QuietHoursVolumePercentage > 0)
            {
                quietHoursPreviousVolume = playbackManager.MasterVolume;
                playbackManager.MasterVolume *= (settings.QuietHoursVolumePercentage / 100);
                return true;
            }
        }
        else if (quietHoursSet)
        {
            quietHoursSet = false;

            if (settings.QuietHoursPause)
            {
                if (playbackManager.PlaybackState != PlaybackState.Playing && quietHoursPreviousPlaying)
                {
                    await playbackManager.PlayOrResume();
                }
                return true;
            }

            if (settings.QuietHoursVolumePercentage > 0 && quietHoursPreviousVolume > 0)
            {
                playbackManager.MasterVolume *= quietHoursPreviousVolume;
                return true;
            }

            quietHoursPreviousPlaying = false;
            quietHoursPreviousVolume = 0;
        }

        return false;
    }

    private bool quietHoursPreviousPlaying;
    private bool quietHoursSet;
    private double quietHoursPreviousVolume;
}