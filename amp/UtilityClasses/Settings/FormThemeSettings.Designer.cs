using System.ComponentModel;
using System.Windows.Forms;

namespace amp.UtilityClasses.Settings
{
    partial class FormThemeSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormThemeSettings));
            this.colorEditor = new Cyotek.Windows.Forms.ColorEditor();
            this.colorWheel = new Cyotek.Windows.Forms.ColorWheel();
            this.listThemeColors = new System.Windows.Forms.ListBox();
            this.pnColorSelection = new System.Windows.Forms.Panel();
            this.pnColorDisplay = new System.Windows.Forms.Panel();
            this.cdMain = new System.Windows.Forms.ColorDialog();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tabThemeColors = new System.Windows.Forms.TabPage();
            this.tabThemeImages = new System.Windows.Forms.TabPage();
            this.pnImage = new System.Windows.Forms.Panel();
            this.listThemeImages = new System.Windows.Forms.ListBox();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.fdOpenImage = new Ookii.Dialogs.WinForms.VistaOpenFileDialog();
            this.sdXmlTheme = new Ookii.Dialogs.WinForms.VistaSaveFileDialog();
            this.fdOpenTheme = new Ookii.Dialogs.WinForms.VistaOpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSetDefaultLightTheme = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSetDefalutDarkTheme = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveAsDefaultTheme = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuLoadTheme = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveThemeAs = new System.Windows.Forms.ToolStripMenuItem();
            this.pnColorSelection.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tabThemeColors.SuspendLayout();
            this.tabThemeImages.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // colorEditor
            // 
            this.colorEditor.Location = new System.Drawing.Point(3, 3);
            this.colorEditor.Name = "colorEditor";
            this.colorEditor.Size = new System.Drawing.Size(283, 231);
            this.colorEditor.TabIndex = 0;
            this.colorEditor.ColorChanged += new System.EventHandler(this.colorEditor_ColorChanged);
            // 
            // colorWheel
            // 
            this.colorWheel.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.colorWheel.Location = new System.Drawing.Point(3, 240);
            this.colorWheel.Name = "colorWheel";
            this.colorWheel.Size = new System.Drawing.Size(173, 173);
            this.colorWheel.TabIndex = 1;
            this.colorWheel.ColorChanged += new System.EventHandler(this.colorWheel_ColorChanged);
            // 
            // listThemeColors
            // 
            this.listThemeColors.FormattingEnabled = true;
            this.listThemeColors.IntegralHeight = false;
            this.listThemeColors.Location = new System.Drawing.Point(6, 6);
            this.listThemeColors.Name = "listThemeColors";
            this.listThemeColors.Size = new System.Drawing.Size(235, 416);
            this.listThemeColors.TabIndex = 2;
            this.listThemeColors.SelectedValueChanged += new System.EventHandler(this.listThemeColors_SelectedValueChanged);
            // 
            // pnColorSelection
            // 
            this.pnColorSelection.Controls.Add(this.pnColorDisplay);
            this.pnColorSelection.Controls.Add(this.colorEditor);
            this.pnColorSelection.Controls.Add(this.colorWheel);
            this.pnColorSelection.Location = new System.Drawing.Point(247, 3);
            this.pnColorSelection.Name = "pnColorSelection";
            this.pnColorSelection.Size = new System.Drawing.Size(294, 419);
            this.pnColorSelection.TabIndex = 4;
            // 
            // pnColorDisplay
            // 
            this.pnColorDisplay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnColorDisplay.Location = new System.Drawing.Point(182, 240);
            this.pnColorDisplay.Name = "pnColorDisplay";
            this.pnColorDisplay.Size = new System.Drawing.Size(104, 173);
            this.pnColorDisplay.TabIndex = 2;
            this.pnColorDisplay.Click += new System.EventHandler(this.pnColorDisplay_Click);
            // 
            // cdMain
            // 
            this.cdMain.AnyColor = true;
            this.cdMain.FullOpen = true;
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tabThemeColors);
            this.tcMain.Controls.Add(this.tabThemeImages);
            this.tcMain.Location = new System.Drawing.Point(12, 37);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(555, 454);
            this.tcMain.TabIndex = 5;
            // 
            // tabThemeColors
            // 
            this.tabThemeColors.Controls.Add(this.listThemeColors);
            this.tabThemeColors.Controls.Add(this.pnColorSelection);
            this.tabThemeColors.Location = new System.Drawing.Point(4, 22);
            this.tabThemeColors.Name = "tabThemeColors";
            this.tabThemeColors.Padding = new System.Windows.Forms.Padding(3);
            this.tabThemeColors.Size = new System.Drawing.Size(547, 428);
            this.tabThemeColors.TabIndex = 0;
            this.tabThemeColors.Text = "Theme colors";
            this.tabThemeColors.UseVisualStyleBackColor = true;
            // 
            // tabThemeImages
            // 
            this.tabThemeImages.Controls.Add(this.pnImage);
            this.tabThemeImages.Controls.Add(this.listThemeImages);
            this.tabThemeImages.Location = new System.Drawing.Point(4, 22);
            this.tabThemeImages.Name = "tabThemeImages";
            this.tabThemeImages.Padding = new System.Windows.Forms.Padding(3);
            this.tabThemeImages.Size = new System.Drawing.Size(547, 428);
            this.tabThemeImages.TabIndex = 1;
            this.tabThemeImages.Text = "Theme images";
            this.tabThemeImages.UseVisualStyleBackColor = true;
            // 
            // pnImage
            // 
            this.pnImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnImage.Location = new System.Drawing.Point(247, 6);
            this.pnImage.Name = "pnImage";
            this.pnImage.Size = new System.Drawing.Size(294, 294);
            this.pnImage.TabIndex = 4;
            this.pnImage.Click += new System.EventHandler(this.pnImage_Click);
            // 
            // listThemeImages
            // 
            this.listThemeImages.FormattingEnabled = true;
            this.listThemeImages.IntegralHeight = false;
            this.listThemeImages.Location = new System.Drawing.Point(6, 6);
            this.listThemeImages.Name = "listThemeImages";
            this.listThemeImages.Size = new System.Drawing.Size(235, 416);
            this.listThemeImages.TabIndex = 3;
            this.listThemeImages.SelectedValueChanged += new System.EventHandler(this.listThemeImages_SelectedValueChanged);
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(12, 497);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 10;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.Location = new System.Drawing.Point(488, 497);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 9;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // fdOpenImage
            // 
            this.fdOpenImage.Filter = "Image files|*.jpg;*.png;*.gif;*,tiff;*.bmp";
            // 
            // sdXmlTheme
            // 
            this.sdXmlTheme.DefaultExt = "*.xml";
            this.sdXmlTheme.Filter = "XML Files|*.xml";
            // 
            // fdOpenTheme
            // 
            this.fdOpenTheme.DefaultExt = "*.xml";
            this.fdOpenTheme.Filter = "XML Files|*.xml";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(579, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSetDefaultLightTheme,
            this.mnuSetDefalutDarkTheme,
            this.toolStripMenuItem1,
            this.mnuSaveAsDefaultTheme,
            this.toolStripMenuItem2,
            this.mnuLoadTheme,
            this.mnuSaveThemeAs});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "File";
            // 
            // mnuSetDefaultLightTheme
            // 
            this.mnuSetDefaultLightTheme.Name = "mnuSetDefaultLightTheme";
            this.mnuSetDefaultLightTheme.Size = new System.Drawing.Size(194, 22);
            this.mnuSetDefaultLightTheme.Text = "Set default light theme";
            this.mnuSetDefaultLightTheme.Click += new System.EventHandler(this.mnuLightDarkTheme_Click);
            // 
            // mnuSetDefalutDarkTheme
            // 
            this.mnuSetDefalutDarkTheme.Name = "mnuSetDefalutDarkTheme";
            this.mnuSetDefalutDarkTheme.Size = new System.Drawing.Size(194, 22);
            this.mnuSetDefalutDarkTheme.Text = "Set defalut dark theme";
            this.mnuSetDefalutDarkTheme.Click += new System.EventHandler(this.mnuLightDarkTheme_Click);
            // 
            // mnuSaveAsDefaultTheme
            // 
            this.mnuSaveAsDefaultTheme.Image = global::amp.Properties.Resources.Save;
            this.mnuSaveAsDefaultTheme.Name = "mnuSaveAsDefaultTheme";
            this.mnuSaveAsDefaultTheme.Size = new System.Drawing.Size(194, 22);
            this.mnuSaveAsDefaultTheme.Text = "Save as default theme";
            this.mnuSaveAsDefaultTheme.Click += new System.EventHandler(this.mnuSaveAsDefaultTheme_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(191, 6);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(191, 6);
            // 
            // mnuLoadTheme
            // 
            this.mnuLoadTheme.Image = global::amp.Properties.Resources.open;
            this.mnuLoadTheme.Name = "mnuLoadTheme";
            this.mnuLoadTheme.Size = new System.Drawing.Size(194, 22);
            this.mnuLoadTheme.Text = "Load theme";
            this.mnuLoadTheme.Click += new System.EventHandler(this.mnuLoadTheme_Click);
            // 
            // mnuSaveThemeAs
            // 
            this.mnuSaveThemeAs.Image = global::amp.Properties.Resources.Save_as32;
            this.mnuSaveThemeAs.Name = "mnuSaveThemeAs";
            this.mnuSaveThemeAs.Size = new System.Drawing.Size(194, 22);
            this.mnuSaveThemeAs.Text = "Save theme as";
            this.mnuSaveThemeAs.Click += new System.EventHandler(this.mnuSaveThemeAs_Click);
            // 
            // FormThemeSettings
            // 
            this.AcceptButton = this.bOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(579, 532);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.tcMain);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormThemeSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Theme settings";
            this.Shown += new System.EventHandler(this.FormTheSettings_Shown);
            this.pnColorSelection.ResumeLayout(false);
            this.tcMain.ResumeLayout(false);
            this.tabThemeColors.ResumeLayout(false);
            this.tabThemeImages.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Cyotek.Windows.Forms.ColorEditor colorEditor;
        private Cyotek.Windows.Forms.ColorWheel colorWheel;
        private ListBox listThemeColors;
        private Panel pnColorSelection;
        private Panel pnColorDisplay;
        private ColorDialog cdMain;
        private TabControl tcMain;
        private TabPage tabThemeColors;
        private TabPage tabThemeImages;
        private Button bCancel;
        private Button bOK;
        private Panel pnImage;
        private ListBox listThemeImages;
        private Ookii.Dialogs.WinForms.VistaOpenFileDialog fdOpenImage;
        private Ookii.Dialogs.WinForms.VistaSaveFileDialog sdXmlTheme;
        private Ookii.Dialogs.WinForms.VistaOpenFileDialog fdOpenTheme;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem mnuFile;
        private ToolStripMenuItem mnuSetDefaultLightTheme;
        private ToolStripMenuItem mnuSetDefalutDarkTheme;
        private ToolStripMenuItem mnuSaveAsDefaultTheme;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem mnuLoadTheme;
        private ToolStripMenuItem mnuSaveThemeAs;
    }
}