#region License
/*
MIT License

Copyright(c) 2020 Petteri Kautonen

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
using VPKSoft.ErrorLogger;

// ReSharper disable IdentifierTypo

// ReSharper disable once CheckNamespace
namespace amp
{
    // For the remote control
    public partial class FormMain
    {
        /// <summary>
        /// Gets a value whether the playback is paused.
        /// </summary>
        /// <returns><c>true</c> if the playback state is paused; otherwise <c>false</c>.</returns>
        public bool Paused()
        {
            return waveOutDevice != null && waveOutDevice.PlaybackState == PlaybackState.Paused;
        }

        /// <summary>
        /// Pauses the playback.
        /// </summary>
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

        /// <summary>
        /// Plays a song with a given database ID number or the next song if the given id is -1.
        /// </summary>
        /// <param name="id">The database ID number for the song to play.</param>
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

        /// <summary>
        /// Displays the playback state in the main window.
        /// </summary>
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

        /// <summary>
        /// Gets a value whether the playback is stopped.
        /// </summary>
        /// <returns><c>true</c> if the playback is stopped; otherwise <c>false</c>.</returns>
        public bool Stopped()
        {
            return waveOutDevice != null && waveOutDevice.PlaybackState == PlaybackState.Stopped;
        }

        /// <summary>
        /// Gets a value whether the playback state is playing.
        /// </summary>
        /// <returns><c>true</c> if the playback is playing; otherwise <c>false</c>.</returns>
        public bool Playing()
        {
            return waveOutDevice != null && waveOutDevice.PlaybackState == PlaybackState.Playing;
        }

        /// <summary>
        /// Sets the playback position in seconds.
        /// </summary>
        /// <param name="seconds">The playback position in seconds.</param>
        public void SetPositionSeconds(double seconds)
        {
            if (mainOutputStream != null)
            {
                tmSeek.Stop();
                try
                {
                    mainOutputStream.CurrentTime = new TimeSpan(0, 0, (int)seconds);
                }
                catch (Exception ex)
                {
                    // log the exception..
                    ExceptionLogger.LogError(ex);
                }
                tmSeek.Start();
            }
        }

        /// <summary>
        /// Queue a song.
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
                    mf.Queue(ref PlayList, StackQueueEnabled);
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
        /// Queue a song.
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
                    mf.Queue(ref PlayList, StackQueueEnabled);
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
        /// Refreshes the main window after loading a queue to it.
        /// </summary>
        /// <param name="queueIndex">The queue index (Database ID number) for the queue to load.</param>
        /// <param name="append">If set to <c>true</c> the queue is appended to the previous queue.</param>
        public void RefreshLoadQueueStats(int queueIndex, bool append)
        {
            Database.LoadQueue(ref PlayList, Connection, queueIndex, append);
            lbMusic.RefreshItems();
            GetQueueCount();
        }

        // a field for the AlbumChanged property..
        private bool albumChanged;

        /// <summary>
        /// Gets a value whether the album has changed.
        /// Note: This is an auto-resetting property; after querying the value the property returns false.
        /// </summary>
        public bool AlbumChanged
        {
            get
            {
                if (AlbumLoading)
                {
                    return false;
                }
                bool tmp = albumChanged;
                albumChanged = false;
                return tmp;
            }
        }

        /// <summary>
        /// Gets a value whether an album is currently loading.
        /// </summary>
        public bool AlbumLoading { get; private set; }

        /// <summary>
        /// Gets a value whether songs in the album have changed.
        /// </summary>
        public bool SongsChanged
        {
            get
            {
                return PlayList.Count(f => f.SongChanged) > 0;
            }
        }

        /// <summary>
        /// Gets or sets a value whether the randomization is enabled in the main form.
        /// </summary>
        public bool Randomizing
        {
            get => tbRand.Checked;

            set => tbRand.Checked = value;
        }

        /// <summary>
        /// Gets or sets a value whether shuffling is enabled in the main form.
        /// </summary>
        public bool Shuffle
        {
            get => tbShuffle.Checked;

            set => tbShuffle.Checked = value;
        }

        /// <summary>
        /// Removes a song from the current album.
        /// </summary>
        /// <param name="asf">A <see cref="AlbumSongWCF"/> class instance to remove from the album.</param>
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

            Database.RemoveSongFromAlbum(CurrentAlbum, removeList, Connection);
            humanActivity.Enabled = true;
            lbMusic.ResumeLayout();
        }

        /// <summary>
        /// Sets the rating for the current song.
        /// </summary>
        /// <param name="rating">The new rating value.</param>
        /// <returns><c>true</c> if there is a song to set a rating for; otherwise <c>false</c>.</returns>
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

        /// <summary>
        /// Sets the volume of the currently playing song.
        /// </summary>
        /// <param name="volume">The new volume value.</param>
        /// <returns><c>true</c> if there is a song to set a volume for; otherwise <c>false</c>.</returns>
        public bool SetVolume(float volume)
        {
            if (volumeStream != null && volume >= 0F && volume <= 2.0F)
            {
                volumeStream.Volume = volume;

                if (MFile != null)
                {
                    MFile.Volume = volumeStream.Volume;
                    Database.SaveVolume(MFile, Connection);
                    return true;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// Sets a volume for multiple songs.
        /// </summary>
        /// <param name="songIdList">A list of song database ID numbers to set the volume for.</param>
        /// <param name="volume">The new volume value.</param>
        /// <returns><c>true</c> if the volume was set successfully; otherwise <c>false</c>.</returns>
        public bool SetVolume(List<int> songIdList, float volume)
        {
            if (volume >= 0F && volume <= 2.0F && songIdList != null && songIdList.Count > 0)
            {
                foreach (var item in PlayList)
                {
                    if (songIdList.Exists(f => f == item.ID))
                    {
                        item.Volume = volume;
                        Database.SaveVolume(item, Connection);
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

        /// <summary>
        /// Sets a rating for multiple songs.
        /// </summary>
        /// <param name="songIdList">A list of song database ID numbers to set the rating for.</param>
        /// <param name="rating"></param>
        /// <returns><c>true</c> if the rating was set successfully; otherwise <c>false</c>.</returns>
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

        /// <summary>
        /// Gets the index of the music file in the main form playlist box.
        /// </summary>
        /// <param name="id">A song database ID number to get the index for.</param>
        /// <returns>An index if the operation was successful; otherwise -1.</returns>
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

        /// <summary>
        /// Gets the albums currently in the software database.
        /// </summary>
        /// <returns>A list of <see cref="AlbumWCF"/> class instances containing the album data.</returns>
        public List<AlbumWCF> GetAlbums()
        {
            List<Album> albums = Database.GetAlbums(Connection);
            List<AlbumWCF> albumsWcf = new List<AlbumWCF>();
            foreach (Album album in albums)
            {
                albumsWcf.Add(new AlbumWCF { Name = album.AlbumName });
            }
            return albumsWcf;
        }

        /// <summary>
        /// Selects an album with a given name.
        /// </summary>
        /// <param name="name">The name of the album to select.</param>
        /// <returns><c>true</c> if the album was selected successfully; otherwise <c>false</c>.</returns>
        public bool SelectAlbum(string name)
        {
            List<Album> albums = Database.GetAlbums(Connection);
            foreach (Album album in albums)
            {
                foreach (ToolStripMenuItem item in mnuAlbum.DropDownItems)
                {
                    if ((album.AlbumName != CurrentAlbum && album.AlbumName == name) 
                        && (int)(item).Tag == album.Id)
                    {
                        DisableChecks();
                        item.Checked = true;
                        Database.SaveQueue(PlayList, Connection, CurrentAlbum);
                        GetAlbum(name);
                        return true;
                    }

                }
            }
            return false;
        }

        /// <summary>
        /// Gets a value whether it is possible to jump to the previously played song.
        /// </summary>
        public bool CanGoPrevious => playedSongs.Count >= 2;
    }
}