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

using amp.Shared.Enumerations;

namespace amp.Shared.Classes;

/// <summary>
/// A class to convert file extension to <see cref="MusicFileType"/> enumeration values and vice-versa.
/// </summary>
public static class FileExtensionConvert
{
    /// <summary>
    /// Files the type from the file extension.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    /// <returns>A <see cref="MusicFileType"/> enumeration value.</returns>
    public static MusicFileType FileNameToFileType(string? fileName)
    {
        if (fileName == null)
        {
            return MusicFileType.Unknown;
        }

        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        extension = extension.TrimStart('.');

        var fileType = extension switch
        {
            "mp3" => MusicFileType.Mp3,
            "ogg" => MusicFileType.Ogg,
            "wav" => MusicFileType.Wav,
            "wma" => MusicFileType.Wma,
            "m4a" => MusicFileType.M4a,
            "aac" => MusicFileType.Aac,
            "aif" => MusicFileType.Aif,
            "aiff" => MusicFileType.Aif,
            "flac" => MusicFileType.Flac,
            _ => MusicFileType.Unknown,
        };

        return fileType;
    }
}