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
            this.lbRemoteControlURIVValue = new System.Windows.Forms.Label();
            this.btAssignRemoteControlURI = new System.Windows.Forms.Button();
            this.lbLatency = new System.Windows.Forms.Label();
            this.nudLatency = new System.Windows.Forms.NumericUpDown();
            this.cbRemoteControlEnabled = new System.Windows.Forms.CheckBox();
            this.gpRemoteControl = new System.Windows.Forms.GroupBox();
            this.lbRemoteControlURI = new System.Windows.Forms.Label();
            this.tbRemoteControlURI = new System.Windows.Forms.TextBox();
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
            this.cbDisplayVolumeAndPoints = new System.Windows.Forms.CheckBox();
            this.gbStackQueue = new System.Windows.Forms.GroupBox();
            this.tlpStackQueueRandomLabel = new System.Windows.Forms.TableLayoutPanel();
            this.lbStackQueueRandom = new System.Windows.Forms.Label();
            this.lbStackQueueRandomValue = new System.Windows.Forms.Label();
            this.tbStackQueueRandom = new System.Windows.Forms.TrackBar();
            this.gbMemoryBuffering = new System.Windows.Forms.GroupBox();
            this.cbMemoryBuffering = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nudMemoryBuffering = new System.Windows.Forms.NumericUpDown();
            this.lbMemoryBuffer = new System.Windows.Forms.Label();
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
            this.cbHideAlbumImage = new System.Windows.Forms.CheckBox();
            this.gpVolumeSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuietHourPercentage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLatency)).BeginInit();
            this.gpRemoteControl.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tpMain.SuspendLayout();
            this.tpAdditional.SuspendLayout();
            this.gbStackQueue.SuspendLayout();
            this.tlpStackQueueRandomLabel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbStackQueueRandom)).BeginInit();
            this.gbMemoryBuffering.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMemoryBuffering)).BeginInit();
            this.gbAudioVisualizationStyle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBarAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAudioVisualizationSize)).BeginInit();
            this.SuspendLayout();
            // 
            // cbQuietHours
            // 
            this.cbQuietHours.AutoSize = true;
            this.cbQuietHours.Location = new System.Drawing.Point(6, 6);
            this.cbQuietHours.Name = "cbQuietHours";
            this.cbQuietHours.Size = new System.Drawing.Size(120, 17);
            this.cbQuietHours.TabIndex = 0;
            this.cbQuietHours.Text = "Enabled quiet hours";
            this.cbQuietHours.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(121, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "_";
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "HH\':\'mm";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(6, 29);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.ShowUpDown = true;
            this.dtpFrom.Size = new System.Drawing.Size(92, 20);
            this.dtpFrom.TabIndex = 1;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "HH\':\'mm";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(158, 29);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.ShowUpDown = true;
            this.dtpTo.Size = new System.Drawing.Size(92, 20);
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
            this.gpVolumeSetting.Location = new System.Drawing.Point(6, 55);
            this.gpVolumeSetting.Name = "gpVolumeSetting";
            this.gpVolumeSetting.Size = new System.Drawing.Size(244, 96);
            this.gpVolumeSetting.TabIndex = 6;
            this.gpVolumeSetting.TabStop = false;
            // 
            // lbByPercent
            // 
            this.lbByPercent.AutoSize = true;
            this.lbByPercent.Location = new System.Drawing.Point(6, 62);
            this.lbByPercent.Name = "lbByPercent";
            this.lbByPercent.Size = new System.Drawing.Size(60, 13);
            this.lbByPercent.TabIndex = 6;
            this.lbByPercent.Text = "by percent:";
            // 
            // nudQuietHourPercentage
            // 
            this.nudQuietHourPercentage.Location = new System.Drawing.Point(110, 60);
            this.nudQuietHourPercentage.Name = "nudQuietHourPercentage";
            this.nudQuietHourPercentage.Size = new System.Drawing.Size(70, 20);
            this.nudQuietHourPercentage.TabIndex = 5;
            this.nudQuietHourPercentage.ValueChanged += new System.EventHandler(this.nudQuietHourPercentage_ValueChanged);
            // 
            // rbDecreaseVolumeQuietHours
            // 
            this.rbDecreaseVolumeQuietHours.AutoSize = true;
            this.rbDecreaseVolumeQuietHours.Location = new System.Drawing.Point(6, 42);
            this.rbDecreaseVolumeQuietHours.Name = "rbDecreaseVolumeQuietHours";
            this.rbDecreaseVolumeQuietHours.Size = new System.Drawing.Size(178, 17);
            this.rbDecreaseVolumeQuietHours.TabIndex = 1;
            this.rbDecreaseVolumeQuietHours.TabStop = true;
            this.rbDecreaseVolumeQuietHours.Text = "Decrease volume on quiet hours";
            this.rbDecreaseVolumeQuietHours.UseVisualStyleBackColor = true;
            // 
            // rbPauseQuiet
            // 
            this.rbPauseQuiet.AutoSize = true;
            this.rbPauseQuiet.Checked = true;
            this.rbPauseQuiet.Location = new System.Drawing.Point(6, 19);
            this.rbPauseQuiet.Name = "rbPauseQuiet";
            this.rbPauseQuiet.Size = new System.Drawing.Size(125, 17);
            this.rbPauseQuiet.TabIndex = 0;
            this.rbPauseQuiet.TabStop = true;
            this.rbPauseQuiet.Text = "Pause on quiet hours";
            this.rbPauseQuiet.UseVisualStyleBackColor = true;
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(13, 519);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 8;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.Location = new System.Drawing.Point(199, 519);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 7;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // lbRemoteControlURIVValue
            // 
            this.lbRemoteControlURIVValue.AutoSize = true;
            this.lbRemoteControlURIVValue.Location = new System.Drawing.Point(6, 76);
            this.lbRemoteControlURIVValue.Name = "lbRemoteControlURIVValue";
            this.lbRemoteControlURIVValue.Size = new System.Drawing.Size(55, 13);
            this.lbRemoteControlURIVValue.TabIndex = 11;
            this.lbRemoteControlURIVValue.Text = "http(s)://?";
            // 
            // btAssignRemoteControlURI
            // 
            this.btAssignRemoteControlURI.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btAssignRemoteControlURI.Location = new System.Drawing.Point(6, 92);
            this.btAssignRemoteControlURI.Name = "btAssignRemoteControlURI";
            this.btAssignRemoteControlURI.Size = new System.Drawing.Size(229, 23);
            this.btAssignRemoteControlURI.TabIndex = 12;
            this.btAssignRemoteControlURI.Text = "Set Url";
            this.btAssignRemoteControlURI.UseVisualStyleBackColor = true;
            this.btAssignRemoteControlURI.Click += new System.EventHandler(this.btAssignRemoteControlURI_Click);
            // 
            // lbLatency
            // 
            this.lbLatency.AutoSize = true;
            this.lbLatency.Location = new System.Drawing.Point(3, 294);
            this.lbLatency.Name = "lbLatency";
            this.lbLatency.Size = new System.Drawing.Size(76, 13);
            this.lbLatency.TabIndex = 14;
            this.lbLatency.Text = "Lantency (ms):";
            // 
            // nudLatency
            // 
            this.nudLatency.Location = new System.Drawing.Point(178, 292);
            this.nudLatency.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nudLatency.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudLatency.Name = "nudLatency";
            this.nudLatency.Size = new System.Drawing.Size(70, 20);
            this.nudLatency.TabIndex = 15;
            this.nudLatency.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // cbRemoteControlEnabled
            // 
            this.cbRemoteControlEnabled.AutoSize = true;
            this.cbRemoteControlEnabled.Location = new System.Drawing.Point(6, 19);
            this.cbRemoteControlEnabled.Name = "cbRemoteControlEnabled";
            this.cbRemoteControlEnabled.Size = new System.Drawing.Size(65, 17);
            this.cbRemoteControlEnabled.TabIndex = 16;
            this.cbRemoteControlEnabled.Text = "Enabled";
            this.cbRemoteControlEnabled.UseVisualStyleBackColor = true;
            // 
            // gpRemoteControl
            // 
            this.gpRemoteControl.Controls.Add(this.lbRemoteControlURI);
            this.gpRemoteControl.Controls.Add(this.cbRemoteControlEnabled);
            this.gpRemoteControl.Controls.Add(this.tbRemoteControlURI);
            this.gpRemoteControl.Controls.Add(this.btAssignRemoteControlURI);
            this.gpRemoteControl.Controls.Add(this.lbRemoteControlURIVValue);
            this.gpRemoteControl.Location = new System.Drawing.Point(6, 157);
            this.gpRemoteControl.Name = "gpRemoteControl";
            this.gpRemoteControl.Size = new System.Drawing.Size(244, 125);
            this.gpRemoteControl.TabIndex = 17;
            this.gpRemoteControl.TabStop = false;
            this.gpRemoteControl.Text = "Remote control API";
            // 
            // lbRemoteControlURI
            // 
            this.lbRemoteControlURI.AutoSize = true;
            this.lbRemoteControlURI.Location = new System.Drawing.Point(6, 37);
            this.lbRemoteControlURI.Name = "lbRemoteControlURI";
            this.lbRemoteControlURI.Size = new System.Drawing.Size(122, 13);
            this.lbRemoteControlURI.TabIndex = 9;
            this.lbRemoteControlURI.Text = "Remote control address:";
            // 
            // tbRemoteControlURI
            // 
            this.tbRemoteControlURI.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbRemoteControlURI.Location = new System.Drawing.Point(6, 53);
            this.tbRemoteControlURI.Name = "tbRemoteControlURI";
            this.tbRemoteControlURI.Size = new System.Drawing.Size(229, 20);
            this.tbRemoteControlURI.TabIndex = 10;
            this.tbRemoteControlURI.TextChanged += new System.EventHandler(this.tbRemoteControlURI_TextChanged);
            // 
            // tbTestQuietHour
            // 
            this.tbTestQuietHour.Location = new System.Drawing.Point(298, 206);
            this.tbTestQuietHour.Multiline = true;
            this.tbTestQuietHour.Name = "tbTestQuietHour";
            this.tbTestQuietHour.Size = new System.Drawing.Size(269, 161);
            this.tbTestQuietHour.TabIndex = 19;
            // 
            // btnTestQuietHour
            // 
            this.btnTestQuietHour.Location = new System.Drawing.Point(298, 177);
            this.btnTestQuietHour.Name = "btnTestQuietHour";
            this.btnTestQuietHour.Size = new System.Drawing.Size(269, 23);
            this.btnTestQuietHour.TabIndex = 20;
            this.btnTestQuietHour.Text = "Quiet hour test";
            this.btnTestQuietHour.UseVisualStyleBackColor = true;
            this.btnTestQuietHour.Click += new System.EventHandler(this.btnTestQuietHour_Click);
            // 
            // lbLanguage
            // 
            this.lbLanguage.AutoSize = true;
            this.lbLanguage.Location = new System.Drawing.Point(3, 319);
            this.lbLanguage.Name = "lbLanguage";
            this.lbLanguage.Size = new System.Drawing.Size(136, 13);
            this.lbLanguage.TabIndex = 21;
            this.lbLanguage.Text = "Language (requires restart):";
            // 
            // cmbSelectLanguageValue
            // 
            this.cmbSelectLanguageValue.DisplayMember = "DisplayName";
            this.cmbSelectLanguageValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectLanguageValue.FormattingEnabled = true;
            this.cmbSelectLanguageValue.Location = new System.Drawing.Point(6, 340);
            this.cmbSelectLanguageValue.Name = "cmbSelectLanguageValue";
            this.cmbSelectLanguageValue.Size = new System.Drawing.Size(244, 21);
            this.cmbSelectLanguageValue.TabIndex = 22;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.mnuThemeSettings});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(288, 24);
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
            this.btnModifiedRandomization.Location = new System.Drawing.Point(6, 368);
            this.btnModifiedRandomization.Name = "btnModifiedRandomization";
            this.btnModifiedRandomization.Size = new System.Drawing.Size(244, 23);
            this.btnModifiedRandomization.TabIndex = 23;
            this.btnModifiedRandomization.Text = "Modified randomization";
            this.btnModifiedRandomization.UseVisualStyleBackColor = true;
            this.btnModifiedRandomization.Click += new System.EventHandler(this.btnModifiedRandomization_Click);
            // 
            // btAlbumNaming
            // 
            this.btAlbumNaming.Image = global::amp.Properties.Resources.Modify;
            this.btAlbumNaming.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btAlbumNaming.Location = new System.Drawing.Point(6, 397);
            this.btAlbumNaming.Name = "btAlbumNaming";
            this.btAlbumNaming.Size = new System.Drawing.Size(244, 23);
            this.btAlbumNaming.TabIndex = 18;
            this.btAlbumNaming.Text = "Album naming";
            this.btAlbumNaming.UseVisualStyleBackColor = true;
            this.btAlbumNaming.Click += new System.EventHandler(this.btAlbumNaming_Click);
            // 
            // cbCheckUpdatesStartup
            // 
            this.cbCheckUpdatesStartup.AutoSize = true;
            this.cbCheckUpdatesStartup.Location = new System.Drawing.Point(6, 426);
            this.cbCheckUpdatesStartup.Name = "cbCheckUpdatesStartup";
            this.cbCheckUpdatesStartup.Size = new System.Drawing.Size(175, 17);
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
            this.tcMain.Location = new System.Drawing.Point(12, 27);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(266, 486);
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
            this.tpMain.Controls.Add(this.lbLatency);
            this.tpMain.Controls.Add(this.nudLatency);
            this.tpMain.Controls.Add(this.btAlbumNaming);
            this.tpMain.Controls.Add(this.gpRemoteControl);
            this.tpMain.Location = new System.Drawing.Point(4, 22);
            this.tpMain.Name = "tpMain";
            this.tpMain.Padding = new System.Windows.Forms.Padding(3);
            this.tpMain.Size = new System.Drawing.Size(258, 448);
            this.tpMain.TabIndex = 0;
            this.tpMain.Text = "Main";
            this.tpMain.UseVisualStyleBackColor = true;
            // 
            // tpAdditional
            // 
            this.tpAdditional.Controls.Add(this.cbHideAlbumImage);
            this.tpAdditional.Controls.Add(this.cbDisplayVolumeAndPoints);
            this.tpAdditional.Controls.Add(this.gbStackQueue);
            this.tpAdditional.Controls.Add(this.gbMemoryBuffering);
            this.tpAdditional.Controls.Add(this.gbAudioVisualizationStyle);
            this.tpAdditional.Location = new System.Drawing.Point(4, 22);
            this.tpAdditional.Name = "tpAdditional";
            this.tpAdditional.Padding = new System.Windows.Forms.Padding(3);
            this.tpAdditional.Size = new System.Drawing.Size(258, 460);
            this.tpAdditional.TabIndex = 1;
            this.tpAdditional.Text = "Additional";
            this.tpAdditional.UseVisualStyleBackColor = true;
            // 
            // cbDisplayVolumeAndPoints
            // 
            this.cbDisplayVolumeAndPoints.AutoSize = true;
            this.cbDisplayVolumeAndPoints.Location = new System.Drawing.Point(7, 407);
            this.cbDisplayVolumeAndPoints.Name = "cbDisplayVolumeAndPoints";
            this.cbDisplayVolumeAndPoints.Size = new System.Drawing.Size(235, 17);
            this.cbDisplayVolumeAndPoints.TabIndex = 3;
            this.cbDisplayVolumeAndPoints.Text = "Display volume settings and points on the UI";
            this.cbDisplayVolumeAndPoints.UseVisualStyleBackColor = true;
            // 
            // gbStackQueue
            // 
            this.gbStackQueue.Controls.Add(this.tlpStackQueueRandomLabel);
            this.gbStackQueue.Controls.Add(this.tbStackQueueRandom);
            this.gbStackQueue.Location = new System.Drawing.Point(6, 328);
            this.gbStackQueue.Name = "gbStackQueue";
            this.gbStackQueue.Size = new System.Drawing.Size(246, 73);
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
            this.tlpStackQueueRandomLabel.Location = new System.Drawing.Point(6, 19);
            this.tlpStackQueueRandomLabel.Name = "tlpStackQueueRandomLabel";
            this.tlpStackQueueRandomLabel.RowCount = 2;
            this.tlpStackQueueRandomLabel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpStackQueueRandomLabel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpStackQueueRandomLabel.Size = new System.Drawing.Size(240, 19);
            this.tlpStackQueueRandomLabel.TabIndex = 3;
            // 
            // lbStackQueueRandom
            // 
            this.lbStackQueueRandom.AutoSize = true;
            this.lbStackQueueRandom.Location = new System.Drawing.Point(3, 0);
            this.lbStackQueueRandom.Name = "lbStackQueueRandom";
            this.lbStackQueueRandom.Size = new System.Drawing.Size(178, 13);
            this.lbStackQueueRandom.TabIndex = 1;
            this.lbStackQueueRandom.Text = "Randomize stack from top (percent):";
            // 
            // lbStackQueueRandomValue
            // 
            this.lbStackQueueRandomValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbStackQueueRandomValue.AutoSize = true;
            this.lbStackQueueRandomValue.Location = new System.Drawing.Point(207, 0);
            this.lbStackQueueRandomValue.Name = "lbStackQueueRandomValue";
            this.lbStackQueueRandomValue.Size = new System.Drawing.Size(30, 13);
            this.lbStackQueueRandomValue.TabIndex = 2;
            this.lbStackQueueRandomValue.Text = "50 %";
            this.lbStackQueueRandomValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbStackQueueRandom
            // 
            this.tbStackQueueRandom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbStackQueueRandom.AutoSize = false;
            this.tbStackQueueRandom.Location = new System.Drawing.Point(6, 44);
            this.tbStackQueueRandom.Maximum = 100;
            this.tbStackQueueRandom.Name = "tbStackQueueRandom";
            this.tbStackQueueRandom.Size = new System.Drawing.Size(240, 20);
            this.tbStackQueueRandom.TabIndex = 0;
            this.tbStackQueueRandom.TickFrequency = 10;
            this.tbStackQueueRandom.Value = 50;
            this.tbStackQueueRandom.ValueChanged += new System.EventHandler(this.tbStackQueueRandom_ValueChanged);
            // 
            // gbMemoryBuffering
            // 
            this.gbMemoryBuffering.Controls.Add(this.cbMemoryBuffering);
            this.gbMemoryBuffering.Controls.Add(this.label2);
            this.gbMemoryBuffering.Controls.Add(this.nudMemoryBuffering);
            this.gbMemoryBuffering.Controls.Add(this.lbMemoryBuffer);
            this.gbMemoryBuffering.Location = new System.Drawing.Point(6, 260);
            this.gbMemoryBuffering.Name = "gbMemoryBuffering";
            this.gbMemoryBuffering.Size = new System.Drawing.Size(246, 62);
            this.gbMemoryBuffering.TabIndex = 1;
            this.gbMemoryBuffering.TabStop = false;
            this.gbMemoryBuffering.Text = "Memory buffering";
            // 
            // cbMemoryBuffering
            // 
            this.cbMemoryBuffering.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbMemoryBuffering.AutoSize = true;
            this.cbMemoryBuffering.Location = new System.Drawing.Point(225, 1);
            this.cbMemoryBuffering.Name = "cbMemoryBuffering";
            this.cbMemoryBuffering.Size = new System.Drawing.Size(15, 14);
            this.cbMemoryBuffering.TabIndex = 3;
            this.cbMemoryBuffering.UseVisualStyleBackColor = true;
            this.cbMemoryBuffering.CheckedChanged += new System.EventHandler(this.cbMemoryBuffering_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(99, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "MB into the memory";
            // 
            // nudMemoryBuffering
            // 
            this.nudMemoryBuffering.Location = new System.Drawing.Point(6, 32);
            this.nudMemoryBuffering.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudMemoryBuffering.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMemoryBuffering.Name = "nudMemoryBuffering";
            this.nudMemoryBuffering.Size = new System.Drawing.Size(87, 20);
            this.nudMemoryBuffering.TabIndex = 1;
            this.nudMemoryBuffering.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudMemoryBuffering.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // lbMemoryBuffer
            // 
            this.lbMemoryBuffer.AutoSize = true;
            this.lbMemoryBuffer.Location = new System.Drawing.Point(6, 16);
            this.lbMemoryBuffer.Name = "lbMemoryBuffer";
            this.lbMemoryBuffer.Size = new System.Drawing.Size(118, 13);
            this.lbMemoryBuffer.TabIndex = 0;
            this.lbMemoryBuffer.Text = "Buffer files smaller than:";
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
            this.gbAudioVisualizationStyle.Location = new System.Drawing.Point(6, 6);
            this.gbAudioVisualizationStyle.Name = "gbAudioVisualizationStyle";
            this.gbAudioVisualizationStyle.Size = new System.Drawing.Size(246, 248);
            this.gbAudioVisualizationStyle.TabIndex = 0;
            this.gbAudioVisualizationStyle.TabStop = false;
            this.gbAudioVisualizationStyle.Tag = "0";
            this.gbAudioVisualizationStyle.Text = "Audio visualization";
            // 
            // nudBarAmount
            // 
            this.nudBarAmount.Location = new System.Drawing.Point(6, 217);
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
            this.nudBarAmount.Size = new System.Drawing.Size(50, 20);
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
            this.lbBarAmount.Location = new System.Drawing.Point(6, 201);
            this.lbBarAmount.Name = "lbBarAmount";
            this.lbBarAmount.Size = new System.Drawing.Size(181, 13);
            this.lbBarAmount.TabIndex = 8;
            this.lbBarAmount.Text = "Amount of bars in audio visualization:";
            // 
            // cbBalancedBars
            // 
            this.cbBalancedBars.AutoSize = true;
            this.cbBalancedBars.Location = new System.Drawing.Point(6, 181);
            this.cbBalancedBars.Name = "cbBalancedBars";
            this.cbBalancedBars.Size = new System.Drawing.Size(141, 17);
            this.cbBalancedBars.TabIndex = 7;
            this.cbBalancedBars.Text = "Balanced bars (min-max)";
            this.cbBalancedBars.UseVisualStyleBackColor = true;
            // 
            // cbAudioVisualizationCombineChannels
            // 
            this.cbAudioVisualizationCombineChannels.AutoSize = true;
            this.cbAudioVisualizationCombineChannels.Location = new System.Drawing.Point(6, 158);
            this.cbAudioVisualizationCombineChannels.Name = "cbAudioVisualizationCombineChannels";
            this.cbAudioVisualizationCombineChannels.Size = new System.Drawing.Size(113, 17);
            this.cbAudioVisualizationCombineChannels.TabIndex = 6;
            this.cbAudioVisualizationCombineChannels.Text = "Combine channels";
            this.cbAudioVisualizationCombineChannels.UseVisualStyleBackColor = true;
            // 
            // lbAudioVisualizationSizePercentage
            // 
            this.lbAudioVisualizationSizePercentage.AutoSize = true;
            this.lbAudioVisualizationSizePercentage.Location = new System.Drawing.Point(108, 134);
            this.lbAudioVisualizationSizePercentage.Name = "lbAudioVisualizationSizePercentage";
            this.lbAudioVisualizationSizePercentage.Size = new System.Drawing.Size(15, 13);
            this.lbAudioVisualizationSizePercentage.TabIndex = 5;
            this.lbAudioVisualizationSizePercentage.Text = "%";
            // 
            // nudAudioVisualizationSize
            // 
            this.nudAudioVisualizationSize.Location = new System.Drawing.Point(6, 132);
            this.nudAudioVisualizationSize.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.nudAudioVisualizationSize.Name = "nudAudioVisualizationSize";
            this.nudAudioVisualizationSize.Size = new System.Drawing.Size(96, 20);
            this.nudAudioVisualizationSize.TabIndex = 4;
            this.nudAudioVisualizationSize.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // lbVisualizationWindowSize
            // 
            this.lbVisualizationWindowSize.Location = new System.Drawing.Point(6, 94);
            this.lbVisualizationWindowSize.Name = "lbVisualizationWindowSize";
            this.lbVisualizationWindowSize.Size = new System.Drawing.Size(234, 35);
            this.lbVisualizationWindowSize.TabIndex = 3;
            this.lbVisualizationWindowSize.Text = "Visualization size in of the main window area:";
            // 
            // rbAudioVisualizationLines
            // 
            this.rbAudioVisualizationLines.AutoSize = true;
            this.rbAudioVisualizationLines.Location = new System.Drawing.Point(6, 65);
            this.rbAudioVisualizationLines.Name = "rbAudioVisualizationLines";
            this.rbAudioVisualizationLines.Size = new System.Drawing.Size(75, 17);
            this.rbAudioVisualizationLines.TabIndex = 2;
            this.rbAudioVisualizationLines.Tag = "2";
            this.rbAudioVisualizationLines.Text = "Line graph";
            this.rbAudioVisualizationLines.UseVisualStyleBackColor = true;
            this.rbAudioVisualizationLines.CheckedChanged += new System.EventHandler(this.RbAudioVisualization_CheckedChanged);
            // 
            // rbAudioVisualizationBars
            // 
            this.rbAudioVisualizationBars.AutoSize = true;
            this.rbAudioVisualizationBars.Location = new System.Drawing.Point(6, 42);
            this.rbAudioVisualizationBars.Name = "rbAudioVisualizationBars";
            this.rbAudioVisualizationBars.Size = new System.Drawing.Size(71, 17);
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
            this.rbAudioVisualizationOff.Location = new System.Drawing.Point(6, 19);
            this.rbAudioVisualizationOff.Name = "rbAudioVisualizationOff";
            this.rbAudioVisualizationOff.Size = new System.Drawing.Size(39, 17);
            this.rbAudioVisualizationOff.TabIndex = 0;
            this.rbAudioVisualizationOff.TabStop = true;
            this.rbAudioVisualizationOff.Tag = "0";
            this.rbAudioVisualizationOff.Text = "Off";
            this.rbAudioVisualizationOff.UseVisualStyleBackColor = true;
            this.rbAudioVisualizationOff.CheckedChanged += new System.EventHandler(this.RbAudioVisualization_CheckedChanged);
            // 
            // cbHideAlbumImage
            // 
            this.cbHideAlbumImage.AutoSize = true;
            this.cbHideAlbumImage.Location = new System.Drawing.Point(6, 430);
            this.cbHideAlbumImage.Name = "cbHideAlbumImage";
            this.cbHideAlbumImage.Size = new System.Drawing.Size(172, 17);
            this.cbHideAlbumImage.TabIndex = 4;
            this.cbHideAlbumImage.Text = "Auto-hide album image window";
            this.cbHideAlbumImage.UseVisualStyleBackColor = true;
            // 
            // FormSettings
            // 
            this.AcceptButton = this.bOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(288, 559);
            this.Controls.Add(this.tcMain);
            this.Controls.Add(this.btnTestQuietHour);
            this.Controls.Add(this.tbTestQuietHour);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
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
            ((System.ComponentModel.ISupportInitialize)(this.nudLatency)).EndInit();
            this.gpRemoteControl.ResumeLayout(false);
            this.gpRemoteControl.PerformLayout();
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
            this.gbMemoryBuffering.ResumeLayout(false);
            this.gbMemoryBuffering.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMemoryBuffering)).EndInit();
            this.gbAudioVisualizationStyle.ResumeLayout(false);
            this.gbAudioVisualizationStyle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudBarAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAudioVisualizationSize)).EndInit();
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
        private Label lbRemoteControlURIVValue;
        private Button btAssignRemoteControlURI;
        private Label lbLatency;
        private NumericUpDown nudLatency;
        private CheckBox cbRemoteControlEnabled;
        private GroupBox gpRemoteControl;
        private Label lbRemoteControlURI;
        private TextBox tbRemoteControlURI;
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
        private GroupBox gbMemoryBuffering;
        private CheckBox cbMemoryBuffering;
        private Label label2;
        private NumericUpDown nudMemoryBuffering;
        private Label lbMemoryBuffer;
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
    }
}