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

using System.Globalization;
using System.Text.RegularExpressions;
using amp.EtoForms.Utilities;
using amp.Shared.Extensions;
using amp.Shared.Interfaces;
using FuzzierSharp;

namespace amp.EtoForms.ExtensionClasses;

internal static class AudioTrackFilters
{
    /// <summary>
    /// Checks if the properties of this music file instance matches the given search string.
    /// </summary>
    /// <param name="audioTrack">The track to check for match to the specified <paramref name="search"/> text.</param>
    /// <param name="search">The search string.</param>
    /// <returns><c>true</c> if one of the properties of this music file instance matches the search string, <c>false</c> otherwise.</returns>
    internal static bool Match(this IAudioTrack audioTrack, string search)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return true;
        }

        // a year match for the tag..
        if (search.StartsWith("y:") || search.StartsWith("Y:"))
        {
            var match = Regex.Match(search, "^(Y|y):(>|<|>=|<=|=)(\\d){4,}").Success;
            if (match)
            {
                if (int.TryParse(audioTrack.Year, out var year))
                {
                    var yearMatch = Regex.Matches(search, "\\d{4,}").FirstOrDefault()?.Value;
                    if (!int.TryParse(yearMatch, out var yearCompare))
                    {
                        return false;
                    }

                    search = search.TrimStart('y', 'Y', ':');
                    if (search.StartsWith(">="))
                    {
                        return yearCompare >= year;
                    }

                    if (search.StartsWith(">"))
                    {
                        return yearCompare > year;
                    }

                    if (search.StartsWith("<="))
                    {
                        return yearCompare <= year;
                    }

                    if (search.StartsWith("<"))
                    {
                        return yearCompare < year;
                    }

                    if (search.StartsWith("="))
                    {
                        return yearCompare == year;
                    }

                    return false;
                }
            }
        }

        var digitMatch = search.StartsWith(">=") || search.StartsWith("<=") || search.StartsWith("<") ||
                         search.StartsWith(">");

        if (digitMatch)
        {
            if (search.Contains('&'))
            {
                try
                {
                    var searches = search.Split('&');
                    var match1 = YearMatch(searches[0], audioTrack.FileNameFull());
                    var match2 = YearMatch(searches[1].TrimStart(), audioTrack.FileNameFull());
                    if (match1 != null && match2 != null)
                    {
                        return (bool)match1 && (bool)match2;
                    }
                }
                catch
                {
                    // allow the search to continue..
                }
            }
            else
            {
                var match1 = YearMatch(search, audioTrack.FileNameFull());
                if (match1 != null)
                {
                    return (bool)match1;
                }
            }
        }

        search = search.ToUpper().Trim();
        var found1 = audioTrack.Artist?.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                     audioTrack.Album?.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                     audioTrack.Title?.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                     audioTrack.Year?.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                     audioTrack.Track?.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                     audioTrack.FileNameFull().IndexOf(search, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                     audioTrack.OverrideName?.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                     TrackDisplayNameGenerate.GetAudioTrackName(audioTrack).IndexOf(search, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                     MatchTagFindString(audioTrack.TagFindString, search);

        var search2 = search.Split(' ');
        if (search2.Length <= 0 || found1)
        {
            return found1;
        }

        return search2.Select(str => str.ToUpper()).Aggregate(true,
            (current, tmpStr) => current &
                                 (audioTrack.Artist?.IndexOf(tmpStr, StringComparison.InvariantCultureIgnoreCase) >
                                  -1 ||
                                  audioTrack.Album?.IndexOf(tmpStr, StringComparison.InvariantCultureIgnoreCase) >
                                  -1 ||
                                  audioTrack.Title?.IndexOf(tmpStr, StringComparison.InvariantCultureIgnoreCase) >
                                  -1 ||
                                  audioTrack.Year?.IndexOf(tmpStr, StringComparison.InvariantCultureIgnoreCase) >
                                  -1 ||
                                  audioTrack.Track?.IndexOf(tmpStr, StringComparison.InvariantCultureIgnoreCase) >
                                  -1 ||
                                  audioTrack.FileNameFull().IndexOf(tmpStr,
                                      StringComparison.InvariantCultureIgnoreCase) > -1 ||
                                  audioTrack.OverrideName?.IndexOf(tmpStr,
                                      StringComparison.InvariantCultureIgnoreCase) > -1 ||
                                  TrackDisplayNameGenerate.GetAudioTrackName(audioTrack).IndexOf(search,
                                      StringComparison.InvariantCultureIgnoreCase) > -1 ||
                                  MatchTagFindString(audioTrack.TagFindString, tmpStr)));
    }

    /// <summary>
    /// Performs a FuzzyWuzzy partial string matching for the specified audio track.
    /// </summary>
    /// <param name="audioTrack">The audio track.</param>
    /// <param name="search">The search string.</param>
    /// <returns>The FuzzyWuzzy partial match score between <c>0</c> to <c>100</c>.</returns>
    public static int FuzzyMatchScore(this IAudioTrack audioTrack, string search)
    {
        var searchStrings = new[]
        {
            audioTrack.Artist, audioTrack.Album, audioTrack.Title, audioTrack.Track, audioTrack.FileNameFull(),
            audioTrack.OverrideName, TrackDisplayNameGenerate.GetAudioTrackName(audioTrack),
        }.Where(f => f != null).Select(s => s!.Trim()).ToList();

        return searchStrings.Select(f => Fuzz.PartialRatio(search.Trim(), f)).DefaultIfEmpty(0).Max();
    }

    /// <summary>
    /// Performs a FuzzyWuzzy partial string matching for the specified audio track.
    /// </summary>
    /// <param name="audioTrack">The audio track.</param>
    /// <param name="search">The search string.</param>
    /// <param name="minimumRatio">The minimum ratio resulting a positive match.</param>
    /// <returns><c>true</c> if the FuzzyWuzzy match was found, <c>false</c> otherwise.</returns>
    internal static bool FuzzyStringMatch(this IAudioTrack audioTrack, string search, int minimumRatio = 70)
    {
        return audioTrack.FuzzyMatchScore(search) >= minimumRatio;
    }

    private static bool MatchTagFindString(string? value, string match)
    {
        if (value == null)
        {
            return false;
        }

        using var reader = new StringReader(value);

        while (reader.ReadLine() is { } line)
        {
            // The new tag find string is in line format and the lines are [value name: value] format.
            if (line.IndexOf(':') != -1)
            {
                if (line.Split(':')[1].Contains(match, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            else if (line.Contains(match, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private static bool? YearMatch(string search, string fullFileName)
    {
        try
        {
            if (DateTime.TryParse(search.TrimStart('<', '>', '='),
                    CultureInfo.CurrentUICulture.DateTimeFormat,
                    DateTimeStyles.AllowInnerWhite | DateTimeStyles.AllowLeadingWhite |
                    DateTimeStyles.AllowTrailingWhite | DateTimeStyles.AllowWhiteSpaces,
                    out var resultDateTime))
            {
                var info = new FileInfo(fullFileName);
                if (search.StartsWith(">="))
                {
                    return info.CreationTime >= resultDateTime;
                }

                if (search.StartsWith(">"))
                {
                    return info.CreationTime > resultDateTime;
                }

                if (search.StartsWith("<="))
                {
                    return info.CreationTime <= resultDateTime;
                }

                if (search.StartsWith("<"))
                {
                    return info.CreationTime < resultDateTime;
                }
            }
        }
        catch
        {
            return null;
        }

        return false;
    }
}