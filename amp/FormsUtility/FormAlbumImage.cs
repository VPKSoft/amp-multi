#region License
/*
MIT License

Copyright(c) 2019 Petteri Kautonen

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion

using System;
using System.Drawing;
using System.IO;
using amp.Properties;
using amp.UtilityClasses;
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
