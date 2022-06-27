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

using System.Globalization;
using System.Text.RegularExpressions;
using amp.Shared.Interfaces;

namespace amp.EtoForms.ExtensionClasses;

internal static class SongFilters
{
    /// <summary>
    /// Checks if the properties of this music file instance matches the given search string.
    /// </summary>
    /// <param name="song">The song to check for match to the specified <paramref name="search"/> text.</param>
    /// <param name="search">The search string.</param>
    /// <returns><c>true</c> if one of the properties of this music file instance matches the search string, <c>false</c> otherwise.</returns>
    internal static bool Match(this ISong song, string search)
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
                if (int.TryParse(song.Year, out var year))
                {
                    var yearMatch = Regex.Matches(search, "\\d{4,}").FirstOrDefault()?.Value;
                    if (int.TryParse(yearMatch, out var yearCompare))
                    {
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
                    var match1 = YearMatch(searches[0], song.FileName);
                    var match2 = YearMatch(searches[1].TrimStart(), song.FileName);
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
                var match1 = YearMatch(search, song.FileName);
                if (match1 != null)
                {
                    return (bool)match1;
                }
            }
        }

        search = search.ToUpper().Trim();
        bool found1 = song.Artist?.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                      song.Album?.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                      song.Title?.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                      song.Year?.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                      song.Track?.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                      song.FileName.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                      song.OverrideName?.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                      song.TagFindString?.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) > -1;

        string[] search2 = search.Split(' ');
        if (search2.Length <= 1 || found1)
        {
            return found1;
        }

        bool found2 = true;
        foreach (string str in search2)
        {
            var tmpStr = str.ToUpper();
            found2 &= song.Artist?.IndexOf(tmpStr, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                      song.Album?.IndexOf(tmpStr, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                      song.Title?.IndexOf(tmpStr, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                      song.Year?.IndexOf(tmpStr, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                      song.Track?.IndexOf(tmpStr, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                      song.FileName.IndexOf(tmpStr, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                      song.OverrideName?.IndexOf(tmpStr, StringComparison.InvariantCultureIgnoreCase) > -1 ||
                      song.TagFindString?.IndexOf(tmpStr, StringComparison.InvariantCultureIgnoreCase) > -1;
        }

        return found2;
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