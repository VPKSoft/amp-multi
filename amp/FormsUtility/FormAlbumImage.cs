#region license
/*
This file is part of amp#, which is licensed
under the terms of the Microsoft Public License (Ms-Pl) license.
See https://opensource.org/licenses/MS-PL for details.

Copyright (c) VPKSoft 2018
*/
#endregion

using System;
using System.Drawing;
using System.IO;
using amp.Properties;
using TagLib;
using VPKSoft.LangLib;

namespace amp.FormsUtility
{
    public partial class FormAlbumImage : DBLangEngineWinforms
    {
        public FormAlbumImage()
        {
            InitializeComponent();
        }

        public static FormAlbumImage ThisInstance;

        private static MainWindow activateWindow;

        private static bool firstShow = true;

        public static void Reposition(MainWindow mw, int top)
        {
            if (ThisInstance != null)
            {
                activateWindow = mw;
                ThisInstance.Left = mw.Left + mw.Width;
                ThisInstance.Top = top;
            }
        }

        public static void Show(MainWindow mw, MusicFile mf, int top)
        {
            if (ThisInstance == null)
            {
                ThisInstance = new FormAlbumImage {Owner = mw};
            }
            mf.LoadPic();
            try
            {
                if (mf.Pictures != null && mf.Pictures.Length > 0)
                {
                    IPicture pic = mf.Pictures[0];
                    MemoryStream ms = new MemoryStream(pic.Data.Data) {Position = 0};
                    Image im = Image.FromStream(ms);
                    ThisInstance.pbAlbum.Image = im;
                }
                else
                {
                    ThisInstance.pbAlbum.Image = Resources.music_note;
                }
            }
            catch
            {
                ThisInstance.pbAlbum.Image = Resources.music_note;
            }
            ThisInstance.Visible = ThisInstance.pbAlbum.Image != null;
            if (firstShow && ThisInstance.Visible)
            {
                mw.BringToFront();
            }

            Reposition(mw, top);
        }

        private void FormAlbumImage_Activated(object sender, EventArgs e)
        {
            activateWindow?.Activate();
        }
    }
}
