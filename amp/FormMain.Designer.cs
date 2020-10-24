using System.ComponentModel;
using System.Windows.Forms;
using amp.UtilityClasses.Controls;

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
            this.msMain = new System.Windows.Forms.MenuStrip();
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
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelpItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tbTool = new System.Windows.Forms.ToolStrip();
            this.tbRand = new System.Windows.Forms.ToolStripButton();
            this.tbShuffle = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbQueueStack = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tbPrevious = new System.Windows.Forms.ToolStripButton();
            this.tbPlayNext = new System.Windows.Forms.ToolStripButton();
            this.tbNext = new System.Windows.Forms.ToolStripButton();
            this.tbShowQueue = new System.Windows.Forms.ToolStripButton();
            this.ssStatus = new System.Windows.Forms.StatusStrip();
            this.lbQueueCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.odM3U = new System.Windows.Forms.OpenFileDialog();
            this.sdM3U = new System.Windows.Forms.SaveFileDialog();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.lbMusic = new amp.UtilityClasses.Controls.RefreshListbox();
            this.lbSong = new System.Windows.Forms.Label();
            this.lbTime = new System.Windows.Forms.Label();
            this.tbFind = new System.Windows.Forms.TextBox();
            this.scProgress = new System.Windows.Forms.HScrollBar();
            this.pnTools = new System.Windows.Forms.Panel();
            this.pnStars0 = new System.Windows.Forms.Panel();
            this.pnStars1 = new System.Windows.Forms.Panel();
            this.pnVol1 = new System.Windows.Forms.Panel();
            this.pnVol2 = new System.Windows.Forms.Panel();
            this.pnAudioVisualizationMain = new System.Windows.Forms.Panel();
            this.avBars = new VPKSoft.AudioVisualization.AudioVisualizationBars();
            this.avLine = new VPKSoft.AudioVisualization.AudioVisualizationPlot();
            this.tmPendOperation = new System.Windows.Forms.Timer(this.components);
            this.tmIPCFiles = new System.Windows.Forms.Timer(this.components);
            this.msMain.SuspendLayout();
            this.tbTool.SuspendLayout();
            this.ssStatus.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.pnTools.SuspendLayout();
            this.pnStars0.SuspendLayout();
            this.pnVol1.SuspendLayout();
            this.pnAudioVisualizationMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmSeek
            // 
            this.tmSeek.Enabled = true;
            this.tmSeek.Tick += new System.EventHandler(this.tmSeek_Tick);
            // 
            // msMain
            // 
            this.msMain.ImageScalingSize = new System.Drawing.Size(30, 30);
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuQueue,
            this.mnuHelp});
            this.msMain.Location = new System.Drawing.Point(0, 0);
            this.msMain.Name = "msMain";
            this.msMain.Size = new System.Drawing.Size(549, 24);
            this.msMain.TabIndex = 5;
            this.msMain.Text = "menuStrip1";
            // 
            // mnuFile
            // 
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
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "File";
            this.mnuFile.DropDownOpening += new System.EventHandler(this.MnuFile_DropDownOpening);
            // 
            // mnuAlbum
            // 
            this.mnuAlbum.Image = global::amp.Properties.Resources.album_yellow;
            this.mnuAlbum.Name = "mnuAlbum";
            this.mnuAlbum.Size = new System.Drawing.Size(190, 22);
            this.mnuAlbum.Text = "Album";
            // 
            // mnuSaveAlbumAs
            // 
            this.mnuSaveAlbumAs.Image = global::amp.Properties.Resources.Save_as32;
            this.mnuSaveAlbumAs.Name = "mnuSaveAlbumAs";
            this.mnuSaveAlbumAs.Size = new System.Drawing.Size(190, 22);
            this.mnuSaveAlbumAs.Text = "Save current album as";
            this.mnuSaveAlbumAs.Click += new System.EventHandler(this.MnuSaveAlbumAs_Click);
            // 
            // mnuNewAlbum
            // 
            this.mnuNewAlbum.Image = ((System.Drawing.Image)(resources.GetObject("mnuNewAlbum.Image")));
            this.mnuNewAlbum.Name = "mnuNewAlbum";
            this.mnuNewAlbum.Size = new System.Drawing.Size(190, 22);
            this.mnuNewAlbum.Text = "New album";
            this.mnuNewAlbum.Click += new System.EventHandler(this.mnuNewAlbum_Click);
            // 
            // mnuDeleteAlbum
            // 
            this.mnuDeleteAlbum.Image = global::amp.Properties.Resources.remove_album;
            this.mnuDeleteAlbum.Name = "mnuDeleteAlbum";
            this.mnuDeleteAlbum.Size = new System.Drawing.Size(190, 22);
            this.mnuDeleteAlbum.Text = "Delete current album";
            this.mnuDeleteAlbum.Click += new System.EventHandler(this.MnuDeleteAlbum_Click);
            // 
            // mnuSelectAll
            // 
            this.mnuSelectAll.Image = ((System.Drawing.Image)(resources.GetObject("mnuSelectAll.Image")));
            this.mnuSelectAll.Name = "mnuSelectAll";
            this.mnuSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.mnuSelectAll.Size = new System.Drawing.Size(190, 22);
            this.mnuSelectAll.Text = "Select all";
            this.mnuSelectAll.Click += new System.EventHandler(this.mnuSelectAll_Click);
            // 
            // mnuPlayListM3U
            // 
            this.mnuPlayListM3U.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPlayListM3UNewAlbum,
            this.mnuPlayListM3UToCurrentAlbum,
            this.mnuPlaylistM3UExport});
            this.mnuPlayListM3U.Image = global::amp.Properties.Resources.m3u2;
            this.mnuPlayListM3U.Name = "mnuPlayListM3U";
            this.mnuPlayListM3U.Size = new System.Drawing.Size(190, 22);
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
            this.mnuSettings.Image = global::amp.Properties.Resources.settings;
            this.mnuSettings.Name = "mnuSettings";
            this.mnuSettings.Size = new System.Drawing.Size(190, 22);
            this.mnuSettings.Text = "Settings";
            this.mnuSettings.Click += new System.EventHandler(this.mnuSettings_Click);
            // 
            // mnuSongInfo
            // 
            this.mnuSongInfo.Image = global::amp.Properties.Resources.info;
            this.mnuSongInfo.Name = "mnuSongInfo";
            this.mnuSongInfo.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.mnuSongInfo.Size = new System.Drawing.Size(190, 22);
            this.mnuSongInfo.Text = "Song information";
            this.mnuSongInfo.Click += new System.EventHandler(this.mnuSongInfo_Click);
            // 
            // mnuShowAllSongs
            // 
            this.mnuShowAllSongs.Image = global::amp.Properties.Resources.list;
            this.mnuShowAllSongs.Name = "mnuShowAllSongs";
            this.mnuShowAllSongs.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.mnuShowAllSongs.Size = new System.Drawing.Size(190, 22);
            this.mnuShowAllSongs.Text = "Show all songs";
            this.mnuShowAllSongs.Click += new System.EventHandler(this.mnuShowAllSongs_Click);
            // 
            // mnuQueue
            // 
            this.mnuQueue.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSaveQueue,
            this.mnuLoadQueue,
            this.mnuShowQueue,
            this.mnuDeQueue,
            this.mnuScrambleQueue,
            this.mnuShowAlternateQueue});
            this.mnuQueue.Name = "mnuQueue";
            this.mnuQueue.Size = new System.Drawing.Size(54, 20);
            this.mnuQueue.Text = "Queue";
            // 
            // mnuSaveQueue
            // 
            this.mnuSaveQueue.Image = ((System.Drawing.Image)(resources.GetObject("mnuSaveQueue.Image")));
            this.mnuSaveQueue.Name = "mnuSaveQueue";
            this.mnuSaveQueue.Size = new System.Drawing.Size(207, 22);
            this.mnuSaveQueue.Text = "Save queue";
            this.mnuSaveQueue.Click += new System.EventHandler(this.saveQueueToolStripMenuItem_Click);
            // 
            // mnuLoadQueue
            // 
            this.mnuLoadQueue.Image = global::amp.Properties.Resources.open;
            this.mnuLoadQueue.Name = "mnuLoadQueue";
            this.mnuLoadQueue.Size = new System.Drawing.Size(207, 22);
            this.mnuLoadQueue.Text = "Load saved queue";
            this.mnuLoadQueue.Click += new System.EventHandler(this.mnuLoadQueue_Click);
            // 
            // mnuShowQueue
            // 
            this.mnuShowQueue.Image = global::amp.Properties.Resources.amp_queue;
            this.mnuShowQueue.Name = "mnuShowQueue";
            this.mnuShowQueue.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.mnuShowQueue.Size = new System.Drawing.Size(207, 22);
            this.mnuShowQueue.Text = "Show queue";
            this.mnuShowQueue.Click += new System.EventHandler(this.tbShowQueue_Click);
            // 
            // mnuDeQueue
            // 
            this.mnuDeQueue.Image = global::amp.Properties.Resources.amp_dequeue;
            this.mnuDeQueue.Name = "mnuDeQueue";
            this.mnuDeQueue.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.mnuDeQueue.Size = new System.Drawing.Size(207, 22);
            this.mnuDeQueue.Text = "Clear queue";
            this.mnuDeQueue.Click += new System.EventHandler(this.mnuDeQueue_Click);
            // 
            // mnuScrambleQueue
            // 
            this.mnuScrambleQueue.Image = global::amp.Properties.Resources.amp_scramble_queue;
            this.mnuScrambleQueue.Name = "mnuScrambleQueue";
            this.mnuScrambleQueue.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.mnuScrambleQueue.Size = new System.Drawing.Size(207, 22);
            this.mnuScrambleQueue.Text = "Scramble queue";
            this.mnuScrambleQueue.Click += new System.EventHandler(this.mnuScrambleQueue_Click);
            // 
            // mnuShowAlternateQueue
            // 
            this.mnuShowAlternateQueue.Image = global::amp.Properties.Resources.amp_alternate_queue;
            this.mnuShowAlternateQueue.Name = "mnuShowAlternateQueue";
            this.mnuShowAlternateQueue.ShortcutKeys = System.Windows.Forms.Keys.F8;
            this.mnuShowAlternateQueue.Size = new System.Drawing.Size(207, 22);
            this.mnuShowAlternateQueue.Text = "Show alternate queue";
            this.mnuShowAlternateQueue.Click += new System.EventHandler(this.mnuShowAlternateQueue_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHelpItem,
            this.mnuAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(44, 20);
            this.mnuHelp.Text = "Help";
            // 
            // mnuHelpItem
            // 
            this.mnuHelpItem.Image = global::amp.Properties.Resources.help;
            this.mnuHelpItem.Name = "mnuHelpItem";
            this.mnuHelpItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.mnuHelpItem.Size = new System.Drawing.Size(118, 22);
            this.mnuHelpItem.Text = "Help";
            this.mnuHelpItem.Click += new System.EventHandler(this.mnuHelpItem_Click);
            // 
            // mnuAbout
            // 
            this.mnuAbout.Image = global::amp.Properties.Resources.info1;
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(118, 22);
            this.mnuAbout.Text = "About";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // tbTool
            // 
            this.tbTool.ImageScalingSize = new System.Drawing.Size(30, 30);
            this.tbTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbRand,
            this.tbShuffle,
            this.toolStripSeparator1,
            this.tsbQueueStack,
            this.toolStripSeparator2,
            this.tbPrevious,
            this.tbPlayNext,
            this.tbNext,
            this.tbShowQueue});
            this.tbTool.Location = new System.Drawing.Point(0, 24);
            this.tbTool.Name = "tbTool";
            this.tbTool.Size = new System.Drawing.Size(549, 37);
            this.tbTool.TabIndex = 8;
            // 
            // tbRand
            // 
            this.tbRand.Checked = true;
            this.tbRand.CheckOnClick = true;
            this.tbRand.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tbRand.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbRand.Image = global::amp.Properties.Resources.amp_shuffle;
            this.tbRand.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbRand.Name = "tbRand";
            this.tbRand.Size = new System.Drawing.Size(34, 34);
            this.tbRand.Text = "tbRand";
            this.tbRand.ToolTipText = "Random";
            // 
            // tbShuffle
            // 
            this.tbShuffle.Checked = true;
            this.tbShuffle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tbShuffle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbShuffle.Image = global::amp.Properties.Resources.amp_repeat;
            this.tbShuffle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbShuffle.Name = "tbShuffle";
            this.tbShuffle.Size = new System.Drawing.Size(34, 34);
            this.tbShuffle.Text = "tbSuffle";
            this.tbShuffle.ToolTipText = "Continous playback";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 37);
            // 
            // tsbQueueStack
            // 
            this.tsbQueueStack.CheckOnClick = true;
            this.tsbQueueStack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbQueueStack.Image = global::amp.Properties.Resources.stack;
            this.tsbQueueStack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbQueueStack.Name = "tsbQueueStack";
            this.tsbQueueStack.Size = new System.Drawing.Size(34, 34);
            this.tsbQueueStack.Text = "Stack queue";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 37);
            // 
            // tbPrevious
            // 
            this.tbPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbPrevious.Image = global::amp.Properties.Resources.amp_back;
            this.tbPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbPrevious.Name = "tbPrevious";
            this.tbPrevious.Size = new System.Drawing.Size(34, 34);
            this.tbPrevious.ToolTipText = "Previous song";
            this.tbPrevious.Click += new System.EventHandler(this.tbPrevious_Click);
            // 
            // tbPlayNext
            // 
            this.tbPlayNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbPlayNext.Image = global::amp.Properties.Resources.amp_play;
            this.tbPlayNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbPlayNext.Name = "tbPlayNext";
            this.tbPlayNext.Size = new System.Drawing.Size(34, 34);
            this.tbPlayNext.ToolTipText = "Play";
            this.tbPlayNext.Click += new System.EventHandler(this.tbPlayNext_Click);
            // 
            // tbNext
            // 
            this.tbNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbNext.Image = global::amp.Properties.Resources.amp_forward;
            this.tbNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbNext.Name = "tbNext";
            this.tbNext.Size = new System.Drawing.Size(34, 34);
            this.tbNext.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.tbNext.ToolTipText = "Next song";
            this.tbNext.Click += new System.EventHandler(this.tbNext_Click);
            // 
            // tbShowQueue
            // 
            this.tbShowQueue.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbShowQueue.Image = global::amp.Properties.Resources.amp_queue;
            this.tbShowQueue.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbShowQueue.Name = "tbShowQueue";
            this.tbShowQueue.Size = new System.Drawing.Size(34, 34);
            this.tbShowQueue.Text = "Show queue";
            this.tbShowQueue.ToolTipText = "Show queue";
            this.tbShowQueue.Click += new System.EventHandler(this.tbShowQueue_Click);
            // 
            // ssStatus
            // 
            this.ssStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbQueueCount});
            this.ssStatus.Location = new System.Drawing.Point(0, 396);
            this.ssStatus.Name = "ssStatus";
            this.ssStatus.Size = new System.Drawing.Size(549, 22);
            this.ssStatus.TabIndex = 11;
            this.ssStatus.Text = "statusStrip1";
            // 
            // lbQueueCount
            // 
            this.lbQueueCount.Name = "lbQueueCount";
            this.lbQueueCount.Size = new System.Drawing.Size(13, 17);
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
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpMain.Controls.Add(this.lbMusic, 0, 4);
            this.tlpMain.Controls.Add(this.lbSong, 0, 1);
            this.tlpMain.Controls.Add(this.lbTime, 1, 1);
            this.tlpMain.Controls.Add(this.tbFind, 0, 3);
            this.tlpMain.Controls.Add(this.scProgress, 0, 2);
            this.tlpMain.Controls.Add(this.pnTools, 0, 0);
            this.tlpMain.Controls.Add(this.pnAudioVisualizationMain, 0, 5);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 61);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 6;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tlpMain.Size = new System.Drawing.Size(549, 335);
            this.tlpMain.TabIndex = 12;
            // 
            // lbMusic
            // 
            this.lbMusic.AllowDrop = true;
            this.tlpMain.SetColumnSpan(this.lbMusic, 2);
            this.lbMusic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbMusic.FormattingEnabled = true;
            this.lbMusic.Location = new System.Drawing.Point(3, 114);
            this.lbMusic.Name = "lbMusic";
            this.lbMusic.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbMusic.Size = new System.Drawing.Size(543, 184);
            this.lbMusic.TabIndex = 14;
            this.lbMusic.DragDrop += new System.Windows.Forms.DragEventHandler(this.lbMusic_DragDrop);
            this.lbMusic.DragEnter += new System.Windows.Forms.DragEventHandler(this.lbMusic_DragEnter);
            this.lbMusic.DragOver += new System.Windows.Forms.DragEventHandler(this.lbMusic_DragOver);
            this.lbMusic.DoubleClick += new System.EventHandler(this.lbMusic_DoubleClick);
            this.lbMusic.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbMusic_KeyDown);
            // 
            // lbSong
            // 
            this.lbSong.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSong.AutoEllipsis = true;
            this.lbSong.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSong.Location = new System.Drawing.Point(3, 59);
            this.lbSong.Name = "lbSong";
            this.lbSong.Size = new System.Drawing.Size(433, 13);
            this.lbSong.TabIndex = 11;
            this.lbSong.Text = "-";
            // 
            // lbTime
            // 
            this.lbTime.AutoSize = true;
            this.lbTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTime.Location = new System.Drawing.Point(442, 59);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(104, 13);
            this.lbTime.TabIndex = 10;
            this.lbTime.Text = "00:00";
            this.lbTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbFind
            // 
            this.tbFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpMain.SetColumnSpan(this.tbFind, 2);
            this.tbFind.Location = new System.Drawing.Point(3, 88);
            this.tbFind.Name = "tbFind";
            this.tbFind.Size = new System.Drawing.Size(543, 20);
            this.tbFind.TabIndex = 13;
            this.tbFind.Click += new System.EventHandler(this.tbFind_Click);
            this.tbFind.TextChanged += new System.EventHandler(this.tbFind_TextChanged);
            this.tbFind.Enter += new System.EventHandler(this.tbFind_Enter);
            this.tbFind.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbFind_KeyDown);
            // 
            // scProgress
            // 
            this.scProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpMain.SetColumnSpan(this.scProgress, 2);
            this.scProgress.Location = new System.Drawing.Point(0, 72);
            this.scProgress.Name = "scProgress";
            this.scProgress.Size = new System.Drawing.Size(549, 13);
            this.scProgress.TabIndex = 12;
            this.scProgress.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scProgress_Scroll);
            // 
            // pnTools
            // 
            this.pnTools.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpMain.SetColumnSpan(this.pnTools, 2);
            this.pnTools.Controls.Add(this.pnStars0);
            this.pnTools.Controls.Add(this.pnVol1);
            this.pnTools.Location = new System.Drawing.Point(3, 3);
            this.pnTools.Name = "pnTools";
            this.pnTools.Size = new System.Drawing.Size(543, 53);
            this.pnTools.TabIndex = 11;
            // 
            // pnStars0
            // 
            this.pnStars0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnStars0.BackgroundImage = global::amp.Properties.Resources.stars;
            this.pnStars0.Controls.Add(this.pnStars1);
            this.pnStars0.Location = new System.Drawing.Point(364, 3);
            this.pnStars0.Name = "pnStars0";
            this.pnStars0.Size = new System.Drawing.Size(176, 35);
            this.pnStars0.TabIndex = 1;
            this.pnStars0.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnStars0_MouseClick);
            // 
            // pnStars1
            // 
            this.pnStars1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnStars1.Location = new System.Drawing.Point(88, 0);
            this.pnStars1.Name = "pnStars1";
            this.pnStars1.Size = new System.Drawing.Size(176, 35);
            this.pnStars1.TabIndex = 0;
            this.pnStars1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnStars0_MouseClick);
            // 
            // pnVol1
            // 
            this.pnVol1.BackgroundImage = global::amp.Properties.Resources.volume_slider;
            this.pnVol1.Controls.Add(this.pnVol2);
            this.pnVol1.Location = new System.Drawing.Point(3, 3);
            this.pnVol1.Name = "pnVol1";
            this.pnVol1.Size = new System.Drawing.Size(100, 35);
            this.pnVol1.TabIndex = 0;
            this.pnVol1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnVol1_MouseClick);
            // 
            // pnVol2
            // 
            this.pnVol2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnVol2.BackColor = System.Drawing.Color.Transparent;
            this.pnVol2.BackgroundImage = global::amp.Properties.Resources.volume_over;
            this.pnVol2.ForeColor = System.Drawing.Color.Transparent;
            this.pnVol2.Location = new System.Drawing.Point(50, 0);
            this.pnVol2.Name = "pnVol2";
            this.pnVol2.Size = new System.Drawing.Size(100, 35);
            this.pnVol2.TabIndex = 0;
            this.pnVol2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnVol1_MouseClick);
            // 
            // pnAudioVisualizationMain
            // 
            this.tlpMain.SetColumnSpan(this.pnAudioVisualizationMain, 2);
            this.pnAudioVisualizationMain.Controls.Add(this.avBars);
            this.pnAudioVisualizationMain.Controls.Add(this.avLine);
            this.pnAudioVisualizationMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnAudioVisualizationMain.Location = new System.Drawing.Point(0, 301);
            this.pnAudioVisualizationMain.Margin = new System.Windows.Forms.Padding(0);
            this.pnAudioVisualizationMain.Name = "pnAudioVisualizationMain";
            this.pnAudioVisualizationMain.Size = new System.Drawing.Size(549, 34);
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
            this.avBars.Location = new System.Drawing.Point(12, 3);
            this.avBars.MinorityCropOnBarLevel = false;
            this.avBars.MinorityCropPercentage = 3;
            this.avBars.MinorityPercentageStepping = 1000;
            this.avBars.Name = "avBars";
            this.avBars.NoiseTolerance = 1D;
            this.avBars.RefreshRate = 30;
            this.avBars.RelativeView = true;
            this.avBars.Size = new System.Drawing.Size(402, 10);
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
            this.avLine.Location = new System.Drawing.Point(34, 19);
            this.avLine.MinorityCropOnBarLevel = false;
            this.avLine.MinorityCropPercentage = 2;
            this.avLine.MinorityPercentageStepping = 1000;
            this.avLine.Name = "avLine";
            this.avLine.NoiseTolerance = 1D;
            this.avLine.RefreshRate = 30;
            this.avLine.Size = new System.Drawing.Size(402, 10);
            this.avLine.TabIndex = 9;
            this.avLine.UseAntiAliasing = true;
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
            // FormMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(549, 418);
            this.Controls.Add(this.tlpMain);
            this.Controls.Add(this.ssStatus);
            this.Controls.Add(this.tbTool);
            this.Controls.Add(this.msMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.msMain;
            this.Name = "FormMain";
            this.Text = "amp#";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.Shown += new System.EventHandler(this.MainWindow_Shown);
            this.ResizeEnd += new System.EventHandler(this.MainWindow_LocationChanged);
            this.LocationChanged += new System.EventHandler(this.MainWindow_LocationChanged);
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.tbTool.ResumeLayout(false);
            this.tbTool.PerformLayout();
            this.ssStatus.ResumeLayout(false);
            this.ssStatus.PerformLayout();
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.pnTools.ResumeLayout(false);
            this.pnStars0.ResumeLayout(false);
            this.pnVol1.ResumeLayout(false);
            this.pnAudioVisualizationMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Timer tmSeek;
        private MenuStrip msMain;
        private ToolStripMenuItem mnuFile;
        private ToolStrip tbTool;
        private ToolStripButton tbRand;
        private ToolStripButton tbShuffle;
        private ToolStripButton tbPlayNext;
        private ToolStripButton tbPrevious;
        private ToolStripButton tbNext;
        private ToolStripMenuItem mnuAlbum;
        private ToolStripMenuItem mnuNewAlbum;
        private StatusStrip ssStatus;
        private ToolStripStatusLabel lbQueueCount;
        private ToolStripButton tbShowQueue;
        private ToolStripMenuItem mnuSelectAll;
        private ToolStripMenuItem mnuPlayListM3U;
        private ToolStripMenuItem mnuPlayListM3UNewAlbum;
        private ToolStripMenuItem mnuPlayListM3UToCurrentAlbum;
        private OpenFileDialog odM3U;
        private ToolStripMenuItem mnuPlaylistM3UExport;
        private SaveFileDialog sdM3U;
        private TableLayoutPanel tlpMain;
        private RefreshListbox lbMusic;
        private TextBox tbFind;
        private HScrollBar scProgress;
        private Panel pnTools;
        private Panel pnStars0;
        private Panel pnStars1;
        private Panel pnVol1;
        private Panel pnVol2;
        private Label lbSong;
        private Label lbTime;
        private Timer tmPendOperation;
        private ToolStripMenuItem mnuQueue;
        private ToolStripMenuItem mnuSaveQueue;
        private ToolStripMenuItem mnuLoadQueue;
        private ToolStripMenuItem mnuSettings;
        private ToolStripMenuItem mnuShowQueue;
        private ToolStripMenuItem mnuDeQueue;
        private ToolStripMenuItem mnuSongInfo;
        private ToolStripMenuItem mnuScrambleQueue;
        private ToolStripMenuItem mnuShowAlternateQueue;
        private ToolStripMenuItem mnuHelp;
        private ToolStripMenuItem mnuHelpItem;
        private ToolStripMenuItem mnuAbout;
        private ToolStripMenuItem mnuShowAllSongs;
        private Timer tmIPCFiles;
        private ToolStripMenuItem mnuSaveAlbumAs;
        private ToolStripMenuItem mnuDeleteAlbum;
        private Panel pnAudioVisualizationMain;
        private VPKSoft.AudioVisualization.AudioVisualizationBars avBars;
        private VPKSoft.AudioVisualization.AudioVisualizationPlot avLine;
        private ToolStripButton tsbQueueStack;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
    }
}

