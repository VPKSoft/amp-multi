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

namespace amp.Database.Enumerations;

/// <summary>
/// An enumeration of music file formats.
/// </summary>
public enum MusicFileType
{
    /// <summary>
    /// The audio file format is unknown.
    /// </summary>
    Unknown,

    /// <summary>
    /// The MPEG-[1, 2, 2.5] Audio Layer III music file format.
    /// </summary>
    Mp3,

    /// <summary>
    /// The Ogg Vorbis music file format.
    /// </summary>
    Ogg,

    /// <summary>
    /// The Waveform Audio file format.
    /// </summary>
    Wav,

    /// <summary>
    /// The Windows Media Audio file format.
    /// </summary>
    Wma,

    /// <summary>
    /// The MPEG-4 Audio file format.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    M4a,

    /// <summary>
    /// The Advanced Audio Coding file format.
    /// </summary>
    Aac,

    /// <summary>
    /// The Audio Interchange file Format.
    /// </summary>
    Aif,

    /// <summary>
    /// The Free Lossless Audio Codec file format.
    /// </summary>
    Flac,
}