#region license
/*
This file is part of amp#, which is licensed
under the terms of the Microsoft Public License (Ms-Pl) license.
See https://opensource.org/licenses/MS-PL for details.

Copyright (c) VPKSoft 2018
*/
#endregion

using System;
using System.IO;
using VPKSoft.LangLib;

namespace amp
{
    public partial class FormAlbumImage : DBLangEngineWinforms
    {
        public FormAlbumImage()
        {
            InitializeComponent();
        }

        public static FormAlbumImage thisInstance = null;

        private static MainWindow activateWindow = null;

        private static bool firstShow = true;

        public static void Reposition(MainWindow mw, int top)
        {
            if (thisInstance != null)
            {
                activateWindow = mw;
                thisInstance.Left = mw.Left + mw.Width;
                thisInstance.Top = top;
            }
        }

        public static void Show(MainWindow mw, MusicFile mf, int top)
        {
            if (thisInstance == null)
            {
                thisInstance = new FormAlbumImage();
                thisInstance.Owner = mw;
            }
            mf.LoadPic();
            try
            {
                if (mf.Pictures != null && mf.Pictures.Length > 0)
                {
                    TagLib.IPicture pic = mf.Pictures[0];
                    MemoryStream ms = new MemoryStream(pic.Data.Data);
                    ms.Position = 0;
                    System.Drawing.Image im = System.Drawing.Image.FromStream(ms);
                    thisInstance.pbAlbum.Image = im;
                }
                else
                {
                    thisInstance.pbAlbum.Image = Properties.Resources.music_note;
                }
            }
            catch
            {
                thisInstance.pbAlbum.Image = Properties.Resources.music_note;
            }
            thisInstance.Visible = thisInstance.pbAlbum.Image != null;
            if (firstShow && thisInstance.Visible)
            {
                mw.BringToFront();
            }

            Reposition(mw, top);
        }

        private void FormAlbumImage_Activated(object sender, EventArgs e)
        {
            if (activateWindow != null)
            {
                activateWindow.Activate();
            }
        }
    }
}
