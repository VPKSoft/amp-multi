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

namespace amp.Playback.Interfaces;

/// <summary>
/// Setting data for audio playback adjustment during a specified time span.
/// </summary>
public interface IQuietHourSettings
{
    /// <summary>
    /// A flag indicating whether the quiet hours is enabled in the settings.
    /// </summary>
    bool QuietHours { get; set; }

    /// <summary>
    /// A value indicating the quiet hour starting time if the <see cref="QuietHours"/> is enabled.
    /// </summary>
    string QuietHoursFrom { get; set; }

    /// <summary>
    /// A value indicating the quiet hour ending time if the <see cref="QuietHours"/> is enabled.
    /// </summary>
    string QuietHoursTo { get; set; }

    /// <summary>
    /// A value indicating whether to pause the playback at a quiet hour in case if the <see cref="QuietHours"/> is enabled.
    /// </summary>
    bool QuietHoursPause { get; set; }

    /// <summary>
    /// A value indicating a volume decrease in percentage if the <see cref="QuietHours"/> is enabled.
    /// </summary>
    double QuietHoursVolumePercentage { get; set; }
}