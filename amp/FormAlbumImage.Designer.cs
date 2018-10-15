namespace amp
{
    partial class FormAlbumImage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAlbumImage));
            this.pnBorderSeparator = new System.Windows.Forms.Panel();
            this.pbAlbum = new System.Windows.Forms.PictureBox();
            this.pnBorderSeparator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAlbum)).BeginInit();
            this.SuspendLayout();
            // 
            // pnBorderSeparator
            // 
            this.pnBorderSeparator.Controls.Add(this.pbAlbum);
            this.pnBorderSeparator.Location = new System.Drawing.Point(12, 12);
            this.pnBorderSeparator.Name = "pnBorderSeparator";
            this.pnBorderSeparator.Size = new System.Drawing.Size(204, 204);
            this.pnBorderSeparator.TabIndex = 0;
            // 
            // pbAlbum
            // 
            this.pbAlbum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbAlbum.Image = global::amp.Properties.Resources.music_note;
            this.pbAlbum.Location = new System.Drawing.Point(0, 0);
            this.pbAlbum.Name = "pbAlbum";
            this.pbAlbum.Size = new System.Drawing.Size(204, 204);
            this.pbAlbum.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbAlbum.TabIndex = 1;
            this.pbAlbum.TabStop = false;
            // 
            // FormAlbumImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(228, 228);
            this.ControlBox = false;
            this.Controls.Add(this.pnBorderSeparator);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAlbumImage";
            this.ShowInTaskbar = false;
            this.Activated += new System.EventHandler(this.FormAlbumImage_Activated);
            this.pnBorderSeparator.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbAlbum)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnBorderSeparator;
        private System.Windows.Forms.PictureBox pbAlbum;

    }
}