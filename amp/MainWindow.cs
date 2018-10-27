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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using NAudio;
using NAudio.Wave;
using amp;
using amp.Properties;
using TagLib.Id3v1;
using TagLib.Id3v2;
using System.Threading;
using NAudio.Flac;
using NAudio.Vorbis;
using NAudio.WindowsMediaFormat;
using System.ServiceModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Globalization;
using VPKSoft.LangLib;
using VPKSoft.KeySendList;
using VPKSoft.Utils;
using VPKSoft.About;
using VPKSoft.PosLib;

namespace amp
{
    public partial class MainWindow : DBLangEngineWinforms
    {
        // the currently playing musing file
        public volatile MusicFile mFile = null;

        // the thread that handles the playback logic (next song, randomizing, UI updates, e.g..
        private Thread thread;

        // a SQLiteConnection for the database access..
        public SQLiteConnection conn; // database connection for the SQLite database

        // the name of a currently playing album..
        public string CurrentAlbum;

        // list of entries in the current album..
        public List<MusicFile> PlayList = new List<MusicFile>();

        // list of indexes of the played songs in the PlayList..
        private List<int> playedSongs = new List<int>();

        // a flag indicating if the player thread is active..
        private volatile bool playerThreadLoaded = false;

        // a flag indicating if the play back progress (the ProgressBar and the time left text) are changing via a user generated event..
        private bool progressUpdating = false;

        // some "useful" settings to enable "quiet hours", latency and the remote control WCF service 
        public static bool QuietHours = false; // this is gotten from the settings
        public static string QuietHoursFrom = "08:00";
        public static string QuietHoursTo = "23:00";
        public static bool QuietHoursPause = false;
        public static double QuietHoursVolPercentage = 0.7;
        public static int LatencyMS = 300;
        public static bool RemoteControlApiWCF = false;
        public static string RemoteControlApiWCFAddress = "http://localhost:11316/ampRemote"; // END: this is gotten from the settings


        AmpRemote remote = new AmpRemote();

        public MainWindow() 
        {
            // Add this form to be positioned..
            PositionForms.Add(this, PositionCore.SizeChangeMode.MoveTopLeft);

            InitializeComponent();

            DBLangEngine.DBName = "lang.sqlite";

            if (VPKSoft.LangLib.Utils.ShouldLocalize() != null)
            {
                DBLangEngine.InitalizeLanguage("amp.Messages", VPKSoft.LangLib.Utils.ShouldLocalize(), false);
                return; // After localization don't do anything more.
            }

            DBLangEngine.InitalizeLanguage("amp.Messages");

            try // as this can be translated to a invalid format :-)
            {
                sdM3U.Filter = DBLangEngine.GetMessage("msgFileExt_m3u", "M3U playlist files (*.m3u;*.m3u8)|*.m3u;*.m3u8|as in the combo box to select file type from a dialog");
                odM3U.Filter = DBLangEngine.GetMessage("msgFileExt_m3u", "M3U playlist files (*.m3u;*.m3u8)|*.m3u;*.m3u8|as in the combo box to select file type from a dialog");
            }
            catch
            {

            }

            sdM3U.Title = DBLangEngine.GetMessage("msgSavePlaylistFile", "Save playlist file|As in export an album to a playlist file (m3u)");
            odM3U.Title = DBLangEngine.GetMessage("msgOpenPlaylistFile", "Open playlist file|As in open a play list file (m3u)");

            Database.DatabaseProgress += Database_DatabaseProgress;

            tmPendOperation.Enabled = true;
            FormSettings.SetMainWindowSettings();
            AmpRemote.MainWindow = this;
        }

        void selectAlbumClick(object sender, EventArgs e)
        {
            List<Album> albums = Database.GetAlbums(conn);
            foreach (Album album in albums)
            {
                ToolStripMenuItem item = sender as ToolStripMenuItem;
                if ((int)item.Tag == album.ID && album.AlbumName != CurrentAlbum)
                {
                    DisableChecks();
                    item.Checked = true;
                    Database.SaveQueue(PlayList, conn, CurrentAlbum);
                    GetAlbum(album.AlbumName);
                    return;
                }
            }
        }

        private HumanActivity humanActivity = null;

        public double seconds = 0;
        public double seconds_total = 0;
        private volatile WaveOut waveOutDevice = null;
        private volatile WaveStream mainOutputStream = null;
        private volatile WaveChannel32 volumeStream = null;
        private volatile bool stopped = false;
        private volatile bool playing = false;
        private volatile bool newsong = false;
        private volatile int latestSongIndex = -1;
        private bool pendNextSong = false; // if a lengthy operation is running, pend the GetNextSong()..

        private delegate void VoidDelegate();

        private Random random = new Random();

        private WaveStream CreateInputStream(string fileName)
        {
            try
            {
                WaveChannel32 inputStream;
                if (fileName.ToUpper().EndsWith(".mp3".ToUpper()))
                {
                    Mp3FileReader fr = new Mp3FileReader(fileName);
                    WaveStream mp3Reader = fr;
                    inputStream = new WaveChannel32(mp3Reader);
                }
                else if (fileName.ToUpper().EndsWith(".ogg".ToUpper()))
                {
                    VorbisWaveReader fr = new VorbisWaveReader(fileName);
                    WaveStream oggReader = fr;
                    inputStream = new WaveChannel32(oggReader);
                }
                else if (fileName.ToUpper().EndsWith(".wav".ToUpper()))
                {
                    WaveFileReader fr = new WaveFileReader(fileName);
                    WaveStream wavReader = fr;
                    inputStream = new WaveChannel32(wavReader);
                }
                else if (fileName.ToUpper().EndsWith(".flac".ToUpper()))
                {
                    FlacReader fr = new FlacReader(fileName);
                    WaveStream wavReader = fr;
                    inputStream = new WaveChannel32(wavReader);
                }
                else if (fileName.ToUpper().EndsWith(".wma".ToUpper()))
                {
                    WMAFileReader fr = new WMAFileReader(fileName);
                    WaveStream wavReader = fr;
                    inputStream = new WaveChannel32(wavReader);
                }
                else if (fileName.ToUpper().EndsWith(".m4a".ToUpper()) || fileName.ToUpper().EndsWith(".aac".ToUpper())) // Added: 01.02.2018
                {
                    MediaFoundationReader fr = new MediaFoundationReader(fileName);
                    WaveStream wavReader = fr;
                    inputStream = new WaveChannel32(wavReader);
                }
                else if (fileName.ToUpper().EndsWith(".aif".ToUpper()) || fileName.ToUpper().EndsWith(".aiff".ToUpper())) // Added: 01.02.2018
                {
                    AiffFileReader fr = new AiffFileReader(fileName);
                    WaveStream wavReader = fr;
                    inputStream = new WaveChannel32(wavReader);
                }
                else
                {
                    throw new InvalidOperationException(DBLangEngine.GetMessage("msgUnsupportedExt", "Unsupported file extension.|The file extension is not in the list of supported file types."));
                }
                inputStream.PadWithZeroes = false;
                volumeStream = inputStream;
                return volumeStream;
            }
            catch
            {
                try
                {
                    GetNextSong(true);
                }
                catch
                {
                    return CreateInputStream(fileName);
                }
            }
            return null;
        }

        private void UpdateSongName()
        {
            lbSong.Text = mFile.SongName;

            FormAlbumImage.Show(this, mFile, tbFind.PointToScreen(Point.Empty).Y);
            DisplayPlayingSong();
        }



        private void RefreshListboxFromThread()
        {
            lbMusic.RefreshItems();
        }

        private void PauseInvoker()
        {
            if (waveOutDevice != null)
            {
                if (waveOutDevice.PlaybackState == PlaybackState.Playing)
                {
                    lastPaused = true;
                    tbPlayNext.Image = Properties.Resources.amp_play;
                    tbPlayNext.ToolTipText = DBLangEngine.GetMessage("msgPlay", "Play|Play a song or resume paused");
                    waveOutDevice.Pause();
                }
            }
            TextInvoker();
        }

        private void PlayInvoker()
        {
            if (waveOutDevice != null && lastPaused)
            {
                if (waveOutDevice.PlaybackState == PlaybackState.Paused)
                {
                    tbPlayNext.Image = Properties.Resources.amp_pause;
                    tbPlayNext.ToolTipText = DBLangEngine.GetMessage("msgPause", "Pause|Pause playback");
                    waveOutDevice.Play();
                    lastPaused = false;
                }
            }
            TextInvoker();
            lastPaused = false;
        }

        private void SetPause()
        {
            tbPlayNext.Image = Properties.Resources.amp_pause;
            tbPlayNext.ToolTipText = DBLangEngine.GetMessage("msgPause", "Pause|Pause playback");
        }

        private volatile int calcMs = 0; // for the thread to calculate "time"

        private void PlayerThread()
        {
            while (!stopped)
            {
                if (mFile != null)
                {
                    if (!playing || newsong)
                    {
                        CloseWaveOut();
                        waveOutDevice = new WaveOut();
                        waveOutDevice.DesiredLatency = LatencyMS;
                        try
                        {
                            mainOutputStream = CreateInputStream(mFile.GetFileName());
                        }
                        catch
                        {
                            GetNextSong();
                            continue;
                        }
                        if (mainOutputStream == null)
                        {
                            continue;
                        }
                        seconds_total = mainOutputStream.TotalTime.TotalSeconds;

                        if (lbSong.InvokeRequired)
                        {
                            lbSong.Invoke(new VoidDelegate((UpdateSongName)));
                        }
                        else
                        {
                            UpdateSongName();
                        }

                        if (pnVol2.InvokeRequired)
                        {
                            lbSong.Invoke(new VoidDelegate((UpdateVolume)));
                        }
                        else
                        {
                            UpdateVolume();
                        }

                        if (pnStars1.InvokeRequired)
                        {
                            lbSong.Invoke(new VoidDelegate((UpdateStars)));
                        }
                        else
                        {
                            UpdateStars();
                        }
                        
                        if (tbTool.InvokeRequired) 
                        {
                            tbTool.Invoke(new VoidDelegate(SetPause));
                        }
                        else
                        {
                            SetPause();
                        }


                        waveOutDevice.Init(mainOutputStream);
                        waveOutDevice.PlaybackStopped += waveOutDevice_PlaybackStopped;
                        waveOutDevice.Play();
                        if (FormSettings.IsQuietHour() && !QuietHoursPause)
                        {
                            volumeStream.Volume = mFile.Volume * (float)QuietHoursVolPercentage;
                        }
                        else
                        {
                            volumeStream.Volume = mFile.Volume;
                        }

                        if (InvokeRequired)
                        {
                            Invoke(new VoidDelegate(TextInvoker));
                        }
                        else
                        {
                            TextInvoker();
                        }


                        playing = true;
                        newsong = false;
                    }

                    if ((calcMs % 100) == 0)
                    {
                        if (FormSettings.IsQuietHour() && QuietHoursPause)
                        {
                            if (InvokeRequired)
                            {
                                Invoke(new VoidDelegate(PauseInvoker));
                                Invoke(new VoidDelegate(TextInvoker));
                            }
                            else
                            {
                                PauseInvoker();
                                TextInvoker();
                            }
                        }
                        else
                        {
                            if (InvokeRequired)
                            {
                                Invoke(new VoidDelegate(PlayInvoker));
                            }
                            else
                            {
                                PlayInvoker();
                            }
                        }
                    }
                }
                Thread.Sleep(100);
                calcMs++; // 100 ms * 10 == second, lets make it ten seconds so 10 * 10 = 100;


                if (mainOutputStream == null)
                {
                    seconds = 0;
                    seconds_total = 0;
                }
                else
                {
                    seconds = mainOutputStream.CurrentTime.TotalSeconds;
                }
                playerThreadLoaded = true;
            }
            CloseWaveOut();
        }

        private void TextInvoker()
        {
            if (CurrentAlbum == "tmp")
            {
                Text = "amp#" + (QuietHours && FormSettings.IsQuietHour() ? " " + DBLangEngine.GetMessage("msgQuietHours", "[Quiet hours ({0} - {1})]|As in quiet hours defined in the settings are occurring now :-(", QuietHoursFrom, QuietHoursTo) : string.Empty);
            }
            else
            {
                Text = "amp# - " + CurrentAlbum + (QuietHours && FormSettings.IsQuietHour() ? " " + DBLangEngine.GetMessage("msgQuietHours", "[Quiet hours ({0} - {1})]|As in quiet hours defined in the settings are occurring now :-(", QuietHoursFrom, QuietHoursTo) : string.Empty);
            }
        }

        void waveOutDevice_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            GetNextSong(true);
        }

        internal bool Skipped
        {
            get
            {
                double percPlayed;
                if (mainOutputStream != null)
                {
                    try
                    {
                        percPlayed = 100.0 - ((mainOutputStream.TotalTime - mainOutputStream.CurrentTime).TotalSeconds / mainOutputStream.TotalTime.TotalSeconds * 100.0);
                    }
                    catch
                    {
                        percPlayed = 100;
                    }
                }
                else
                {
                    percPlayed = 100;
                }

                return percPlayed < 15.0;
            }
        }

        private void PlaySong(int index, bool random, bool addPlayedSong = true)
        {
            if (random)
            {
                UpdateRPlayed(mFile, Skipped);
            }
            else
            {
                Database.UpdateNPlayed(mFile, conn, Skipped);
            }

            mFile = PlayList[index];
            if (addPlayedSong)
            {
                playedSongs.Add(index);
            }

            if (random)
            {
                UpdateRPlayed(mFile, false);
            }
            else
            {
                Database.UpdateNPlayed(mFile, conn, false);
            }
            newsong = true;
            DisplayPlayingSong();
        }

        public void GetPrevSong()
        {
            if (playedSongs.Count < 2)
            {
                return;
            }
            else
            {
                int tmpInt = playedSongs[playedSongs.Count - 2];
                playedSongs.RemoveAt(playedSongs.Count - 1);
                PlaySong(tmpInt, false, false);
            }
        }

        private void CloseWaveOut()
        {
            if (waveOutDevice != null)
            {
                waveOutDevice.PlaybackStopped -= waveOutDevice_PlaybackStopped;
                waveOutDevice.Stop();
            }
            if (mainOutputStream != null)
            {
                // this one really closes the file and ACM conversion
                volumeStream.Close();
                volumeStream = null;
                // this one does the metering stream
                mainOutputStream.Close();
                mainOutputStream = null;
            }
            if (waveOutDevice != null)
            {
                waveOutDevice.Dispose();
                waveOutDevice = null;
            }
        }




        private void lbMusic_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        // File drag drop operation hangs the Windows Explorer (the event duration) so do it in a thread..
        #region DragDropThread
        private Thread fileAddThread = null;
        private List<string> fileAddList = null;
        private object _lock = new object();
        private volatile bool addFiles = false;
        private SynchronizationContext context = null;
        private bool lastPaused = false;

        private void ListFiles(object state)
        {
            List<MusicFile> addList = (List<MusicFile>)state;
            lbMusic.SuspendLayout();
            foreach (MusicFile mf in addList)
            {
                lbMusic.Items.Add(mf);
                PlayList.Add(mf);
            }
            lbMusic.ResumeLayout();
            ReIndexVisual();
            GetAlbum(CurrentAlbum, false);
            FormPsycho.UnExecute();
            this.Enabled = true;
        }


        private void StartFileAddThread()
        {
            if (fileAddThread != null)
            {
                while (!fileAddThread.Join(1000))
                {
                    Application.DoEvents();
                }
//                fileAddThread.Join();
                fileAddThread = null;
            }
            this.Enabled = false;
            FormPsycho.Execute(this);
            FormPsycho.SetStatusText(DBLangEngine.GetMessage("msgWorking", "Working...|The program is loading something"));
            context = SynchronizationContext.Current == null ? new SynchronizationContext() : SynchronizationContext.Current;
            fileAddThread = new Thread(new ThreadStart(ThreadFilesAdd));
            fileAddThread.Start();
        }

        private void ThreadFilesAdd()
        {
            if (addFiles)
            {
                humanActivity.Enabled = false;
                List<MusicFile> addList = new List<MusicFile>();
                foreach (string filePath in fileAddList)
                {
                    if (Path.GetExtension(filePath).ToUpper() == ".mp3".ToUpper() ||
                        Path.GetExtension(filePath).ToUpper() == ".ogg".ToUpper() ||
                        Path.GetExtension(filePath).ToUpper() == ".flac".ToUpper() ||
                        Path.GetExtension(filePath).ToUpper() == ".wma".ToUpper() ||
                        Path.GetExtension(filePath).ToUpper() == ".wav".ToUpper() ||
                        Path.GetExtension(filePath).ToUpper() == ".m4a".ToUpper() || // Added: 01.02.2018
                        Path.GetExtension(filePath).ToUpper() == ".aac".ToUpper() || // Added: 01.02.2018
                        Path.GetExtension(filePath).ToUpper() == ".aif".ToUpper() || // Added: 01.02.2018
                        Path.GetExtension(filePath).ToUpper() == ".aiff".ToUpper()) // Added: 01.02.2018
                    {
                        MusicFile mf;
                        if (!File.Exists(filePath))
                        {
                            continue;
                        }

                        mf = new MusicFile(filePath);
                        addList.Add(mf);
                    }
                }

                Database.AddFileToDB(addList, conn);

                Database.GetIDsForSongs(ref addList, conn);
                Database.AddSongToAlbum(CurrentAlbum, addList, conn);
                context.Send(ListFiles, addList);
                humanActivity.Enabled = true;
            }
            addFiles = false;
        }
        #endregion

        private void DoAddFileList(List<string> musicFiles, bool usePsycho = true)
        {
            lbMusic.SuspendLayout();
            humanActivity.Enabled = false;
            if (usePsycho)
            {
                FormPsycho.Execute(this);
                FormPsycho.SetStatusText(DBLangEngine.GetMessage("msgWorking", "Working...|The program is loading something"));
            }
             
            List<MusicFile> addList = new List<MusicFile>();

            foreach (string filePath in musicFiles)
            {
                if (Path.GetExtension(filePath).ToUpper() == ".mp3".ToUpper() ||
                    Path.GetExtension(filePath).ToUpper() == ".ogg".ToUpper() ||
                    Path.GetExtension(filePath).ToUpper() == ".flac".ToUpper() ||
                    Path.GetExtension(filePath).ToUpper() == ".wma".ToUpper() ||
                    Path.GetExtension(filePath).ToUpper() == ".wav".ToUpper() ||
                    Path.GetExtension(filePath).ToUpper() == ".m4a".ToUpper() || // Added: 01.02.2018
                    Path.GetExtension(filePath).ToUpper() == ".aac".ToUpper() || // Added: 01.02.2018
                    Path.GetExtension(filePath).ToUpper() == ".aif".ToUpper() || // Added: 01.02.2018
                    Path.GetExtension(filePath).ToUpper() == ".aiff".ToUpper()) // Added: 01.02.2018
                {
                    MusicFile mf;
                    if (!File.Exists(filePath))
                    {
                        continue;
                    }

                    mf = new MusicFile(filePath);
                    addList.Add(mf);
                }
            }
            Database.AddFileToDB(addList, conn);

            Database.GetIDsForSongs(ref addList, conn);
            Database.AddSongToAlbum(CurrentAlbum, addList, conn);
            foreach (MusicFile mf in addList)
            {
                lbMusic.Items.Add(mf);
                PlayList.Add(mf);
            }
            addList = null;
            ReIndexVisual();

            lbMusic.ResumeLayout();
            if (usePsycho)
            {
                FormPsycho.UnExecute();
            }
            humanActivity.Enabled = true;
        }



        private void lbMusic_DragDrop(object sender, DragEventArgs e)
        {            
            List<string> musicFiles = new List<string>();
            humanActivity.Enabled = false;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] dropFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string filePath in dropFiles)
                {
                    if (Directory.Exists(filePath))
                    {
                        musicFiles.AddRange(Directory.GetFiles(filePath + "\\", "*.*", SearchOption.AllDirectories).ToArray());
                    }
                    else if (File.Exists(filePath))
                    {
                        musicFiles.Add(filePath);
                    }
                }
            }
            fileAddList = musicFiles;
            addFiles = true;
            StartFileAddThread();
            humanActivity.Enabled = true;
        }

        void Database_DatabaseProgress(DatabaseEventArgs e)
        {
            if (e.EventType == DatabaseEventType.UpdateSongDB)
            {                
                FormPsycho.SetStatusText(DBLangEngine.GetMessage("msgUpdateDB", "Updating song list: {0} / {1}...|A conditional database update is in progress.", e.Progress, e.ProgressEnd));
            }
            else if (e.EventType == DatabaseEventType.InsertSongDB)
            {
                FormPsycho.SetStatusText(DBLangEngine.GetMessage("msgAddDB", "Adding songs: {0} / {1}...|A conditional database add is in progress.", e.Progress, e.ProgressEnd));
            }
            else if (e.EventType == DatabaseEventType.InsertSongAlbum)
            {
                FormPsycho.SetStatusText(DBLangEngine.GetMessage("msgAddDBAlbum", "Adding songs to album: {0} / {1}...|A conditional database album add is in progress.", e.Progress, e.ProgressEnd));
            }
            else if (e.EventType == DatabaseEventType.GetSongID)
            {
                FormPsycho.SetStatusText(DBLangEngine.GetMessage("msgIDSong", "Identifying songs: {0} / {1}...|Songs are identified based on the database song data.", e.Progress, e.ProgressEnd));
            }
            else if (e.EventType == DatabaseEventType.LoadMeta)
            {
                FormPsycho.SetStatusText(DBLangEngine.GetMessage("msgLoadMeta", "Metadata loading: {0} / {1}...|Song metadata(tags) is being loaded.", e.Progress, e.ProgressEnd));
            }
        }


        private void lbMusic_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void tmSeek_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!progressUpdating)
                {
                    scProgress.Maximum = (int)seconds_total;
                    scProgress.Value = (int)seconds;
                    TimeSpan ts = TimeSpan.FromSeconds(seconds_total - seconds);
                    lbTime.Text = "-" + ts.ToString(@"mm\:ss");
                }
            }
            catch
            {

            }
        }

        private void lbMusic_DoubleClick(object sender, EventArgs e)
        {
            if (lbMusic.SelectedItem != null)
            {
                PlaySong((lbMusic.SelectedItem as MusicFile).VisualIndex, false);
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            tmSeek.Stop();
            tmPendOperation.Stop();
            CloseWaveOut();
            stopped = true;
            while (!thread.Join(1000))
            {
                Application.DoEvents();
            }
//            thread.Join();
            Database.SaveQueue(PlayList, conn, CurrentAlbum);
        }


        private void GetAlbum(string name, bool usePsycho = true)
        {
            albumLoading = true;
            if (usePsycho)
            {
                FormPsycho.Execute(this);
                FormPsycho.SetStatusText(DBLangEngine.GetMessage("msgLoadingAlbum", "Loading album '{0}'...|Text for loading an album (enumerating files and their tags)", name));
            }
            Database.GetAlbum(name, ref PlayList, conn);
            CurrentAlbum = name;
            if (name == "tmp")
            {
                Text = "amp#" + (QuietHours && FormSettings.IsQuietHour() ? " " + DBLangEngine.GetMessage("msgQuietHours", "[Quiet hours ({0} - {1})]|As in quiet hours defined in the settings are occurring now :-(", QuietHoursFrom, QuietHoursTo) : string.Empty); 
            }
            else
            {
                Text = "amp# - " + CurrentAlbum + (QuietHours && FormSettings.IsQuietHour() ? " " + DBLangEngine.GetMessage("msgQuietHours", "[Quiet hours ({0} - {1})]|As in quiet hours defined in the settings are occurring now :-(", QuietHoursFrom, QuietHoursTo) : string.Empty); 
            }

            lbMusic.Items.Clear(); // LOCATION:NOT FILTERED

            if (usePsycho)
            {
                foreach (MusicFile mf in PlayList)
                {
                    FormPsycho.SetStatusText(mf.GetFileName());
                }
            }
            lbMusic.Items.AddRange(PlayList.ToArray());
            GetQueueCount();
            if (usePsycho)
            {
                FormPsycho.UnExecute();
            }
            Filtered = false;
            albumLoading = false;
            albumChanged = true;
        }

        private void UpdateRPlayed(MusicFile mf, bool skipped)
        {
            if (mf == null)
            {
                return;
            }

            using (SQLiteCommand command = new SQLiteCommand(conn))
            {
                command.CommandText = "UPDATE SONG SET NPLAYED_RAND = IFNULL(NPLAYED_RAND, 0) + 1, SKIPPED_EARLY = IFNULL(SKIPPED_EARLY, 0) + " + (skipped ? "1" : "0") + " WHERE ID = " + mf.ID + " ";
                command.ExecuteNonQuery();
            }
        }

        private void ReIndexVisual()
        {
            int iCount = 0;
            foreach (MusicFile mf in PlayList)
            {
                mf.VisualIndex = iCount++;
            }
        }

        private void tbFind_TextChanged(object sender, EventArgs e)
        {
            Find();
        }

        private void tbFind_KeyDown(object sender, KeyEventArgs e)
        {
            if (lbMusic.Items.Count > 0)
            {
                if (e.KeyCode == Keys.Down)
                {
                    int iIndex = lbMusic.SelectedIndex;
                    lbMusic.ClearSelected();
                    if (iIndex + 1 == lbMusic.Items.Count)
                    {
                        lbMusic.SelectedIndex = 0;
                    }
                    else
                    {
                        iIndex++;
                        lbMusic.SelectedIndex = iIndex;
                    }
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    lbMusic.Focus();
                }
                else if (e.KeyCode == Keys.Up)
                {
                    int iIndex = lbMusic.SelectedIndex;
                    lbMusic.ClearSelected();
                    if (iIndex - 1 < 0)
                    {
                        lbMusic.SelectedIndex = lbMusic.Items.Count - 1;
                    }
                    else
                    {
                        iIndex--;
                        lbMusic.SelectedIndex = iIndex;
                    }
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    lbMusic.Focus();
                }
            }
        }

        private void tbNext_Click(object sender, EventArgs e)
        {
            humanActivity.Stop();
            tbPlayNext.Image = Properties.Resources.amp_pause;
            tbPlayNext.ToolTipText = DBLangEngine.GetMessage("msgPause", "Pause|Pause playback");
            GetNextSong();
        }

        private void tbPrevious_Click(object sender, EventArgs e)
        {
            humanActivity.Stop();
            tbPlayNext.Image = Properties.Resources.amp_pause;
            tbPlayNext.ToolTipText = DBLangEngine.GetMessage("msgPause", "Pause|Pause playback");
            GetPrevSong();
        }

        private void mnuNewAlbum_Click(object sender, EventArgs e)
        {
            string name = FormAddAlbum.Execute();
            if (name != string.Empty)
            {
                ListAlbums(Database.AddNewAlbum(name, conn));
            }
        }

        private int GetQueueCountNum()
        {
            return PlayList.Count(f => f.QueueIndex > 0);
        }

        private int queueCount = 0;
        /// <summary>
        /// Saves the current queue count.
        /// </summary>
        private void PushQueueCount()
        {
            queueCount = GetQueueCountNum();
        }

        /// <summary>
        /// Gets a value indicating whether the queue count has been changed after last call to PushQueueCount method.
        /// </summary>
        private bool QueueChanged
        {
            get
            {
                bool result = queueCount != GetQueueCountNum();
                queueCount = 0; // do auto-reset..
                return result;
            }
        }

        private void GetQueueCount()
        {
            lbQueueCount.Text = DBLangEngine.GetMessage("msgInQueue", "In queue: {0}|How many songs are in the queue", GetQueueCountNum());
        }

        private void UpdateVolume()
        {
            pnVol2.Left = (int)(mFile.Volume * 50F);
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {

            conn = new SQLiteConnection("Data Source=" + DBLangEngine.DataDir + "amp.sqlite;Pooling=true;FailIfMissing=false;Cache Size=10000;"); // PRAGMA synchronous=OFF;PRAGMA journal_mode=OFF
            conn.Open();

            if (!ScriptRunner.RunScript(DBLangEngine.DataDir + "amp.sqlite"))
            {
                MessageBox.Show(
                    DBLangEngine.GetMessage("msgErrorInScript",
                    "A script error occurred on the database update|Something failed during running the database update script"),
                    DBLangEngine.GetMessage("msgError", "Error|A message describing that some kind of error occurred."),
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                // at this point there is no reason to continue the program's execution as the database might be in an invalid state..
                throw new Exception(DBLangEngine.GetMessage("msgErrorInScript",
                    "A script error occurred on the database update|Something failed during running the database update script"));
            }

            CurrentAlbum = DBLangEngine.GetMessage("msgDefault", "Default|Default as in default album");
            Database.AddDefaultAlbum(DBLangEngine.GetMessage("msgDefault", "Default|Default as in default album"), conn);

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            thread = new Thread(new ThreadStart(PlayerThread));
            thread.Start();
            while (!playerThreadLoaded)
            {
                Thread.Sleep(500);
            }

            if (Program.Arguments.Count > 1)
            {
                CurrentAlbum = "tmp";
                Database.ClearTmpAlbum(ref PlayList, conn);
                this.DoAddFileList(Program.Arguments);
                ListAlbums(0);
            }
            else if (Program.Arguments.Count == 1 && !Program.Arguments.Contains("#--dblang")) 
                // this is the way the debug argument doesn't dump the language database (#--dblang), but the argument may remain as an invalid argument vs the right one (--dblang).. #ref1
            {
                if (CurrentAlbum != "tmp")
                {
                    CurrentAlbum = "tmp";
                    Database.ClearTmpAlbum(ref PlayList, conn);
                    tbShuffle.Checked = true;
                    tbRand.Checked = false;
                    tbFind.Text = string.Empty;
                }
                CurrentAlbum = "tmp";
                this.DoAddFileList(Program.Arguments, false);

                GetAlbum(CurrentAlbum, false);
                ListAlbums(0);
            }
            else
            {
                ListAlbums(1);
            }

            if (CurrentAlbum != "tmp")
            {
                GetAlbum(CurrentAlbum);
            }

            if (Program.Arguments.Count > 0 && !Program.Arguments.Contains("#--dblang")) // see "#ref1" for the '#--dblang'...
            {
                if (!playing)
                {
                    GetNextSong(true);
                }
            }
            albumChanged = false;
            remote.InitAmpRemote();
        }

        private void tbPlayNext_Click(object sender, EventArgs e)
        {
            if (waveOutDevice != null)
            {
                if (waveOutDevice.PlaybackState == PlaybackState.Paused)
                {
                    tbPlayNext.Image = Properties.Resources.amp_pause;
                    tbPlayNext.ToolTipText = DBLangEngine.GetMessage("msgPause", "Pause|Pause playback");
                    waveOutDevice.Play();
                }
                else if (waveOutDevice.PlaybackState == PlaybackState.Playing)
                {
                    tbPlayNext.Image = Properties.Resources.amp_play;
                    tbPlayNext.ToolTipText = DBLangEngine.GetMessage("msgPlay", "Play|Play a song or resume paused");
                    waveOutDevice.Pause();
                }
            }
            else
            {
                humanActivity.Stop();
                tbPlayNext.Image = Properties.Resources.amp_pause;
                tbPlayNext.ToolTipText = DBLangEngine.GetMessage("msgPause", "Pause|Pause playback");
                GetNextSong();
            }
        }

        private void scProgress_Scroll(object sender, ScrollEventArgs e)
        {
            tmSeek.Stop();
            mainOutputStream.CurrentTime = new TimeSpan(0, 0, e.NewValue);
            tmSeek.Start();
        }

        private void UpdateStars()
        {
            pnStars1.Left = (int)((double)mFile.Rating / 1000.0 * 176.0);
        }

        private void pnStars0_MouseClick(object sender, MouseEventArgs e)
        {
            if (mFile != null || lbMusic.SelectedIndices.Count > 0)
            {
                int stars = 0;
                if (sender == pnStars0)
                {
                    stars = (int)(500 * ((double)e.X / 88.0));
                    pnStars1.Left = e.X;
                }
                else if (sender == pnStars1)
                {
                    pnStars1.Left += e.X;
                    stars = (int)(500 * ((double)pnStars1.Left / 88.0));
                }

                if (mFile != null)
                {
                    mFile.Rating = stars;
                    mFile.RatingChanged = true;
                    Database.SaveRating(mFile, conn);
                }

                for (int i = 0; i < lbMusic.SelectedIndices.Count; i++)
                {
                    MusicFile mf = (MusicFile)lbMusic.Items[lbMusic.SelectedIndices[i]];
                    mf.Rating = stars;
                    mf.RatingChanged = true;
                    Database.SaveRating(mf, conn);
                    lbMusic.Items[lbMusic.SelectedIndices[i]] = mf;
                }
            }
        }

        private void tbShowQueue_Click(object sender, EventArgs e)
        {
            ShowQueue();
        }

        internal void ShowQueue()
        {
            lbMusic.Invoke(new MethodInvoker(() =>
            {
                if (PlayList.Count(f => f.QueueIndex > 0) == 0) // don't show an empty queue..
                {
                    return;
                }
                lbMusic.Items.Clear();
                List<MusicFile> queuedSongs = new List<MusicFile>();
                foreach (MusicFile mf in PlayList)
                {
                    if (mf.QueueIndex > 0)
                    {
                        queuedSongs.Add(mf);
                    }
                }
                queuedSongs = queuedSongs.OrderBy(f => f.QueueIndex).ToList();

                foreach (MusicFile mf in queuedSongs)
                {
                    lbMusic.Items.Add(mf);
                }
            }));

            Filtered = true;
            queueShowing = true;
        }

        internal void ShowAlternateQueue()
        {
            lbMusic.Invoke(new MethodInvoker(() =>
            {
                if (PlayList.Count(f => f.AlternateQueueIndex > 0) == 0) // don't show an empty queue..
                {
                    return;
                }
                lbMusic.Items.Clear();
                List<MusicFile> queuedSongs = new List<MusicFile>();
                foreach (MusicFile mf in PlayList)
                {
                    if (mf.AlternateQueueIndex > 0)
                    {
                        queuedSongs.Add(mf);
                    }
                }
                queuedSongs = queuedSongs.OrderBy(f => f.AlternateQueueIndex).ToList();

                foreach (MusicFile mf in queuedSongs)
                {
                    lbMusic.Items.Add(mf);
                }
            }));
        }

        /// <summary>
        /// Gets a value indicating whether the queue is being displayed on the form.
        /// </summary>
        internal bool QueueShowing
        {
            get
            {
                if (PlayList.Count(f => f.QueueIndex > 0) == 0)
                {
                    queueShowing = false;
                    Filtered = false;
                }
                return queueShowing;
            }
        }

        private void mnuDeQueue_Click(object sender, EventArgs e)
        {
            bool affected = false;
            foreach (MusicFile mf in PlayList)
            {
                if (mf.QueueIndex > 0)
                {
                    mf.QueueIndex = 0; //::QUEUE
                    affected = true;
                }
            }
            if (affected)
            {
                lbMusic.RefreshItems();
                GetQueueCount();                
                if (!PlayList.Exists(f => f.QueueIndex > 0)) // no empty queue..
                {
                    ShowPlayingSong();
                }
            }
        }

        private void mnuScrambleQueue_Click(object sender, EventArgs e)
        {
            ScrambleQueue(); // scramble the queue to have new random indices..
        }

        /// <summary>
        /// Scrambles the queue to have new random indices.
        /// </summary>
        /// <returns>True if any songs were affected; otherwise false.</returns>
        public bool ScrambleQueue()
        {
            humanActivity.Enabled = false;
            bool affected = false; // if any songs in the play list was affected..
            List<int> queueIndices = new List<int>(); // the list of current queue indices..
            List<int> newQueueIndices = new List<int>(); // the list of new current queue indices..

            // get the current queue indices..
            foreach (MusicFile mf in PlayList)
            {
                if (mf.QueueIndex > 0)
                {
                    queueIndices.Add(mf.QueueIndex);
                }
            }

            // if there is nothing queued do not continue the method execution..
            if (queueIndices.Count == 0)
            {
                humanActivity.Enabled = true;
                return affected;
            }

            // randomize the new indices..
            for (int i = 0; i < queueIndices.Count; i++)
            {
                int newQueueIndex = random.Next(queueIndices.Count) + 1;
                while ((queueIndices[i] == newQueueIndex && i != queueIndices.Count - 1) || newQueueIndices.Exists(f => f == newQueueIndex))
                {
                    newQueueIndex = random.Next(queueIndices.Count) + 1;
                }
                newQueueIndices.Add(newQueueIndex);
                affected = true;
            }

            int nextIndex = 0;
            foreach (MusicFile mf in PlayList)
            {
                if (mf.QueueIndex > 0)
                {
                    mf.QueueIndex = newQueueIndices[nextIndex++];
                }
            }

            if (affected)
            {
                ShowQueue();
            }
            humanActivity.Enabled = true;
            return affected;
        }

        private void mnuSelectAll_Click(object sender, EventArgs e)
        {
            humanActivity.Enabled = false;
            lbMusic.SuspendLayout();
            for (int i = 0; i < lbMusic.Items.Count; i++)
            {
                lbMusic.SetSelected(i, true);
            }
            humanActivity.Enabled = true;
            lbMusic.ResumeLayout();
            lbMusic.Focus();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            RemotingMessageServer.Register(50670, "apm_sharp_remoting", remotingOnMessage, 1000, 1000000);
            humanActivity = new HumanActivity(15);
            humanActivity.UserSleep += humanActivity_OnUserSleep;
        }

        private void DisplayPlayingSong()
        {
            if (humanActivity.Sleeping)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new VoidDelegate(ShowPlayingSong));
                }
                else
                {
                    ShowPlayingSong();
                }
            }
        }

        private bool queueEmpty = false;

        private void ShowPlayingSong()
        {
            for (int i = 0; i < lbMusic.Items.Count; i++ )
            {
                if (mFile != null && mFile.ID == (lbMusic.Items[i] as MusicFile).ID)
                {
                    lbMusic.SetIndex(i);
                    return;
                }
            }

            if (QueueShowing) // avoid the queue to go
            {
                return;
            }

            if (Filtered || queueEmpty != QueueShowing) // only do this "jump" if the list is filtered..
            {
                lbMusic.Items.Clear();
                foreach (MusicFile mf in PlayList) // LOCATION:NOT FILTERED
                {
                    lbMusic.Items.Add(mf);
                }
            }

            queueEmpty = QueueShowing;

            Filtered = false;
            for (int i = 0; i < lbMusic.Items.Count; i++)
            {
                if (mFile != null && mFile.ID == (lbMusic.Items[i] as MusicFile).ID)
                {
                    lbMusic.SetIndex(i);
                    return;
                }
            }
        }

        void humanActivity_OnUserSleep(object sender, UserSleepEventArgs e)
        {
            DisplayPlayingSong();
        }

        void remotingOnMessage(string message)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, e) => 
            {
                while (!playerThreadLoaded)
                {
                    Thread.Sleep(500);
                }
            };
            bw.RunWorkerCompleted += (s, e) => 
            {
                Program.Arguments.Clear();

                Program.Arguments.AddRange(message.Split(';'));
                if (Program.Arguments.Count > 1)
                {
                    CurrentAlbum = "tmp";
                    Database.ClearTmpAlbum(ref PlayList, conn);
                    this.DoAddFileList(Program.Arguments);


                    GetAlbum(CurrentAlbum, false);
                    tbRand.Checked = false;
                    tbShuffle.Checked = true;
                    tbFind.Text = string.Empty;
                    GetNextSong(true);
                }
                else if (Program.Arguments.Count == 1)
                {
                    if (CurrentAlbum != "tmp")
                    {
                        CurrentAlbum = "tmp";
                        Database.ClearTmpAlbum(ref PlayList, conn);
                        tbShuffle.Checked = true;
                        tbRand.Checked = false;
                        tbFind.Text = string.Empty;
                    }
                    CurrentAlbum = "tmp";
                    this.DoAddFileList(Program.Arguments, false);

                    GetAlbum(CurrentAlbum, false);
                    ListAlbums(0);
                }
            };
            bw.RunWorkerAsync();
        }

        private void mnuPlayListM3UNewAlbum_Click(object sender, EventArgs e)
        {
            if (odM3U.ShowDialog() == DialogResult.OK)
            {
                string name = FormAddAlbum.Execute(Path.GetFileNameWithoutExtension(odM3U.FileName));
                if (name != string.Empty)
                {
                    int albumIndex = Database.AddNewAlbum(name, conn);
                    M3U m3u = new M3U(odM3U.FileName);
                    List<MusicFile> m3uAdd = new List<MusicFile>();
                    foreach(M3UEntry m in m3u.M3UFiles)
                    {
                        MusicFile addMusicFile = new MusicFile(m.FileName);
                        addMusicFile.OverrideName = m.FileDesc;
                        m3uAdd.Add(addMusicFile);
                    }

                    Database.AddFileToDB(m3uAdd, conn);
                    Database.GetIDsForSongs(ref m3uAdd, conn);
                    Database.AddSongToAlbum(name, m3uAdd, conn);
                    ListAlbums(albumIndex);
                    GetAlbum(name);
                }
            }
        }

        private void mnuPlayListM3UToCurrentAlbum_Click(object sender, EventArgs e)
        {
            if (odM3U.ShowDialog() == DialogResult.OK)
            {
                M3U m3u = new M3U(odM3U.FileName);
                List<MusicFile> m3uAdd = new List<MusicFile>();
                foreach (M3UEntry m in m3u.M3UFiles)
                {
                    MusicFile addMusicFile = new MusicFile(m.FileName);
                    addMusicFile.OverrideName = m.FileDesc;
                    m3uAdd.Add(addMusicFile);
                }
                Database.GetIDsForSongs(ref m3uAdd, conn);
                Database.AddSongToAlbum(CurrentAlbum, m3uAdd, conn);
                GetAlbum(CurrentAlbum);
            }
        }

        private void mnuPlaylistM3UExport_Click(object sender, EventArgs e)
        {
            if (PlayList.Count > 0)
            {
                sdM3U.FileName = CurrentAlbum;
                if (sdM3U.ShowDialog() == DialogResult.OK)
                {
                    Encoding enc;
                    if (sdM3U.FileName.ToUpper().EndsWith("m3u".ToUpper()))
                    {
                        enc = Encoding.GetEncoding(1252);
                    }
                    else if (sdM3U.FileName.ToUpper().EndsWith("m3u8".ToUpper()))
                    {
                        enc = Encoding.UTF8;
                    }
                    else
                    {
                        return;
                    }

                    if (File.Exists(sdM3U.FileName))
                    {
                        File.Delete(sdM3U.FileName);
                    }

                    using (FileStream fs = new FileStream(sdM3U.FileName, FileMode.Create, FileAccess.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, enc))
                        {
                            sw.WriteLine("#EXTM3U");
                            foreach (MusicFile mf in PlayList)
                            {
                                sw.WriteLine("#EXTINF:" + mf.Duration + "," + ((mf.OverrideName == string.Empty) ? mf.ToString() : mf.OverrideName));
                                sw.WriteLine(mf.FullFileName);
                                sw.WriteLine();
                            }
                        }
                    }
                }
            }
        }

        private void tbFind_Click(object sender, EventArgs e)
        {
            Find(true);
        }

        private void tbFind_Enter(object sender, EventArgs e)
        {
            Find(true);
        }

        private void pnVol1_MouseClick(object sender, MouseEventArgs e)
        {
            if ((mFile != null && volumeStream != null) || lbMusic.SelectedIndices.Count > 0)
            {
                float volume = 1;
                if (sender == pnVol1)
                {
                    volume = 1.0F * ((float)e.X / 50F);
                    pnVol2.Left = e.X;
                }
                else if (sender == pnVol2)
                {
                    volume = 1.0F * ((float)pnVol2.Left / 50F);
                    pnVol2.Left += e.X;                    
                }

                if (volumeStream != null)
                {
                    volumeStream.Volume = volume;
                }

                if (mFile != null)
                {
                    mFile.Volume = volumeStream.Volume;
                    Database.SaveVolume(mFile, conn);
                }

                for (int i = 0; i < lbMusic.SelectedIndices.Count; i++)
                {
                    int idx = lbMusic.SelectedIndices[i];
                    int index = PlayList.FindIndex(f => f.ID == ((MusicFile)lbMusic.Items[idx]).ID);
                    if (index >= 0)
                    {
                        PlayList[index].Volume = volume;
                        lbMusic.Items[idx] = PlayList[index];
                        Database.SaveVolume(PlayList[index], conn);
                    }
                }
            }
        }

        private void tmPendOperation_Tick(object sender, EventArgs e)
        {
            if (pendNextSong && !addFiles)
            {
                pendNextSong = false;
                GetNextSong(true);
            }
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            new VPKSoft.About.FormAbout(this, "Ms-Pl", "https://opensource.org/licenses/MS-PL");
        }

        private void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            FormAlbumImage.Reposition(this, tbFind.PointToScreen(Point.Empty).Y);
        }

        private void saveQueueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GetQueueCountNum() == 0)
            {
                MessageBox.Show(DBLangEngine.GetMessage("msgQueueSomething", "Please add some songs to the queue.|Ask in nicely ask the user to queue some songs."),
                    DBLangEngine.GetMessage("msgInformation", "Information|Some information is given to the user, do add more definitive message to make some sense to the 'information'..."),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string queueName = FormQueueSnapshotName.Execute(CurrentAlbum);
            if (queueName != string.Empty)
            {
                Database.SaveQueueSnapshot(PlayList, conn, CurrentAlbum, queueName);
            }

            // the alternate queue must go away..
            if (PlayList.Exists(f => f.AlternateQueueIndex >= 0))
            {
                for (int i = 0; i < PlayList.Count; i++)
                {
                    PlayList[i].AlternateQueueIndex = 0;
                }
                lbMusic.RefreshItems();
            }
        }

        private void mnuLoadQueue_Click(object sender, EventArgs e)
        {
            bool append;
            int qID =  FormSavedQueues.Execute(CurrentAlbum, ref conn, out append);
            if (qID != -1)
            {
                Database.LoadQueue(ref PlayList, conn, qID, append);
            }
            lbMusic.RefreshItems();
            GetQueueCount();
        }

        private void mnuSettings_Click(object sender, EventArgs e)
        {
            new FormSettings().ShowDialog();
            TextInvoker();
        }

        private void mnuSongInfo_Click(object sender, EventArgs e)
        {
            if (lbMusic.SelectedItem != null)
            {
                MusicFile mf = lbMusic.SelectedItem as MusicFile;
                FormTagInfo.Execute(mf, this);
            }
        }

        private void mnuShowAlternateQueue_Click(object sender, EventArgs e)
        {
            ShowAlternateQueue();
        }
    }
}
