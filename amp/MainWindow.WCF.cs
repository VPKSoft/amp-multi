#region license
/*
This file is part of amp#, which is licensed
under the terms of the Microsoft Public License (Ms-Pl) license.
See https://opensource.org/licenses/MS-PL for details.

Copyright (c) VPKSoft 2018
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;

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

        public void Play(int ID = -1)
        {
            if (ID != -1)
            {
                for (int i = 0; i < lbMusic.Items.Count; i++)
                {
                    if ((lbMusic.Items[i] as MusicFile).ID == ID)
                    {
                        Database.UpdateNPlayed(mFile, conn, Skipped);
                        mFile = lbMusic.Items[i] as MusicFile;
                        latestSongIndex = mFile.VisualIndex;
                        Database.UpdateNPlayed(mFile, conn, false);
                        newsong = true;
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
                    tbPlayNext.Image = Properties.Resources.amp_play;
                    tbPlayNext.ToolTipText = DBLangEngine.GetMessage("msgPlay", "Play|Play a song or resume paused");
                }
                else if (waveOutDevice.PlaybackState == PlaybackState.Playing)
                {
                    tbPlayNext.Image = Properties.Resources.amp_pause;
                    tbPlayNext.ToolTipText = DBLangEngine.GetMessage("msgPause", "Pause|Pause playback");
                }
            }
            else
            {
                tbPlayNext.Image = Properties.Resources.amp_play;
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
                        mf.QueueInsert(ref PlayList, false, PlayList.IndexOf(mFile));
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
            foreach (int songID in songIDs)
            {
                foreach (MusicFile mf in lbMusic.Items)
                {
                    if (mf.ID == songID)
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
                        mf.QueueInsert(ref PlayList, false, PlayList.IndexOf(mFile));
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
            Database.LoadQueue(ref PlayList, conn, queueIndex, append);
            lbMusic.RefreshItems();
            GetQueueCount();
        }

        private bool albumChanged = false;

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

        private bool albumLoading = false;

        public bool AlbumLoading
        {
            get
            {
                return albumLoading;
            }
        }

        public bool SongsChanged
        {
            get
            {
                return PlayList.Count(f => f.SongChanged) > 0;
            }
        }

        public bool Randomizing
        {
            get
            {
                return tbRand.Checked;
            }

            set
            {
                tbRand.Checked = value;
            }
        }

        public bool Suffle
        {
            get
            {
                return tbShuffle.Checked;
            }

            set
            {
                tbShuffle.Checked = value;
            }
        }

        public void RemoveSongFromAlbum(AlbumSongWCF asf)
        {
            lbMusic.SuspendLayout();
            humanActivity.Enabled = false;
            List<MusicFile> removeList = new List<MusicFile>();

            for (int i = lbMusic.Items.Count - 1; i >= 0; i--)
            {
                if ((lbMusic.Items[i] as MusicFile).ID == asf.ID)
                {
                    lbMusic.Items.RemoveAt(i);
                    break;
                }
            }

            MusicFile mf = PlayList.Find(f => f.ID == asf.ID);

            if (mf != null)
            {
                removeList.Add(mf);
                MusicFile.RemoveByID(ref PlayList, mf.ID);
            }

            Database.RemoveSongFromAlbum(CurrentAlbum, removeList, conn);
            humanActivity.Enabled = true;
            lbMusic.ResumeLayout();
        }

        public bool SetRating(int rating)
        {
            if (mFile != null && rating >= 0 && rating <= 1000)
            {
                mFile.Rating = rating;
                mFile.RatingChanged = true;
                Database.SaveRating(mFile, conn);
                return true;
            }
            return false;
        }

        public bool SetVolume(float volume)
        {
            if (volumeStream != null && volume >= 0F && volume <= 2.0F)
            {
                volumeStream.Volume = volume;

                if (mFile != null)
                {
                    mFile.Volume = volumeStream.Volume;
                    Database.SaveVolume(mFile, conn);
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool SetVolume(List<int> songIDList, float volume)
        {
            if (volume >= 0F && volume <= 2.0F && songIDList != null && songIDList.Count > 0)
            {
                for (int i = 0; i < PlayList.Count; i++)
                {                    
                    if (songIDList.Exists(f => f == PlayList[i].ID))
                    {
                        PlayList[i].Volume = volume;
                        Database.SaveVolume(PlayList[i], conn);
                        int lbIdx = GetListBoxIndexByID(PlayList[i].ID);
                        if (lbIdx >= 0)
                        {
                            lbMusic.Items[lbIdx] = PlayList[i];
                        }
                    }
                }
                return true;
            }
            return false;
        }

        public bool SetRating(List<int> songIDList, int rating)
        {
            if (rating >= 0 && rating <= 1000 && songIDList != null && songIDList.Count > 0)
            {
                for (int i = 0; i < PlayList.Count; i++)
                {
                    if (songIDList.Exists(f => f == PlayList[i].ID))
                    {
                        PlayList[i].Rating = rating;
                        PlayList[i].RatingChanged = true;
                        Database.SaveRating(PlayList[i], conn);
                        int lbIdx = GetListBoxIndexByID(PlayList[i].ID);
                        if (lbIdx >= 0)
                        {
                            lbMusic.Items[lbIdx] = PlayList[i];
                        }
                    }
                }
                return true;
            }
            return false;
        }

        private int GetListBoxIndexByID(int ID)
        {
            for (int i = 0; i < lbMusic.Items.Count; i++)
            {
                if (((MusicFile)lbMusic.Items[i]).ID == ID)
                {
                    return i;
                }
            }
            return -1;
        }

        public List<AlbumWCF> GetAlbums()
        {
            List<Album> albums = Database.GetAlbums(conn);
            List<AlbumWCF> albumsWCF = new List<AlbumWCF>();
            foreach (Album album in albums)
            {
                albumsWCF.Add(new AlbumWCF { Name = album.AlbumName });
            }
            return albumsWCF;
        }

        public bool SelectAlbum(string name)
        {
            List<Album> albums = Database.GetAlbums(conn);
            foreach (Album album in albums)
            {
                foreach (ToolStripMenuItem item in mnuAlbum.DropDownItems)
                {
                    if ((album.AlbumName != CurrentAlbum && album.AlbumName == name) 
                        && (int)(item).Tag == album.ID)
                    {
                        DisableChecks();
                        item.Checked = true;
                        Database.SaveQueue(PlayList, conn, CurrentAlbum);
                        GetAlbum(name);
                        return true;
                    }

                }
            }
            return false;
        }

        public bool CanGoPrevious
        {
            get
            {
                return playedSongs.Count >= 2;
            }
        }
    }
}