namespace amp.FormsUtility.Visual
{
    partial class FormAudioVisualization
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAudioVisualization));
            this.avLine = new VPKSoft.AudioVisualization.AudioVisualizationPlot();
            this.avBars = new VPKSoft.AudioVisualization.AudioVisualizationBars();
            this.SuspendLayout();
            // 
            // avLine
            // 
            this.avLine.ColorAudioChannelLeft = System.Drawing.Color.Aqua;
            this.avLine.ColorAudioChannelRight = System.Drawing.Color.LimeGreen;
            this.avLine.ColorHertzLabels = System.Drawing.Color.Magenta;
            this.avLine.CombineChannels = false;
            this.avLine.DisplayHertzLabels = false;
            this.avLine.Location = new System.Drawing.Point(0, 0);
            this.avLine.MinorityCropPercentage = 2;
            this.avLine.Name = "avLine";
            this.avLine.RefreshRate = 30;
            this.avLine.Size = new System.Drawing.Size(401, 138);
            this.avLine.TabIndex = 1;
            this.avLine.UseAntiAliasing = true;
            // 
            // avBars
            // 
            this.avBars.ColorAudioChannelLeft = System.Drawing.Color.Aqua;
            this.avBars.ColorAudioChannelRight = System.Drawing.Color.LimeGreen;
            this.avBars.ColorGradientLeftEnd = System.Drawing.Color.DarkGreen;
            this.avBars.ColorGradientLeftStart = System.Drawing.Color.SpringGreen;
            this.avBars.ColorGradientRightEnd = System.Drawing.Color.MidnightBlue;
            this.avBars.ColorGradientRightStart = System.Drawing.Color.LightSteelBlue;
            this.avBars.ColorHertzLabels = System.Drawing.Color.Magenta;
            this.avBars.CombineChannels = false;
            this.avBars.DisplayHertzLabels = false;
            this.avBars.DrawWithGradient = true;
            this.avBars.HertzSpan = 92;
            this.avBars.Location = new System.Drawing.Point(0, 12);
            this.avBars.MinorityCropPercentage = 3;
            this.avBars.Name = "avBars";
            this.avBars.RefreshRate = 30;
            this.avBars.Size = new System.Drawing.Size(389, 163);
            this.avBars.TabIndex = 6;
            // 
            // FormAudioVisualization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 200);
            this.ControlBox = false;
            this.Controls.Add(this.avBars);
            this.Controls.Add(this.avLine);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormAudioVisualization";
            this.ShowInTaskbar = false;
            this.Activated += new System.EventHandler(this.FormAudioVisualization_Activated);
            this.ResumeLayout(false);

        }

        #endregion

        private VPKSoft.AudioVisualization.AudioVisualizationPlot avLine;
        private VPKSoft.AudioVisualization.AudioVisualizationBars avBars;
    }
}