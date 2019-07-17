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
            this.btnModifiedRandomization = new System.Windows.Forms.Button();
            this.btAlbumNaming = new System.Windows.Forms.Button();
            this.gpVolumeSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuietHourPercentage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLatency)).BeginInit();
            this.gpRemoteControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbQuietHours
            // 
            this.cbQuietHours.AutoSize = true;
            this.cbQuietHours.Location = new System.Drawing.Point(12, 12);
            this.cbQuietHours.Name = "cbQuietHours";
            this.cbQuietHours.Size = new System.Drawing.Size(120, 17);
            this.cbQuietHours.TabIndex = 0;
            this.cbQuietHours.Text = "Enabled quiet hours";
            this.cbQuietHours.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(127, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "_";
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "HH\':\'mm";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(12, 35);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.ShowUpDown = true;
            this.dtpFrom.Size = new System.Drawing.Size(92, 20);
            this.dtpFrom.TabIndex = 1;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "HH\':\'mm";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(164, 35);
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
            this.gpVolumeSetting.Location = new System.Drawing.Point(12, 61);
            this.gpVolumeSetting.Name = "gpVolumeSetting";
            this.gpVolumeSetting.Size = new System.Drawing.Size(245, 96);
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
            this.bCancel.Location = new System.Drawing.Point(12, 432);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 8;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.Location = new System.Drawing.Point(182, 432);
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
            this.lbLatency.Location = new System.Drawing.Point(9, 300);
            this.lbLatency.Name = "lbLatency";
            this.lbLatency.Size = new System.Drawing.Size(76, 13);
            this.lbLatency.TabIndex = 14;
            this.lbLatency.Text = "Lantency (ms):";
            // 
            // nudLatency
            // 
            this.nudLatency.Location = new System.Drawing.Point(184, 298);
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
            this.gpRemoteControl.Location = new System.Drawing.Point(12, 163);
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
            this.tbRemoteControlURI.TextChanged += new System.EventHandler(this.tbRemoteControllURI_TextChanged);
            // 
            // tbTestQuietHour
            // 
            this.tbTestQuietHour.Location = new System.Drawing.Point(281, 200);
            this.tbTestQuietHour.Multiline = true;
            this.tbTestQuietHour.Name = "tbTestQuietHour";
            this.tbTestQuietHour.Size = new System.Drawing.Size(269, 161);
            this.tbTestQuietHour.TabIndex = 19;
            // 
            // btnTestQuietHour
            // 
            this.btnTestQuietHour.Location = new System.Drawing.Point(281, 171);
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
            this.lbLanguage.Location = new System.Drawing.Point(9, 325);
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
            this.cmbSelectLanguageValue.Location = new System.Drawing.Point(12, 346);
            this.cmbSelectLanguageValue.Name = "cmbSelectLanguageValue";
            this.cmbSelectLanguageValue.Size = new System.Drawing.Size(244, 21);
            this.cmbSelectLanguageValue.TabIndex = 22;
            // 
            // btnModifiedRandomization
            // 
            this.btnModifiedRandomization.Image = global::amp.Properties.Resources.media_shuffle;
            this.btnModifiedRandomization.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnModifiedRandomization.Location = new System.Drawing.Point(12, 374);
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
            this.btAlbumNaming.Location = new System.Drawing.Point(12, 403);
            this.btAlbumNaming.Name = "btAlbumNaming";
            this.btAlbumNaming.Size = new System.Drawing.Size(244, 23);
            this.btAlbumNaming.TabIndex = 18;
            this.btAlbumNaming.Text = "Album naming";
            this.btAlbumNaming.UseVisualStyleBackColor = true;
            this.btAlbumNaming.Click += new System.EventHandler(this.btAlbumNaming_Click);
            // 
            // FormSettings
            // 
            this.AcceptButton = this.bOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(269, 465);
            this.Controls.Add(this.btnModifiedRandomization);
            this.Controls.Add(this.cmbSelectLanguageValue);
            this.Controls.Add(this.lbLanguage);
            this.Controls.Add(this.btnTestQuietHour);
            this.Controls.Add(this.tbTestQuietHour);
            this.Controls.Add(this.btAlbumNaming);
            this.Controls.Add(this.gpRemoteControl);
            this.Controls.Add(this.nudLatency);
            this.Controls.Add(this.lbLatency);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.gpVolumeSetting);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.cbQuietHours);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
    }
}