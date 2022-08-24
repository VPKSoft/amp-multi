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

namespace VPKSoft.Utils.Common.UpdateCheck;

/// <summary>
/// A class for single version Json data.
/// </summary>
public class VersionData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VersionData"/> class.
    /// </summary>
    /// <param name="version">The version.</param>
    /// <param name="downloadUrl">The download URL.</param>
    /// <param name="changeLog">The change log.</param>
    /// <param name="releaseDateTime">The release date time.</param>
    /// <param name="isDirectDownload">if set to <c>true</c> the <paramref name="isDirectDownload"/> links to a file.</param>
    /// <param name="versionTag">The version tag. E.g. beta-2</param>
    public VersionData(string version, string downloadUrl, string changeLog, DateTimeOffset releaseDateTime, bool isDirectDownload, string? versionTag = null)
    {
        Version = version;
        DownloadUrl = downloadUrl;
        ChangeLog = changeLog;
        VersionTag = versionTag;
        ReleaseDateTime = releaseDateTime;
        IsDirectDownload = isDirectDownload;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VersionData"/> class.
    /// </summary>
    /// <remarks>Default constructor for Json deserialization.</remarks>
    public VersionData()
    {
        Version = "1.0.0";
        DownloadUrl = string.Empty;
        ChangeLog = string.Empty;
    }

    private string version = string.Empty;

    /// <summary>
    /// Gets or sets the version.
    /// </summary>
    /// <value>The version.</value>
    public string Version
    {
        get => version;

        set
        {
            _ = System.Version.Parse(value);
            version = value;
        }
    }

    /// <summary>
    /// Gets or sets the version tag.
    /// </summary>
    /// <value>The version tag.</value>
    public string? VersionTag { get; set; }

    /// <summary>
    /// Gets or sets the download URL.
    /// </summary>
    /// <value>The download URL.</value>
    public string DownloadUrl { get; set; }

    /// <summary>
    /// Gets or sets the change log.
    /// </summary>
    /// <value>The change log.</value>
    public string ChangeLog { get; set; }

    /// <summary>
    /// Gets or sets the release date time.
    /// </summary>
    /// <value>The release date time.</value>
    public DateTimeOffset ReleaseDateTime { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="DownloadUrl"/> links to a file.
    /// </summary>
    /// <value><c>true</c> if the <see cref="DownloadUrl"/> links to a file; otherwise, <c>false</c>.</value>
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public bool IsDirectDownload { get; set; }
}