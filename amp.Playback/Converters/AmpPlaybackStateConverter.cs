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

using amp.Playback.Enumerations;

namespace amp.Playback.Converters;

/// <summary>
/// A class to convert between the <see cref="ManagedBass.PlaybackState"/> and the <see cref="amp.Playback.Enumerations.PlaybackState"/> enumeration values.
/// </summary>
public static class AmpPlaybackStateConverter
{
    /// <summary>
    /// Converts the specified <see cref="amp.Playback.Enumerations.PlaybackState"/> to <inheritdoc cref="ManagedBass.PlaybackState"/> value.
    /// </summary>
    /// <param name="playbackState">State of the playback.</param>
    /// <returns>The converted value.</returns>
    public static ManagedBass.PlaybackState ConvertTo(PlaybackState playbackState)
    {
        return playbackState switch
        {
            PlaybackState.Stopped => ManagedBass.PlaybackState.Stopped,
            PlaybackState.Playing => ManagedBass.PlaybackState.Playing,
            PlaybackState.Paused => ManagedBass.PlaybackState.Paused,
            _ => ManagedBass.PlaybackState.Stopped,
        };
    }

    /// <summary>
    /// Converts the <see cref="amp.Playback.Enumerations.PlaybackState"/> from the specified <see cref="ManagedBass.PlaybackState"/> value.
    /// </summary>
    /// <param name="playbackState">State of the playback.</param>
    /// <returns>The converted value.</returns>
    public static PlaybackState ConvertFrom(ManagedBass.PlaybackState playbackState)
    {
        return playbackState switch
        {
            ManagedBass.PlaybackState.Stopped => PlaybackState.Stopped,
            ManagedBass.PlaybackState.Playing => PlaybackState.Playing,
            ManagedBass.PlaybackState.Paused => PlaybackState.Paused,
            ManagedBass.PlaybackState.Stalled => PlaybackState.Paused,
            _ => PlaybackState.Stopped,
        };
    }
}