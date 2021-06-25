using System.ComponentModel;
using System.Windows.Forms;

namespace amp.UtilityClasses.Settings
{
    partial class FormSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.cbQuietHours = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.gpVolumeSetting = new System.Windows.Forms.GroupBox();
            this.lbByPercent = new System.Windows.Forms.Label();
            this.nudQuietHourPercentage = new System.Windows.Forms.NumericUpDown();
            this.rbDecreaseVolumeQuietHours = new System.Windows.Forms.RadioButton();
            this.rbPauseQuiet = new System.Windows.Forms.RadioButton();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.tbTestQuietHour = new System.Windows.Forms.TextBox();
            this.btnTestQuietHour = new System.Windows.Forms.Button();
            this.lbLanguage = new System.Windows.Forms.Label();
            this.cmbSelectLanguageValue = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLocalization = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDumpLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDatabaseMigration = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuThemeSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.btnModifiedRandomization = new System.Windows.Forms.Button();
            this.btAlbumNaming = new System.Windows.Forms.Button();
            this.cbCheckUpdatesStartup = new System.Windows.Forms.CheckBox();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpMain = new System.Windows.Forms.TabPage();
            this.tpAdditional = new System.Windows.Forms.TabPage();
            this.cbHideAlbumImage = new System.Windows.Forms.CheckBox();
            this.cbDisplayVolumeAndPoints = new System.Windows.Forms.CheckBox();
            this.gbStackQueue = new System.Windows.Forms.GroupBox();
            this.tlpStackQueueRandomLabel = new System.Windows.Forms.TableLayoutPanel();
            this.lbStackQueueRandom = new System.Windows.Forms.Label();
            this.lbStackQueueRandomValue = new System.Windows.Forms.Label();
            this.tbStackQueueRandom = new System.Windows.Forms.TrackBar();
            this.gbAudioVisualizationStyle = new System.Windows.Forms.GroupBox();
            this.nudBarAmount = new System.Windows.Forms.NumericUpDown();
            this.lbBarAmount = new System.Windows.Forms.Label();
            this.cbBalancedBars = new System.Windows.Forms.CheckBox();
            this.cbAudioVisualizationCombineChannels = new System.Windows.Forms.CheckBox();
            this.lbAudioVisualizationSizePercentage = new System.Windows.Forms.Label();
            this.nudAudioVisualizationSize = new System.Windows.Forms.NumericUpDown();
            this.lbVisualizationWindowSize = new System.Windows.Forms.Label();
            this.rbAudioVisualizationLines = new System.Windows.Forms.RadioButton();
            this.rbAudioVisualizationBars = new System.Windows.Forms.RadioButton();
            this.rbAudioVisualizationOff = new System.Windows.Forms.RadioButton();
            this.tabRemote = new System.Windows.Forms.TabPage();
            this.lbRestApiHeader = new System.Windows.Forms.Label();
            this.gbRestApi = new System.Windows.Forms.GroupBox();
            this.nudRestPort = new System.Windows.Forms.NumericUpDown();
            this.lbRemoteControlPort = new System.Windows.Forms.Label();
            this.cbRestEnabled = new System.Windows.Forms.CheckBox();
            this.gpVolumeSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuietHourPercentage)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tpMain.SuspendLayout();
            this.tpAdditional.SuspendLayout();
            this.gbStackQueue.SuspendLayout();
            this.tlpStackQueueRandomLabel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbStackQueueRandom)).BeginInit();
            this.gbAudioVisualizationStyle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBarAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAudioVisualizationSize)).BeginInit();
            this.tabRemote.SuspendLayout();
            this.gbRestApi.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRestPort)).BeginInit();
            this.SuspendLayout();
            // 
            // cbQuietHours
            // 
            this.cbQuietHours.AutoSize = true;
            this.cbQuietHours.Location = new System.Drawing.Point(7, 7);
            this.cbQuietHours.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbQuietHours.Name = "cbQuietHours";
            this.cbQuietHours.Size = new System.Drawing.Size(131, 19);
            this.cbQuietHours.TabIndex = 0;
            this.cbQuietHours.Text = "Enabled quiet hours";
            this.cbQuietHours.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(141, 33);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "_";
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "HH\':\'mm";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(7, 33);
            this.dtpFrom.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.ShowUpDown = true;
            this.dtpFrom.Size = new System.Drawing.Size(107, 23);
            this.dtpFrom.TabIndex = 1;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "HH\':\'mm";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(184, 33);
            this.dtpTo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.ShowUpDown = true;
            this.dtpTo.Size = new System.Drawing.Size(107, 23);
            this.dtpTo.TabIndex = 3;
            // 
            // gpVolumeSetting
            // 
            this.gpVolumeSetting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpVolumeSetting.Controls.Add(this.lbByPercent);
            this.gpVolumeSetting.Controls.Add(this.nudQuietHourPercentage);
            this.gpVolumeSetting.Controls.Add(this.rbDecreaseVolumeQuietHours);
            this.gpVolumeSetting.Controls.Add(this.rbPauseQuiet);
            this.gpVolumeSetting.Location = new System.Drawing.Point(7, 63);
            this.gpVolumeSetting.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gpVolumeSetting.Name = "gpVolumeSetting";
            this.gpVolumeSetting.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gpVolumeSetting.Size = new System.Drawing.Size(285, 111);
            this.gpVolumeSetting.TabIndex = 6;
            this.gpVolumeSetting.TabStop = false;
            // 
            // lbByPercent
            // 
            this.lbByPercent.AutoSize = true;
            this.lbByPercent.Location = new System.Drawing.Point(7, 72);
            this.lbByPercent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbByPercent.Name = "lbByPercent";
            this.lbByPercent.Size = new System.Drawing.Size(66, 15);
            this.lbByPercent.TabIndex = 6;
            this.lbByPercent.Text = "by percent:";
            // 
            // nudQuietHourPercentage
            // 
            this.nudQuietHourPercentage.Location = new System.Drawing.Point(128, 69);
            this.nudQuietHourPercentage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nudQuietHourPercentage.Name = "nudQuietHourPercentage";
            this.nudQuietHourPercentage.Size = new System.Drawing.Size(82, 23);
            this.nudQuietHourPercentage.TabIndex = 5;
            this.nudQuietHourPercentage.ValueChanged += new System.EventHandler(this.nudQuietHourPercentage_ValueChanged);
            // 
            // rbDecreaseVolumeQuietHours
            // 
            this.rbDecreaseVolumeQuietHours.AutoSize = true;
            this.rbDecreaseVolumeQuietHours.Location = new System.Drawing.Point(7, 48);
            this.rbDecreaseVolumeQuietHours.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbDecreaseVolumeQuietHours.Name = "rbDecreaseVolumeQuietHours";
            this.rbDecreaseVolumeQuietHours.Size = new System.Drawing.Size(195, 19);
            this.rbDecreaseVolumeQuietHours.TabIndex = 1;
            this.rbDecreaseVolumeQuietHours.TabStop = true;
            this.rbDecreaseVolumeQuietHours.Text = "Decrease volume on quiet hours";
            this.rbDecreaseVolumeQuietHours.UseVisualStyleBackColor = true;
            // 
            // rbPauseQuiet
            // 
            this.rbPauseQuiet.AutoSize = true;
            this.rbPauseQuiet.Checked = true;
            this.rbPauseQuiet.Location = new System.Drawing.Point(7, 22);
            this.rbPauseQuiet.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbPauseQuiet.Name = "rbPauseQuiet";
            this.rbPauseQuiet.Size = new System.Drawing.Size(136, 19);
            this.rbPauseQuiet.TabIndex = 0;
            this.rbPauseQuiet.TabStop = true;
            this.rbPauseQuiet.Text = "Pause on quiet hours";
            this.rbPauseQuiet.UseVisualStyleBackColor = true;
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(15, 599);
            this.bCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(88, 27);
            this.bCancel.TabIndex = 8;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.Location = new System.Drawing.Point(232, 599);
            this.bOK.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(88, 27);
            this.bOK.TabIndex = 7;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // tbTestQuietHour
            // 
            this.tbTestQuietHour.Location = new System.Drawing.Point(348, 238);
            this.tbTestQuietHour.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tbTestQuietHour.Multiline = true;
            this.tbTestQuietHour.Name = "tbTestQuietHour";
            this.tbTestQuietHour.Size = new System.Drawing.Size(313, 185);
            this.tbTestQuietHour.TabIndex = 19;
            // 
            // btnTestQuietHour
            // 
            this.btnTestQuietHour.Location = new System.Drawing.Point(348, 204);
            this.btnTestQuietHour.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnTestQuietHour.Name = "btnTestQuietHour";
            this.btnTestQuietHour.Size = new System.Drawing.Size(314, 27);
            this.btnTestQuietHour.TabIndex = 20;
            this.btnTestQuietHour.Text = "Quiet hour test";
            this.btnTestQuietHour.UseVisualStyleBackColor = true;
            this.btnTestQuietHour.Click += new System.EventHandler(this.btnTestQuietHour_Click);
            // 
            // lbLanguage
            // 
            this.lbLanguage.AutoSize = true;
            this.lbLanguage.Location = new System.Drawing.Point(4, 368);
            this.lbLanguage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbLanguage.Name = "lbLanguage";
            this.lbLanguage.Size = new System.Drawing.Size(151, 15);
            this.lbLanguage.TabIndex = 21;
            this.lbLanguage.Text = "Language (requires restart):";
            // 
            // cmbSelectLanguageValue
            // 
            this.cmbSelectLanguageValue.DisplayMember = "DisplayName";
            this.cmbSelectLanguageValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectLanguageValue.FormattingEnabled = true;
            this.cmbSelectLanguageValue.Location = new System.Drawing.Point(7, 392);
            this.cmbSelectLanguageValue.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbSelectLanguageValue.Name = "cmbSelectLanguageValue";
            this.cmbSelectLanguageValue.Size = new System.Drawing.Size(284, 23);
            this.cmbSelectLanguageValue.TabIndex = 22;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.mnuThemeSettings});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(336, 24);
            this.menuStrip1.TabIndex = 24;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuLocalization,
            this.mnuDumpLanguage,
            this.mnuDatabaseMigration});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // mnuLocalization
            // 
            this.mnuLocalization.Image = global::amp.Properties.Resources.education_languages;
            this.mnuLocalization.Name = "mnuLocalization";
            this.mnuLocalization.Size = new System.Drawing.Size(196, 22);
            this.mnuLocalization.Text = "Localization";
            this.mnuLocalization.Click += new System.EventHandler(this.MnuLocalization_Click);
            // 
            // mnuDumpLanguage
            // 
            this.mnuDumpLanguage.Image = global::amp.Properties.Resources.database_go;
            this.mnuDumpLanguage.Name = "mnuDumpLanguage";
            this.mnuDumpLanguage.Size = new System.Drawing.Size(196, 22);
            this.mnuDumpLanguage.Text = "Dumb language";
            this.mnuDumpLanguage.Click += new System.EventHandler(this.MnuDumpLanguage_Click);
            // 
            // mnuDatabaseMigration
            // 
            this.mnuDatabaseMigration.Image = global::amp.Properties.Resources.Toolbox;
            this.mnuDatabaseMigration.Name = "mnuDatabaseMigration";
            this.mnuDatabaseMigration.Size = new System.Drawing.Size(196, 22);
            this.mnuDatabaseMigration.Text = "Database management";
            this.mnuDatabaseMigration.Click += new System.EventHandler(this.MnuDatabaseMigration_Click);
            // 
            // mnuThemeSettings
            // 
            this.mnuThemeSettings.Name = "mnuThemeSettings";
            this.mnuThemeSettings.Size = new System.Drawing.Size(99, 20);
            this.mnuThemeSettings.Text = "Theme settings";
            this.mnuThemeSettings.Click += new System.EventHandler(this.mnuThemeSettings_Click);
            // 
            // btnModifiedRandomization
            // 
            this.btnModifiedRandomization.Image = global::amp.Properties.Resources.media_shuffle;
            this.btnModifiedRandomization.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnModifiedRandomization.Location = new System.Drawing.Point(7, 425);
            this.btnModifiedRandomization.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnModifiedRandomization.Name = "btnModifiedRandomization";
            this.btnModifiedRandomization.Size = new System.Drawing.Size(285, 27);
            this.btnModifiedRandomization.TabIndex = 23;
            this.btnModifiedRandomization.Text = "Modified randomization";
            this.btnModifiedRandomization.UseVisualStyleBackColor = true;
            this.btnModifiedRandomization.Click += new System.EventHandler(this.btnModifiedRandomization_Click);
            // 
            // btAlbumNaming
            // 
            this.btAlbumNaming.Image = global::amp.Properties.Resources.Modify;
            this.btAlbumNaming.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btAlbumNaming.Location = new System.Drawing.Point(7, 458);
            this.btAlbumNaming.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btAlbumNaming.Name = "btAlbumNaming";
            this.btAlbumNaming.Size = new System.Drawing.Size(285, 27);
            this.btAlbumNaming.TabIndex = 18;
            this.btAlbumNaming.Text = "Album naming";
            this.btAlbumNaming.UseVisualStyleBackColor = true;
            this.btAlbumNaming.Click += new System.EventHandler(this.btAlbumNaming_Click);
            // 
            // cbCheckUpdatesStartup
            // 
            this.cbCheckUpdatesStartup.AutoSize = true;
            this.cbCheckUpdatesStartup.Location = new System.Drawing.Point(7, 492);
            this.cbCheckUpdatesStartup.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbCheckUpdatesStartup.Name = "cbCheckUpdatesStartup";
            this.cbCheckUpdatesStartup.Size = new System.Drawing.Size(193, 19);
            this.cbCheckUpdatesStartup.TabIndex = 25;
            this.cbCheckUpdatesStartup.Text = "Check for updates upon startup";
            this.cbCheckUpdatesStartup.UseVisualStyleBackColor = true;
            // 
            // tcMain
            // 
            this.tcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcMain.Controls.Add(this.tpMain);
            this.tcMain.Controls.Add(this.tpAdditional);
            this.tcMain.Controls.Add(this.tabRemote);
            this.tcMain.Location = new System.Drawing.Point(14, 31);
            this.tcMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(310, 561);
            this.tcMain.TabIndex = 26;
            // 
            // tpMain
            // 
            this.tpMain.Controls.Add(this.cbQuietHours);
            this.tpMain.Controls.Add(this.cbCheckUpdatesStartup);
            this.tpMain.Controls.Add(this.dtpFrom);
            this.tpMain.Controls.Add(this.btnModifiedRandomization);
            this.tpMain.Controls.Add(this.label1);
            this.tpMain.Controls.Add(this.cmbSelectLanguageValue);
            this.tpMain.Controls.Add(this.dtpTo);
            this.tpMain.Controls.Add(this.lbLanguage);
            this.tpMain.Controls.Add(this.gpVolumeSetting);
            this.tpMain.Controls.Add(this.btAlbumNaming);
            this.tpMain.Location = new System.Drawing.Point(4, 24);
            this.tpMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpMain.Name = "tpMain";
            this.tpMain.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpMain.Size = new System.Drawing.Size(302, 533);
            this.tpMain.TabIndex = 0;
            this.tpMain.Text = "Main";
            this.tpMain.UseVisualStyleBackColor = true;
            // 
            // tpAdditional
            // 
            this.tpAdditional.Controls.Add(this.cbHideAlbumImage);
            this.tpAdditional.Controls.Add(this.cbDisplayVolumeAndPoints);
            this.tpAdditional.Controls.Add(this.gbStackQueue);
            this.tpAdditional.Controls.Add(this.gbAudioVisualizationStyle);
            this.tpAdditional.Location = new System.Drawing.Point(4, 24);
            this.tpAdditional.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpAdditional.Name = "tpAdditional";
            this.tpAdditional.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tpAdditional.Size = new System.Drawing.Size(302, 533);
            this.tpAdditional.TabIndex = 1;
            this.tpAdditional.Text = "Additional";
            this.tpAdditional.UseVisualStyleBackColor = true;
            // 
            // cbHideAlbumImage
            // 
            this.cbHideAlbumImage.AutoSize = true;
            this.cbHideAlbumImage.Location = new System.Drawing.Point(7, 496);
            this.cbHideAlbumImage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbHideAlbumImage.Name = "cbHideAlbumImage";
            this.cbHideAlbumImage.Size = new System.Drawing.Size(198, 19);
            this.cbHideAlbumImage.TabIndex = 4;
            this.cbHideAlbumImage.Text = "Auto-hide album image window";
            this.cbHideAlbumImage.UseVisualStyleBackColor = true;
            // 
            // cbDisplayVolumeAndPoints
            // 
            this.cbDisplayVolumeAndPoints.AutoSize = true;
            this.cbDisplayVolumeAndPoints.Location = new System.Drawing.Point(8, 470);
            this.cbDisplayVolumeAndPoints.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbDisplayVolumeAndPoints.Name = "cbDisplayVolumeAndPoints";
            this.cbDisplayVolumeAndPoints.Size = new System.Drawing.Size(261, 19);
            this.cbDisplayVolumeAndPoints.TabIndex = 3;
            this.cbDisplayVolumeAndPoints.Text = "Display volume settings and points on the UI";
            this.cbDisplayVolumeAndPoints.UseVisualStyleBackColor = true;
            // 
            // gbStackQueue
            // 
            this.gbStackQueue.Controls.Add(this.tlpStackQueueRandomLabel);
            this.gbStackQueue.Controls.Add(this.tbStackQueueRandom);
            this.gbStackQueue.Location = new System.Drawing.Point(7, 378);
            this.gbStackQueue.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gbStackQueue.Name = "gbStackQueue";
            this.gbStackQueue.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gbStackQueue.Size = new System.Drawing.Size(287, 84);
            this.gbStackQueue.TabIndex = 2;
            this.gbStackQueue.TabStop = false;
            this.gbStackQueue.Text = "Stack queue";
            // 
            // tlpStackQueueRandomLabel
            // 
            this.tlpStackQueueRandomLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpStackQueueRandomLabel.AutoSize = true;
            this.tlpStackQueueRandomLabel.ColumnCount = 2;
            this.tlpStackQueueRandomLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpStackQueueRandomLabel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpStackQueueRandomLabel.Controls.Add(this.lbStackQueueRandom, 0, 0);
            this.tlpStackQueueRandomLabel.Controls.Add(this.lbStackQueueRandomValue, 1, 0);
            this.tlpStackQueueRandomLabel.Location = new System.Drawing.Point(7, 22);
            this.tlpStackQueueRandomLabel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tlpStackQueueRandomLabel.Name = "tlpStackQueueRandomLabel";
            this.tlpStackQueueRandomLabel.RowCount = 2;
            this.tlpStackQueueRandomLabel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpStackQueueRandomLabel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpStackQueueRandomLabel.Size = new System.Drawing.Size(280, 35);
            this.tlpStackQueueRandomLabel.TabIndex = 3;
            // 
            // lbStackQueueRandom
            // 
            this.lbStackQueueRandom.AutoSize = true;
            this.lbStackQueueRandom.Location = new System.Drawing.Point(4, 0);
            this.lbStackQueueRandom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbStackQueueRandom.Name = "lbStackQueueRandom";
            this.lbStackQueueRandom.Size = new System.Drawing.Size(200, 15);
            this.lbStackQueueRandom.TabIndex = 1;
            this.lbStackQueueRandom.Text = "Randomize stack from top (percent):";
            // 
            // lbStackQueueRandomValue
            // 
            this.lbStackQueueRandomValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbStackQueueRandomValue.AutoSize = true;
            this.lbStackQueueRandomValue.Location = new System.Drawing.Point(244, 0);
            this.lbStackQueueRandomValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbStackQueueRandomValue.Name = "lbStackQueueRandomValue";
            this.lbStackQueueRandomValue.Size = new System.Drawing.Size(32, 15);
            this.lbStackQueueRandomValue.TabIndex = 2;
            this.lbStackQueueRandomValue.Text = "50 %";
            this.lbStackQueueRandomValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbStackQueueRandom
            // 
            this.tbStackQueueRandom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbStackQueueRandom.AutoSize = false;
            this.tbStackQueueRandom.Location = new System.Drawing.Point(7, 51);
            this.tbStackQueueRandom.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tbStackQueueRandom.Maximum = 100;
            this.tbStackQueueRandom.Name = "tbStackQueueRandom";
            this.tbStackQueueRandom.Size = new System.Drawing.Size(280, 23);
            this.tbStackQueueRandom.TabIndex = 0;
            this.tbStackQueueRandom.TickFrequency = 10;
            this.tbStackQueueRandom.Value = 50;
            this.tbStackQueueRandom.ValueChanged += new System.EventHandler(this.tbStackQueueRandom_ValueChanged);
            // 
            // gbAudioVisualizationStyle
            // 
            this.gbAudioVisualizationStyle.Controls.Add(this.nudBarAmount);
            this.gbAudioVisualizationStyle.Controls.Add(this.lbBarAmount);
            this.gbAudioVisualizationStyle.Controls.Add(this.cbBalancedBars);
            this.gbAudioVisualizationStyle.Controls.Add(this.cbAudioVisualizationCombineChannels);
            this.gbAudioVisualizationStyle.Controls.Add(this.lbAudioVisualizationSizePercentage);
            this.gbAudioVisualizationStyle.Controls.Add(this.nudAudioVisualizationSize);
            this.gbAudioVisualizationStyle.Controls.Add(this.lbVisualizationWindowSize);
            this.gbAudioVisualizationStyle.Controls.Add(this.rbAudioVisualizationLines);
            this.gbAudioVisualizationStyle.Controls.Add(this.rbAudioVisualizationBars);
            this.gbAudioVisualizationStyle.Controls.Add(this.rbAudioVisualizationOff);
            this.gbAudioVisualizationStyle.Location = new System.Drawing.Point(7, 7);
            this.gbAudioVisualizationStyle.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gbAudioVisualizationStyle.Name = "gbAudioVisualizationStyle";
            this.gbAudioVisualizationStyle.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gbAudioVisualizationStyle.Size = new System.Drawing.Size(287, 286);
            this.gbAudioVisualizationStyle.TabIndex = 0;
            this.gbAudioVisualizationStyle.TabStop = false;
            this.gbAudioVisualizationStyle.Tag = "0";
            this.gbAudioVisualizationStyle.Text = "Audio visualization";
            // 
            // nudBarAmount
            // 
            this.nudBarAmount.Location = new System.Drawing.Point(7, 250);
            this.nudBarAmount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nudBarAmount.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.nudBarAmount.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudBarAmount.Name = "nudBarAmount";
            this.nudBarAmount.Size = new System.Drawing.Size(58, 23);
            this.nudBarAmount.TabIndex = 9;
            this.nudBarAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudBarAmount.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // lbBarAmount
            // 
            this.lbBarAmount.AutoSize = true;
            this.lbBarAmount.Location = new System.Drawing.Point(7, 232);
            this.lbBarAmount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbBarAmount.Name = "lbBarAmount";
            this.lbBarAmount.Size = new System.Drawing.Size(207, 15);
            this.lbBarAmount.TabIndex = 8;
            this.lbBarAmount.Text = "Amount of bars in audio visualization:";
            // 
            // cbBalancedBars
            // 
            this.cbBalancedBars.AutoSize = true;
            this.cbBalancedBars.Location = new System.Drawing.Point(7, 209);
            this.cbBalancedBars.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbBalancedBars.Name = "cbBalancedBars";
            this.cbBalancedBars.Size = new System.Drawing.Size(159, 19);
            this.cbBalancedBars.TabIndex = 7;
            this.cbBalancedBars.Text = "Balanced bars (min-max)";
            this.cbBalancedBars.UseVisualStyleBackColor = true;
            // 
            // cbAudioVisualizationCombineChannels
            // 
            this.cbAudioVisualizationCombineChannels.AutoSize = true;
            this.cbAudioVisualizationCombineChannels.Location = new System.Drawing.Point(7, 182);
            this.cbAudioVisualizationCombineChannels.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbAudioVisualizationCombineChannels.Name = "cbAudioVisualizationCombineChannels";
            this.cbAudioVisualizationCombineChannels.Size = new System.Drawing.Size(125, 19);
            this.cbAudioVisualizationCombineChannels.TabIndex = 6;
            this.cbAudioVisualizationCombineChannels.Text = "Combine channels";
            this.cbAudioVisualizationCombineChannels.UseVisualStyleBackColor = true;
            // 
            // lbAudioVisualizationSizePercentage
            // 
            this.lbAudioVisualizationSizePercentage.AutoSize = true;
            this.lbAudioVisualizationSizePercentage.Location = new System.Drawing.Point(126, 155);
            this.lbAudioVisualizationSizePercentage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbAudioVisualizationSizePercentage.Name = "lbAudioVisualizationSizePercentage";
            this.lbAudioVisualizationSizePercentage.Size = new System.Drawing.Size(17, 15);
            this.lbAudioVisualizationSizePercentage.TabIndex = 5;
            this.lbAudioVisualizationSizePercentage.Text = "%";
            // 
            // nudAudioVisualizationSize
            // 
            this.nudAudioVisualizationSize.Location = new System.Drawing.Point(7, 152);
            this.nudAudioVisualizationSize.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nudAudioVisualizationSize.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.nudAudioVisualizationSize.Name = "nudAudioVisualizationSize";
            this.nudAudioVisualizationSize.Size = new System.Drawing.Size(112, 23);
            this.nudAudioVisualizationSize.TabIndex = 4;
            this.nudAudioVisualizationSize.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // lbVisualizationWindowSize
            // 
            this.lbVisualizationWindowSize.Location = new System.Drawing.Point(7, 108);
            this.lbVisualizationWindowSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbVisualizationWindowSize.Name = "lbVisualizationWindowSize";
            this.lbVisualizationWindowSize.Size = new System.Drawing.Size(273, 40);
            this.lbVisualizationWindowSize.TabIndex = 3;
            this.lbVisualizationWindowSize.Text = "Visualization size in of the main window area:";
            // 
            // rbAudioVisualizationLines
            // 
            this.rbAudioVisualizationLines.AutoSize = true;
            this.rbAudioVisualizationLines.Location = new System.Drawing.Point(7, 75);
            this.rbAudioVisualizationLines.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbAudioVisualizationLines.Name = "rbAudioVisualizationLines";
            this.rbAudioVisualizationLines.Size = new System.Drawing.Size(81, 19);
            this.rbAudioVisualizationLines.TabIndex = 2;
            this.rbAudioVisualizationLines.Tag = "2";
            this.rbAudioVisualizationLines.Text = "Line graph";
            this.rbAudioVisualizationLines.UseVisualStyleBackColor = true;
            this.rbAudioVisualizationLines.CheckedChanged += new System.EventHandler(this.RbAudioVisualization_CheckedChanged);
            // 
            // rbAudioVisualizationBars
            // 
            this.rbAudioVisualizationBars.AutoSize = true;
            this.rbAudioVisualizationBars.Location = new System.Drawing.Point(7, 48);
            this.rbAudioVisualizationBars.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbAudioVisualizationBars.Name = "rbAudioVisualizationBars";
            this.rbAudioVisualizationBars.Size = new System.Drawing.Size(76, 19);
            this.rbAudioVisualizationBars.TabIndex = 1;
            this.rbAudioVisualizationBars.Tag = "1";
            this.rbAudioVisualizationBars.Text = "Bar graph";
            this.rbAudioVisualizationBars.UseVisualStyleBackColor = true;
            this.rbAudioVisualizationBars.CheckedChanged += new System.EventHandler(this.RbAudioVisualization_CheckedChanged);
            // 
            // rbAudioVisualizationOff
            // 
            this.rbAudioVisualizationOff.AutoSize = true;
            this.rbAudioVisualizationOff.Checked = true;
            this.rbAudioVisualizationOff.Location = new System.Drawing.Point(7, 22);
            this.rbAudioVisualizationOff.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rbAudioVisualizationOff.Name = "rbAudioVisualizationOff";
            this.rbAudioVisualizationOff.Size = new System.Drawing.Size(42, 19);
            this.rbAudioVisualizationOff.TabIndex = 0;
            this.rbAudioVisualizationOff.TabStop = true;
            this.rbAudioVisualizationOff.Tag = "0";
            this.rbAudioVisualizationOff.Text = "Off";
            this.rbAudioVisualizationOff.UseVisualStyleBackColor = true;
            this.rbAudioVisualizationOff.CheckedChanged += new System.EventHandler(this.RbAudioVisualization_CheckedChanged);
            // 
            // tabRemote
            // 
            this.tabRemote.Controls.Add(this.lbRestApiHeader);
            this.tabRemote.Controls.Add(this.gbRestApi);
            this.tabRemote.Location = new System.Drawing.Point(4, 24);
            this.tabRemote.Margin = new System.Windows.Forms.Padding(4, 9, 4, 3);
            this.tabRemote.Name = "tabRemote";
            this.tabRemote.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabRemote.Size = new System.Drawing.Size(302, 533);
            this.tabRemote.TabIndex = 2;
            this.tabRemote.Text = "Remote control";
            this.tabRemote.UseVisualStyleBackColor = true;
            // 
            // lbRestApiHeader
            // 
            this.lbRestApiHeader.AutoSize = true;
            this.lbRestApiHeader.Location = new System.Drawing.Point(7, 9);
            this.lbRestApiHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbRestApiHeader.Name = "lbRestApiHeader";
            this.lbRestApiHeader.Size = new System.Drawing.Size(126, 15);
            this.lbRestApiHeader.TabIndex = 21;
            this.lbRestApiHeader.Text = "RESTful API with JSON:";
            // 
            // gbRestApi
            // 
            this.gbRestApi.Controls.Add(this.nudRestPort);
            this.gbRestApi.Controls.Add(this.lbRemoteControlPort);
            this.gbRestApi.Controls.Add(this.cbRestEnabled);
            this.gbRestApi.Location = new System.Drawing.Point(7, 28);
            this.gbRestApi.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gbRestApi.Name = "gbRestApi";
            this.gbRestApi.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gbRestApi.Size = new System.Drawing.Size(285, 97);
            this.gbRestApi.TabIndex = 20;
            this.gbRestApi.TabStop = false;
            this.gbRestApi.Text = "Remote control API";
            // 
            // nudRestPort
            // 
            this.nudRestPort.Location = new System.Drawing.Point(10, 61);
            this.nudRestPort.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nudRestPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudRestPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRestPort.Name = "nudRestPort";
            this.nudRestPort.Size = new System.Drawing.Size(140, 23);
            this.nudRestPort.TabIndex = 17;
            this.nudRestPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudRestPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lbRemoteControlPort
            // 
            this.lbRemoteControlPort.AutoSize = true;
            this.lbRemoteControlPort.Location = new System.Drawing.Point(7, 43);
            this.lbRemoteControlPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbRemoteControlPort.Name = "lbRemoteControlPort";
            this.lbRemoteControlPort.Size = new System.Drawing.Size(117, 15);
            this.lbRemoteControlPort.TabIndex = 9;
            this.lbRemoteControlPort.Text = "Remote control port:";
            // 
            // cbRestEnabled
            // 
            this.cbRestEnabled.AutoSize = true;
            this.cbRestEnabled.Location = new System.Drawing.Point(7, 22);
            this.cbRestEnabled.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cbRestEnabled.Name = "cbRestEnabled";
            this.cbRestEnabled.Size = new System.Drawing.Size(68, 19);
            this.cbRestEnabled.TabIndex = 16;
            this.cbRestEnabled.Text = "Enabled";
            this.cbRestEnabled.UseVisualStyleBackColor = true;
            this.cbRestEnabled.CheckedChanged += new System.EventHandler(this.cbRestEnabled_CheckedChanged);
            // 
            // FormSettings
            // 
            this.AcceptButton = this.bOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(336, 645);
            this.Controls.Add(this.tcMain);
            this.Controls.Add(this.btnTestQuietHour);
            this.Controls.Add(this.tbTestQuietHour);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Shown += new System.EventHandler(this.FormSettings_Shown);
            this.gpVolumeSetting.ResumeLayout(false);
            this.gpVolumeSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuietHourPercentage)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tcMain.ResumeLayout(false);
            this.tpMain.ResumeLayout(false);
            this.tpMain.PerformLayout();
            this.tpAdditional.ResumeLayout(false);
            this.tpAdditional.PerformLayout();
            this.gbStackQueue.ResumeLayout(false);
            this.gbStackQueue.PerformLayout();
            this.tlpStackQueueRandomLabel.ResumeLayout(false);
            this.tlpStackQueueRandomLabel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbStackQueueRandom)).EndInit();
            this.gbAudioVisualizationStyle.ResumeLayout(false);
            this.gbAudioVisualizationStyle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBarAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAudioVisualizationSize)).EndInit();
            this.tabRemote.ResumeLayout(false);
            this.tabRemote.PerformLayout();
            this.gbRestApi.ResumeLayout(false);
            this.gbRestApi.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRestPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CheckBox cbQuietHours;
        private Label label1;
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private GroupBox gpVolumeSetting;
        private Label lbByPercent;
        private NumericUpDown nudQuietHourPercentage;
        private RadioButton rbDecreaseVolumeQuietHours;
        private RadioButton rbPauseQuiet;
        private Button bCancel;
        private Button bOK;
        private Button btAlbumNaming;
        private TextBox tbTestQuietHour;
        private Button btnTestQuietHour;
        private Label lbLanguage;
        private ComboBox cmbSelectLanguageValue;
        private Button btnModifiedRandomization;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem mnuLocalization;
        private ToolStripMenuItem mnuDumpLanguage;
        private CheckBox cbCheckUpdatesStartup;
        private TabControl tcMain;
        private TabPage tpMain;
        private TabPage tpAdditional;
        private GroupBox gbAudioVisualizationStyle;
        private RadioButton rbAudioVisualizationLines;
        private RadioButton rbAudioVisualizationBars;
        private RadioButton rbAudioVisualizationOff;
        private Label lbVisualizationWindowSize;
        private NumericUpDown nudAudioVisualizationSize;
        private Label lbAudioVisualizationSizePercentage;
        private CheckBox cbAudioVisualizationCombineChannels;
        private ToolStripMenuItem mnuDatabaseMigration;
        private GroupBox gbStackQueue;
        private TrackBar tbStackQueueRandom;
        private Label lbStackQueueRandom;
        private TableLayoutPanel tlpStackQueueRandomLabel;
        private Label lbStackQueueRandomValue;
        private CheckBox cbBalancedBars;
        private NumericUpDown nudBarAmount;
        private Label lbBarAmount;
        private CheckBox cbDisplayVolumeAndPoints;
        private ToolStripMenuItem mnuThemeSettings;
        private CheckBox cbHideAlbumImage;
        private TabPage tabRemote;
        private Label lbRestApiHeader;
        private GroupBox gbRestApi;
        private NumericUpDown nudRestPort;
        private Label lbRemoteControlPort;
        private CheckBox cbRestEnabled;
    }
}