#region License
/*
MIT License

Copyright(c) 2021 Petteri Kautonen

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


namespace amp.EtoForms.Utilities;

/// <summary>
/// A class containing constant data used around this software.
/// </summary>
public static class Constants
{
    /// <summary>
    /// The extensions of the audio formats used within the software.
    /// </summary>
    public static readonly List<string> Extensions =
        new(new[]
        {
            ".MP3",
            ".OGG",
            // ReSharper disable once StringLiteralTypo
            ".FLAC",
            ".WMA",
            ".WAV",
            ".M4A",
            ".AAC",
            ".AIF",
            // ReSharper disable once StringLiteralTypo
            ".AIFF",
        });

    /// <summary>
    /// Gets a value indicating based on the file name whether the file is a music file.
    /// </summary>
    /// <param name="fileName">The name of the file check.</param>
    /// <returns><c>true</c> if the file is a music file, <c>false</c> otherwise.</returns>
    public static bool FileIsMusicFile(string fileName)
    {
        return
            FileIsMp3(fileName) ||
            FileIsOgg(fileName) ||
            FileIsWav(fileName) ||
            FileIsFlac(fileName) ||
            FileIsWma(fileName) ||
            FileIsAacOrM4A(fileName) ||
            FileIsAif(fileName);
    }

    /// <summary>
    /// Gets a value indicating based on the file name if the file is a MP3 file.
    /// </summary>
    /// <param name="fileName">The name of the file check.</param>
    /// <returns><c>true</c> if the file is a MP3 file, <c>false</c> otherwise.</returns>
    public static bool FileIsMp3(string fileName)
    {
        return fileName.ToUpper().EndsWith(".MP3");
    }

    /// <summary>
    /// Gets a value indicating based on the file name if the file is an OGG file.
    /// </summary>
    /// <param name="fileName">The name of the file check.</param>
    /// <returns><c>true</c> if the file is a OGG file, <c>false</c> otherwise.</returns>
    public static bool FileIsOgg(string fileName)
    {
        return fileName.ToUpper().EndsWith(".OGG");
    }

    /// <summary>
    /// Gets a value indicating based on the file name if the file is a WAV file.
    /// </summary>
    /// <param name="fileName">The name of the file check.</param>
    /// <returns><c>true</c> if the file is a WAV file, <c>false</c> otherwise.</returns>
    public static bool FileIsWav(string fileName)
    {
        return fileName.ToUpper().EndsWith(".WAV");
    }

    // ReSharper disable once CommentTypo
    /// <summary>
    /// Gets a value indicating based on the file name if the file is a FLAC file.
    /// </summary>
    /// <param name="fileName">The name of the file check.</param>
    /// <returns><c>true</c> if the file is a Flac file, <c>false</c> otherwise.</returns>
    // ReSharper disable once IdentifierTypo
    public static bool FileIsFlac(string fileName)
    {
        // ReSharper disable once StringLiteralTypo
        return fileName.ToUpper().EndsWith(".FLAC");
    }

    /// <summary>
    /// Gets a value indicating based on the file name if the file is a WMA file.
    /// </summary>
    /// <param name="fileName">The name of the file check.</param>
    /// <returns><c>true</c> if the file is a WMA file, <c>false</c> otherwise.</returns>
    public static bool FileIsWma(string fileName)
    {
        // ReSharper disable once StringLiteralTypo
        return fileName.ToUpper().EndsWith(".WMA");
    }

    /// <summary>
    /// Gets a value indicating based on the file name if the file is an AAC/M4A file.
    /// </summary>
    /// <param name="fileName">The name of the file check.</param>
    /// <returns><c>true</c> if the file is an AAC/M4A file, <c>false</c> otherwise.</returns>
    public static bool FileIsAacOrM4A(string fileName)
    {
        return fileName.ToUpper().EndsWith(".M4A") ||
               fileName.ToUpper().EndsWith(".AAC");
    }

    /// <summary>
    /// Gets a value indicating based on the file name if the file is an AIF file.
    /// </summary>
    /// <param name="fileName">The name of the file check.</param>
    /// <returns><c>true</c> if the file is an AIF file, <c>false</c> otherwise.</returns>
    public static bool FileIsAif(string fileName)
    {
        return fileName.ToUpper().EndsWith(".AIF") ||
               // ReSharper disable once StringLiteralTypo
               fileName.ToUpper().EndsWith(".AIFF");
    }
}

