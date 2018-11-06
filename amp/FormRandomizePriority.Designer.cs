namespace amp
{
    partial class FormRandomizePriority
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRandomizePriority));
            this.tbRating = new System.Windows.Forms.TrackBar();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.lbTolerancePercentageValue = new System.Windows.Forms.Label();
            this.tbTolerancePercentage = new System.Windows.Forms.TrackBar();
            this.lbTolerancePercentage = new System.Windows.Forms.Label();
            this.cbSkippedCountEnabled = new System.Windows.Forms.CheckBox();
            this.cbRandomizedCountEnabled = new System.Windows.Forms.CheckBox();
            this.cbPlayedCountEnabled = new System.Windows.Forms.CheckBox();
            this.tbSkippedCount = new System.Windows.Forms.TrackBar();
            this.tbRandomizedCount = new System.Windows.Forms.TrackBar();
            this.tbPlayedCount = new System.Windows.Forms.TrackBar();
            this.lbRating = new System.Windows.Forms.Label();
            this.lbPlayedCount = new System.Windows.Forms.Label();
            this.lbRandomizedCount = new System.Windows.Forms.Label();
            this.lbSkippedCount = new System.Windows.Forms.Label();
            this.cbModifiedRandomizationEnabled = new System.Windows.Forms.CheckBox();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.btDefault = new System.Windows.Forms.Button();
            this.cbRatingEnabled = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.tbRating)).BeginInit();
            this.tlpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbTolerancePercentage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSkippedCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRandomizedCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPlayedCount)).BeginInit();
            this.SuspendLayout();
            // 
            // tbRating
            // 
            this.tbRating.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbRating.Location = new System.Drawing.Point(314, 3);
            this.tbRating.Maximum = 1000;
            this.tbRating.Name = "tbRating";
            this.tbRating.Size = new System.Drawing.Size(174, 23);
            this.tbRating.TabIndex = 0;
            this.tbRating.TickFrequency = 100;
            this.tbRating.Value = 500;
            // 
            // tlpMain
            // 
            this.tlpMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpMain.ColumnCount = 3;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpMain.Controls.Add(this.lbTolerancePercentageValue, 2, 4);
            this.tlpMain.Controls.Add(this.tbTolerancePercentage, 1, 4);
            this.tlpMain.Controls.Add(this.lbTolerancePercentage, 0, 4);
            this.tlpMain.Controls.Add(this.cbSkippedCountEnabled, 2, 3);
            this.tlpMain.Controls.Add(this.cbRandomizedCountEnabled, 2, 2);
            this.tlpMain.Controls.Add(this.cbPlayedCountEnabled, 2, 1);
            this.tlpMain.Controls.Add(this.tbSkippedCount, 1, 3);
            this.tlpMain.Controls.Add(this.tbRandomizedCount, 1, 2);
            this.tlpMain.Controls.Add(this.tbRating, 1, 0);
            this.tlpMain.Controls.Add(this.tbPlayedCount, 1, 1);
            this.tlpMain.Controls.Add(this.lbRating, 0, 0);
            this.tlpMain.Controls.Add(this.lbPlayedCount, 0, 1);
            this.tlpMain.Controls.Add(this.lbRandomizedCount, 0, 2);
            this.tlpMain.Controls.Add(this.lbSkippedCount, 0, 3);
            this.tlpMain.Controls.Add(this.cbModifiedRandomizationEnabled, 0, 5);
            this.tlpMain.Controls.Add(this.btOK, 0, 7);
            this.tlpMain.Controls.Add(this.btCancel, 1, 7);
            this.tlpMain.Controls.Add(this.btDefault, 1, 5);
            this.tlpMain.Controls.Add(this.cbRatingEnabled, 2, 0);
            this.tlpMain.Location = new System.Drawing.Point(12, 12);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 8;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.Size = new System.Drawing.Size(546, 254);
            this.tlpMain.TabIndex = 1;
            // 
            // lbTolerancePercentageValue
            // 
            this.lbTolerancePercentageValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbTolerancePercentageValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTolerancePercentageValue.Location = new System.Drawing.Point(494, 116);
            this.lbTolerancePercentageValue.Name = "lbTolerancePercentageValue";
            this.lbTolerancePercentageValue.Size = new System.Drawing.Size(49, 29);
            this.lbTolerancePercentageValue.TabIndex = 18;
            this.lbTolerancePercentageValue.Text = "50";
            this.lbTolerancePercentageValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbTolerancePercentage
            // 
            this.tbTolerancePercentage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbTolerancePercentage.Location = new System.Drawing.Point(314, 119);
            this.tbTolerancePercentage.Maximum = 1000;
            this.tbTolerancePercentage.Name = "tbTolerancePercentage";
            this.tbTolerancePercentage.Size = new System.Drawing.Size(174, 23);
            this.tbTolerancePercentage.TabIndex = 17;
            this.tbTolerancePercentage.TickFrequency = 100;
            this.tbTolerancePercentage.Value = 500;
            this.tbTolerancePercentage.ValueChanged += new System.EventHandler(this.tbTolerancePercentage_ValueChanged);
            // 
            // lbTolerancePercentage
            // 
            this.lbTolerancePercentage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbTolerancePercentage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTolerancePercentage.Location = new System.Drawing.Point(3, 116);
            this.lbTolerancePercentage.Name = "lbTolerancePercentage";
            this.lbTolerancePercentage.Size = new System.Drawing.Size(305, 29);
            this.lbTolerancePercentage.TabIndex = 16;
            this.lbTolerancePercentage.Text = "Tolerance (%)";
            this.lbTolerancePercentage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSkippedCountEnabled
            // 
            this.cbSkippedCountEnabled.AutoSize = true;
            this.cbSkippedCountEnabled.Location = new System.Drawing.Point(511, 97);
            this.cbSkippedCountEnabled.Margin = new System.Windows.Forms.Padding(20, 10, 3, 3);
            this.cbSkippedCountEnabled.Name = "cbSkippedCountEnabled";
            this.cbSkippedCountEnabled.Size = new System.Drawing.Size(15, 14);
            this.cbSkippedCountEnabled.TabIndex = 15;
            this.cbSkippedCountEnabled.UseVisualStyleBackColor = true;
            this.cbSkippedCountEnabled.CheckedChanged += new System.EventHandler(this.cbCommon_CheckedChanged);
            // 
            // cbRandomizedCountEnabled
            // 
            this.cbRandomizedCountEnabled.AutoSize = true;
            this.cbRandomizedCountEnabled.Location = new System.Drawing.Point(511, 68);
            this.cbRandomizedCountEnabled.Margin = new System.Windows.Forms.Padding(20, 10, 3, 3);
            this.cbRandomizedCountEnabled.Name = "cbRandomizedCountEnabled";
            this.cbRandomizedCountEnabled.Size = new System.Drawing.Size(15, 14);
            this.cbRandomizedCountEnabled.TabIndex = 14;
            this.cbRandomizedCountEnabled.UseVisualStyleBackColor = true;
            this.cbRandomizedCountEnabled.CheckedChanged += new System.EventHandler(this.cbCommon_CheckedChanged);
            // 
            // cbPlayedCountEnabled
            // 
            this.cbPlayedCountEnabled.AutoSize = true;
            this.cbPlayedCountEnabled.Location = new System.Drawing.Point(511, 39);
            this.cbPlayedCountEnabled.Margin = new System.Windows.Forms.Padding(20, 10, 3, 3);
            this.cbPlayedCountEnabled.Name = "cbPlayedCountEnabled";
            this.cbPlayedCountEnabled.Size = new System.Drawing.Size(15, 14);
            this.cbPlayedCountEnabled.TabIndex = 13;
            this.cbPlayedCountEnabled.UseVisualStyleBackColor = true;
            this.cbPlayedCountEnabled.CheckedChanged += new System.EventHandler(this.cbCommon_CheckedChanged);
            // 
            // tbSkippedCount
            // 
            this.tbSkippedCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSkippedCount.Location = new System.Drawing.Point(314, 90);
            this.tbSkippedCount.Maximum = 1000;
            this.tbSkippedCount.Name = "tbSkippedCount";
            this.tbSkippedCount.Size = new System.Drawing.Size(174, 23);
            this.tbSkippedCount.TabIndex = 3;
            this.tbSkippedCount.TickFrequency = 100;
            this.tbSkippedCount.Value = 500;
            // 
            // tbRandomizedCount
            // 
            this.tbRandomizedCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbRandomizedCount.Location = new System.Drawing.Point(314, 61);
            this.tbRandomizedCount.Maximum = 1000;
            this.tbRandomizedCount.Name = "tbRandomizedCount";
            this.tbRandomizedCount.Size = new System.Drawing.Size(174, 23);
            this.tbRandomizedCount.TabIndex = 2;
            this.tbRandomizedCount.TickFrequency = 100;
            this.tbRandomizedCount.Value = 500;
            // 
            // tbPlayedCount
            // 
            this.tbPlayedCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbPlayedCount.Location = new System.Drawing.Point(314, 32);
            this.tbPlayedCount.Maximum = 1000;
            this.tbPlayedCount.Name = "tbPlayedCount";
            this.tbPlayedCount.Size = new System.Drawing.Size(174, 23);
            this.tbPlayedCount.TabIndex = 1;
            this.tbPlayedCount.TickFrequency = 100;
            this.tbPlayedCount.Value = 500;
            // 
            // lbRating
            // 
            this.lbRating.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbRating.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbRating.Location = new System.Drawing.Point(3, 0);
            this.lbRating.Name = "lbRating";
            this.lbRating.Size = new System.Drawing.Size(305, 29);
            this.lbRating.TabIndex = 3;
            this.lbRating.Text = "Rating";
            this.lbRating.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbPlayedCount
            // 
            this.lbPlayedCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPlayedCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPlayedCount.Location = new System.Drawing.Point(3, 29);
            this.lbPlayedCount.Name = "lbPlayedCount";
            this.lbPlayedCount.Size = new System.Drawing.Size(305, 29);
            this.lbPlayedCount.TabIndex = 4;
            this.lbPlayedCount.Text = "Played count";
            this.lbPlayedCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbRandomizedCount
            // 
            this.lbRandomizedCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbRandomizedCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbRandomizedCount.Location = new System.Drawing.Point(3, 58);
            this.lbRandomizedCount.Name = "lbRandomizedCount";
            this.lbRandomizedCount.Size = new System.Drawing.Size(305, 29);
            this.lbRandomizedCount.TabIndex = 5;
            this.lbRandomizedCount.Text = "Randomized count";
            this.lbRandomizedCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbSkippedCount
            // 
            this.lbSkippedCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbSkippedCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSkippedCount.Location = new System.Drawing.Point(3, 87);
            this.lbSkippedCount.Name = "lbSkippedCount";
            this.lbSkippedCount.Size = new System.Drawing.Size(305, 29);
            this.lbSkippedCount.TabIndex = 6;
            this.lbSkippedCount.Text = "Skipped count";
            this.lbSkippedCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbModifiedRandomizationEnabled
            // 
            this.cbModifiedRandomizationEnabled.AutoSize = true;
            this.cbModifiedRandomizationEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbModifiedRandomizationEnabled.Location = new System.Drawing.Point(3, 155);
            this.cbModifiedRandomizationEnabled.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.cbModifiedRandomizationEnabled.Name = "cbModifiedRandomizationEnabled";
            this.cbModifiedRandomizationEnabled.Size = new System.Drawing.Size(205, 17);
            this.cbModifiedRandomizationEnabled.TabIndex = 8;
            this.cbModifiedRandomizationEnabled.Text = "Modified randomization enabled";
            this.cbModifiedRandomizationEnabled.UseVisualStyleBackColor = true;
            // 
            // btOK
            // 
            this.btOK.Dock = System.Windows.Forms.DockStyle.Left;
            this.btOK.Location = new System.Drawing.Point(3, 212);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 39);
            this.btOK.TabIndex = 9;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.tlpMain.SetColumnSpan(this.btCancel, 2);
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btCancel.Location = new System.Drawing.Point(468, 212);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 39);
            this.btCancel.TabIndex = 10;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btDefault
            // 
            this.btDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpMain.SetColumnSpan(this.btDefault, 2);
            this.btDefault.Location = new System.Drawing.Point(468, 148);
            this.btDefault.Name = "btDefault";
            this.btDefault.Size = new System.Drawing.Size(75, 29);
            this.btDefault.TabIndex = 11;
            this.btDefault.Text = "Default";
            this.btDefault.UseVisualStyleBackColor = true;
            this.btDefault.Click += new System.EventHandler(this.btDefault_Click);
            // 
            // cbRatingEnabled
            // 
            this.cbRatingEnabled.AutoSize = true;
            this.cbRatingEnabled.Location = new System.Drawing.Point(511, 10);
            this.cbRatingEnabled.Margin = new System.Windows.Forms.Padding(20, 10, 3, 3);
            this.cbRatingEnabled.Name = "cbRatingEnabled";
            this.cbRatingEnabled.Size = new System.Drawing.Size(15, 14);
            this.cbRatingEnabled.TabIndex = 12;
            this.cbRatingEnabled.UseVisualStyleBackColor = true;
            this.cbRatingEnabled.CheckedChanged += new System.EventHandler(this.cbCommon_CheckedChanged);
            // 
            // FormRandomizePriority
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(570, 278);
            this.Controls.Add(this.tlpMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormRandomizePriority";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Randomization priority";
            ((System.ComponentModel.ISupportInitialize)(this.tbRating)).EndInit();
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbTolerancePercentage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbSkippedCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbRandomizedCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPlayedCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TrackBar tbRating;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.TrackBar tbSkippedCount;
        private System.Windows.Forms.TrackBar tbRandomizedCount;
        private System.Windows.Forms.TrackBar tbPlayedCount;
        private System.Windows.Forms.Label lbRating;
        private System.Windows.Forms.Label lbPlayedCount;
        private System.Windows.Forms.Label lbRandomizedCount;
        private System.Windows.Forms.Label lbSkippedCount;
        private System.Windows.Forms.CheckBox cbModifiedRandomizationEnabled;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btDefault;
        private System.Windows.Forms.CheckBox cbSkippedCountEnabled;
        private System.Windows.Forms.CheckBox cbRandomizedCountEnabled;
        private System.Windows.Forms.CheckBox cbPlayedCountEnabled;
        private System.Windows.Forms.CheckBox cbRatingEnabled;
        private System.Windows.Forms.TrackBar tbTolerancePercentage;
        private System.Windows.Forms.Label lbTolerancePercentage;
        private System.Windows.Forms.Label lbTolerancePercentageValue;
    }
}