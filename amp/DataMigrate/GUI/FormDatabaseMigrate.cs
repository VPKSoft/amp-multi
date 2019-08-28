using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using amp.FormsUtility.Progress;
using amp.SQLiteDatabase;
using VPKSoft.LangLib;

namespace amp.DataMigrate.GUI
{
    /// <summary>
    /// A form to relocate file entries within the database if moved in the file system.
    /// Implements the <see cref="VPKSoft.LangLib.DBLangEngineWinforms" />
    /// </summary>
    /// <seealso cref="VPKSoft.LangLib.DBLangEngineWinforms" />
    public partial class FormDatabaseMigrate : DBLangEngineWinforms
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormDatabaseMigrate"/> class.
        /// </summary>
        public FormDatabaseMigrate()
        {
            InitializeComponent();

            // ReSharper disable once StringLiteralTypo
            DBLangEngine.DBName = "lang.sqlite";

            if (Utils.ShouldLocalize() != null)
            {
                DBLangEngine.InitalizeLanguage("amp.Messages", Utils.ShouldLocalize(), false);
                return; // After localization don't do anything more.
            }

            DBLangEngine.InitalizeLanguage("amp.Messages");
        }

        private SQLiteConnection Connection { get; set; }

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <param name="connection">A <see cref="SQLiteConnection"/> instance.</param>
        /// <returns><c>true</c> if changes were made to the database, <c>false</c> otherwise.</returns>
        public static bool ShowDialog(SQLiteConnection connection)
        {
            var form = new FormDatabaseMigrate {Connection = connection};

            return form.ShowDialog() == DialogResult.OK;
        }

        private void FormFileRelocate_Shown(object sender, EventArgs e)
        {
            ListPaths((int) nudDirectoryDepth.Value);
        }

        private void ListPaths(int depth)
        {
            lbPathsUsedList.Items.Clear();
            var paths = DatabaseDataMigrate.GetDirectories(Connection, depth);
            // ReSharper disable once CoVariantArrayConversion
            lbPathsUsedList.Items.AddRange(paths.ToArray());
        }

        private void BtUpdateFileLocation_Click(object sender, EventArgs e)
        {
            FormProgressBackground.Execute(this,
                DatabaseDataMigrate.UpdateSongLocations(fbdDirectory, Connection), "Database update",
                DBLangEngine.GetMessage("msgProgressPercentage",
                    "Progress: {0} %|A message describing some operation progress in percentage."));
        }

        private void BtDeleteNonExistingSongs_Click(object sender, EventArgs e)
        {
            FormProgressBackground.Execute(this,
                DatabaseDataMigrate.DeleteNonExistingSongs(Connection), "Database update",
                DBLangEngine.GetMessage("msgProgressPercentage",
                    "Progress: {0} %|A message describing some operation progress in percentage."));
        }

        private void NudDirectoryDepth_ValueChanged(object sender, EventArgs e)
        {
            ListPaths((int) nudDirectoryDepth.Value);
        }
    }
}
