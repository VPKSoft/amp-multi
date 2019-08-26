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
    public partial class FormFileRelocate : DBLangEngineWinforms
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormFileRelocate"/> class.
        /// </summary>
        public FormFileRelocate()
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
            var form = new FormFileRelocate {Connection = connection};

            return form.ShowDialog() == DialogResult.OK;
        }

        private void FormFileRelocate_Shown(object sender, EventArgs e)
        {
            var paths = DatabaseDataMigrate.GetDirectories(Connection, true, 3);
            // ReSharper disable once CoVariantArrayConversion
            clbAlbumPaths.Items.AddRange(paths.ToArray());
        }

        private void BtUpdateFileLocation_Click(object sender, EventArgs e)
        {
            FormProgressBackground.Execute(this, DatabaseDataMigrate.UpdateSongLocations(fbdDirectory, Connection), "Database update", DBLangEngine.GetMessage("msgProgressPercentage",
                "Progress: {0} %|A message describing some operation progress in percentage."));
        }
    }
}
