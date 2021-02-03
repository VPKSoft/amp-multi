
namespace AmpRESTfulTest
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.lbRemoteUrl = new System.Windows.Forms.Label();
            this.tbRemoteUrl = new System.Windows.Forms.TextBox();
            this.lbRemotePort = new System.Windows.Forms.Label();
            this.nudRemotePort = new System.Windows.Forms.NumericUpDown();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tsbPrevious = new System.Windows.Forms.ToolStripButton();
            this.tsbPlayPause = new System.Windows.Forms.ToolStripButton();
            this.tsbNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbShowQueue = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.lbSongs = new System.Windows.Forms.ListBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.nudRemotePort)).BeginInit();
            this.tlpMain.SuspendLayout();
            this.tsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbRemoteUrl
            // 
            this.lbRemoteUrl.AutoSize = true;
            this.lbRemoteUrl.Location = new System.Drawing.Point(12, 15);
            this.lbRemoteUrl.Name = "lbRemoteUrl";
            this.lbRemoteUrl.Size = new System.Drawing.Size(75, 15);
            this.lbRemoteUrl.TabIndex = 2;
            this.lbRemoteUrl.Text = "Remote URL:";
            // 
            // tbRemoteUrl
            // 
            this.tbRemoteUrl.Location = new System.Drawing.Point(93, 12);
            this.tbRemoteUrl.Name = "tbRemoteUrl";
            this.tbRemoteUrl.Size = new System.Drawing.Size(209, 23);
            this.tbRemoteUrl.TabIndex = 3;
            this.tbRemoteUrl.Text = "http://localhost";
            // 
            // lbRemotePort
            // 
            this.lbRemotePort.AutoSize = true;
            this.lbRemotePort.Location = new System.Drawing.Point(308, 15);
            this.lbRemotePort.Name = "lbRemotePort";
            this.lbRemotePort.Size = new System.Drawing.Size(76, 15);
            this.lbRemotePort.TabIndex = 4;
            this.lbRemotePort.Text = "Remote port:";
            // 
            // nudRemotePort
            // 
            this.nudRemotePort.Location = new System.Drawing.Point(390, 13);
            this.nudRemotePort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudRemotePort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRemotePort.Name = "nudRemotePort";
            this.nudRemotePort.Size = new System.Drawing.Size(69, 23);
            this.nudRemotePort.TabIndex = 5;
            this.nudRemotePort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudRemotePort.Value = new decimal(new int[] {
            12345,
            0,
            0,
            0});
            // 
            // tlpMain
            // 
            this.tlpMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpMain.ColumnCount = 5;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMain.Controls.Add(this.tsMain, 0, 0);
            this.tlpMain.Controls.Add(this.tbLog, 0, 8);
            this.tlpMain.Controls.Add(this.lbSongs, 0, 1);
            this.tlpMain.Location = new System.Drawing.Point(12, 41);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 10;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tlpMain.Size = new System.Drawing.Size(447, 397);
            this.tlpMain.TabIndex = 6;
            // 
            // tsMain
            // 
            this.tlpMain.SetColumnSpan(this.tsMain, 5);
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbPrevious,
            this.tsbPlayPause,
            this.tsbNext,
            this.toolStripSeparator1,
            this.tsbShowQueue,
            this.toolStripSeparator2,
            this.toolStripButton5,
            this.toolStripButton6,
            this.toolStripSeparator3,
            this.toolStripButton1});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(447, 25);
            this.tsMain.TabIndex = 0;
            // 
            // tsbPrevious
            // 
            this.tsbPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPrevious.Image = ((System.Drawing.Image)(resources.GetObject("tsbPrevious.Image")));
            this.tsbPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPrevious.Name = "tsbPrevious";
            this.tsbPrevious.Size = new System.Drawing.Size(23, 22);
            this.tsbPrevious.Click += new System.EventHandler(this.tsbPrevious_Click);
            // 
            // tsbPlayPause
            // 
            this.tsbPlayPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPlayPause.Image = ((System.Drawing.Image)(resources.GetObject("tsbPlayPause.Image")));
            this.tsbPlayPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPlayPause.Name = "tsbPlayPause";
            this.tsbPlayPause.Size = new System.Drawing.Size(23, 22);
            this.tsbPlayPause.Click += new System.EventHandler(this.tsbPlayPause_Click);
            // 
            // tsbNext
            // 
            this.tsbNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNext.Image = ((System.Drawing.Image)(resources.GetObject("tsbNext.Image")));
            this.tsbNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNext.Name = "tsbNext";
            this.tsbNext.Size = new System.Drawing.Size(23, 22);
            this.tsbNext.Click += new System.EventHandler(this.tsbNext_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbShowQueue
            // 
            this.tsbShowQueue.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbShowQueue.Image = ((System.Drawing.Image)(resources.GetObject("tsbShowQueue.Image")));
            this.tsbShowQueue.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbShowQueue.Name = "tsbShowQueue";
            this.tsbShowQueue.Size = new System.Drawing.Size(23, 22);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton5.Text = "toolStripButton5";
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton6.Image")));
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton6.Text = "toolStripButton6";
            // 
            // tbLog
            // 
            this.tlpMain.SetColumnSpan(this.tbLog, 5);
            this.tbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLog.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbLog.Location = new System.Drawing.Point(3, 315);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tlpMain.SetRowSpan(this.tbLog, 2);
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLog.Size = new System.Drawing.Size(441, 79);
            this.tbLog.TabIndex = 1;
            this.tbLog.WordWrap = false;
            // 
            // lbSongs
            // 
            this.tlpMain.SetColumnSpan(this.lbSongs, 5);
            this.lbSongs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbSongs.FormattingEnabled = true;
            this.lbSongs.ItemHeight = 15;
            this.lbSongs.Location = new System.Drawing.Point(3, 28);
            this.lbSongs.Name = "lbSongs";
            this.tlpMain.SetRowSpan(this.lbSongs, 4);
            this.lbSongs.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbSongs.Size = new System.Drawing.Size(441, 158);
            this.lbSongs.TabIndex = 2;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 450);
            this.Controls.Add(this.tlpMain);
            this.Controls.Add(this.nudRemotePort);
            this.Controls.Add(this.lbRemotePort);
            this.Controls.Add(this.tbRemoteUrl);
            this.Controls.Add(this.lbRemoteUrl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.Text = "Sample remote amp# music player";
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.nudRemotePort)).EndInit();
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbRemoteUrl;
        private System.Windows.Forms.TextBox tbRemoteUrl;
        private System.Windows.Forms.Label lbRemotePort;
        private System.Windows.Forms.NumericUpDown nudRemotePort;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton tsbPlayPause;
        private System.Windows.Forms.ToolStripButton tsbNext;
        private System.Windows.Forms.ToolStripButton tsbShowQueue;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripButton tsbPrevious;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.ListBox lbSongs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}

