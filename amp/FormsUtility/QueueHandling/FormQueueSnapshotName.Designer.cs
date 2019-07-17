using System.ComponentModel;
using System.Windows.Forms;

namespace amp.FormsUtility.QueueHandling
{
    partial class FormQueueSnapshotName
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQueueSnapshotName));
            this.tbQueueName = new System.Windows.Forms.TextBox();
            this.lbNewQueueName = new System.Windows.Forms.Label();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbQueueName
            // 
            this.tbQueueName.Location = new System.Drawing.Point(107, 12);
            this.tbQueueName.Name = "tbQueueName";
            this.tbQueueName.Size = new System.Drawing.Size(351, 20);
            this.tbQueueName.TabIndex = 11;
            this.tbQueueName.TextChanged += new System.EventHandler(this.tbQueueName_TextChanged);
            // 
            // lbNewQueueName
            // 
            this.lbNewQueueName.AutoSize = true;
            this.lbNewQueueName.Location = new System.Drawing.Point(12, 15);
            this.lbNewQueueName.Name = "lbNewQueueName";
            this.lbNewQueueName.Size = new System.Drawing.Size(70, 13);
            this.lbNewQueueName.TabIndex = 10;
            this.lbNewQueueName.Text = "Give a name:";
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(12, 38);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 9;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bOK
            // 
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.Enabled = false;
            this.bOK.Location = new System.Drawing.Point(383, 38);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 8;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            // 
            // FormQueueSnapshotName
            // 
            this.AcceptButton = this.bOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(472, 73);
            this.Controls.Add(this.tbQueueName);
            this.Controls.Add(this.lbNewQueueName);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormQueueSnapshotName";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Save queue snapshot";
            this.Shown += new System.EventHandler(this.FormQueueSnapshotName_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox tbQueueName;
        private Label lbNewQueueName;
        private Button bCancel;
        private Button bOK;
    }
}