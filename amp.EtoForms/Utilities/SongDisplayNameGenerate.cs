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

namespace amp.EtoForms.Utilities;

public static class SongDisplayNameGenerate
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
    private static string FormulaReplace(string formula, string value, string formulaStr)
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
    /// <param name="albumSong">The album song to get a display name for.</param>
    /// <param name="queue">A value indicating whether to include queue information into the song display name.</param>
    /// <param name="error">A value indicating if an error occurred while parsing the formula.</param>
    /// <returns>A string parsed from the given parameters.</returns>
    private static string GetString(string formula, AlbumSong albumSong, bool queue, out bool error)
    {
        var song = albumSong.Song;
        var artist = song?.Artist;
        var album = song?.Album;
        int.TryParse(song?.Track, out var trackNo);
        var title = song?.Title;
        var songName = Path.GetFileNameWithoutExtension(song?.FileName);
        var queueIndex = albumSong.QueueIndex;
        var alternateQueueIndex = albumSong.QueueIndexAlternate;
        var overrideName = song?.OverrideName;
        var onError = ToStringOld(albumSong, queue);

        string FixPathExtension()
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
                    formula = FormulaReplace(formula, string.IsNullOrWhiteSpace(title) ? songName ?? string.Empty : FixPathExtension(), formulaStr);
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
                    formula = FormulaReplace(formula, (string.IsNullOrWhiteSpace(overrideName) ? songName : overrideName) ?? string.Empty, formulaStr);
                }
            }

            error = false;
            return formula;
        }
        catch
        {
            error = true;
            return onError;
        }
    }

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance (old version).
    /// </summary>
    /// <param name="queue">if set to <c>true</c> the possible queue index should also be included in the result.</param>
    /// <returns>A <see cref="System.String" /> that represents this instance (old version).</returns>
    private static string ToStringOld(AlbumSong albumSong, bool queue)
    {
        var alternateQueue = albumSong.QueueIndexAlternate > 0 && queue ? " [*=" + albumSong.QueueIndexAlternate + "]" : string.Empty;
        if (!string.IsNullOrWhiteSpace(albumSong.Song?.OverrideName))
        {
            return albumSong.Song?.OverrideName + ((albumSong.QueueIndex >= 1 && queue) ? " [" + albumSong.QueueIndex + "]" : "") + alternateQueue;
        }

        var songName = Path.GetFileNameWithoutExtension(albumSong.Song?.FileName);
        var songTitle = (string.IsNullOrWhiteSpace(albumSong.Song?.Title) ? albumSong.Song?.Title : songName) ?? string.Empty;

        return
            (string.IsNullOrWhiteSpace(albumSong.Song?.Artist) ? string.Empty : albumSong.Song?.Artist + " - ") +
            (string.IsNullOrWhiteSpace(albumSong.Album?.AlbumName)
                ? string.Empty
                : albumSong.Album?.AlbumName + " - ") +
            songTitle +
            (albumSong.QueueIndex >= 1 ? " [" + albumSong.QueueIndex + "]" : "") + alternateQueue;
    }

    public static string Formula { get; set; } =
        // ReSharper disable once StringLiteralTypo
        @"    #ARTIST? - ##ALBUM? - ##TRACKNO?(^) ##TITLE?##QUEUE? [^]##ALTERNATE_QUEUE?[ *=^]#";

    /// <summary>
    /// Gets or sets the renamed album naming instructions.
    /// </summary>
    public static string FormulaSongRenamed { get; set; } = @"    #RENAMED?##QUEUE? [^]##ALTERNATE_QUEUE?[ *=^]#";

    public static string GetAlbumName(this AlbumSong albumSong, bool queue)
    {
        var formula = string.IsNullOrWhiteSpace(albumSong.Song?.OverrideName) ? Formula : FormulaSongRenamed;
        return GetString(formula, albumSong, queue, out _);
    }

    public static string GetAlbumName(this AlbumSong albumSong)
    {
        return GetAlbumName(albumSong, true);
    }

}