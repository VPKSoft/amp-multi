
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
            this.components = new System.ComponentModel.Container();
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
            this.sp1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbShowQueue = new System.Windows.Forms.ToolStripButton();
            this.sp2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbShuffle = new System.Windows.Forms.ToolStripButton();
            this.tsbRepeat = new System.Windows.Forms.ToolStripButton();
            this.sp3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbStackQueue = new System.Windows.Forms.ToolStripButton();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.lbSongs = new AmpControls.RefreshListbox();
            this.tlpMiddleControls = new System.Windows.Forms.TableLayoutPanel();
            this.lbSongName = new System.Windows.Forms.Label();
            this.lbMinusTime = new System.Windows.Forms.Label();
            this.tbSongPosition = new System.Windows.Forms.TrackBar();
            this.tbFind = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tmPlayerState = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nudRemotePort)).BeginInit();
            this.tlpMain.SuspendLayout();
            this.tsMain.SuspendLayout();
            this.tlpMiddleControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbSongPosition)).BeginInit();
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
            this.tlpMain.Controls.Add(this.lbSongs, 0, 2);
            this.tlpMain.Controls.Add(this.tlpMiddleControls, 0, 1);
            this.tlpMain.Controls.Add(this.button1, 0, 6);
            this.tlpMain.Location = new System.Drawing.Point(12, 41);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 10;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tlpMain.Size = new System.Drawing.Size(515, 473);
            this.tlpMain.TabIndex = 6;
            // 
            // tsMain
            // 
            this.tlpMain.SetColumnSpan(this.tsMain, 5);
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbPrevious,
            this.tsbPlayPause,
            this.tsbNext,
            this.sp1,
            this.tsbShowQueue,
            this.sp2,
            this.tsbShuffle,
            this.tsbRepeat,
            this.sp3,
            this.tsbStackQueue});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(515, 25);
            this.tsMain.TabIndex = 0;
            // 
            // tsbPrevious
            // 
            this.tsbPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPrevious.Image = ((System.Drawing.Image)(resources.GetObject("tsbPrevious.Image")));
            this.tsbPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPrevious.Name = "tsbPrevious";
            this.tsbPrevious.Size = new System.Drawing.Size(23, 22);
            this.tsbPrevious.Text = "Previous song";
            this.tsbPrevious.Click += new System.EventHandler(this.tsbPrevious_Click);
            // 
            // tsbPlayPause
            // 
            this.tsbPlayPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPlayPause.Image = ((System.Drawing.Image)(resources.GetObject("tsbPlayPause.Image")));
            this.tsbPlayPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPlayPause.Name = "tsbPlayPause";
            this.tsbPlayPause.Size = new System.Drawing.Size(23, 22);
            this.tsbPlayPause.Text = "Play";
            this.tsbPlayPause.Click += new System.EventHandler(this.tsbPlayPause_Click);
            // 
            // tsbNext
            // 
            this.tsbNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNext.Image = ((System.Drawing.Image)(resources.GetObject("tsbNext.Image")));
            this.tsbNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNext.Name = "tsbNext";
            this.tsbNext.Size = new System.Drawing.Size(23, 22);
            this.tsbNext.Text = "Next song";
            this.tsbNext.Click += new System.EventHandler(this.tsbNext_Click);
            // 
            // sp1
            // 
            this.sp1.Name = "sp1";
            this.sp1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbShowQueue
            // 
            this.tsbShowQueue.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbShowQueue.Image = ((System.Drawing.Image)(resources.GetObject("tsbShowQueue.Image")));
            this.tsbShowQueue.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbShowQueue.Name = "tsbShowQueue";
            this.tsbShowQueue.Size = new System.Drawing.Size(23, 22);
            this.tsbShowQueue.Text = "Show queue";
            // 
            // sp2
            // 
            this.sp2.Name = "sp2";
            this.sp2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbShuffle
            // 
            this.tsbShuffle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbShuffle.Image = ((System.Drawing.Image)(resources.GetObject("tsbShuffle.Image")));
            this.tsbShuffle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbShuffle.Name = "tsbShuffle";
            this.tsbShuffle.Size = new System.Drawing.Size(23, 22);
            this.tsbShuffle.Text = "Shuffle";
            this.tsbShuffle.ToolTipText = "Shuffle";
            // 
            // tsbRepeat
            // 
            this.tsbRepeat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRepeat.Image = ((System.Drawing.Image)(resources.GetObject("tsbRepeat.Image")));
            this.tsbRepeat.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRepeat.Name = "tsbRepeat";
            this.tsbRepeat.Size = new System.Drawing.Size(23, 22);
            this.tsbRepeat.Text = "Repeat";
            // 
            // sp3
            // 
            this.sp3.Name = "sp3";
            this.sp3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbStackQueue
            // 
            this.tsbStackQueue.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbStackQueue.Image = ((System.Drawing.Image)(resources.GetObject("tsbStackQueue.Image")));
            this.tsbStackQueue.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbStackQueue.Name = "tsbStackQueue";
            this.tsbStackQueue.Size = new System.Drawing.Size(23, 22);
            this.tsbStackQueue.Text = "Stack queue";
            // 
            // tbLog
            // 
            this.tlpMain.SetColumnSpan(this.tbLog, 5);
            this.tbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLog.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbLog.Location = new System.Drawing.Point(3, 373);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tlpMain.SetRowSpan(this.tbLog, 2);
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLog.Size = new System.Drawing.Size(509, 97);
            this.tbLog.TabIndex = 1;
            this.tbLog.WordWrap = false;
            // 
            // lbSongs
            // 
            this.tlpMain.SetColumnSpan(this.lbSongs, 5);
            this.lbSongs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbSongs.FormattingEnabled = true;
            this.lbSongs.ItemHeight = 15;
            this.lbSongs.Location = new System.Drawing.Point(3, 79);
            this.lbSongs.Name = "lbSongs";
            this.tlpMain.SetRowSpan(this.lbSongs, 4);
            this.lbSongs.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbSongs.Size = new System.Drawing.Size(509, 190);
            this.lbSongs.TabIndex = 2;
            this.lbSongs.VScrollPosition = 0;
            // 
            // tlpMiddleControls
            // 
            this.tlpMiddleControls.AutoSize = true;
            this.tlpMiddleControls.ColumnCount = 5;
            this.tlpMain.SetColumnSpan(this.tlpMiddleControls, 5);
            this.tlpMiddleControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMiddleControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMiddleControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMiddleControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMiddleControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMiddleControls.Controls.Add(this.lbSongName, 0, 1);
            this.tlpMiddleControls.Controls.Add(this.lbMinusTime, 4, 1);
            this.tlpMiddleControls.Controls.Add(this.tbSongPosition, 0, 0);
            this.tlpMiddleControls.Controls.Add(this.tbFind, 0, 2);
            this.tlpMiddleControls.Location = new System.Drawing.Point(0, 25);
            this.tlpMiddleControls.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMiddleControls.Name = "tlpMiddleControls";
            this.tlpMiddleControls.RowCount = 4;
            this.tlpMiddleControls.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMiddleControls.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMiddleControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMiddleControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMiddleControls.Size = new System.Drawing.Size(515, 51);
            this.tlpMiddleControls.TabIndex = 3;
            // 
            // lbSongName
            // 
            this.lbSongName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpMiddleControls.SetColumnSpan(this.lbSongName, 4);
            this.lbSongName.Location = new System.Drawing.Point(3, 16);
            this.lbSongName.Name = "lbSongName";
            this.lbSongName.Size = new System.Drawing.Size(406, 14);
            this.lbSongName.TabIndex = 5;
            this.lbSongName.Text = "-";
            // 
            // lbMinusTime
            // 
            this.lbMinusTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbMinusTime.Location = new System.Drawing.Point(415, 16);
            this.lbMinusTime.Name = "lbMinusTime";
            this.lbMinusTime.Size = new System.Drawing.Size(97, 15);
            this.lbMinusTime.TabIndex = 6;
            this.lbMinusTime.Text = "-00:00";
            this.lbMinusTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbSongPosition
            // 
            this.tbSongPosition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSongPosition.AutoSize = false;
            this.tlpMiddleControls.SetColumnSpan(this.tbSongPosition, 5);
            this.tbSongPosition.Location = new System.Drawing.Point(0, 0);
            this.tbSongPosition.Margin = new System.Windows.Forms.Padding(0);
            this.tbSongPosition.Name = "tbSongPosition";
            this.tbSongPosition.Size = new System.Drawing.Size(515, 16);
            this.tbSongPosition.TabIndex = 4;
            this.tbSongPosition.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbSongPosition.Scroll += new System.EventHandler(this.tbSongPosition_Scroll);
            this.tbSongPosition.Leave += new System.EventHandler(this.tbSongPosition_MouseLeave);
            this.tbSongPosition.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbSongPosition_MouseDown);
            this.tbSongPosition.MouseLeave += new System.EventHandler(this.tbSongPosition_MouseLeave);
            this.tbSongPosition.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbSongPosition_MouseUp);
            // 
            // tbFind
            // 
            this.tbFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpMiddleControls.SetColumnSpan(this.tbFind, 5);
            this.tbFind.Location = new System.Drawing.Point(3, 34);
            this.tbFind.Name = "tbFind";
            this.tbFind.Size = new System.Drawing.Size(509, 23);
            this.tbFind.TabIndex = 7;
            this.tbFind.TextChanged += new System.EventHandler(this.tbFind_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 275);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 33);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tmPlayerState
            // 
            this.tmPlayerState.Interval = 1000;
            this.tmPlayerState.Tick += new System.EventHandler(this.tmPlayerState_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 526);
            this.Controls.Add(this.tlpMain);
            this.Controls.Add(this.nudRemotePort);
            this.Controls.Add(this.lbRemotePort);
            this.Controls.Add(this.tbRemoteUrl);
            this.Controls.Add(this.lbRemoteUrl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.Text = "Sample remote amp# music player";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.nudRemotePort)).EndInit();
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.tlpMiddleControls.ResumeLayout(false);
            this.tlpMiddleControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbSongPosition)).EndInit();
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
        private System.Windows.Forms.ToolStripButton tsbStackQueue;
        private System.Windows.Forms.ToolStripButton tsbPlayPause;
        private System.Windows.Forms.ToolStripButton tsbNext;
        private System.Windows.Forms.ToolStripButton tsbShowQueue;
        private System.Windows.Forms.ToolStripButton tsbShuffle;
        private System.Windows.Forms.ToolStripButton tsbRepeat;
        private System.Windows.Forms.ToolStripButton tsbPrevious;
        private System.Windows.Forms.TextBox tbLog;
        private AmpControls.RefreshListbox lbSongs;
        private System.Windows.Forms.ToolStripSeparator sp1;
        private System.Windows.Forms.ToolStripSeparator sp2;
        private System.Windows.Forms.ToolStripSeparator sp3;
        private System.Windows.Forms.Timer tmPlayerState;
        private System.Windows.Forms.TableLayoutPanel tlpMiddleControls;
        private System.Windows.Forms.Label lbSongName;
        private System.Windows.Forms.Label lbMinusTime;
        private System.Windows.Forms.TrackBar tbSongPosition;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbFind;
    }
}

