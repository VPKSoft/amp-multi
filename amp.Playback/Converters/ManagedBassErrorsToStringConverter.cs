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

using ManagedBass;

namespace amp.Playback.Converters;

/// <summary>
/// A class to convert <see cref="ManagedBass.Errors"/> values to strings.
/// </summary>
public static class ManagedBassErrorsToStringConverter
{
    /// <summary>
    /// Converts a <see cref="ManagedBass.Errors"/> value into a string representation.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A string representation of the <see cref="ManagedBass.Errors"/> value.</returns>
    public static string ErrorString(this Errors value)
    {
        var result = value switch
        {
            Errors.Unknown => "Some other mystery error",
            Errors.OK => "No Error",
            Errors.Memory => "Memory Error",
            Errors.FileOpen => "Can't open the file",
            Errors.Driver => "Can't find a free/valid driver",
            Errors.BufferLost => "The sample Buffer was lost",
            Errors.Handle => "Invalid Handle",
            Errors.SampleFormat => "Unsupported sample format",
            Errors.Position => "Invalid playback position",
            Errors.Init => "ManagedBass.Bass.Init(System.Int32,System.Int32,ManagedBass.DeviceInitFlags,System.IntPtr,System.IntPtr) has not been successfully called",
            Errors.Start => "ManagedBass.Bass.Start has not been successfully called",
            Errors.SSL => "SSL/HTTPS support isn't available",
            Errors.NoCD => "No CD in drive",
            Errors.CDTrack => "Invalid track number",
            Errors.Already => "Already initialized/paused/whatever",
            Errors.NotPaused => "Not paused",
            Errors.NotAudioTrack => "Not an audio track",
            Errors.NoChannel => "Can't get a free channel",
            Errors.Type => "An illegal Type was specified",
            Errors.Parameter => "An illegal parameter was specified",
            Errors.No3D => "No 3D support",
            Errors.NoEAX => "No EAX support",
            Errors.Device => "Illegal device number",
            Errors.NotPlaying => "Not playing",
            Errors.SampleRate => "Illegal sample rate",
            Errors.NotFile => "The stream is not a file stream",
            Errors.NoHW => "No hardware voices available",
            Errors.Empty => "The MOD music has no sequence data",
            Errors.NoInternet => "No internet connection could be opened",
            Errors.Create => "Couldn't create the file",
            Errors.NoFX => "Effects are not available",
            Errors.Playing => "The channel is playing",
            Errors.NotAvailable => "Requested data is not available",
            Errors.Decode => "The channel is a 'Decoding Channel'",
            Errors.DirectX => "A sufficient DirectX version is not installed",
            Errors.Timeout => "Connection timed out",
            Errors.FileFormat => "Unsupported file format",
            Errors.Speaker => "Unavailable speaker",
            Errors.Version => "Invalid BASS version (used by add-ons)",
            Errors.Codec => "Codec is not available/supported",
            Errors.Ended => "The channel/file has ended",
            Errors.Busy => "The device is busy (eg. in \"exclusive\" use by another process)",
            // ReSharper disable twice StringLiteralTypo
            Errors.Unstreamable => "The file cannot be streamed using the buffered file system.\nThis could be because an MP4 file's \"mdat\" atom comes before its \"moov\" atom.",
            Errors.WmaLicense => "BassWma: The file is protected",
            Errors.WM9 => "BassWma: WM9 is required",
            Errors.WmaAccesDenied => "BassWma: Access denied (Username/Password is invalid)",
            Errors.WmaCodec => "BassWma: No appropriate codec is installed",
            Errors.WmaIndividual => "BassWma: individualization is needed",
            Errors.AcmCancel => "BassEnc: ACM codec selection cancelled",
            Errors.CastDenied => "BassEnc: Access denied (invalid password)",
            Errors.Wasapi => "BassWASAPI: WASAPI Not available",
            Errors.Mp4NoStream => "BassAAC: Non-Streamable due to MP4 atom order (\"mdat\" before \"moov\")",
            _ => "Some other mystery error",
        };

        return result;
    }
}
