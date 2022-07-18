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

using amp.Database.DataModel;
using amp.EtoForms.Enumerations;
using amp.Shared.Interfaces;

namespace amp.EtoForms.Utilities;

/// <summary>
/// A class to generate display names for audio files.
/// </summary>
public static class TrackDisplayNameGenerate
{
    /// <summary>
    /// Gets the next formula part of the given formula string.
    /// </summary>
    /// <param name="formula">The formula from which the next formula part to get.</param>
    /// <param name="formulaType">Type of the formula found; if nothing was found, then <see cref="FormulaType.None"/>.</param>
    /// <returns>The next formula part if found; otherwise a <see cref="string.Empty"/>.</returns>
    private static string GetNextFormula(string formula, out FormulaType formulaType)
    {
        var startIndex = formula.IndexOf('#');
        var endIndex = -1;
        for (var i = startIndex + 1; i < formula.Length; i++)
        {
            if (formula[i] == '#')
            {
                endIndex = i;
                break;
            }
        }

        if (startIndex != -1 && endIndex != -1 && startIndex < endIndex)
        {
            var result = formula.Substring(startIndex, endIndex - startIndex);
            if (result.StartsWith("#ARTIST"))
            {
                formulaType = FormulaType.Artist;
            }
            else if (result.StartsWith("#ALBUM"))
            {
                formulaType = FormulaType.Album;
            }
            // ReSharper disable once StringLiteralTypo
            else if (result.StartsWith("#TRACKNO"))
            {
                formulaType = FormulaType.TrackNo;
            }
            else if (result.StartsWith("#TITLE"))
            {
                formulaType = FormulaType.Title;
            }
            else if (result.StartsWith("#QUEUE"))
            {
                formulaType = FormulaType.QueueIndex;
            }
            else if (result.StartsWith("#ALTERNATE_QUEUE"))
            {
                formulaType = FormulaType.AlternateQueueIndex;
            }
            else if (result.StartsWith("#RENAMED"))
            {
                formulaType = FormulaType.Renamed;
            }
            else
            {
                formulaType = FormulaType.None;
                return string.Empty;
            }

            return result;
        }

        formulaType = FormulaType.None;
        return string.Empty;
    }

    /// <summary>
    /// Replaces a given formula part with with a given string value.
    /// </summary>
    /// <param name="formula">The formula of which part is to be replaced.</param>
    /// <param name="value">The value to replace the <paramref name="formula"/> part with.</param>
    /// <param name="formulaStr">The formula part to replace from the <paramref name="formula"/> string.</param>
    /// <returns>The <paramref name="formula"/> with replacements.</returns>
    private static string FormulaReplace(string formula, string? value, string formulaStr)
    {
        var formulaStart = formula.IndexOf(formulaStr, StringComparison.Ordinal);

        if (formulaStr + "#" == "#ARTIST#" ||
            formulaStr + "#" == "#ALBUM#" ||
            formulaStr + "#" == "#TITLE#" ||
            formulaStr + "#" == "#QUEUE#" ||
            formulaStr + "#" == "#ALTERNATE_QUEUE#" ||
            formulaStr + "#" == "#RENAMED#" ||
            // ReSharper disable once StringLiteralTypo
            formulaStr + "#" == "#TRACKNO#")
        {
            formula = formula.Replace(formulaStr + "#", value);
            return formula;
        }

        var stringPos = -1;
        var formulaEnd = -1;
        var formulaPart = formula;
        for (var i = formulaStart + 1; i < formulaPart.Length && (stringPos == -1 || formulaEnd == -1); i++)
        {
            if (formulaPart[i] == '?' && stringPos == -1)
            {
                stringPos = i;
            }

            if (formulaPart[i] == '#' && formulaEnd == -1)
            {
                formulaEnd = i;
            }
        }

        if (stringPos != -1 && formulaEnd != -1 && stringPos <= formulaEnd)
        {
            var stringPart = formulaPart.Substring(stringPos + 1, formulaEnd - stringPos - 1);
            var startPart = formula.Substring(0, formulaStart);
            var endPart = formula.Substring(formulaEnd + 1);

            var tmpParts = stringPart.Split('^');

            var stringPartStart =
                tmpParts.Length == 1 ? string.Empty : (tmpParts.Length > 1 ? tmpParts[0] : string.Empty);
            var stringPartEnd = tmpParts.Length > 1 ? tmpParts[1] : stringPart;

            formula = string.IsNullOrEmpty(value)
                ? startPart + endPart
                : startPart + stringPartStart + value + stringPartEnd + endPart;
        }
        else
        {
            return string.Empty;
        }

        return formula;
    }

    /// <summary>
    /// Gets a string based on a given formula and a the given parameters.
    /// </summary>
    /// <param name="formula">The formula to create string from.</param>
    /// <param name="albumTrack">The album track to get a display name for.</param>
    /// <param name="queue">A value indicating whether to include queue information into the track display name.</param>
    /// <returns>A string parsed from the given parameters.</returns>
    private static string GetString<TAudioTrack, TAlbum>(string formula, IAlbumTrack<TAudioTrack, TAlbum> albumTrack, bool queue) where TAudioTrack : IAudioTrack where TAlbum : IAlbum
    {
        var track = albumTrack.AudioTrack;
        var artist = track?.Artist;
        var album = track?.Album;
        int.TryParse(track?.Track, out var trackNo);
        var title = track?.Title;
        var trackName = Path.GetFileNameWithoutExtension(track?.FileName);
        var queueIndex = albumTrack.QueueIndex;
        var alternateQueueIndex = albumTrack.QueueIndexAlternate;
        var overrideName = track?.OverrideName;
        var onError = ToStringOld(albumTrack, queue);

        string? FixPathExtension()
        {
            try
            {
                return Path.GetFileNameWithoutExtension(title);
            }
            catch
            {
                return title;
            }
        }

        try
        {
            string formulaStr;
            while ((formulaStr = GetNextFormula(formula, out var formulaType)) != string.Empty)
            {
                if (formulaType == FormulaType.Artist)
                {
                    formula = FormulaReplace(formula, string.IsNullOrWhiteSpace(artist) ? string.Empty : artist, formulaStr);
                }
                else if (formulaType == FormulaType.Album)
                {
                    formula = FormulaReplace(formula, string.IsNullOrWhiteSpace(album) ? string.Empty : album, formulaStr);
                }
                else if (formulaType == FormulaType.TrackNo)
                {
                    formula = FormulaReplace(formula, trackNo <= 0 ? string.Empty : trackNo.ToString(), formulaStr);
                }
                else if (formulaType == FormulaType.Title)
                {
                    formula = FormulaReplace(formula, string.IsNullOrWhiteSpace(title) ? trackName ?? FixPathExtension() : title, formulaStr);
                }
                else if (formulaType == FormulaType.QueueIndex)
                {
                    formula = FormulaReplace(formula, queueIndex <= 0 ? string.Empty : queueIndex.ToString(), formulaStr);
                }
                else if (formulaType == FormulaType.AlternateQueueIndex)
                {
                    formula = FormulaReplace(formula, alternateQueueIndex <= 0 ? string.Empty : alternateQueueIndex.ToString(), formulaStr);
                }
                else if (formulaType == FormulaType.Renamed)
                {
                    formula = FormulaReplace(formula, (string.IsNullOrWhiteSpace(overrideName) ? trackName : overrideName) ?? string.Empty, formulaStr);
                }
            }

            return formula;
        }
        catch
        {
            return onError;
        }
    }

    /// <summary>
    /// Gets a string based on a given formula and a the given parameters.
    /// </summary>
    /// <param name="formula">The formula to create string from.</param>
    /// <param name="audioTrack">The track to get a display name for.</param>
    /// <returns>A string parsed from the given parameters.</returns>
    private static string GetString(string formula, IAudioTrack audioTrack)
    {
        var artist = audioTrack.Artist;
        var album = audioTrack.Album;
        _ = int.TryParse(audioTrack.Track, out var trackNo);
        var title = audioTrack.Title;
        var trackName = Path.GetFileNameWithoutExtension(audioTrack.FileName);
        var overrideName = audioTrack.OverrideName;
        var onError = ToStringOld(audioTrack);

        string? FixPathExtension()
        {
            try
            {
                return Path.GetFileNameWithoutExtension(title);
            }
            catch
            {
                return title;
            }
        }

        try
        {
            string formulaStr;
            while ((formulaStr = GetNextFormula(formula, out var formulaType)) != string.Empty)
            {
                if (formulaType == FormulaType.Artist)
                {
                    formula = FormulaReplace(formula, string.IsNullOrWhiteSpace(artist) ? string.Empty : artist,
                        formulaStr);
                }
                else if (formulaType == FormulaType.Album)
                {
                    formula = FormulaReplace(formula, string.IsNullOrWhiteSpace(album) ? string.Empty : album,
                        formulaStr);
                }
                else if (formulaType == FormulaType.TrackNo)
                {
                    formula = FormulaReplace(formula, trackNo <= 0 ? string.Empty : trackNo.ToString(), formulaStr);
                }
                else if (formulaType == FormulaType.Title)
                {
                    formula = FormulaReplace(formula,
                        string.IsNullOrWhiteSpace(title) ? trackName ?? FixPathExtension() : title, formulaStr);
                }
                else if (formulaType == FormulaType.QueueIndex)
                {
                    formula = FormulaReplace(formula, string.Empty, formulaStr);
                }
                else if (formulaType == FormulaType.AlternateQueueIndex)
                {
                    formula = FormulaReplace(formula, string.Empty, formulaStr);
                }
                else if (formulaType == FormulaType.Renamed)
                {
                    formula = FormulaReplace(formula,
                        (string.IsNullOrWhiteSpace(overrideName) ? trackName : overrideName) ?? string.Empty,
                        formulaStr);
                }
            }

            return formula;
        }
        catch
        {
            return onError;
        }
    }

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance (old version).
    /// </summary>
    /// <param name="albumTrack">The album track to use to generate the string representation.</param>
    /// <param name="queue">if set to <c>true</c> the possible queue index should also be included in the result.</param>
    /// <returns>A <see cref="System.String" /> that represents this instance (old version).</returns>
    private static string ToStringOld<TAudioTrack, TAlbum>(IAlbumTrack<TAudioTrack, TAlbum> albumTrack, bool queue) where TAudioTrack : IAudioTrack where TAlbum : IAlbum
    {
        var alternateQueue = albumTrack.QueueIndexAlternate > 0 && queue ? " [*=" + albumTrack.QueueIndexAlternate + "]" : string.Empty;
        if (!string.IsNullOrWhiteSpace(albumTrack.AudioTrack?.OverrideName))
        {
            return albumTrack.AudioTrack?.OverrideName + ((albumTrack.QueueIndex >= 1 && queue) ? " [" + albumTrack.QueueIndex + "]" : "") + alternateQueue;
        }

        var trackName = Path.GetFileNameWithoutExtension(albumTrack.AudioTrack?.FileName);
        var audioTrackTitle = (string.IsNullOrWhiteSpace(albumTrack.AudioTrack?.Title) ? albumTrack.AudioTrack?.Title : trackName) ?? string.Empty;

        return
            (string.IsNullOrWhiteSpace(albumTrack.AudioTrack?.Artist) ? string.Empty : albumTrack.AudioTrack?.Artist + " - ") +
            (string.IsNullOrWhiteSpace(albumTrack.Album?.AlbumName)
                ? string.Empty
                : albumTrack.Album?.AlbumName + " - ") +
            audioTrackTitle +
            (albumTrack.QueueIndex >= 1 ? " [" + albumTrack.QueueIndex + "]" : "") + alternateQueue;
    }

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance (old version).
    /// </summary>
    /// <param name="audioTrack">The track to use to generate the string representation.</param>
    /// <returns>A <see cref="System.String" /> that represents this instance (old version).</returns>
    private static string ToStringOld(IAudioTrack audioTrack)
    {
        if (!string.IsNullOrWhiteSpace(audioTrack.OverrideName))
        {
            return audioTrack.OverrideName;
        }

        var trackName = Path.GetFileNameWithoutExtension(audioTrack.FileName);
        var audioTrackTitle = (string.IsNullOrWhiteSpace(audioTrack.Title) ? audioTrack.Title : trackName) ?? string.Empty;

        return
            (string.IsNullOrWhiteSpace(audioTrack.Artist) ? string.Empty : audioTrack.Artist + " - ") +
            audioTrackTitle;
    }

    /// <summary>
    /// Gets or sets the track naming formula.
    /// </summary>
    /// <value>The formula.</value>
    public static string Formula { get; set; } =
        // ReSharper disable once StringLiteralTypo
        @"    #ARTIST? - ##ALBUM? - ##TRACKNO?(^) ##TITLE?##QUEUE? [^]##ALTERNATE_QUEUE?[ *=^]#";

    /// <summary>
    /// Gets or sets the renamed track naming formula.
    /// </summary>
    public static string FormulaTrackRenamed { get; set; } = @"    #RENAMED?##QUEUE? [^]##ALTERNATE_QUEUE?[ *=^]#";

    /// <summary>
    /// Gets the display name for the specified track.
    /// </summary>
    /// <param name="albumTrack">The album track.</param>
    /// <param name="queue">if set to <c>true</c> the track queue information is included into the display name.</param>
    /// <returns>A display name for a <see cref="AlbumTrack"/> instance.</returns>
    public static string GetAudioTrackName<TAudioTrack, TAlbum>(this IAlbumTrack<TAudioTrack, TAlbum> albumTrack, bool queue) where TAudioTrack : IAudioTrack where TAlbum : IAlbum
    {
        var formula = string.IsNullOrWhiteSpace(albumTrack.AudioTrack?.OverrideName) ? Formula : FormulaTrackRenamed;
        var result = GetString(formula, albumTrack, queue);
        return result;
    }

    /// <summary>
    /// Gets the display name for the specified track.
    /// </summary>
    /// <param name="audioTrack">The album track.</param>
    /// <returns>A display name for a <see cref="AlbumTrack"/> instance.</returns>
    public static string GetAudioTrackName(IAudioTrack audioTrack)
    {
        var formula = string.IsNullOrWhiteSpace(audioTrack.OverrideName) ? Formula : FormulaTrackRenamed;
        var result = GetString(formula, audioTrack);
        return result;
    }


    /// <summary>
    /// Gets the display name for the specified track.
    /// </summary>
    /// <param name="albumTrack">The album track.</param>
    /// <returns>A display name for a <see cref="AlbumTrack"/> instance.</returns>
    public static string GetAudioTrackName<TAudioTrack, TAlbum>(this IAlbumTrack<TAudioTrack, TAlbum> albumTrack) where TAudioTrack : IAudioTrack where TAlbum : IAlbum
    {
        return GetAudioTrackName(albumTrack, true);
    }
}