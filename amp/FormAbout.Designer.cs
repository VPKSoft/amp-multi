namespace amp
{
    partial class FormAbout
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
            this.tlpProductInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lbVersionText = new System.Windows.Forms.Label();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.lbProductNameText = new System.Windows.Forms.Label();
            this.lbProductName = new System.Windows.Forms.Label();
            this.lbVersion = new System.Windows.Forms.Label();
            this.lbCopyrightText = new System.Windows.Forms.Label();
            this.lbCopyright = new System.Windows.Forms.Label();
            this.lbCompanyNameText = new System.Windows.Forms.Label();
            this.lbCompanyName = new System.Windows.Forms.Label();
            this.lbBoxDescriptionText = new System.Windows.Forms.Label();
            this.tbBoxDescription = new System.Windows.Forms.TextBox();
            this.btOK = new System.Windows.Forms.Button();
            this.tlpProductInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpProductInfo
            // 
            this.tlpProductInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpProductInfo.ColumnCount = 2;
            this.tlpProductInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpProductInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpProductInfo.Controls.Add(this.lbVersionText, 0, 1);
            this.tlpProductInfo.Controls.Add(this.pbLogo, 0, 5);
            this.tlpProductInfo.Controls.Add(this.lbProductNameText, 0, 0);
            this.tlpProductInfo.Controls.Add(this.lbProductName, 1, 0);
            this.tlpProductInfo.Controls.Add(this.lbVersion, 1, 1);
            this.tlpProductInfo.Controls.Add(this.lbCopyrightText, 0, 2);
            this.tlpProductInfo.Controls.Add(this.lbCopyright, 1, 2);
            this.tlpProductInfo.Controls.Add(this.lbCompanyNameText, 0, 3);
            this.tlpProductInfo.Controls.Add(this.lbCompanyName, 1, 3);
            this.tlpProductInfo.Controls.Add(this.lbBoxDescriptionText, 0, 4);
            this.tlpProductInfo.Controls.Add(this.tbBoxDescription, 1, 4);
            this.tlpProductInfo.Location = new System.Drawing.Point(12, 12);
            this.tlpProductInfo.Name = "tlpProductInfo";
            this.tlpProductInfo.RowCount = 6;
            this.tlpProductInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProductInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProductInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProductInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProductInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProductInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProductInfo.Size = new System.Drawing.Size(323, 173);
            this.tlpProductInfo.TabIndex = 2;
            // 
            // lbVersionText
            // 
            this.lbVersionText.AutoSize = true;
            this.lbVersionText.Location = new System.Drawing.Point(3, 13);
            this.lbVersionText.Name = "lbVersionText";
            this.lbVersionText.Size = new System.Drawing.Size(45, 13);
            this.lbVersionText.TabIndex = 3;
            this.lbVersionText.Text = "Version:";
            // 
            // pbLogo
            // 
            this.pbLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbLogo.BackColor = System.Drawing.Color.Transparent;
            this.tlpProductInfo.SetColumnSpan(this.pbLogo, 2);
            this.pbLogo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbLogo.Image = global::amp.Properties.Resources.VPKSoftLogo_App;
            this.pbLogo.Location = new System.Drawing.Point(3, 100);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(317, 70);
            this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbLogo.TabIndex = 0;
            this.pbLogo.TabStop = false;
            this.pbLogo.Click += new System.EventHandler(this.pbLogo_Click);
            // 
            // lbProductNameText
            // 
            this.lbProductNameText.AutoSize = true;
            this.lbProductNameText.Location = new System.Drawing.Point(3, 0);
            this.lbProductNameText.Name = "lbProductNameText";
            this.lbProductNameText.Size = new System.Drawing.Size(76, 13);
            this.lbProductNameText.TabIndex = 2;
            this.lbProductNameText.Text = "Product name:";
            // 
            // lbProductName
            // 
            this.lbProductName.AutoSize = true;
            this.lbProductName.Location = new System.Drawing.Point(164, 0);
            this.lbProductName.Name = "lbProductName";
            this.lbProductName.Size = new System.Drawing.Size(80, 13);
            this.lbProductName.TabIndex = 3;
            this.lbProductName.Text = "lbProductName";
            // 
            // lbVersion
            // 
            this.lbVersion.AutoSize = true;
            this.lbVersion.Location = new System.Drawing.Point(164, 13);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(50, 13);
            this.lbVersion.TabIndex = 4;
            this.lbVersion.Text = "lbVersion";
            // 
            // lbCopyrightText
            // 
            this.lbCopyrightText.AutoSize = true;
            this.lbCopyrightText.Location = new System.Drawing.Point(3, 26);
            this.lbCopyrightText.Name = "lbCopyrightText";
            this.lbCopyrightText.Size = new System.Drawing.Size(54, 13);
            this.lbCopyrightText.TabIndex = 5;
            this.lbCopyrightText.Text = "Copyright:";
            // 
            // lbCopyright
            // 
            this.lbCopyright.AutoSize = true;
            this.lbCopyright.Location = new System.Drawing.Point(164, 26);
            this.lbCopyright.Name = "lbCopyright";
            this.lbCopyright.Size = new System.Drawing.Size(59, 13);
            this.lbCopyright.TabIndex = 6;
            this.lbCopyright.Text = "lbCopyright";
            // 
            // lbCompanyNameText
            // 
            this.lbCompanyNameText.AutoSize = true;
            this.lbCompanyNameText.Location = new System.Drawing.Point(3, 39);
            this.lbCompanyNameText.Name = "lbCompanyNameText";
            this.lbCompanyNameText.Size = new System.Drawing.Size(83, 13);
            this.lbCompanyNameText.TabIndex = 7;
            this.lbCompanyNameText.Text = "Company name:";
            // 
            // lbCompanyName
            // 
            this.lbCompanyName.AutoSize = true;
            this.lbCompanyName.Location = new System.Drawing.Point(164, 39);
            this.lbCompanyName.Name = "lbCompanyName";
            this.lbCompanyName.Size = new System.Drawing.Size(87, 13);
            this.lbCompanyName.TabIndex = 8;
            this.lbCompanyName.Text = "lbCompanyName";
            // 
            // lbBoxDescriptionText
            // 
            this.lbBoxDescriptionText.AutoSize = true;
            this.lbBoxDescriptionText.Location = new System.Drawing.Point(3, 52);
            this.lbBoxDescriptionText.Name = "lbBoxDescriptionText";
            this.lbBoxDescriptionText.Size = new System.Drawing.Size(63, 13);
            this.lbBoxDescriptionText.TabIndex = 9;
            this.lbBoxDescriptionText.Text = "Description:";
            // 
            // tbBoxDescription
            // 
            this.tbBoxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBoxDescription.Location = new System.Drawing.Point(164, 55);
            this.tbBoxDescription.Multiline = true;
            this.tbBoxDescription.Name = "tbBoxDescription";
            this.tbBoxDescription.ReadOnly = true;
            this.tbBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbBoxDescription.Size = new System.Drawing.Size(156, 39);
            this.tbBoxDescription.TabIndex = 10;
            this.tbBoxDescription.TabStop = false;
            // 
            // btOK
            // 
            this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Image = global::amp.Properties.Resources.OK;
            this.btOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btOK.Location = new System.Drawing.Point(249, 194);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(86, 26);
            this.btOK.TabIndex = 11;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            // 
            // FormAbout
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btOK;
            this.ClientSize = new System.Drawing.Size(347, 232);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.tlpProductInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAbout";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.tlpProductInfo.ResumeLayout(false);
            this.tlpProductInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.TableLayoutPanel tlpProductInfo;
        private System.Windows.Forms.Label lbProductNameText;
        private System.Windows.Forms.Label lbProductName;
        private System.Windows.Forms.Label lbVersionText;
        private System.Windows.Forms.Label lbVersion;
        private System.Windows.Forms.Label lbCopyrightText;
        private System.Windows.Forms.Label lbCopyright;
        private System.Windows.Forms.Label lbCompanyNameText;
        private System.Windows.Forms.Label lbCompanyName;
        private System.Windows.Forms.Label lbBoxDescriptionText;
        private System.Windows.Forms.TextBox tbBoxDescription;
        private System.Windows.Forms.Button btOK;

    }
}
