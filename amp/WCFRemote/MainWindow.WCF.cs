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
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using amp.Properties;
using amp.SQLiteDatabase;
using amp.UtilityClasses;
using amp.WCFRemote;
using NAudio.Wave;
// ReSharper disable IdentifierTypo

// ReSharper disable once CheckNamespace
namespace amp
{
    // For the remote control
    public partial class MainWindow
    {
        public bool Paused()
        {
            if (waveOutDevice == null)
            {
                return false;
            }
            return waveOutDevice.PlaybackState == PlaybackState.Paused;
        }

        public void Pause()
        {
            if (waveOutDevice == null)
            {
                VisualizePlaybackState();
                return;
            }

            if (waveOutDevice.PlaybackState == PlaybackState.Playing)
            {
                waveOutDevice.Pause();
            }
            VisualizePlaybackState();
        }

        public void Play(int id = -1)
        {
            if (id != -1)
            {
                foreach (var item in lbMusic.Items)
                {
                    if (((MusicFile) item).ID == id)
                    {
                        UpdateNPlayed(MFile, Skipped);
                        MFile = item as MusicFile;
                        if (MFile != null)
                        {
                            latestSongIndex = MFile.VisualIndex;
                            UpdateNPlayed(MFile, false);
                        }

                        newSong = true;
                    }
                }
            }
            else if (waveOutDevice == null)
            {
                GetNextSong();
            }
            else if (waveOutDevice.PlaybackState != PlaybackState.Playing)
            {
                waveOutDevice.Play();
            }
            VisualizePlaybackState();
        }

        internal void VisualizePlaybackState()
        {
            if (waveOutDevice != null)
            {
                if (waveOutDevice.PlaybackState == PlaybackState.Paused)
                {
                    tbPlayNext.Image = Resources.amp_play;
                    tbPlayNext.ToolTipText = DBLangEngine.GetMessage("msgPlay", "Play|Play a song or resume paused");
                }
                else if (waveOutDevice.PlaybackState == PlaybackState.Playing)
                {
                    tbPlayNext.Image = Resources.amp_pause;
                    tbPlayNext.ToolTipText = DBLangEngine.GetMessage("msgPause", "Pause|Pause playback");
                }
            }
            else
            {
                tbPlayNext.Image = Resources.amp_play;
                tbPlayNext.ToolTipText = DBLangEngine.GetMessage("msgPlay", "Play|Play a song or resume paused");
            }
        }

        public bool Stopped()
        {
            if (waveOutDevice == null)
            {
                return true;
            }
            return waveOutDevice.PlaybackState == PlaybackState.Stopped;
        }

        public bool Playing()
        {
            if (waveOutDevice == null)
            {
                return false;
            }
            return waveOutDevice.PlaybackState == PlaybackState.Playing;
        }

        public void SetPositionSeconds(double seconds)
        {
            if (mainOutputStream != null)
            {
                tmSeek.Stop();
                try
                {
                    mainOutputStream.CurrentTime = new TimeSpan(0, 0, (int)seconds);
                }
                catch
                {
                    // do nothing
                }
                tmSeek.Start();
            }
        }

        /// <summary>
        /// For the remote interface..
        /// </summary>
        /// <param name="insert">The remote GUI is in insert into the queue mode.</param>
        /// <param name="queueList">A list of songs which are to be queued from the remote GUI.</param>
        public void Queue(bool insert, List<AlbumSongWCF> queueList)
        {
            List<MusicFile> qFiles = new List<MusicFile>();
            foreach (AlbumSongWCF mfWcf in queueList)
            {
                foreach (MusicFile mf in lbMusic.Items)
                {
                    if (mf.ID == mfWcf.ID)
                    {
                        qFiles.Add(mf);
                    }
                }
            }

            foreach (MusicFile mf in qFiles)
            {
                if (insert)
                {
                    if (playing)
                    {
                        mf.QueueInsert(ref PlayList, false, PlayList.IndexOf(MFile));
                    }
                    else
                    {
                        mf.QueueInsert(ref PlayList, false);
                    }
                }
                else
                {
                    mf.Queue(ref PlayList);
                }
            }

            if (Filtered == FilterType.QueueFiltered) // refresh the queue list if it's showing..
            {
                ShowQueue();
            }

            lbMusic.RefreshItems();
            GetQueueCount();
        }



        /// <summary>
        /// For the remote interface..
        /// </summary>
        /// <param name="insert">The remote GUI is in insert into the queue mode.</param>
        /// <param name="songIDs">A list of song IDs which are to be queued from the remote GUI.</param>
        public void Queue(bool insert, List<int> songIDs)
        {
            List<MusicFile> qFiles = new List<MusicFile>();
            foreach (int songId in songIDs)
            {
                foreach (MusicFile mf in lbMusic.Items)
                {
                    if (mf.ID == songId)
                    {
                        qFiles.Add(mf);
                    }
                }
            }

            foreach (MusicFile mf in qFiles)
            {
                if (insert)
                {
                    if (playing)
                    {
                        mf.QueueInsert(ref PlayList, false, PlayList.IndexOf(MFile));
                    }
                    else
                    {
                        mf.QueueInsert(ref PlayList, false);
                    }
                }
                else
                {
                    mf.Queue(ref PlayList);
                }
            }

            if (Filtered == FilterType.QueueFiltered) // refresh the queue list if it's showing..
            {
                ShowQueue();
            }

            lbMusic.RefreshItems();
            GetQueueCount();
        }

        public void RefreshLoadQueueStats(int queueIndex, bool append)
        {
            Database.LoadQueue(ref PlayList, Conn, queueIndex, append);
            lbMusic.RefreshItems();
            GetQueueCount();
        }

        private bool albumChanged;

        public bool AlbumChanged
        {
            get
            {
                if (AlbumLoading)
                {
                    return false;
                }
                bool bTmp = albumChanged;
                albumChanged = false;
                return bTmp;
            }
        }

        public bool AlbumLoading { get; private set; }

        public bool SongsChanged
        {
            get
            {
                return PlayList.Count(f => f.SongChanged) > 0;
            }
        }

        public bool Randomizing
        {
            get => tbRand.Checked;

            set => tbRand.Checked = value;
        }

        public bool Suffle
        {
            get => tbShuffle.Checked;

            set => tbShuffle.Checked = value;
        }

        public void RemoveSongFromAlbum(AlbumSongWCF asf)
        {
            lbMusic.SuspendLayout();
            humanActivity.Enabled = false;
            List<MusicFile> removeList = new List<MusicFile>();

            for (int i = lbMusic.Items.Count - 1; i >= 0; i--)
            {
                if (((MusicFile) lbMusic.Items[i]).ID == asf.ID)
                {
                    lbMusic.Items.RemoveAt(i);
                    break;
                }
            }

            MusicFile mf = PlayList.Find(f => f.ID == asf.ID);

            if (mf != null)
            {
                removeList.Add(mf);
                MusicFile.RemoveById(ref PlayList, mf.ID);
            }

            Database.RemoveSongFromAlbum(CurrentAlbum, removeList, Conn);
            humanActivity.Enabled = true;
            lbMusic.ResumeLayout();
        }

        public bool SetRating(int rating)
        {
            if (MFile != null && rating >= 0 && rating <= 1000)
            {
                MFile.Rating = rating;
                MFile.RatingChanged = true;
                SaveRating(MFile);
                return true;
            }
            return false;
        }

        public bool SetVolume(float volume)
        {
            if (volumeStream != null && volume >= 0F && volume <= 2.0F)
            {
                volumeStream.Volume = volume;

                if (MFile != null)
                {
                    MFile.Volume = volumeStream.Volume;
                    Database.SaveVolume(MFile, Conn);
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool SetVolume(List<int> songIdList, float volume)
        {
            if (volume >= 0F && volume <= 2.0F && songIdList != null && songIdList.Count > 0)
            {
                foreach (var item in PlayList)
                {
                    if (songIdList.Exists(f => f == item.ID))
                    {
                        item.Volume = volume;
                        Database.SaveVolume(item, Conn);
                        int lbIdx = GetListBoxIndexById(item.ID);
                        if (lbIdx >= 0)
                        {
                            lbMusic.Items[lbIdx] = item;
                        }
                    }
                }

                return true;
            }
            return false;
        }

        public bool SetRating(List<int> songIdList, int rating)
        {
            if (rating >= 0 && rating <= 1000 && songIdList != null && songIdList.Count > 0)
            {
                foreach (var item in PlayList)
                {
                    if (songIdList.Exists(f => f == item.ID))
                    {
                        item.Rating = rating;
                        item.RatingChanged = true;
                        SaveRating(item);
                        int lbIdx = GetListBoxIndexById(item.ID);
                        if (lbIdx >= 0)
                        {
                            lbMusic.Items[lbIdx] = item;
                        }
                    }
                }

                return true;
            }
            return false;
        }

        private int GetListBoxIndexById(int id)
        {
            for (int i = 0; i < lbMusic.Items.Count; i++)
            {
                if (((MusicFile)lbMusic.Items[i]).ID == id)
                {
                    return i;
                }
            }
            return -1;
        }

        public List<AlbumWCF> GetAlbums()
        {
            List<Album> albums = Database.GetAlbums(Conn);
            List<AlbumWCF> albumsWcf = new List<AlbumWCF>();
            foreach (Album album in albums)
            {
                albumsWcf.Add(new AlbumWCF { Name = album.AlbumName });
            }
            return albumsWcf;
        }

        public bool SelectAlbum(string name)
        {
            List<Album> albums = Database.GetAlbums(Conn);
            foreach (Album album in albums)
            {
                foreach (ToolStripMenuItem item in mnuAlbum.DropDownItems)
                {
                    if ((album.AlbumName != CurrentAlbum && album.AlbumName == name) 
                        && (int)(item).Tag == album.Id)
                    {
                        DisableChecks();
                        item.Checked = true;
                        Database.SaveQueue(PlayList, Conn, CurrentAlbum);
                        GetAlbum(name);
                        return true;
                    }

                }
            }
            return false;
        }

        public bool CanGoPrevious => playedSongs.Count >= 2;
    }
}