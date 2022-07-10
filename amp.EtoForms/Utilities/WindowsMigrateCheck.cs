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

using System.Runtime.InteropServices;
using amp.Database;
using amp.Database.LegacyConvert;
using amp.EtoForms.Dialogs;
using amp.Shared.Localization;
using Eto.Forms;
using MessageBox = Eto.Forms.MessageBox;

namespace amp.EtoForms.Utilities;

/// <summary>
/// Migration class from the old amp# format to the new EF Core format.
/// </summary>
public static class WindowsMigrateCheck
{
    /// <summary>
    /// Converts the amp# database specified by command line argument into EF Core format.
    /// </summary>
    /// <param name="parent">The parent for the dialogs used by the migration process.</param>
    public static void ConvertOldCommandLine(Control parent)
    {
        var args = Environment.GetCommandLineArgs();
        foreach (var arg in args)
        {
            // ReSharper disable once StringLiteralTypo
            if (File.Exists(arg) && Path.GetExtension(arg) == ".sqlite")
            {
                Globals.Settings.MigrateDatabase = true;
                ConvertOld(parent, arg);
                break;
            }
        }
    }

    /// <summary>
    /// Converts the amp# database into EF Core format.
    /// </summary>
    /// <param name="parent">The parent for the dialogs used by the migration process.</param>
    /// <param name="databaseFile">The old database file name.</param>
    public static void ConvertOld(Control parent, string databaseFile)
    {
        // The WinForms version might be installed in this case.
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var ampPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "amp#");

            // ReSharper disable once StringLiteralTypo
            var oldDatabaseFileName = Path.Combine(ampPath, "amp.sqlite");

            if (Globals.Settings.MigrateDatabase &&
                Directory.Exists(ampPath) &&
                File.Exists(oldDatabaseFileName))
            {
                var statistics = MigrateOld.OldDatabaseStatistics(oldDatabaseFileName);

                var statisticsMessage =
                    string.Format(
                        Messages.ConversionStatistics, Environment.NewLine,
                        statistics.songs, statistics.albums, statistics.albumSongs, statistics.queueSnaphots);

                if (MessageBox.Show(parent,
                        Messages
                            .DoYouWantToConvertTheOldDatabaseIntoTheNewFormatTheOperationMightTakeFewMinutes + statisticsMessage,
                        Messages.ConvertDatabase, MessageBoxButtons.YesNo,
                        MessageBoxType.Question) == DialogResult.Yes)
                {
                    File.Delete(databaseFile);
                    var migrate = new Migrate($"Data Source={databaseFile}");
                    migrate.RunMigrateUp();

                    new DialogDatabaseConvertProgress().ShowModal(parent, oldDatabaseFileName, databaseFile);

                    Globals.Settings.MigrateDatabase = false;
                    Globals.SaveSettings();
                }
                else
                {
                    Globals.Settings.MigrateDatabase = false;
                    Globals.SaveSettings();
                }
            }
        }
    }
}