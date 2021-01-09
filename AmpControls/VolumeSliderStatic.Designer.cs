
namespace AmpControls
{
    partial class VolumeSliderStatic
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnVol1 = new System.Windows.Forms.Panel();
            this.pnVol2 = new System.Windows.Forms.Panel();
            this.pnVol1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnVol1
            // 
            this.pnVol1.BackgroundImage = global::AmpControls.Properties.Resources.volume_slider;
            this.pnVol1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnVol1.Controls.Add(this.pnVol2);
            this.pnVol1.Location = new System.Drawing.Point(0, 0);
            this.pnVol1.Name = "pnVol1";
            this.pnVol1.Size = new System.Drawing.Size(100, 35);
            this.pnVol1.TabIndex = 1;
            this.pnVol1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnVol1_MouseClick);
            // 
            // pnVol2
            // 
            this.pnVol2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnVol2.BackColor = System.Drawing.Color.Transparent;
            this.pnVol2.BackgroundImage = global::AmpControls.Properties.Resources.volume_over;
            this.pnVol2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnVol2.ForeColor = System.Drawing.Color.Transparent;
            this.pnVol2.Location = new System.Drawing.Point(50, 0);
            this.pnVol2.Name = "pnVol2";
            this.pnVol2.Size = new System.Drawing.Size(100, 35);
            this.pnVol2.TabIndex = 0;
            this.pnVol2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnVol1_MouseClick);
            // 
            // VolumeSliderStatic
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnVol1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximumSize = new System.Drawing.Size(100, 35);
            this.MinimumSize = new System.Drawing.Size(100, 35);
            this.Name = "VolumeSliderStatic";
            this.Size = new System.Drawing.Size(100, 35);
            this.SizeChanged += new System.EventHandler(this.VolumeSliderStatic_SizeChanged);
            this.pnVol1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnVol1;
        private System.Windows.Forms.Panel pnVol2;
    }
}
