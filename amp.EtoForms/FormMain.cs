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
using Eto.Drawing;
using Eto.Forms;

namespace amp.EtoForms;


public class FormMain : Form
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FormMain"/> class.
    /// </summary>
    public FormMain()
    {
        MinimumSize = new Size(500, 500);

        var database = Path.Combine(Globals.DataFolder, "amp_ef_core.sqlite");

        // The WinForms version might be installed in this case.
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var ampPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "amp#");

            var oldDatabaseFileName = Path.Combine(ampPath, "amp.sqlite");

            if (Globals.Settings.MigrateDatabase &&
                Directory.Exists(ampPath) &&
                File.Exists(oldDatabaseFileName))
            {
                var statistics = MigrateOld.OldDatabaseStatistics(oldDatabaseFileName);

                var statisticsMessage =
                    string.Format(
                        Localization.Messages.ConversionStatistics, Environment.NewLine,
                        statistics.songs, statistics.albums, statistics.albumSongs, statistics.queueSnaphots);

                if (MessageBox.Show(this,
                        Localization.Messages
                            .DoYouWantToConvertTheOldDatabaseIntoTheNewFormatTheOperationMightTakeFewMinutes + statisticsMessage,
                        Localization.Messages.ConvertDatabase, MessageBoxButtons.YesNo,
                        MessageBoxType.Question) == DialogResult.Yes)
                {
                    File.Delete(database);
                    var migrate = new Migrate($"Data Source={database}");
                    migrate.RunMigrateUp();

                    new DialogDatabaseConvertProgress().ShowModal(this, oldDatabaseFileName, database);
                    //// TODO:Splash the conversion progress && speedup!
                    //Migrate.MigrateExistingData(oldDatabaseFileName, database);
                    //Globals.Settings.MigrateDatabase = false;
                    //Globals.SaveSettings();
                }
            }
        }

        var migration = new Migrate($"Data Source={database}");
        migration.RunMigrateUp();
    }
}