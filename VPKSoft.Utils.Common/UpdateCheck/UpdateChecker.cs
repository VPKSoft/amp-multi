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


using System.Text.Json;
using VPKSoft.Utils.Common.EventArgs;
using VPKSoft.Utils.Common.Interfaces;

namespace VPKSoft.Utils.Common.UpdateCheck;

/// <summary>
/// An application update checker class.
/// </summary>
public class UpdateChecker : IExceptionReporter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateChecker"/> class.
    /// </summary>
    /// <param name="checkUri">The check URI.</param>
    /// <param name="version">The version.</param>
    /// <param name="versionTag">The version tag.</param>
    public UpdateChecker(string checkUri, Version version, string? versionTag = null)
    {
        this.checkUri = checkUri;
        this.version = version;
        this.versionTag = versionTag;
    }

    /// <summary>
    /// Generates a file from the specified <see cref="VersionData"/> collection as JSON.
    /// </summary>
    /// <param name="data">The data to deserialize into a JSON file.</param>
    /// <param name="fileName">Name of the JSON file.</param>
    public static void GenerateFile(IEnumerable<VersionData> data, string fileName)
    {
        var jsonText = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true, });
        File.WriteAllText(fileName, jsonText);
    }

    /// <summary>
    /// Gets or sets a version to not to check for again.
    /// </summary>
    public static string SkipVersion { get; set; } = string.Empty;

    /// <summary>
    /// Checks the updates and returns the array of version data larger than the version specified in the constructor.
    /// </summary>
    /// <returns>IEnumerable&lt;VersionData&gt;.</returns>
    public async Task<IEnumerable<VersionData>> CheckUpdates()
    {
        var result = new List<VersionData>();

        try
        {
            using var client = new HttpClient();
            var bytes = await client.GetByteArrayAsync(checkUri);

            var options = new JsonDocumentOptions
            {
                AllowTrailingCommas = true,
            };

            var document = JsonDocument.Parse(bytes, options);
            var items = document.Deserialize<List<VersionData>>()?.ToList() ?? new List<VersionData>();

            var larger = items.Where(f =>
                    new Version(f.Version) > version || new Version(f.Version) == version &&
                    string.CompareOrdinal(f.VersionTag, versionTag) > 0)
                .ToList();

            result.AddRange(larger);

            var check = result.OrderByDescending(f => f.ReleaseDateTime).FirstOrDefault();

            if (check != null && $"{check.Version}|{check.VersionTag ?? string.Empty}" == SkipVersion)
            {
                result.Clear();
            }

            return result;
        }
        catch (Exception ex)
        {
            RaiseExceptionOccurred(ex, string.Empty, nameof(CheckUpdates));
            return result;
        }
    }

    /// <summary>
    /// Generates a string representation of the version string and optional tag with the specified string delimiter.
    /// </summary>
    /// <param name="version">The version string.</param>
    /// <param name="versionTagDelimiter">The version tag delimiter.</param>
    /// <param name="versionTag">The version tag.</param>
    /// <param name="versionPrefix">An optional version prefix. E.g. 'v.'.</param>
    /// <returns>A string representation of the application version.</returns>
    public static string VersionAndTagToString(string? version, string versionTagDelimiter, string? versionTag, string versionPrefix = "")
    {
        version = version == null ? versionPrefix + "1.0.0.0" : versionPrefix + version;

        if (!string.IsNullOrWhiteSpace(versionTag))
        {
            return $"{version} {versionTagDelimiter} {versionTag}";
        }

        return $"{version}";
    }

    /// <summary>
    /// Generates a string representation of the <paramref name="version"/> value and optional tag with the specified string delimiter.
    /// </summary>
    /// <param name="version">The version.</param>
    /// <param name="versionTagDelimiter">The version tag delimiter.</param>
    /// <param name="versionTag">The version tag.</param>
    /// <param name="versionFieldCount">The number of components to take from the <paramref name="version"/>. The fieldCount ranges from 0 to 4.</param>
    /// <param name="versionPrefix">An optional version prefix. E.g. 'v.'.</param>
    /// <returns>A string representation of the application version.</returns>
    public static string VersionAndTagToString(Version version, string versionTagDelimiter, string? versionTag, int versionFieldCount = 3, string versionPrefix = "")
    {
        return VersionAndTagToString(version.ToString(versionFieldCount), versionTagDelimiter, versionTag, versionPrefix);
    }

    private readonly string checkUri;
    private readonly Version version;
    private readonly string? versionTag;

    /// <inheritdoc cref="IExceptionReporter.ExceptionOccurred"/>
    public event EventHandler<ExceptionOccurredEventArgs>? ExceptionOccurred;

    /// <inheritdoc cref="IExceptionReporter.RaiseExceptionOccurred"/>
    public void RaiseExceptionOccurred(Exception exception, string _, string method)
    {
        ExceptionOccurred?.Invoke(this, new ExceptionOccurredEventArgs(exception, nameof(UpdateChecker), method));
    }
}