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

using System.Text;
using ATL;

namespace amp.Shared.Classes;

/// <summary>
/// Helper methods for the <see cref="Track"/> class.
/// </summary>
public static class FileTagInfoHelper
{
    /// <summary>
    /// Gets the tag data as string for searching purposes.
    /// </summary>
    /// <param name="track">The track data.</param>
    /// <returns>A string with the tag data.</returns>
    public static string GetTagString(this Track track)
    {
        var result = new StringBuilder();

        var propertyInfos = track.GetType().GetProperties();

        foreach (var propertyInfo in propertyInfos)
        {
            if (!propertyInfo.PropertyType.IsPrimitive && propertyInfo.PropertyType != typeof(string) && !
                    propertyInfo.PropertyType.GetGenericArguments().Any(t => t is { IsValueType: true, IsPrimitive: true }))
            {
                continue;
            }

            try
            {
                if (propertyInfo.GetValue(track) == null)
                {
                    continue;
                }
            }
            catch
            {
                continue;
            }

            var value = propertyInfo.GetValue(track);
            if (!string.IsNullOrWhiteSpace(value?.ToString()))
            {
                result.AppendLine($"{propertyInfo.Name}: {value}");
            }
        }

        foreach (var trackAdditionalField in track.AdditionalFields)
        {
            if (!string.IsNullOrWhiteSpace(trackAdditionalField.Value))
            {
                result.AppendLine($"{trackAdditionalField.Key}: {trackAdditionalField.Value}");
            }
        }

        return result.ToString();
    }
}