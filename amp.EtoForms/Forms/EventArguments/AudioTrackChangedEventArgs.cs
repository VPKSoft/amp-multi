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

using amp.DataAccessLayer.DtoClasses;

namespace amp.EtoForms.Forms.EventArguments;

/// <summary>
/// Event arguments for an event where the <see cref="AudioTrack"/> instance data has been changed.
/// Implements the <see cref="System.EventArgs" />
/// </summary>
/// <seealso cref="System.EventArgs" />
public class AudioTrackChangedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AudioTrackChangedEventArgs"/> class.
    /// </summary>
    /// <param name="audioTrack">The audio track data.</param>
    public AudioTrackChangedEventArgs(AudioTrack audioTrack)
    {
        AudioTrack = audioTrack;
    }

    /// <summary>
    /// Gets or sets the audio track data with changes.
    /// </summary>
    /// <value>The audio track data.</value>
    public AudioTrack AudioTrack { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the data updated to entity was saved successfully into the database.
    /// </summary>
    /// <value><c>true</c> if database save was successful; otherwise, <c>false</c>.</value>
    public bool SaveSuccess { get; set; }
}