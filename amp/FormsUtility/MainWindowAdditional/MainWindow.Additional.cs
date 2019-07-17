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

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using amp.Properties;
using amp.SQLiteDatabase;
using amp.UtilityClasses;
using VPKSoft.LangLib;
using Settings = amp.UtilityClasses.Settings.Settings;

// ReSharper disable once CheckNamespace
namespace amp
{
    public partial class MainWindow : DBLangEngineWinforms
    {
        private void DisableChecks()
        {
            for (int i = 0; i < mnuAlbum.DropDownItems.Count; i++)
            {
                (mnuAlbum.DropDownItems[i] as ToolStripMenuItem).Checked = false;
            }
        }

        private Bitmap GetNextImg(int goNum)
        {
            List<Bitmap> albumImages = new List<Bitmap>
            {
                Resources.album_blue,
                Resources.album_byellow,
                Resources.album_green,
                Resources.album_red,
                Resources.album_teal
            };
            return albumImages[goNum % 5];
        }

        private void ListAlbums(int checkAlbum = -1)
        {
            mnuAlbum.DropDownItems.Clear();
            List<Album> albums = Database.GetAlbums(Conn);

            int aNum = 0;
            foreach (Album album in albums)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(album.AlbumName) { Image = GetNextImg(aNum++) };
                item.Tag = album.ID;
                if (album.ID == checkAlbum)
                {
                    item.Checked = true;
                }
                item.Click += SelectAlbumClick;
                mnuAlbum.DropDownItems.Add(item);
            }
        }

        private void GetLyric()
        {
            /*
            LyricWiki wiki = new LyricWiki();
            LyricsResult result;
            string artist = artistTextBox.Text;
            string song = SongTextBox.Text;
            if (wiki.checkSongExists(artist, song))
            {
                result = wiki.getSong(artist, song);
                Encoding iso8859 = Encoding.GetEncoding("ISO-8859-1");
                LyricsRichTextBox.Text = Encoding.UTF8.GetString(iso8859.GetBytes(result.lyrics));
            }
            else
            {
                StatusLabel.Text = "Lyrics not found in database";
            }
            */
        }

        public void GetNextSong(bool fromEvent = false)
        {
            if (addFiles)
            {
                pendNextSong = true;
            }

            if (pendNextSong)
            {
                return;
            }

            if (PlayList.Count > 0)
            {
                int iQueue = int.MaxValue;
                int iSongIndex = -1;
                for (int i = 0; i < PlayList.Count; i++)
                {
                    if (PlayList[i].QueueIndex >= 1)
                    {
                        if (iQueue > PlayList[i].QueueIndex)
                        {
                            iQueue = PlayList[i].QueueIndex;
                            iSongIndex = i;
                        }
                    }
                }
                if (iSongIndex != -1)
                {
                    PlayList[iSongIndex].Queue(ref PlayList);
                    if (Filtered == amp.MainWindow.FilterType.QueueFiltered) // refresh the queue list if it's showing..
                    {
                        ShowQueue();
                    }

                    latestSongIndex = iSongIndex;
                    PlaySong(iSongIndex, false);
                }
                if (iSongIndex == -1)
                {
                    if (tbRand.Checked)
                    {
                        if (Settings.BiasedRandom)
                        {
                            iSongIndex = MusicFile.RandomWeighted(PlayList);
                        }
                        else
                        {
                            iSongIndex = Random.Next(0, PlayList.Count);
                        }
                        latestSongIndex = iSongIndex;
                        PlaySong(iSongIndex, true);
                    }
                }

                if (iSongIndex == -1)
                {
                    if (!fromEvent || tbShuffle.Checked)
                    {
                        latestSongIndex++;
                        if (latestSongIndex >= PlayList.Count)
                        {
                            latestSongIndex = 0;
                        }
                    }
                    PlaySong(latestSongIndex, false);
                }
                if (lbMusic.InvokeRequired)
                {
                    lbMusic.Invoke(new amp.MainWindow.VoidDelegate(RefreshListboxFromThread));
                }
                else
                {
                    RefreshListboxFromThread();
                }
                if (ssStatus.InvokeRequired)
                {
                    ssStatus.Invoke(new amp.MainWindow.VoidDelegate(GetQueueCount));
                }
                else
                {
                    GetQueueCount();
                }

                if (Filtered == amp.MainWindow.FilterType.QueueFiltered)
                {
                    ShowQueue();
                }
            }
        }
    }
}
