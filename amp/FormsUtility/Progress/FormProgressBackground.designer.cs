using System.ComponentModel;
using System.Windows.Forms;
using AmpControls;

namespace amp.FormsUtility.Progress
{
    partial class FormProgressBackground
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProgressBackground));
            this.lbLoading = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.btCancel = new System.Windows.Forms.Button();
            this.pbProgress = new ImageProgressBar();
            this.SuspendLayout();
            // 
            // lbLoading
            // 
            this.lbLoading.AutoSize = true;
            this.lbLoading.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLoading.Location = new System.Drawing.Point(129, 20);
            this.lbLoading.Name = "lbLoading";
            this.lbLoading.Size = new System.Drawing.Size(131, 20);
            this.lbLoading.TabIndex = 0;
            this.lbLoading.Text = "amp# loading...";
            // 
            // lbStatus
            // 
            this.lbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStatus.Location = new System.Drawing.Point(129, 52);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(353, 20);
            this.lbStatus.TabIndex = 1;
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.Location = new System.Drawing.Point(407, 91);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // pbProgress
            // 
            this.pbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbProgress.Location = new System.Drawing.Point(133, 91);
            this.pbProgress.Margin = new System.Windows.Forms.Padding(0);
            this.pbProgress.Maximum = 100;
            this.pbProgress.Minimum = 0;
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.ProgressBarImage = ((System.Drawing.Image)(resources.GetObject("pbProgress.ProgressBarImage")));
            this.pbProgress.Size = new System.Drawing.Size(245, 25);
            this.pbProgress.TabIndex = 4;
            this.pbProgress.Value = 0;
            // 
            // FormProgressBackground
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.BackgroundImage = global::amp.Properties.Resources.loading;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(494, 126);
            this.ControlBox = false;
            this.Controls.Add(this.pbProgress);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.lbLoading);
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormProgressBackground";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Shown += new System.EventHandler(this.FormProgressBackground_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lbLoading;
        private Label lbStatus;
        private Button btCancel;
        private ImageProgressBar pbProgress;
    }
}