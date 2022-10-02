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

using System.IO.Compression;

namespace amp.EtoForms.Utilities;

/// <summary>
/// A class to backup the software application data.
/// </summary>
public static class ApplicationDataBackup
{
    /// <summary>
    /// Creates the backup zip of the software application data.
    /// </summary>
    /// <param name="backupFolder">The backup folder to backup the data from.</param>
    /// <param name="backupFileName">Name of the backup file to backup the data into.</param>
    /// <remarks>The <paramref name="backupFileName"/> will be overridden if one already exists.</remarks>
    public static void CreateBackupZip(string backupFolder, string backupFileName)
    {
        if (File.Exists(backupFileName))
        {
            File.Delete(backupFileName);
        }

        using var archive = ZipFile.Open(backupFileName, ZipArchiveMode.Create);

        var files = new[]
            // ReSharper disable once StringLiteralTypo, SQLite in lower case.
            { "colorSettings.json", "FormMain.json", "icons.json", "layoutSettings.json", "settings.json", "amp_ef_core.sqlite", };

        foreach (var file in files)
        {
            var fullFileName = Path.Combine(backupFolder, file);
            archive.CreateEntryFromFile(fullFileName, file);
        }
    }
}