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

namespace amp.Playback.EventArguments;

/// <summary>
/// Event argument passed when a track volume has changed in the <see cref="PlaybackManager{TAudioTrack,TAlbumTrack,TAlbum}"/> instance.
/// Implements the <see cref="EventArgs" />
/// </summary>
/// <seealso cref="EventArgs" />
public class TrackVolumeChangedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TrackVolumeChangedEventArgs"/> class.
    /// </summary>
    /// <param name="trackVolume">The track volume.</param>
    /// <param name="audioTrackId">The audio track reference identifier.</param>
    public TrackVolumeChangedEventArgs(double trackVolume, long audioTrackId)
    {
        TrackVolume = trackVolume;
        AudioTrackId = audioTrackId;
    }

    /// <summary>
    /// Gets the track volume.
    /// </summary>
    /// <value>The track volume.</value>
    public double TrackVolume { get; }

    /// <summary>
    /// Gets the audio track reference identifier.
    /// </summary>
    /// <value>The audio track identifier.</value>
    public long AudioTrackId { get; }
}