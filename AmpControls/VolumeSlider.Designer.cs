
namespace AmpControls
{
    partial class VolumeSlider
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
            this.tlpVolumeSlider = new System.Windows.Forms.TableLayoutPanel();
            this.pnMainVolumeLeft = new System.Windows.Forms.Panel();
            this.pnMainVolumeRight = new System.Windows.Forms.Panel();
            this.pnSlider = new System.Windows.Forms.Panel();
            this.tlpVolumeSlider.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpVolumeSlider
            // 
            this.tlpVolumeSlider.ColumnCount = 3;
            this.tlpVolumeSlider.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpVolumeSlider.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpVolumeSlider.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpVolumeSlider.Controls.Add(this.pnMainVolumeLeft, 0, 0);
            this.tlpVolumeSlider.Controls.Add(this.pnMainVolumeRight, 2, 0);
            this.tlpVolumeSlider.Controls.Add(this.pnSlider, 1, 0);
            this.tlpVolumeSlider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpVolumeSlider.Location = new System.Drawing.Point(0, 0);
            this.tlpVolumeSlider.Margin = new System.Windows.Forms.Padding(0);
            this.tlpVolumeSlider.Name = "tlpVolumeSlider";
            this.tlpVolumeSlider.RowCount = 1;
            this.tlpVolumeSlider.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpVolumeSlider.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpVolumeSlider.Size = new System.Drawing.Size(435, 32);
            this.tlpVolumeSlider.TabIndex = 4;
            // 
            // pnMainVolumeLeft
            // 
            this.pnMainVolumeLeft.BackgroundImage = global::AmpControls.Properties.Resources.volume_small;
            this.pnMainVolumeLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnMainVolumeLeft.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnMainVolumeLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnMainVolumeLeft.Location = new System.Drawing.Point(0, 0);
            this.pnMainVolumeLeft.Margin = new System.Windows.Forms.Padding(0);
            this.pnMainVolumeLeft.Name = "pnMainVolumeLeft";
            this.pnMainVolumeLeft.Size = new System.Drawing.Size(32, 32);
            this.pnMainVolumeLeft.TabIndex = 2;
            this.pnMainVolumeLeft.Click += new System.EventHandler(this.pnMainVolume_Click);
            // 
            // pnMainVolumeRight
            // 
            this.pnMainVolumeRight.BackgroundImage = global::AmpControls.Properties.Resources.volume_high;
            this.pnMainVolumeRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnMainVolumeRight.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnMainVolumeRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnMainVolumeRight.Location = new System.Drawing.Point(403, 0);
            this.pnMainVolumeRight.Margin = new System.Windows.Forms.Padding(0);
            this.pnMainVolumeRight.Name = "pnMainVolumeRight";
            this.pnMainVolumeRight.Size = new System.Drawing.Size(32, 32);
            this.pnMainVolumeRight.TabIndex = 3;
            this.pnMainVolumeRight.Click += new System.EventHandler(this.pnMainVolume_Click);
            // 
            // pnSlider
            // 
            this.pnSlider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnSlider.Location = new System.Drawing.Point(35, 3);
            this.pnSlider.Name = "pnSlider";
            this.pnSlider.Size = new System.Drawing.Size(365, 26);
            this.pnSlider.TabIndex = 4;
            this.pnSlider.Paint += new System.Windows.Forms.PaintEventHandler(this.pnSlider_Paint);
            this.pnSlider.Leave += new System.EventHandler(this.pnSlider_MouseLeave);
            this.pnSlider.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnSlider_MouseMove);
            this.pnSlider.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnSlider_MouseDown);
            this.pnSlider.MouseLeave += new System.EventHandler(this.pnSlider_MouseLeave);
            this.pnSlider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnSlider_MouseMove);
            this.pnSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnSlider_MouseUp);
            // 
            // VolumeSlider
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tlpVolumeSlider);
            this.Name = "VolumeSlider";
            this.Size = new System.Drawing.Size(435, 32);
            this.tlpVolumeSlider.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnMainVolumeRight;
        private System.Windows.Forms.Panel pnMainVolumeLeft;
        private System.Windows.Forms.TableLayoutPanel tlpVolumeSlider;
        private System.Windows.Forms.Panel pnSlider;
    }
}
