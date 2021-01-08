
namespace AmpControls
{
    partial class StarSlider
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
            this.pnStars0 = new System.Windows.Forms.Panel();
            this.pnStars1 = new System.Windows.Forms.Panel();
            this.pnStars0.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnStars0
            // 
            this.pnStars0.BackgroundImage = global::AmpControls.Properties.Resources.stars;
            this.pnStars0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnStars0.Controls.Add(this.pnStars1);
            this.pnStars0.Location = new System.Drawing.Point(0, 0);
            this.pnStars0.Name = "pnStars0";
            this.pnStars0.Size = new System.Drawing.Size(176, 35);
            this.pnStars0.TabIndex = 2;
            this.pnStars0.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnStars0_MouseClick);
            // 
            // pnStars1
            // 
            this.pnStars1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnStars1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnStars1.Location = new System.Drawing.Point(88, 0);
            this.pnStars1.Name = "pnStars1";
            this.pnStars1.Size = new System.Drawing.Size(176, 35);
            this.pnStars1.TabIndex = 0;
            this.pnStars1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnStars0_MouseClick);
            // 
            // StarSlider
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnStars0);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximumSize = new System.Drawing.Size(176, 35);
            this.MinimumSize = new System.Drawing.Size(176, 35);
            this.Name = "StarSlider";
            this.Size = new System.Drawing.Size(176, 35);
            this.SizeChanged += new System.EventHandler(this.StarSlider_SizeChanged);
            this.pnStars0.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnStars0;
        private System.Windows.Forms.Panel pnStars1;
    }
}
