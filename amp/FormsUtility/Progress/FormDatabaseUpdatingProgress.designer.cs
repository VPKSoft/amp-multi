namespace amp.FormsUtility.Progress
{
    partial class FormDatabaseUpdatingProgress
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDatabaseUpdatingProgress));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.lbDatabaseUpdating = new System.Windows.Forms.Label();
            this.lbItem = new System.Windows.Forms.Label();
            this.lbItemIndex = new System.Windows.Forms.Label();
            this.pbUpdateProgress = new System.Windows.Forms.ProgressBar();
            this.bwDatabaseUpdate = new System.ComponentModel.BackgroundWorker();
            this.tlpMain.SuspendLayout();
            this.SuspendLayout();
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
            this.tlpMain.Controls.Add(this.lbDatabaseUpdating, 0, 0);
            this.tlpMain.Controls.Add(this.lbItem, 0, 1);
            this.tlpMain.Controls.Add(this.lbItemIndex, 2, 1);
            this.tlpMain.Controls.Add(this.pbUpdateProgress, 0, 3);
            this.tlpMain.Location = new System.Drawing.Point(128, 12);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 4;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.Size = new System.Drawing.Size(354, 103);
            this.tlpMain.TabIndex = 0;
            // 
            // lbDatabaseUpdating
            // 
            this.lbDatabaseUpdating.AutoSize = true;
            this.tlpMain.SetColumnSpan(this.lbDatabaseUpdating, 5);
            this.lbDatabaseUpdating.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDatabaseUpdating.Location = new System.Drawing.Point(3, 3);
            this.lbDatabaseUpdating.Margin = new System.Windows.Forms.Padding(3);
            this.lbDatabaseUpdating.Name = "lbDatabaseUpdating";
            this.lbDatabaseUpdating.Size = new System.Drawing.Size(223, 20);
            this.lbDatabaseUpdating.TabIndex = 1;
            this.lbDatabaseUpdating.Text = "amp# database updating...";
            // 
            // lbItem
            // 
            this.lbItem.AutoSize = true;
            this.tlpMain.SetColumnSpan(this.lbItem, 2);
            this.lbItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbItem.Location = new System.Drawing.Point(3, 29);
            this.lbItem.Margin = new System.Windows.Forms.Padding(3);
            this.lbItem.Name = "lbItem";
            this.lbItem.Size = new System.Drawing.Size(50, 20);
            this.lbItem.TabIndex = 2;
            this.lbItem.Text = "Item:";
            // 
            // lbItemIndex
            // 
            this.lbItemIndex.AutoSize = true;
            this.tlpMain.SetColumnSpan(this.lbItemIndex, 3);
            this.lbItemIndex.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbItemIndex.Location = new System.Drawing.Point(143, 29);
            this.lbItemIndex.Margin = new System.Windows.Forms.Padding(3);
            this.lbItemIndex.Name = "lbItemIndex";
            this.lbItemIndex.Size = new System.Drawing.Size(56, 20);
            this.lbItemIndex.TabIndex = 3;
            this.lbItemIndex.Text = "(0 / 0)";
            // 
            // pbUpdateProgress
            // 
            this.pbUpdateProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpMain.SetColumnSpan(this.pbUpdateProgress, 5);
            this.pbUpdateProgress.Location = new System.Drawing.Point(3, 77);
            this.pbUpdateProgress.Name = "pbUpdateProgress";
            this.pbUpdateProgress.Size = new System.Drawing.Size(348, 23);
            this.pbUpdateProgress.Step = 1;
            this.pbUpdateProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbUpdateProgress.TabIndex = 4;
            this.pbUpdateProgress.Value = 50;
            // 
            // bwDatabaseUpdate
            // 
            this.bwDatabaseUpdate.WorkerReportsProgress = true;
            this.bwDatabaseUpdate.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwDatabaseUpdate_DoWork);
            this.bwDatabaseUpdate.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwDatabaseUpdate_ProgressChanged);
            this.bwDatabaseUpdate.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwDatabaseUpdate_RunWorkerCompleted);
            // 
            // FormDatabaseUpdatingProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.BackgroundImage = global::amp.Properties.Resources.loading;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(494, 127);
            this.ControlBox = false;
            this.Controls.Add(this.tlpMain);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDatabaseUpdatingProgress";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDatabaseUpdatingProgress_FormClosing);
            this.Shown += new System.EventHandler(this.FormDatabaseUpdatingProgress_Shown);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Label lbDatabaseUpdating;
        private System.Windows.Forms.Label lbItem;
        private System.Windows.Forms.Label lbItemIndex;
        private System.Windows.Forms.ProgressBar pbUpdateProgress;
        private System.ComponentModel.BackgroundWorker bwDatabaseUpdate;
    }
}