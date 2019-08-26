namespace amp.DataMigrate.GUI
{
    partial class FormFileRelocate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFileRelocate));
            this.clbAlbumPaths = new System.Windows.Forms.CheckedListBox();
            this.btUpdateFileLocation = new System.Windows.Forms.Button();
            this.fbdDirectory = new Ookii.Dialogs.WinForms.VistaFolderBrowserDialog();
            this.SuspendLayout();
            // 
            // clbAlbumPaths
            // 
            this.clbAlbumPaths.FormattingEnabled = true;
            this.clbAlbumPaths.Location = new System.Drawing.Point(12, 44);
            this.clbAlbumPaths.Name = "clbAlbumPaths";
            this.clbAlbumPaths.Size = new System.Drawing.Size(394, 394);
            this.clbAlbumPaths.TabIndex = 0;
            // 
            // btUpdateFileLocation
            // 
            this.btUpdateFileLocation.Location = new System.Drawing.Point(458, 44);
            this.btUpdateFileLocation.Name = "btUpdateFileLocation";
            this.btUpdateFileLocation.Size = new System.Drawing.Size(130, 23);
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
            // FormFileRelocate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btUpdateFileLocation);
            this.Controls.Add(this.clbAlbumPaths);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormFileRelocate";
            this.Text = "Relocate files";
            this.Shown += new System.EventHandler(this.FormFileRelocate_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox clbAlbumPaths;
        private System.Windows.Forms.Button btUpdateFileLocation;
        private Ookii.Dialogs.WinForms.VistaFolderBrowserDialog fbdDirectory;
    }
}