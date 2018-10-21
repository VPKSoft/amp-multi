#region license
/*
This file is part of amp#, which is licensed
under the terms of the Microsoft Public License (Ms-Pl) license.
See https://opensource.org/licenses/MS-PL for details.

Copyright (c) VPKSoft 2018
*/
#endregion

using System.Windows.Forms;
using System.Collections.Generic;
using System.ServiceModel;
using VPKSoft.LangLib;

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

        private System.Drawing.Bitmap GetNextImg(int goNum)
        {
            List<System.Drawing.Bitmap> albumImages = new List<System.Drawing.Bitmap>();
            albumImages.Add(Properties.Resources.album_blue);
            albumImages.Add(Properties.Resources.album_byellow);
            albumImages.Add(Properties.Resources.album_green);
            albumImages.Add(Properties.Resources.album_red);
            albumImages.Add(Properties.Resources.album_teal);
            return albumImages[goNum % 5];
        }

        private void ListAlbums(int checkAlbum = -1)
        {
            mnuAlbum.DropDownItems.Clear();
            List<Album> albums = Database.GetAlbums(conn);

            int aNum = 0;
            foreach (Album album in albums)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(album.AlbumName) { Image = GetNextImg(aNum++) };
                item.Tag = album.ID;
                if (album.ID == checkAlbum)
                {
                    item.Checked = true;
                }
                item.Click += selectAlbumClick;
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
                    if (QueueShowing) // refresh the queue list if it's showing..
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
                        iSongIndex = random.Next(0, PlayList.Count);
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
                    lbMusic.Invoke(new VoidDelegate(RefreshListboxFromThread));
                }
                else
                {
                    RefreshListboxFromThread();
                }
                if (ssStatus.InvokeRequired)
                {
                    ssStatus.Invoke(new VoidDelegate(GetQueueCount));
                }
                else
                {
                    GetQueueCount();
                }

                if (!QueueShowing)
                {
                    ShowQueue();
                }
            }
        }
    }
}
