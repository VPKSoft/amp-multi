namespace amp
{
    partial class FormAddAlbum
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddAlbum));
            this.bOK = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.lbGiveAlbumName = new System.Windows.Forms.Label();
            this.tbAlbumName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // bOK
            // 
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Enabled = false;
            this.bOK.Location = new System.Drawing.Point(383, 32);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 0;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(12, 32);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 1;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // lbGiveAlbumName
            // 
            this.lbGiveAlbumName.AutoSize = true;
            this.lbGiveAlbumName.Location = new System.Drawing.Point(12, 9);
            this.lbGiveAlbumName.Name = "lbGiveAlbumName";
            this.lbGiveAlbumName.Size = new System.Drawing.Size(70, 13);
            this.lbGiveAlbumName.TabIndex = 2;
            this.lbGiveAlbumName.Text = "Give a name:";
            // 
            // tbAlbumName
            // 
            this.tbAlbumName.Location = new System.Drawing.Point(107, 6);
            this.tbAlbumName.Name = "tbAlbumName";
            this.tbAlbumName.Size = new System.Drawing.Size(351, 20);
            this.tbAlbumName.TabIndex = 3;
            this.tbAlbumName.TextChanged += new System.EventHandler(this.tbAlbumName_TextChanged);
            // 
            // FormAddAlbum
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(470, 66);
            this.Controls.Add(this.tbAlbumName);
            this.Controls.Add(this.lbGiveAlbumName);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddAlbum";
            this.ShowInTaskbar = false;
            this.Text = "New album";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Label lbGiveAlbumName;
        private System.Windows.Forms.TextBox tbAlbumName;
    }
}