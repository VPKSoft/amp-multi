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

using amp.Playback.Interfaces;
using ManagedBass;

namespace amp.Playback.EventArguments;

/// <summary>
/// Event arguments for trying to play a non-existing file.
/// Implements the <see cref="EventArgs" />
/// </summary>
/// <seealso cref="EventArgs" />
public class PlaybackErrorFileNotFoundEventArgs : EventArgs, IPlaybackError
{
    /// <inheritdoc cref="IPlaybackError.Error"/>
    public Errors Error { get; set; }

    /// <inheritdoc cref="IPlaybackError.AudioTrackId"/>
    public long AudioTrackId { get; set; }

    /// <inheritdoc cref="IPlaybackError.PlayAnother"/>
    public bool PlayAnother { get; set; }

    /// <summary>
    /// Gets the name of the file which was not found.
    /// </summary>
    /// <value>The name of the file which was not found.</value>
    public string FileName { get; internal set; } = string.Empty;
}