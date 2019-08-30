namespace amp.DataMigrate.GUI
{
    partial class FormDatabaseMigrate
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDatabaseMigrate));
            this.btUpdateFileLocation = new System.Windows.Forms.Button();
            this.fbdDirectory = new Ookii.Dialogs.WinForms.VistaFolderBrowserDialog();
            this.btDeleteNonExistingSongs = new System.Windows.Forms.Button();
            this.tbUpdateFileLocation = new System.Windows.Forms.TextBox();
            this.tbDeleteNonExistingSongs = new System.Windows.Forms.TextBox();
            this.tbExportUserData = new System.Windows.Forms.TextBox();
            this.btExportUserData = new System.Windows.Forms.Button();
            this.tbImportUserData = new System.Windows.Forms.TextBox();
            this.btImportUserData = new System.Windows.Forms.Button();
            this.btOKRestartClose = new System.Windows.Forms.Button();
            this.lbPathsUsed = new System.Windows.Forms.Label();
            this.lbDirectoryDepth = new System.Windows.Forms.Label();
            this.nudDirectoryDepth = new System.Windows.Forms.NumericUpDown();
            this.lbPathsUsedList = new System.Windows.Forms.ListBox();
            this.sdZip = new System.Windows.Forms.SaveFileDialog();
            this.odZip = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.nudDirectoryDepth)).BeginInit();
            this.SuspendLayout();
            // 
            // btUpdateFileLocation
            // 
            this.btUpdateFileLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btUpdateFileLocation.Location = new System.Drawing.Point(12, 12);
            this.btUpdateFileLocation.Name = "btUpdateFileLocation";
            this.btUpdateFileLocation.Size = new System.Drawing.Size(434, 23);
            this.btUpdateFileLocation.TabIndex = 1;
            this.btUpdateFileLocation.Text = "Update file locations";
            this.btUpdateFileLocation.UseVisualStyleBackColor = true;
            this.btUpdateFileLocation.Click += new System.EventHandler(this.BtUpdateFileLocation_Click);
            // 
            // fbdDirectory
            // 
            this.fbdDirectory.Description = "Select music file directory";
            this.fbdDirectory.RootFolder = System.Environment.SpecialFolder.MyMusic;
            this.fbdDirectory.UseDescriptionForTitle = true;
            // 
            // btDeleteNonExistingSongs
            // 
            this.btDeleteNonExistingSongs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btDeleteNonExistingSongs.Location = new System.Drawing.Point(12, 105);
            this.btDeleteNonExistingSongs.Name = "btDeleteNonExistingSongs";
            this.btDeleteNonExistingSongs.Size = new System.Drawing.Size(434, 23);
            this.btDeleteNonExistingSongs.TabIndex = 2;
            this.btDeleteNonExistingSongs.Text = "Delete non-existing songs";
            this.btDeleteNonExistingSongs.UseVisualStyleBackColor = true;
            this.btDeleteNonExistingSongs.Click += new System.EventHandler(this.BtDeleteNonExistingSongs_Click);
            // 
            // tbUpdateFileLocation
            // 
            this.tbUpdateFileLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUpdateFileLocation.BackColor = System.Drawing.SystemColors.Control;
            this.tbUpdateFileLocation.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbUpdateFileLocation.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.tbUpdateFileLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbUpdateFileLocation.ForeColor = System.Drawing.Color.DarkCyan;
            this.tbUpdateFileLocation.Location = new System.Drawing.Point(12, 41);
            this.tbUpdateFileLocation.Multiline = true;
            this.tbUpdateFileLocation.Name = "tbUpdateFileLocation";
            this.tbUpdateFileLocation.ReadOnly = true;
            this.tbUpdateFileLocation.Size = new System.Drawing.Size(434, 58);
            this.tbUpdateFileLocation.TabIndex = 9;
            this.tbUpdateFileLocation.TabStop = false;
            this.tbUpdateFileLocation.Text = "Updates the file locations based on the file name and its size in case the files " +
    "were moved to another location within the file system. \r\nThis operation can be d" +
    "one multiple times.";
            // 
            // tbDeleteNonExistingSongs
            // 
            this.tbDeleteNonExistingSongs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDeleteNonExistingSongs.BackColor = System.Drawing.SystemColors.Control;
            this.tbDeleteNonExistingSongs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbDeleteNonExistingSongs.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.tbDeleteNonExistingSongs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDeleteNonExistingSongs.ForeColor = System.Drawing.Color.DarkCyan;
            this.tbDeleteNonExistingSongs.Location = new System.Drawing.Point(12, 134);
            this.tbDeleteNonExistingSongs.Multiline = true;
            this.tbDeleteNonExistingSongs.Name = "tbDeleteNonExistingSongs";
            this.tbDeleteNonExistingSongs.ReadOnly = true;
            this.tbDeleteNonExistingSongs.Size = new System.Drawing.Size(434, 58);
            this.tbDeleteNonExistingSongs.TabIndex = 10;
            this.tbDeleteNonExistingSongs.TabStop = false;
            this.tbDeleteNonExistingSongs.Text = "Deletes the songs from the database which locations can not be determined.\r\nNote:" +
    " Use with caution.";
            // 
            // tbExportUserData
            // 
            this.tbExportUserData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbExportUserData.BackColor = System.Drawing.SystemColors.Control;
            this.tbExportUserData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbExportUserData.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.tbExportUserData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbExportUserData.ForeColor = System.Drawing.Color.DarkCyan;
            this.tbExportUserData.Location = new System.Drawing.Point(12, 227);
            this.tbExportUserData.Multiline = true;
            this.tbExportUserData.Name = "tbExportUserData";
            this.tbExportUserData.ReadOnly = true;
            this.tbExportUserData.Size = new System.Drawing.Size(434, 58);
            this.tbExportUserData.TabIndex = 12;
            this.tbExportUserData.TabStop = false;
            this.tbExportUserData.Text = "Export the user data to a ZIP file which,\r\nallows the data to be imported from an" +
    "other computer.\r\nNote: This can be used for backup purposes too.";
            // 
            // btExportUserData
            // 
            this.btExportUserData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btExportUserData.Location = new System.Drawing.Point(12, 198);
            this.btExportUserData.Name = "btExportUserData";
            this.btExportUserData.Size = new System.Drawing.Size(434, 23);
            this.btExportUserData.TabIndex = 11;
            this.btExportUserData.Text = "Export user data";
            this.btExportUserData.UseVisualStyleBackColor = true;
            this.btExportUserData.Click += new System.EventHandler(this.BtExportUserData_Click);
            // 
            // tbImportUserData
            // 
            this.tbImportUserData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbImportUserData.BackColor = System.Drawing.SystemColors.Control;
            this.tbImportUserData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbImportUserData.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.tbImportUserData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbImportUserData.ForeColor = System.Drawing.Color.DarkCyan;
            this.tbImportUserData.Location = new System.Drawing.Point(12, 320);
            this.tbImportUserData.Multiline = true;
            this.tbImportUserData.Name = "tbImportUserData";
            this.tbImportUserData.ReadOnly = true;
            this.tbImportUserData.Size = new System.Drawing.Size(434, 58);
            this.tbImportUserData.TabIndex = 14;
            this.tbImportUserData.TabStop = false;
            this.tbImportUserData.Text = "Import user data from a ZIP file.";
            // 
            // btImportUserData
            // 
            this.btImportUserData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btImportUserData.Location = new System.Drawing.Point(12, 291);
            this.btImportUserData.Name = "btImportUserData";
            this.btImportUserData.Size = new System.Drawing.Size(434, 23);
            this.btImportUserData.TabIndex = 13;
            this.btImportUserData.Text = "Import user data";
            this.btImportUserData.UseVisualStyleBackColor = true;
            this.btImportUserData.Click += new System.EventHandler(this.BtImportUserData_Click);
            // 
            // btOKRestartClose
            // 
            this.btOKRestartClose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btOKRestartClose.Location = new System.Drawing.Point(12, 385);
            this.btOKRestartClose.Name = "btOKRestartClose";
            this.btOKRestartClose.Size = new System.Drawing.Size(776, 23);
            this.btOKRestartClose.TabIndex = 15;
            this.btOKRestartClose.Text = "OK";
            this.btOKRestartClose.UseVisualStyleBackColor = true;
            // 
            // lbPathsUsed
            // 
            this.lbPathsUsed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPathsUsed.AutoSize = true;
            this.lbPathsUsed.Location = new System.Drawing.Point(468, 12);
            this.lbPathsUsed.Name = "lbPathsUsed";
            this.lbPathsUsed.Size = new System.Drawing.Size(125, 13);
            this.lbPathsUsed.TabIndex = 16;
            this.lbPathsUsed.Text = "Paths used in music files:";
            // 
            // lbDirectoryDepth
            // 
            this.lbDirectoryDepth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbDirectoryDepth.AutoSize = true;
            this.lbDirectoryDepth.Location = new System.Drawing.Point(468, 38);
            this.lbDirectoryDepth.Name = "lbDirectoryDepth";
            this.lbDirectoryDepth.Size = new System.Drawing.Size(82, 13);
            this.lbDirectoryDepth.TabIndex = 18;
            this.lbDirectoryDepth.Text = "Directory depth:";
            // 
            // nudDirectoryDepth
            // 
            this.nudDirectoryDepth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudDirectoryDepth.Location = new System.Drawing.Point(719, 36);
            this.nudDirectoryDepth.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudDirectoryDepth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDirectoryDepth.Name = "nudDirectoryDepth";
            this.nudDirectoryDepth.Size = new System.Drawing.Size(69, 20);
            this.nudDirectoryDepth.TabIndex = 19;
            this.nudDirectoryDepth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudDirectoryDepth.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nudDirectoryDepth.ValueChanged += new System.EventHandler(this.NudDirectoryDepth_ValueChanged);
            // 
            // lbPathsUsedList
            // 
            this.lbPathsUsedList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPathsUsedList.FormattingEnabled = true;
            this.lbPathsUsedList.Location = new System.Drawing.Point(471, 63);
            this.lbPathsUsedList.Name = "lbPathsUsedList";
            this.lbPathsUsedList.Size = new System.Drawing.Size(317, 316);
            this.lbPathsUsedList.TabIndex = 20;
            // 
            // sdZip
            // 
            this.sdZip.DefaultExt = "zip";
            this.sdZip.Filter = "Zip files|*.zip";
            // 
            // odZip
            // 
            this.odZip.DefaultExt = "zip";
            this.odZip.Filter = "Zip files|*.zip";
            // 
            // FormDatabaseMigrate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 420);
            this.Controls.Add(this.lbPathsUsedList);
            this.Controls.Add(this.nudDirectoryDepth);
            this.Controls.Add(this.lbDirectoryDepth);
            this.Controls.Add(this.lbPathsUsed);
            this.Controls.Add(this.btOKRestartClose);
            this.Controls.Add(this.tbImportUserData);
            this.Controls.Add(this.btImportUserData);
            this.Controls.Add(this.tbExportUserData);
            this.Controls.Add(this.btExportUserData);
            this.Controls.Add(this.tbDeleteNonExistingSongs);
            this.Controls.Add(this.tbUpdateFileLocation);
            this.Controls.Add(this.btDeleteNonExistingSongs);
            this.Controls.Add(this.btUpdateFileLocation);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDatabaseMigrate";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Database management";
            this.Shown += new System.EventHandler(this.FormFileRelocate_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.nudDirectoryDepth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btUpdateFileLocation;
        private Ookii.Dialogs.WinForms.VistaFolderBrowserDialog fbdDirectory;
        private System.Windows.Forms.Button btDeleteNonExistingSongs;
        private System.Windows.Forms.TextBox tbUpdateFileLocation;
        private System.Windows.Forms.TextBox tbDeleteNonExistingSongs;
        private System.Windows.Forms.TextBox tbExportUserData;
        private System.Windows.Forms.Button btExportUserData;
        private System.Windows.Forms.TextBox tbImportUserData;
        private System.Windows.Forms.Button btImportUserData;
        private System.Windows.Forms.Button btOKRestartClose;
        private System.Windows.Forms.Label lbPathsUsed;
        private System.Windows.Forms.Label lbDirectoryDepth;
        private System.Windows.Forms.NumericUpDown nudDirectoryDepth;
        private System.Windows.Forms.ListBox lbPathsUsedList;
        private System.Windows.Forms.SaveFileDialog sdZip;
        private System.Windows.Forms.OpenFileDialog odZip;
    }
}