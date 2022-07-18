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

namespace amp.Playback.EventArguments;

/// <summary>
/// Event arguments base class for various playback events.
/// Implements the <see cref="EventArgs" />
/// </summary>
/// <seealso cref="EventArgs" />
public abstract class PositionEventArgsBase : EventArgs
{
    /// <summary>
    /// Gets or sets the current playback position.
    /// </summary>
    /// <value>The current playback position.</value>
    public virtual double CurrentPosition { get; set; }

    /// <summary>
    /// Gets or sets the length of the current playback item.
    /// </summary>
    /// <value>The length of the current playback item.</value>
    public virtual double PlaybackLength { get; set; }

    /// <summary>
    /// Gets or sets the state of the playback.
    /// </summary>
    /// <value>The state of the playback.</value>
    public virtual PlaybackState PlaybackState { get; set; }

    /// <summary>
    /// Gets or sets the audio track identifier.
    /// </summary>
    /// <value>The audio track identifier.</value>
    public virtual long AudioTrackId { get; set; }
}