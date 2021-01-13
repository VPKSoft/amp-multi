using System.ComponentModel;
using System.Windows.Forms;
using AmpControls;

namespace amp
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.tmSeek = new System.Windows.Forms.Timer(this.components);
            this.ssStatus = new ReaLTaiizor.Controls.CrownStatusStrip();
            this.lbQueueCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.odM3U = new System.Windows.Forms.OpenFileDialog();
            this.sdM3U = new System.Windows.Forms.SaveFileDialog();
            this.tmPendOperation = new System.Windows.Forms.Timer(this.components);
            this.tmIPCFiles = new System.Windows.Forms.Timer(this.components);
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.msMain = new ReaLTaiizor.Controls.CrownMenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAlbum = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveAlbumAs = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNewAlbum = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDeleteAlbum = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPlayListM3U = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPlayListM3UNewAlbum = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPlayListM3UToCurrentAlbum = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPlaylistM3UExport = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSongInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowAllSongs = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuQueue = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveQueue = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLoadQueue = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowQueue = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDeQueue = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuScrambleQueue = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowAlternateQueue = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMore = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuScrambleQueueSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuQueueMoveToTop = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelpItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tbTool = new ReaLTaiizor.Controls.CrownToolStrip();
            this.tbPrevious = new System.Windows.Forms.ToolStripButton();
            this.tbPlayNext = new System.Windows.Forms.ToolStripButton();
            this.tbNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbShowQueue = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tbRand = new System.Windows.Forms.ToolStripButton();
            this.tbShuffle = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbQueueStack = new System.Windows.Forms.ToolStripButton();
            this.tsbToggleVolumeAndStars = new System.Windows.Forms.ToolStripButton();
            this.lbSong = new ReaLTaiizor.Controls.CrownLabel();
            this.lbTime = new ReaLTaiizor.Controls.CrownLabel();
            this.tbFind = new ReaLTaiizor.Controls.CrownTextBox();
            this.pnAudioVisualizationMain = new System.Windows.Forms.Panel();
            this.avBars = new VPKSoft.AudioVisualization.AudioVisualizationBars();
            this.avLine = new VPKSoft.AudioVisualization.AudioVisualizationPlot();
            this.scProgress = new ReaLTaiizor.Controls.ForeverTrackBar();
            this.pnListBox = new System.Windows.Forms.Panel();
            this.lbMusicScroll = new ReaLTaiizor.Controls.CrownScrollBar();
            this.lbMusic = new AmpControls.RefreshListbox();
            this.pnTools = new System.Windows.Forms.Panel();
            this.lbSongPoints = new ReaLTaiizor.Controls.CrownLabel();
            this.lbVolume = new ReaLTaiizor.Controls.CrownLabel();
            this.sliderMainVolume = new AmpControls.VolumeSlider();
            this.sliderVolumeSong = new AmpControls.VolumeSlider();
            this.lbSongVolume = new ReaLTaiizor.Controls.CrownLabel();
            this.sliderStars = new AmpControls.StarSlider();
            this.miniToolStrip = new ReaLTaiizor.Controls.CrownMenuStrip();
            this.tfMain = new ReaLTaiizor.Forms.ThemeForm();
            this.foreverMinimize1 = new ReaLTaiizor.Controls.ForeverMinimize();
            this.foreverMaximize1 = new ReaLTaiizor.Controls.ForeverMaximize();
            this.foreverClose1 = new ReaLTaiizor.Controls.ForeverClose();
            this.ssStatus.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.msMain.SuspendLayout();
            this.tbTool.SuspendLayout();
            this.pnAudioVisualizationMain.SuspendLayout();
            this.pnListBox.SuspendLayout();
            this.pnTools.SuspendLayout();
            this.tfMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmSeek
            // 
            this.tmSeek.Enabled = true;
            this.tmSeek.Tick += new System.EventHandler(this.tmSeek_Tick);
            // 
            // ssStatus
            // 
            this.ssStatus.AutoSize = false;
            this.ssStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.tlpMain.SetColumnSpan(this.ssStatus, 2);
            this.ssStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ssStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbQueueCount});
            this.ssStatus.Location = new System.Drawing.Point(0, 520);
            this.ssStatus.Name = "ssStatus";
            this.ssStatus.Padding = new System.Windows.Forms.Padding(0, 5, 0, 3);
            this.ssStatus.Size = new System.Drawing.Size(542, 25);
            this.ssStatus.SizingGrip = false;
            this.ssStatus.TabIndex = 11;
            this.ssStatus.Text = "statusStrip1";
            // 
            // lbQueueCount
            // 
            this.lbQueueCount.BackColor = System.Drawing.Color.DarkGray;
            this.lbQueueCount.Name = "lbQueueCount";
            this.lbQueueCount.Size = new System.Drawing.Size(13, 12);
            this.lbQueueCount.Text = "0";
            // 
            // odM3U
            // 
            this.odM3U.Filter = "M3U playlist files (*.m3u;*.m3u8)|*.m3u;*.m3u8";
            this.odM3U.Title = "Open playlist file";
            // 
            // sdM3U
            // 
            this.sdM3U.Filter = "M3U playlist files (*.m3u)|*.m3u|M3U unicode (UTF-8) files (*.m3u8)|*.m3u8";
            this.sdM3U.Title = "Save playlist file";
            // 
            // tmPendOperation
            // 
            this.tmPendOperation.Interval = 1000;
            this.tmPendOperation.Tick += new System.EventHandler(this.tmPendOperation_Tick);
            // 
            // tmIPCFiles
            // 
            this.tmIPCFiles.Interval = 1000;
            this.tmIPCFiles.Tick += new System.EventHandler(this.TmIPCFiles_Tick);
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMain.Controls.Add(this.msMain, 0, 0);
            this.tlpMain.Controls.Add(this.ssStatus, 0, 8);
            this.tlpMain.Controls.Add(this.tbTool, 0, 1);
            this.tlpMain.Controls.Add(this.lbSong, 0, 3);
            this.tlpMain.Controls.Add(this.lbTime, 1, 3);
            this.tlpMain.Controls.Add(this.tbFind, 0, 5);
            this.tlpMain.Controls.Add(this.pnAudioVisualizationMain, 0, 7);
            this.tlpMain.Controls.Add(this.scProgress, 0, 4);
            this.tlpMain.Controls.Add(this.pnListBox, 0, 6);
            this.tlpMain.Controls.Add(this.pnTools, 0, 2);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(2, 70);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 8;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.Size = new System.Drawing.Size(542, 545);
            this.tlpMain.TabIndex = 12;
            // 
            // msMain
            // 
            this.msMain.BackColor = System.Drawing.Color.Transparent;
            this.tlpMain.SetColumnSpan(this.msMain, 2);
            this.msMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.msMain.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.msMain.ImageScalingSize = new System.Drawing.Size(30, 30);
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuQueue,
            this.mnuHelp});
            this.msMain.Location = new System.Drawing.Point(0, 0);
            this.msMain.Name = "msMain";
            this.msMain.Padding = new System.Windows.Forms.Padding(3, 2, 0, 2);
            this.msMain.Size = new System.Drawing.Size(542, 24);
            this.msMain.TabIndex = 5;
            this.msMain.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAlbum,
            this.mnuSaveAlbumAs,
            this.mnuNewAlbum,
            this.mnuDeleteAlbum,
            this.mnuSelectAll,
            this.mnuPlayListM3U,
            this.mnuSettings,
            this.mnuSongInfo,
            this.mnuShowAllSongs});
            this.mnuFile.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "File";
            this.mnuFile.DropDownOpening += new System.EventHandler(this.MnuFile_DropDownOpening);
            // 
            // mnuAlbum
            // 
            this.mnuAlbum.BackColor = System.Drawing.Color.Transparent;
            this.mnuAlbum.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mnuAlbum.Image = global::amp.Properties.Resources.album_yellow;
            this.mnuAlbum.Name = "mnuAlbum";
            this.mnuAlbum.Size = new System.Drawing.Size(204, 36);
            this.mnuAlbum.Text = "Album";
            // 
            // mnuSaveAlbumAs
            // 
            this.mnuSaveAlbumAs.BackColor = System.Drawing.Color.Transparent;
            this.mnuSaveAlbumAs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mnuSaveAlbumAs.Image = global::amp.Properties.Resources.Save_as32;
            this.mnuSaveAlbumAs.Name = "mnuSaveAlbumAs";
            this.mnuSaveAlbumAs.Size = new System.Drawing.Size(204, 36);
            this.mnuSaveAlbumAs.Text = "Save current album as";
            this.mnuSaveAlbumAs.Click += new System.EventHandler(this.MnuSaveAlbumAs_Click);
            // 
            // mnuNewAlbum
            // 
            this.mnuNewAlbum.BackColor = System.Drawing.Color.Transparent;
            this.mnuNewAlbum.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mnuNewAlbum.Image = ((System.Drawing.Image)(resources.GetObject("mnuNewAlbum.Image")));
            this.mnuNewAlbum.Name = "mnuNewAlbum";
            this.mnuNewAlbum.Size = new System.Drawing.Size(204, 36);
            this.mnuNewAlbum.Text = "New album";
            this.mnuNewAlbum.Click += new System.EventHandler(this.mnuNewAlbum_Click);
            // 
            // mnuDeleteAlbum
            // 
            this.mnuDeleteAlbum.BackColor = System.Drawing.Color.Transparent;
            this.mnuDeleteAlbum.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mnuDeleteAlbum.Image = global::amp.Properties.Resources.remove_album;
            this.mnuDeleteAlbum.Name = "mnuDeleteAlbum";
            this.mnuDeleteAlbum.Size = new System.Drawing.Size(204, 36);
            this.mnuDeleteAlbum.Text = "Delete current album";
            this.mnuDeleteAlbum.Click += new System.EventHandler(this.MnuDeleteAlbum_Click);
            // 
            // mnuSelectAll
            // 
            this.mnuSelectAll.BackColor = System.Drawing.Color.Transparent;
            this.mnuSelectAll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mnuSelectAll.Image = ((System.Drawing.Image)(resources.GetObject("mnuSelectAll.Image")));
            this.mnuSelectAll.Name = "mnuSelectAll";
            this.mnuSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.mnuSelectAll.Size = new System.Drawing.Size(204, 36);
            this.mnuSelectAll.Text = "Select all";
            this.mnuSelectAll.Click += new System.EventHandler(this.mnuSelectAll_Click);
            // 
            // mnuPlayListM3U
            // 
            this.mnuPlayListM3U.BackColor = System.Drawing.Color.Transparent;
            this.mnuPlayListM3U.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPlayListM3UNewAlbum,
            this.mnuPlayListM3UToCurrentAlbum,
            this.mnuPlaylistM3UExport});
            this.mnuPlayListM3U.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mnuPlayListM3U.Image = global::amp.Properties.Resources.m3u2;
            this.mnuPlayListM3U.Name = "mnuPlayListM3U";
            this.mnuPlayListM3U.Size = new System.Drawing.Size(204, 36);
            this.mnuPlayListM3U.Text = "Playlist (m3u)";
            // 
            // mnuPlayListM3UNewAlbum
            // 
            this.mnuPlayListM3UNewAlbum.Name = "mnuPlayListM3UNewAlbum";
            this.mnuPlayListM3UNewAlbum.Size = new System.Drawing.Size(205, 22);
            this.mnuPlayListM3UNewAlbum.Text = "Create new album";
            this.mnuPlayListM3UNewAlbum.Click += new System.EventHandler(this.mnuPlayListM3UNewAlbum_Click);
            // 
            // mnuPlayListM3UToCurrentAlbum
            // 
            this.mnuPlayListM3UToCurrentAlbum.Name = "mnuPlayListM3UToCurrentAlbum";
            this.mnuPlayListM3UToCurrentAlbum.Size = new System.Drawing.Size(205, 22);
            this.mnuPlayListM3UToCurrentAlbum.Text = "Insert into current album";
            this.mnuPlayListM3UToCurrentAlbum.Click += new System.EventHandler(this.mnuPlayListM3UToCurrentAlbum_Click);
            // 
            // mnuPlaylistM3UExport
            // 
            this.mnuPlaylistM3UExport.Name = "mnuPlaylistM3UExport";
            this.mnuPlaylistM3UExport.Size = new System.Drawing.Size(205, 22);
            this.mnuPlaylistM3UExport.Text = "Export current album";
            this.mnuPlaylistM3UExport.Click += new System.EventHandler(this.mnuPlaylistM3UExport_Click);
            // 
            // mnuSettings
            // 
            this.mnuSettings.BackColor = System.Drawing.Color.Transparent;
            this.mnuSettings.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mnuSettings.Image = global::amp.Properties.Resources.settings;
            this.mnuSettings.Name = "mnuSettings";
            this.mnuSettings.Size = new System.Drawing.Size(204, 36);
            this.mnuSettings.Text = "Settings";
            this.mnuSettings.Click += new System.EventHandler(this.mnuSettings_Click);
            // 
            // mnuSongInfo
            // 
            this.mnuSongInfo.BackColor = System.Drawing.Color.Transparent;
            this.mnuSongInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mnuSongInfo.Image = global::amp.Properties.Resources.info;
            this.mnuSongInfo.Name = "mnuSongInfo";
            this.mnuSongInfo.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.mnuSongInfo.Size = new System.Drawing.Size(204, 36);
            this.mnuSongInfo.Text = "Song information";
            this.mnuSongInfo.Click += new System.EventHandler(this.mnuSongInfo_Click);
            // 
            // mnuShowAllSongs
            // 
            this.mnuShowAllSongs.BackColor = System.Drawing.Color.Transparent;
            this.mnuShowAllSongs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mnuShowAllSongs.Image = global::amp.Properties.Resources.list;
            this.mnuShowAllSongs.Name = "mnuShowAllSongs";
            this.mnuShowAllSongs.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.mnuShowAllSongs.Size = new System.Drawing.Size(204, 36);
            this.mnuShowAllSongs.Text = "Show all songs";
            this.mnuShowAllSongs.Click += new System.EventHandler(this.mnuShowAllSongs_Click);
            // 
            // mnuQueue
            // 
            this.mnuQueue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.mnuQueue.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSaveQueue,
            this.mnuLoadQueue,
            this.mnuShowQueue,
            this.mnuDeQueue,
            this.mnuScrambleQueue,
            this.mnuShowAlternateQueue,
            this.mnuMore});
            this.mnuQueue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mnuQueue.Name = "mnuQueue";
            this.mnuQueue.Size = new System.Drawing.Size(54, 20);
            this.mnuQueue.Text = "Queue";
            // 
            // mnuSaveQueue
            // 
            this.mnuSaveQueue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.mnuSaveQueue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mnuSaveQueue.Image = ((System.Drawing.Image)(resources.GetObject("mnuSaveQueue.Image")));
            this.mnuSaveQueue.Name = "mnuSaveQueue";
            this.mnuSaveQueue.Size = new System.Drawing.Size(221, 36);
            this.mnuSaveQueue.Text = "Save queue";
            this.mnuSaveQueue.Click += new System.EventHandler(this.saveQueueToolStripMenuItem_Click);
            // 
            // mnuLoadQueue
            // 
            this.mnuLoadQueue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.mnuLoadQueue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mnuLoadQueue.Image = global::amp.Properties.Resources.open;
            this.mnuLoadQueue.Name = "mnuLoadQueue";
            this.mnuLoadQueue.Size = new System.Drawing.Size(221, 36);
            this.mnuLoadQueue.Text = "Load saved queue";
            this.mnuLoadQueue.Click += new System.EventHandler(this.mnuLoadQueue_Click);
            // 
            // mnuShowQueue
            // 
            this.mnuShowQueue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.mnuShowQueue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mnuShowQueue.Image = global::amp.Properties.Resources.amp_queue;
            this.mnuShowQueue.Name = "mnuShowQueue";
            this.mnuShowQueue.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.mnuShowQueue.Size = new System.Drawing.Size(221, 36);
            this.mnuShowQueue.Text = "Show queue";
            this.mnuShowQueue.Click += new System.EventHandler(this.tbShowQueue_Click);
            // 
            // mnuDeQueue
            // 
            this.mnuDeQueue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.mnuDeQueue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mnuDeQueue.Image = global::amp.Properties.Resources.amp_dequeue;
            this.mnuDeQueue.Name = "mnuDeQueue";
            this.mnuDeQueue.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.mnuDeQueue.Size = new System.Drawing.Size(221, 36);
            this.mnuDeQueue.Text = "Clear queue";
            this.mnuDeQueue.Click += new System.EventHandler(this.mnuDeQueue_Click);
            // 
            // mnuScrambleQueue
            // 
            this.mnuScrambleQueue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.mnuScrambleQueue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mnuScrambleQueue.Image = global::amp.Properties.Resources.amp_scramble_queue;
            this.mnuScrambleQueue.Name = "mnuScrambleQueue";
            this.mnuScrambleQueue.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.mnuScrambleQueue.Size = new System.Drawing.Size(221, 36);
            this.mnuScrambleQueue.Text = "Scramble queue";
            this.mnuScrambleQueue.Click += new System.EventHandler(this.mnuScrambleQueue_Click);
            // 
            // mnuShowAlternateQueue
            // 
            this.mnuShowAlternateQueue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.mnuShowAlternateQueue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mnuShowAlternateQueue.Image = global::amp.Properties.Resources.amp_alternate_queue;
            this.mnuShowAlternateQueue.Name = "mnuShowAlternateQueue";
            this.mnuShowAlternateQueue.ShortcutKeys = System.Windows.Forms.Keys.F8;
            this.mnuShowAlternateQueue.Size = new System.Drawing.Size(221, 36);
            this.mnuShowAlternateQueue.Text = "Show alternate queue";
            this.mnuShowAlternateQueue.Click += new System.EventHandler(this.mnuShowAlternateQueue_Click);
            // 
            // mnuMore
            // 
            this.mnuMore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.mnuMore.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuScrambleQueueSelected,
            this.mnuQueueMoveToTop});
            this.mnuMore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mnuMore.Image = global::amp.Properties.Resources.amp_dots;
            this.mnuMore.Name = "mnuMore";
            this.mnuMore.Size = new System.Drawing.Size(221, 36);
            this.mnuMore.Text = "More...";
            // 
            // mnuScrambleQueueSelected
            // 
            this.mnuScrambleQueueSelected.Name = "mnuScrambleQueueSelected";
            this.mnuScrambleQueueSelected.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F7)));
            this.mnuScrambleQueueSelected.Size = new System.Drawing.Size(275, 22);
            this.mnuScrambleQueueSelected.Text = "Scramble selected in queue";
            this.mnuScrambleQueueSelected.Click += new System.EventHandler(this.mnuScrambleQueueSelected_Click);
            // 
            // mnuQueueMoveToTop
            // 
            this.mnuQueueMoveToTop.Name = "mnuQueueMoveToTop";
            this.mnuQueueMoveToTop.Size = new System.Drawing.Size(275, 22);
            this.mnuQueueMoveToTop.Text = "Move selected to the top of the queue";
            this.mnuQueueMoveToTop.Click += new System.EventHandler(this.mnuQueueMoveToTop_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHelpItem,
            this.mnuAbout});
            this.mnuHelp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(44, 20);
            this.mnuHelp.Text = "Help";
            // 
            // mnuHelpItem
            // 
            this.mnuHelpItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.mnuHelpItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mnuHelpItem.Image = global::amp.Properties.Resources.help;
            this.mnuHelpItem.Name = "mnuHelpItem";
            this.mnuHelpItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.mnuHelpItem.Size = new System.Drawing.Size(132, 36);
            this.mnuHelpItem.Text = "Help";
            this.mnuHelpItem.Click += new System.EventHandler(this.mnuHelpItem_Click);
            // 
            // mnuAbout
            // 
            this.mnuAbout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.mnuAbout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.mnuAbout.Image = global::amp.Properties.Resources.info1;
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(132, 36);
            this.mnuAbout.Text = "About";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // tbTool
            // 
            this.tbTool.AutoSize = false;
            this.tbTool.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.tlpMain.SetColumnSpan(this.tbTool, 2);
            this.tbTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbTool.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tbTool.ImageScalingSize = new System.Drawing.Size(30, 30);
            this.tbTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbPrevious,
            this.tbPlayNext,
            this.tbNext,
            this.toolStripSeparator1,
            this.tbShowQueue,
            this.toolStripSeparator2,
            this.tbRand,
            this.tbShuffle,
            this.toolStripSeparator3,
            this.tsbQueueStack,
            this.tsbToggleVolumeAndStars});
            this.tbTool.Location = new System.Drawing.Point(0, 24);
            this.tbTool.Name = "tbTool";
            this.tbTool.Padding = new System.Windows.Forms.Padding(5, 0, 1, 0);
            this.tbTool.Size = new System.Drawing.Size(542, 37);
            this.tbTool.TabIndex = 8;
            // 
            // tbPrevious
            // 
            this.tbPrevious.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.tbPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbPrevious.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tbPrevious.Image = global::amp.Properties.Resources.amp_back;
            this.tbPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbPrevious.Name = "tbPrevious";
            this.tbPrevious.Size = new System.Drawing.Size(34, 34);
            this.tbPrevious.ToolTipText = "Previous song";
            this.tbPrevious.Click += new System.EventHandler(this.tbPrevious_Click);
            // 
            // tbPlayNext
            // 
            this.tbPlayNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.tbPlayNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbPlayNext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tbPlayNext.Image = global::amp.Properties.Resources.amp_play;
            this.tbPlayNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbPlayNext.Name = "tbPlayNext";
            this.tbPlayNext.Size = new System.Drawing.Size(34, 34);
            this.tbPlayNext.ToolTipText = "Play";
            this.tbPlayNext.Click += new System.EventHandler(this.tbPlayNext_Click);
            // 
            // tbNext
            // 
            this.tbNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.tbNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbNext.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tbNext.Image = global::amp.Properties.Resources.amp_forward;
            this.tbNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbNext.Name = "tbNext";
            this.tbNext.Size = new System.Drawing.Size(34, 34);
            this.tbNext.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.tbNext.ToolTipText = "Next song";
            this.tbNext.Click += new System.EventHandler(this.tbNext_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AutoSize = false;
            this.toolStripSeparator1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.toolStripSeparator1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripSeparator1.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(15, 37);
            // 
            // tbShowQueue
            // 
            this.tbShowQueue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.tbShowQueue.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbShowQueue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tbShowQueue.Image = global::amp.Properties.Resources.amp_queue;
            this.tbShowQueue.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbShowQueue.Name = "tbShowQueue";
            this.tbShowQueue.Size = new System.Drawing.Size(34, 34);
            this.tbShowQueue.Text = "Show queue";
            this.tbShowQueue.ToolTipText = "Show queue";
            this.tbShowQueue.Click += new System.EventHandler(this.tbShowQueue_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AutoSize = false;
            this.toolStripSeparator2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.toolStripSeparator2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripSeparator2.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(15, 37);
            // 
            // tbRand
            // 
            this.tbRand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.tbRand.Checked = true;
            this.tbRand.CheckOnClick = true;
            this.tbRand.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tbRand.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbRand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tbRand.Image = global::amp.Properties.Resources.amp_shuffle;
            this.tbRand.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbRand.Name = "tbRand";
            this.tbRand.Size = new System.Drawing.Size(34, 34);
            this.tbRand.Text = "tbRand";
            this.tbRand.ToolTipText = "Random";
            // 
            // tbShuffle
            // 
            this.tbShuffle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.tbShuffle.Checked = true;
            this.tbShuffle.CheckOnClick = true;
            this.tbShuffle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tbShuffle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbShuffle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tbShuffle.Image = global::amp.Properties.Resources.amp_repeat;
            this.tbShuffle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbShuffle.Name = "tbShuffle";
            this.tbShuffle.Size = new System.Drawing.Size(34, 34);
            this.tbShuffle.Text = "tbSuffle";
            this.tbShuffle.ToolTipText = "Continous playback";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.AutoSize = false;
            this.toolStripSeparator3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.toolStripSeparator3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.toolStripSeparator3.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(15, 37);
            // 
            // tsbQueueStack
            // 
            this.tsbQueueStack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.tsbQueueStack.CheckOnClick = true;
            this.tsbQueueStack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbQueueStack.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tsbQueueStack.Image = global::amp.Properties.Resources.stack;
            this.tsbQueueStack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbQueueStack.Name = "tsbQueueStack";
            this.tsbQueueStack.Size = new System.Drawing.Size(34, 34);
            this.tsbQueueStack.Text = "Stack queue";
            // 
            // tsbToggleVolumeAndStars
            // 
            this.tsbToggleVolumeAndStars.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbToggleVolumeAndStars.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.tsbToggleVolumeAndStars.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbToggleVolumeAndStars.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tsbToggleVolumeAndStars.Image = global::amp.Properties.Resources.tick_down;
            this.tsbToggleVolumeAndStars.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbToggleVolumeAndStars.Name = "tsbToggleVolumeAndStars";
            this.tsbToggleVolumeAndStars.Size = new System.Drawing.Size(34, 34);
            this.tsbToggleVolumeAndStars.Text = "Toggle volume and points visibility";
            this.tsbToggleVolumeAndStars.Click += new System.EventHandler(this.tsbToggleVolumeAndStars_Click);
            // 
            // lbSong
            // 
            this.lbSong.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSong.AutoEllipsis = true;
            this.lbSong.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lbSong.Location = new System.Drawing.Point(3, 181);
            this.lbSong.Name = "lbSong";
            this.lbSong.Size = new System.Drawing.Size(427, 13);
            this.lbSong.TabIndex = 11;
            this.lbSong.Text = "-";
            // 
            // lbTime
            // 
            this.lbTime.AutoSize = true;
            this.lbTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lbTime.Location = new System.Drawing.Point(436, 181);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(103, 13);
            this.lbTime.TabIndex = 10;
            this.lbTime.Text = "00:00";
            this.lbTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbFind
            // 
            this.tbFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFind.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.tbFind.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tlpMain.SetColumnSpan(this.tbFind, 2);
            this.tbFind.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.tbFind.Location = new System.Drawing.Point(3, 220);
            this.tbFind.Name = "tbFind";
            this.tbFind.Size = new System.Drawing.Size(536, 21);
            this.tbFind.TabIndex = 13;
            this.tbFind.Click += new System.EventHandler(this.tbFind_Click);
            this.tbFind.TextChanged += new System.EventHandler(this.tbFind_TextChanged);
            this.tbFind.Enter += new System.EventHandler(this.tbFind_Enter);
            this.tbFind.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbFind_KeyDown);
            // 
            // pnAudioVisualizationMain
            // 
            this.tlpMain.SetColumnSpan(this.pnAudioVisualizationMain, 2);
            this.pnAudioVisualizationMain.Controls.Add(this.avBars);
            this.pnAudioVisualizationMain.Controls.Add(this.avLine);
            this.pnAudioVisualizationMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnAudioVisualizationMain.Location = new System.Drawing.Point(0, 479);
            this.pnAudioVisualizationMain.Margin = new System.Windows.Forms.Padding(0);
            this.pnAudioVisualizationMain.Name = "pnAudioVisualizationMain";
            this.pnAudioVisualizationMain.Size = new System.Drawing.Size(542, 41);
            this.pnAudioVisualizationMain.TabIndex = 15;
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
            this.avBars.CustomWindowFunc = null;
            this.avBars.DisplayHertzLabels = false;
            this.avBars.DrawWithGradient = true;
            this.avBars.FftWindowType = VPKSoft.AudioVisualization.WindowType.Hanning;
            this.avBars.HertzSpan = 92;
            this.avBars.Location = new System.Drawing.Point(14, 3);
            this.avBars.MinorityCropOnBarLevel = false;
            this.avBars.MinorityCropPercentage = 3;
            this.avBars.MinorityPercentageStepping = 1000;
            this.avBars.Name = "avBars";
            this.avBars.NoiseTolerance = 1D;
            this.avBars.RefreshRate = 15;
            this.avBars.RelativeView = true;
            this.avBars.RelativeViewTimeAdjust = 1.001D;
            this.avBars.Size = new System.Drawing.Size(469, 12);
            this.avBars.TabIndex = 10;
            // 
            // avLine
            // 
            this.avLine.ColorAudioChannelLeft = System.Drawing.Color.Aqua;
            this.avLine.ColorAudioChannelRight = System.Drawing.Color.LimeGreen;
            this.avLine.ColorHertzLabels = System.Drawing.Color.Magenta;
            this.avLine.CombineChannels = false;
            this.avLine.CustomWindowFunc = null;
            this.avLine.DisplayHertzLabels = false;
            this.avLine.FftWindowType = VPKSoft.AudioVisualization.WindowType.Hanning;
            this.avLine.LineCurveTension = 1F;
            this.avLine.LineDrawMode = VPKSoft.AudioVisualization.CommonClasses.BaseClasses.LineDrawMode.Line;
            this.avLine.Location = new System.Drawing.Point(40, 22);
            this.avLine.MinorityCropOnBarLevel = false;
            this.avLine.MinorityCropPercentage = 2;
            this.avLine.MinorityPercentageStepping = 1000;
            this.avLine.Name = "avLine";
            this.avLine.NoiseTolerance = 1D;
            this.avLine.RefreshRate = 30;
            this.avLine.Size = new System.Drawing.Size(469, 12);
            this.avLine.TabIndex = 9;
            this.avLine.UseAntiAliasing = true;
            // 
            // scProgress
            // 
            this.scProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scProgress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.scProgress.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.tlpMain.SetColumnSpan(this.scProgress, 2);
            this.scProgress.Cursor = System.Windows.Forms.Cursors.Hand;
            this.scProgress.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.scProgress.ForeColor = System.Drawing.Color.White;
            this.scProgress.HatchColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(148)))), ((int)(((byte)(92)))));
            this.scProgress.Location = new System.Drawing.Point(0, 194);
            this.scProgress.Margin = new System.Windows.Forms.Padding(0);
            this.scProgress.Maximum = 100;
            this.scProgress.Minimum = 0;
            this.scProgress.MinimumSize = new System.Drawing.Size(47, 22);
            this.scProgress.Name = "scProgress";
            this.scProgress.ShowValue = false;
            this.scProgress.Size = new System.Drawing.Size(542, 23);
            this.scProgress.SliderColor = System.Drawing.Color.White;
            this.scProgress.Style = ReaLTaiizor.Controls.ForeverTrackBar._Style.Slider;
            this.scProgress.TabIndex = 16;
            this.scProgress.TrackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.scProgress.Value = 0;
            this.scProgress.Scroll += new ReaLTaiizor.Controls.ForeverTrackBar.ScrollEventHandler(this.scProgress_Scroll);
            // 
            // pnListBox
            // 
            this.tlpMain.SetColumnSpan(this.pnListBox, 2);
            this.pnListBox.Controls.Add(this.lbMusicScroll);
            this.pnListBox.Controls.Add(this.lbMusic);
            this.pnListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnListBox.Location = new System.Drawing.Point(0, 244);
            this.pnListBox.Margin = new System.Windows.Forms.Padding(0);
            this.pnListBox.Name = "pnListBox";
            this.pnListBox.Size = new System.Drawing.Size(542, 235);
            this.pnListBox.TabIndex = 17;
            // 
            // lbMusicScroll
            // 
            this.lbMusicScroll.Dock = System.Windows.Forms.DockStyle.Right;
            this.lbMusicScroll.Location = new System.Drawing.Point(525, 0);
            this.lbMusicScroll.Name = "lbMusicScroll";
            this.lbMusicScroll.Size = new System.Drawing.Size(17, 235);
            this.lbMusicScroll.TabIndex = 15;
            this.lbMusicScroll.Text = "crownScrollBar1";
            this.lbMusicScroll.ValueChanged += new System.EventHandler<ReaLTaiizor.Util.ScrollValueEventArgs>(this.lbMusicScroll_ValueChanged);
            // 
            // lbMusic
            // 
            this.lbMusic.AllowDrop = true;
            this.lbMusic.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.lbMusic.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbMusic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbMusic.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lbMusic.ForeColor = System.Drawing.Color.White;
            this.lbMusic.FormattingEnabled = true;
            this.lbMusic.IntegralHeight = false;
            this.lbMusic.ItemHeight = 18;
            this.lbMusic.Location = new System.Drawing.Point(0, 0);
            this.lbMusic.Name = "lbMusic";
            this.lbMusic.ScrollAlwaysVisible = true;
            this.lbMusic.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbMusic.Size = new System.Drawing.Size(542, 235);
            this.lbMusic.TabIndex = 14;
            this.lbMusic.VScrollPosition = 0;
            this.lbMusic.ItemsChanged += new System.EventHandler(this.lbMusic_ItemsChanged);
            this.lbMusic.VScrollChanged += new System.EventHandler<AmpControls.VScrollChangedEventArgs>(this.lbMusic_VScrollChanged);
            this.lbMusic.DragDrop += new System.Windows.Forms.DragEventHandler(this.lbMusic_DragDrop);
            this.lbMusic.DragEnter += new System.Windows.Forms.DragEventHandler(this.lbMusic_DragEnter);
            this.lbMusic.DragOver += new System.Windows.Forms.DragEventHandler(this.lbMusic_DragOver);
            this.lbMusic.DoubleClick += new System.EventHandler(this.lbMusic_DoubleClick);
            this.lbMusic.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbMusic_KeyDown);
            // 
            // pnTools
            // 
            this.tlpMain.SetColumnSpan(this.pnTools, 2);
            this.pnTools.Controls.Add(this.lbSongPoints);
            this.pnTools.Controls.Add(this.lbVolume);
            this.pnTools.Controls.Add(this.sliderMainVolume);
            this.pnTools.Controls.Add(this.sliderVolumeSong);
            this.pnTools.Controls.Add(this.lbSongVolume);
            this.pnTools.Controls.Add(this.sliderStars);
            this.pnTools.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnTools.Location = new System.Drawing.Point(3, 64);
            this.pnTools.Name = "pnTools";
            this.pnTools.Size = new System.Drawing.Size(536, 114);
            this.pnTools.TabIndex = 11;
            // 
            // lbSongPoints
            // 
            this.lbSongPoints.AutoSize = true;
            this.lbSongPoints.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSongPoints.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lbSongPoints.Location = new System.Drawing.Point(8, 84);
            this.lbSongPoints.Name = "lbSongPoints";
            this.lbSongPoints.Size = new System.Drawing.Size(39, 13);
            this.lbSongPoints.TabIndex = 14;
            this.lbSongPoints.Text = "Points:";
            this.lbSongPoints.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbVolume
            // 
            this.lbVolume.AutoSize = true;
            this.lbVolume.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbVolume.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lbVolume.Location = new System.Drawing.Point(8, 45);
            this.lbVolume.Name = "lbVolume";
            this.lbVolume.Size = new System.Drawing.Size(45, 13);
            this.lbVolume.TabIndex = 13;
            this.lbVolume.Text = "Volume:";
            this.lbVolume.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sliderMainVolume
            // 
            this.sliderMainVolume.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sliderMainVolume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sliderMainVolume.ColorMaximum = System.Drawing.Color.OrangeRed;
            this.sliderMainVolume.ColorMinimum = System.Drawing.Color.Yellow;
            this.sliderMainVolume.CurrentValue = 50;
            this.sliderMainVolume.CurrentValueFractional = 50D;
            this.sliderMainVolume.ImageSliderTracker = global::amp.Properties.Resources.volume_slide_2;
            this.sliderMainVolume.ImageVolumeLeft = ((System.Drawing.Image)(resources.GetObject("sliderMainVolume.ImageVolumeLeft")));
            this.sliderMainVolume.ImageVolumeRight = ((System.Drawing.Image)(resources.GetObject("sliderMainVolume.ImageVolumeRight")));
            this.sliderMainVolume.LeftImageVisible = true;
            this.sliderMainVolume.Location = new System.Drawing.Point(131, 36);
            this.sliderMainVolume.MaximumValue = 100;
            this.sliderMainVolume.MinimumValue = 0;
            this.sliderMainVolume.Name = "sliderMainVolume";
            this.sliderMainVolume.RightImageVisible = true;
            this.sliderMainVolume.Size = new System.Drawing.Size(401, 34);
            this.sliderMainVolume.TabIndex = 2;
            this.sliderMainVolume.ValueChanged += new System.EventHandler<AmpControls.SliderValueChangedEventArgs>(this.sliderMainVolume_ValueChanged);
            // 
            // sliderVolumeSong
            // 
            this.sliderVolumeSong.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sliderVolumeSong.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sliderVolumeSong.ColorMaximum = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(1)))), ((int)(((byte)(7)))));
            this.sliderVolumeSong.ColorMinimum = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(21)))), ((int)(((byte)(164)))));
            this.sliderVolumeSong.CurrentValue = 250;
            this.sliderVolumeSong.CurrentValueFractional = 250D;
            this.sliderVolumeSong.ImageSliderTracker = global::amp.Properties.Resources.volume_slide_2;
            this.sliderVolumeSong.ImageVolumeLeft = ((System.Drawing.Image)(resources.GetObject("sliderVolumeSong.ImageVolumeLeft")));
            this.sliderVolumeSong.ImageVolumeRight = ((System.Drawing.Image)(resources.GetObject("sliderVolumeSong.ImageVolumeRight")));
            this.sliderVolumeSong.LeftImageVisible = true;
            this.sliderVolumeSong.Location = new System.Drawing.Point(131, 3);
            this.sliderVolumeSong.MaximumValue = 500;
            this.sliderVolumeSong.MinimumValue = 0;
            this.sliderVolumeSong.Name = "sliderVolumeSong";
            this.sliderVolumeSong.RightImageVisible = true;
            this.sliderVolumeSong.Size = new System.Drawing.Size(401, 34);
            this.sliderVolumeSong.TabIndex = 12;
            this.sliderVolumeSong.ValueChanged += new System.EventHandler<AmpControls.SliderValueChangedEventArgs>(this.sliderVolumeSong_ValueChanged);
            // 
            // lbSongVolume
            // 
            this.lbSongVolume.AutoSize = true;
            this.lbSongVolume.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSongVolume.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.lbSongVolume.Location = new System.Drawing.Point(7, 12);
            this.lbSongVolume.Name = "lbSongVolume";
            this.lbSongVolume.Size = new System.Drawing.Size(77, 13);
            this.lbSongVolume.TabIndex = 11;
            this.lbSongVolume.Text = "Volume (song):";
            this.lbSongVolume.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sliderStars
            // 
            this.sliderStars.CurrentValue = 0;
            this.sliderStars.CurrentValueFractional = 0D;
            this.sliderStars.ImageStars = null;
            this.sliderStars.Location = new System.Drawing.Point(131, 73);
            this.sliderStars.Margin = new System.Windows.Forms.Padding(0);
            this.sliderStars.MaximumSize = new System.Drawing.Size(176, 35);
            this.sliderStars.MaximumValue = 1000;
            this.sliderStars.MinimumSize = new System.Drawing.Size(176, 35);
            this.sliderStars.MinimumValue = 0;
            this.sliderStars.Name = "sliderStars";
            this.sliderStars.Size = new System.Drawing.Size(176, 35);
            this.sliderStars.TabIndex = 1;
            this.sliderStars.ValueChanged += new System.EventHandler<AmpControls.SliderValueChangedEventArgs>(this.sliderStars_ValueChanged);
            // 
            // miniToolStrip
            // 
            this.miniToolStrip.AccessibleName = "New item selection";
            this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.miniToolStrip.AutoSize = false;
            this.miniToolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.miniToolStrip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.miniToolStrip.Location = new System.Drawing.Point(44, 2);
            this.miniToolStrip.Name = "miniToolStrip";
            this.miniToolStrip.Padding = new System.Windows.Forms.Padding(3, 2, 0, 2);
            this.miniToolStrip.Size = new System.Drawing.Size(543, 24);
            this.miniToolStrip.TabIndex = 0;
            // 
            // tfMain
            // 
            this.tfMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(41)))), ((int)(((byte)(50)))));
            this.tfMain.Controls.Add(this.foreverMinimize1);
            this.tfMain.Controls.Add(this.foreverMaximize1);
            this.tfMain.Controls.Add(this.foreverClose1);
            this.tfMain.Controls.Add(this.tlpMain);
            this.tfMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tfMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.tfMain.Image = global::amp.Properties.Resources.icon;
            this.tfMain.Location = new System.Drawing.Point(0, 0);
            this.tfMain.Margin = new System.Windows.Forms.Padding(0);
            this.tfMain.Name = "tfMain";
            this.tfMain.Padding = new System.Windows.Forms.Padding(2, 70, 5, 5);
            this.tfMain.RoundCorners = true;
            this.tfMain.Sizable = true;
            this.tfMain.Size = new System.Drawing.Size(549, 620);
            this.tfMain.SmartBounds = true;
            this.tfMain.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.tfMain.TabIndex = 13;
            this.tfMain.Text = "amp#";
            this.tfMain.MouseLeave += new System.EventHandler(this.tfMain_MouseLeave);
            // 
            // foreverMinimize1
            // 
            this.foreverMinimize1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.foreverMinimize1.BackColor = System.Drawing.Color.White;
            this.foreverMinimize1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.foreverMinimize1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.foreverMinimize1.DefaultLocation = true;
            this.foreverMinimize1.DownColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.foreverMinimize1.Font = new System.Drawing.Font("Marlett", 12F);
            this.foreverMinimize1.Location = new System.Drawing.Point(471, 16);
            this.foreverMinimize1.Name = "foreverMinimize1";
            this.foreverMinimize1.OverColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.foreverMinimize1.Size = new System.Drawing.Size(18, 18);
            this.foreverMinimize1.TabIndex = 15;
            this.foreverMinimize1.Text = "foreverMinimize1";
            this.foreverMinimize1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            // 
            // foreverMaximize1
            // 
            this.foreverMaximize1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.foreverMaximize1.BackColor = System.Drawing.Color.White;
            this.foreverMaximize1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.foreverMaximize1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.foreverMaximize1.DefaultLocation = true;
            this.foreverMaximize1.DownColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.foreverMaximize1.Enabled = false;
            this.foreverMaximize1.Font = new System.Drawing.Font("Marlett", 12F);
            this.foreverMaximize1.Location = new System.Drawing.Point(495, 16);
            this.foreverMaximize1.Name = "foreverMaximize1";
            this.foreverMaximize1.OverColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.foreverMaximize1.Size = new System.Drawing.Size(18, 18);
            this.foreverMaximize1.TabIndex = 14;
            this.foreverMaximize1.Text = "foreverMaximize1";
            this.foreverMaximize1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            // 
            // foreverClose1
            // 
            this.foreverClose1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.foreverClose1.BackColor = System.Drawing.Color.White;
            this.foreverClose1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.foreverClose1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.foreverClose1.DefaultLocation = true;
            this.foreverClose1.DownColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.foreverClose1.Font = new System.Drawing.Font("Marlett", 10F);
            this.foreverClose1.Location = new System.Drawing.Point(519, 16);
            this.foreverClose1.Name = "foreverClose1";
            this.foreverClose1.OverColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.foreverClose1.Size = new System.Drawing.Size(18, 18);
            this.foreverClose1.TabIndex = 13;
            this.foreverClose1.Text = "foreverClose1";
            this.foreverClose1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            // 
            // FormMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(549, 620);
            this.Controls.Add(this.tfMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.msMain;
            this.MinimumSize = new System.Drawing.Size(261, 61);
            this.Name = "FormMain";
            this.Text = "amp#";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.Shown += new System.EventHandler(this.MainWindow_Shown);
            this.ResizeEnd += new System.EventHandler(this.MainWindow_LocationChanged);
            this.LocationChanged += new System.EventHandler(this.MainWindow_LocationChanged);
            this.ssStatus.ResumeLayout(false);
            this.ssStatus.PerformLayout();
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.tbTool.ResumeLayout(false);
            this.tbTool.PerformLayout();
            this.pnAudioVisualizationMain.ResumeLayout(false);
            this.pnListBox.ResumeLayout(false);
            this.pnTools.ResumeLayout(false);
            this.pnTools.PerformLayout();
            this.tfMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Timer tmSeek;
        private ReaLTaiizor.Controls.CrownStatusStrip ssStatus;
        private ToolStripStatusLabel lbQueueCount;
        private OpenFileDialog odM3U;
        private SaveFileDialog sdM3U;
        private Timer tmPendOperation;
        private Timer tmIPCFiles;
        private TableLayoutPanel tlpMain;
        private ReaLTaiizor.Controls.CrownMenuStrip msMain;
        private ToolStripMenuItem mnuFile;
        private ToolStripMenuItem mnuAlbum;
        private ToolStripMenuItem mnuSaveAlbumAs;
        private ToolStripMenuItem mnuNewAlbum;
        private ToolStripMenuItem mnuDeleteAlbum;
        private ToolStripMenuItem mnuSelectAll;
        private ToolStripMenuItem mnuPlayListM3U;
        private ToolStripMenuItem mnuPlayListM3UNewAlbum;
        private ToolStripMenuItem mnuPlayListM3UToCurrentAlbum;
        private ToolStripMenuItem mnuPlaylistM3UExport;
        private ToolStripMenuItem mnuSettings;
        private ToolStripMenuItem mnuSongInfo;
        private ToolStripMenuItem mnuShowAllSongs;
        private ToolStripMenuItem mnuQueue;
        private ToolStripMenuItem mnuSaveQueue;
        private ToolStripMenuItem mnuLoadQueue;
        private ToolStripMenuItem mnuShowQueue;
        private ToolStripMenuItem mnuDeQueue;
        private ToolStripMenuItem mnuScrambleQueue;
        private ToolStripMenuItem mnuShowAlternateQueue;
        private ToolStripMenuItem mnuMore;
        private ToolStripMenuItem mnuScrambleQueueSelected;
        private ToolStripMenuItem mnuQueueMoveToTop;
        private ToolStripMenuItem mnuHelp;
        private ToolStripMenuItem mnuHelpItem;
        private ToolStripMenuItem mnuAbout;
        private ReaLTaiizor.Controls.CrownToolStrip tbTool;
        private ToolStripButton tbPrevious;
        private ToolStripButton tbPlayNext;
        private ToolStripButton tbNext;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton tbShowQueue;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton tbRand;
        private ToolStripButton tbShuffle;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton tsbQueueStack;
        private RefreshListbox lbMusic;
        private ReaLTaiizor.Controls.CrownLabel lbSong;
        private ReaLTaiizor.Controls.CrownTextBox tbFind;
        private Panel pnTools;
        private Panel pnAudioVisualizationMain;
        private VPKSoft.AudioVisualization.AudioVisualizationBars avBars;
        private VPKSoft.AudioVisualization.AudioVisualizationPlot avLine;
        private ReaLTaiizor.Controls.ForeverTrackBar scProgress;
        private ReaLTaiizor.Controls.CrownMenuStrip miniToolStrip;
        private ReaLTaiizor.Forms.ThemeForm tfMain;
        private Panel pnListBox;
        private ReaLTaiizor.Controls.CrownScrollBar lbMusicScroll;
        private AmpControls.StarSlider sliderStars;
        private AmpControls.VolumeSlider sliderMainVolume;
        private ReaLTaiizor.Controls.CrownLabel lbTime;
        private AmpControls.VolumeSlider sliderVolumeSong;
        private ReaLTaiizor.Controls.CrownLabel lbSongVolume;
        private ReaLTaiizor.Controls.CrownLabel lbVolume;
        private ReaLTaiizor.Controls.CrownLabel lbSongPoints;
        private ToolStripButton tsbToggleVolumeAndStars;
        private ReaLTaiizor.Controls.ForeverMinimize foreverMinimize1;
        private ReaLTaiizor.Controls.ForeverMaximize foreverMaximize1;
        private ReaLTaiizor.Controls.ForeverClose foreverClose1;
    }
}

