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

#region Usings
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using amp.FormsUtility.Help;
using amp.FormsUtility.Information;
using amp.FormsUtility.Progress;
using amp.FormsUtility.QueueHandling;
using amp.FormsUtility.UserInteraction;
using amp.FormsUtility.Visual;
using amp.Properties;
using amp.SQLiteDatabase;
using amp.UtilityClasses;
using amp.UtilityClasses.Settings;
using amp.UtilityClasses.WindowsPowerSave;
using amp.WCFRemote;
using NAudio.Flac;
using NAudio.Vorbis;
using NAudio.Wave;
using NAudio.WindowsMediaFormat;
using VPKSoft.ErrorLogger;
using VPKSoft.LangLib;
using VPKSoft.PosLib;
using VPKSoft.ScriptRunner;
using VPKSoft.Utils;
using VPKSoft.VersionCheck.Forms;
using Utils = VPKSoft.LangLib.Utils;
#endregion

namespace amp
{
    // ReSharper disable once RedundantExtendsListEntry
    /// <summary>
    /// The main form of the application.
    /// Implements the <see cref="VPKSoft.LangLib.DBLangEngineWinforms"/>
    /// </summary>
    public partial class FormMain : DBLangEngineWinforms
    {
        #region Fields        
        /// <summary>
        /// Gets or sets the files passed by the IPC channel.
        /// </summary>
        internal static List<string> RemoteFiles { get; set; } = new List<string>();

        /// <summary>
        /// A value indicating if the playback state was previously paused.
        /// </summary>
        private bool lastPaused;

        /// <summary>
        /// Gets a value indicating whether the software is processing the <see cref="RemoteFiles"/> list.
        /// </summary>
        internal static volatile bool RemoteFileBeingProcessed;

        /// <summary>
        /// A flag to indicate for the <see cref="tmIPCFiles"/> timer whether to execute its code or not.
        /// </summary>
        internal static volatile bool StopIpcTimer = false;

        // the self-hosted WCF remote control API for the software.
        readonly AmpRemote remote = new AmpRemote();

        /// <summary>
        /// The currently playing musing file.
        /// </summary>
        public volatile MusicFile MFile;

        // the thread that handles the playback logic (next song, randomizing, UI updates, e.g.)..
        private Thread thread;

        /// <summary>
        /// The SQLiteConnection for the database access.
        /// </summary>
        public static SQLiteConnection Connection { get; set; } // database connection for the SQLite database

        /// <summary>
        /// Gets or sets a value indicating whether a restart for the application is required.
        /// </summary>
        public static bool RestartRequired { get; set; }

        /// <summary>
        /// The name of a currently playing album.
        /// </summary>
        public string CurrentAlbum;

        /// <summary>
        /// The list of entries in the current album.
        /// </summary>
        public List<MusicFile> PlayList = new List<MusicFile>();

        // list of indexes of the played songs in the PlayList..
        private readonly List<int> playedSongs = new List<int>();

        // a flag indicating if the player thread is active..
        private volatile bool playerThreadLoaded;

        // a flag indicating if the play back progress (the ProgressBar and the time left text) are changing via a user generated event..
        private readonly bool progressUpdating = false;

        /// <summary>
        /// A screen refresh counter for the playback thread to calculate "time".
        /// </summary>
        private volatile int calcMs;
        #endregion

        #region Settings
        /// <summary>
        /// A flag indicating whether the quiet hours is enabled in the settings.
        /// </summary>
        public static bool QuietHours { get; set; } = false;

        /// <summary>
        /// A value indicating the quiet hour starting time if the <see cref="QuietHours"/> is enabled.
        /// </summary>
        public static string QuietHoursFrom  { get; set; } = "08:00";

        /// <summary>
        /// The audio visualization style.
        /// </summary>
        public static int AudioVisualizationStyle { get; set; } = 0;

        /// <summary>
        /// The percentage the audio visualization should take from the main form's playlist area.
        /// </summary>
        public static int AudioVisualizationVisualPercentage { get; set; } = 15;

        /// <summary>
        /// A value indicating whether the audio visualization should combine the channels into a single view.
        /// </summary>
        public static bool AudioVisualizationCombineChannels { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the audio visualization bar graph bars follow minimum and maximum intensity levels of the current song.
        /// </summary>
        public static bool BalancedBars { get; set; }

        /// <summary>
        /// Gets or sets the bar amount to display in the audio visualization.
        /// </summary>
        public static int BarAmount { get; set; }

        /// <summary>
        /// A value indicating the quiet hour ending time if the <see cref="QuietHours"/> is enabled.
        /// </summary>
        public static string QuietHoursTo { get; set; } = "23:00";

        /// <summary>
        /// A value indicating whether to pause the playback at a quiet hour in case if the <see cref="QuietHours"/> is enabled.
        /// </summary>
        public static bool QuietHoursPause { get; set; } = false;

        /// <summary>
        /// A value indicating a volume decrease in percentage if the <see cref="QuietHours"/> is enabled.
        /// </summary>
        public static double QuietHoursVolPercentage { get; set; } = 0.7;

        /// <summary>
        /// The latency in milliseconds for the <see cref="WaveOut.DesiredLatency"/>.
        /// </summary>
        public static int LatencyMs { get; set; } = 300;

        /// <summary>
        /// A value indicating if the remote control WCF API is enabled.
        /// </summary>
        public static bool RemoteControlApiWcf { get; set; } = false;

        /// <summary>
        /// The remote control WCF API address (URL).
        /// </summary>
        public static string RemoteControlApiWcfAddress { get; set; } = "http://localhost:11316/ampRemote";

        /// <summary>
        /// A value indicating whether the software should check for updates automatically upon startup.
        /// </summary>
        public static bool AutoCheckUpdates { get; set; } = false;

        /// <summary>
        /// Gets or sets the stack random percentage.
        /// </summary>
        internal static int StackRandomPercentage { get => MusicFile.StackRandomPercentage; set => MusicFile.StackRandomPercentage = value; }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="FormMain"/> class.
        /// </summary>
        public FormMain() 
        {
            // Add this form to be positioned..
            PositionForms.Add(this);

            InitializeComponent();

            // ReSharper disable once StringLiteralTypo
            DBLangEngine.DBName = "lang.sqlite";

            if (Utils.ShouldLocalize() != null)
            {
                DBLangEngine.InitializeLanguage("amp.Messages", Utils.ShouldLocalize(), false);
                return; // After localization don't do anything more.
            }

            DBLangEngine.InitializeLanguage("amp.Messages");

            try // as this can be translated to a invalid format :-)
            {
                sdM3U.Filter = DBLangEngine.GetMessage("msgFileExt_m3u", "M3U playlist files (*.m3u;*.m3u8)|*.m3u;*.m3u8|as in the combo box to select file type from a dialog");
                odM3U.Filter = DBLangEngine.GetMessage("msgFileExt_m3u", "M3U playlist files (*.m3u;*.m3u8)|*.m3u;*.m3u8|as in the combo box to select file type from a dialog");
            }
            catch
            {
                // ignored..
            }

            sdM3U.Title = DBLangEngine.GetMessage("msgSavePlaylistFile", "Save playlist file|As in export an album to a playlist file (m3u)");
            odM3U.Title = DBLangEngine.GetMessage("msgOpenPlaylistFile", "Open playlist file|As in open a play list file (m3u)");

            Database.DatabaseProgress += Database_DatabaseProgress;

            tmPendOperation.Enabled = true;
            FormSettings.SetMainWindowSettings();
            AmpRemote.MainWindow = this;

            MusicFile.StackRandomPercentage = StackRandomPercentage;

            // no designer (!?)..
            mnuQueueMoveToTop.ShortcutKeys = Keys.Control | Keys.PageUp;

            SetAudioVisualization();
        }

        #region PrivateMethods
        /// <summary>
        /// Displays the currently playing song.
        /// </summary>
        private void DisplayPlayingSong()
        {
            if (humanActivity.Sleeping)
            {
                if (InvokeRequired)
                {
                    Invoke(new VoidDelegate(ShowPlayingSong));
                }
                else
                {
                    ShowPlayingSong();
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected music files within the play list.
        /// </summary>
        private MusicFile[] SelectedMusicFiles
        {
            get
            {
                List<MusicFile> result = new List<MusicFile>();
                for (int i = 0; i < lbMusic.Items.Count; i++)
                {
                    if (lbMusic.SelectedIndices.Contains(i))
                    {
                        result.Add((MusicFile)lbMusic.Items[i]);
                    }
                }

                return result.ToArray();
            }
        }

        private void SelectMusicFiles(params MusicFile[] musicFiles)
        {
            //lbMusic.SelectedIndices.
            foreach (var musicFile in musicFiles)
            {
                for (int i = 0; i < lbMusic.Items.Count; i++)
                {
                    if (((MusicFile)lbMusic.Items[i]).ID == musicFile.ID)
                    {
                        if (!lbMusic.SelectedIndices.Contains(i))
                        {
                            lbMusic.SelectedIndices.Add(i);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the visual indices of the current playlist within the playlist box.
        /// </summary>
        private void ReIndexVisual()
        {
            int iCount = 0;
            foreach (MusicFile mf in PlayList)
            {
                mf.VisualIndex = iCount++;
            }
        }

        /// <summary>
        /// Gets the current count of songs in the queue.
        /// </summary>
        /// <returns>The current count of songs in the queue.</returns>
        private int GetQueueCountNum()
        {
            return PlayList.Count(f => f.QueueIndex > 0);
        }

        /// <summary>
        /// Updates the status strip text with the current count of songs in the queue.
        /// </summary>
        private void GetQueueCount()
        {
            lbQueueCount.Text = DBLangEngine.GetMessage("msgInQueue", "In queue: {0}|How many songs are in the queue", GetQueueCountNum());
        }

        /// <summary>
        /// Updates the current playback volume to the GUI.
        /// </summary>
        private void UpdateVolume()
        {
            pnVol2.Left = (int)(MFile.Volume * 50F);
        }


        /// <summary>
        /// Checks for new version of the application.
        /// </summary>
        private void CheckForNewVersion()
        {
            // no going to the internet if the user doesn't allow it..
            if (AutoCheckUpdates)
            {
                FormCheckVersion.CheckForNewVersion("https://www.vpksoft.net/versions/version.php",
                    Assembly.GetEntryAssembly(), UtilityClasses.Settings.Settings.Culture.Name);
            }
        }

        // the play/pause toggle to call within the main form..
        private void TogglePause()
        {
            if (waveOutDevice != null)
            {
                if (waveOutDevice.PlaybackState == PlaybackState.Paused)
                {
                    tbPlayNext.Image = Resources.amp_pause;
                    tbPlayNext.ToolTipText = DBLangEngine.GetMessage("msgPause", "Pause|Pause playback");
                    waveOutDevice.Play();
                    ResetAudioVisualizationBars();
                }
                else if (waveOutDevice.PlaybackState == PlaybackState.Playing)
                {
                    tbPlayNext.Image = Resources.amp_play;
                    tbPlayNext.ToolTipText = DBLangEngine.GetMessage("msgPlay", "Play|Play a song or resume paused");
                    waveOutDevice.Pause();
                }
            }
            else
            {
                humanActivity.Stop();
                tbPlayNext.Image = Resources.amp_pause;
                tbPlayNext.ToolTipText = DBLangEngine.GetMessage("msgPause", "Pause|Pause playback");
                GetNextSong();
            }
        }

        /// <summary>
        /// Updates the current song rating to the GUI.
        /// </summary>
        private void UpdateStars()
        {
            pnStars1.Left = (int)(MFile.Rating / 1000.0 * 176.0);
        }

        /// <summary>
        /// Gets an album with a given name and lists it to the playlist.
        /// </summary>
        /// <param name="name">The name of the album.</param>
        /// <param name="usePsycho">A value indicating whether to use the progress dialog while loading the album to the playlist.</param>
        private void GetAlbum(string name, bool usePsycho = true)
        {
            AlbumLoading = true;
            if (usePsycho)
            {
                FormPsycho.Execute(this);
                FormPsycho.SetStatusText(DBLangEngine.GetMessage("msgLoadingAlbum", "Loading album '{0}'...|Text for loading an album (enumerating files and their tags)", name));
            }
            Database.GetAlbum(name, ref PlayList, Connection);
            CurrentAlbum = name;
            if (name == "tmp")
            {
                Text = @"amp#" + (QuietHours && FormSettings.IsQuietHour() ? " " + DBLangEngine.GetMessage("msgQuietHours", "[Quiet hours ({0} - {1})]|As in quiet hours defined in the settings are occurring now :-(", QuietHoursFrom, QuietHoursTo) : string.Empty); 
            }
            else
            {
                Text = @"amp# - " + CurrentAlbum + (QuietHours && FormSettings.IsQuietHour() ? " " + DBLangEngine.GetMessage("msgQuietHours", "[Quiet hours ({0} - {1})]|As in quiet hours defined in the settings are occurring now :-(", QuietHoursFrom, QuietHoursTo) : string.Empty); 
            }

            lbMusic.Items.Clear(); // LOCATION:NOT FILTERED

            if (usePsycho)
            {
                foreach (MusicFile mf in PlayList)
                {
                    FormPsycho.SetStatusText(mf.GetFileName());
                }
            }

            if (PlayList != null)
            {
                // ReSharper disable once CoVariantArrayConversion
                lbMusic.Items.AddRange(PlayList.ToArray());
            }
            GetQueueCount();
            if (usePsycho)
            {
                FormPsycho.UnExecute();
            }
            Filtered = FilterType.NoneFiltered;
            AlbumLoading = false;
            albumChanged = true;
        }

        /// <summary>
        /// Updates the time of how many times the file has been played via randomization to the program database.
        /// </summary>
        /// <param name="mf">A <see cref="MusicFile"/> class instance to update to the database.</param>
        /// <param name="skipped">A value indicating whether the song was skipped; I.e. less than 15 percent played.</param>
        private void UpdateRPlayed(MusicFile mf, bool skipped)
        {
            if (mf == null)
            {
                return;
            }

            int mfIdx = PlayList.FindIndex(f => f.ID == mf.ID);
            if (mfIdx != -1)
            {
                PlayList[mfIdx].NPLAYED_RAND++;
                PlayList[mfIdx].SKIPPED_EARLY += skipped ? 1 : 0;
            }

            using (SQLiteCommand command = new SQLiteCommand(Connection))
            {
                command.CommandText = "UPDATE SONG SET NPLAYED_RAND = IFNULL(NPLAYED_RAND, 0) + 1, SKIPPED_EARLY = IFNULL(SKIPPED_EARLY, 0) + " + (skipped ? "1" : "0") + " WHERE ID = " + mf.ID + " ";
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Adds a list of files to the album.
        /// </summary>
        /// <param name="musicFiles">A list of file names to add.</param>
        /// <param name="usePsycho">A value indicating whether to use a funny-named "progress" dialog while adding the files.</param>
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
                if (Constants.Extensions.Contains(Path.GetExtension(filePath)?.ToUpper()))
                {
                    if (!File.Exists(filePath))
                    {
                        continue;
                    }

                    var mf = new MusicFile(filePath);
                    addList.Add(mf);
                }
            }
            Database.AddFileToDb(addList, Connection);

            Database.GetIDsForSongs(ref addList, Connection);
            Database.AddSongToAlbum(CurrentAlbum, addList, Connection);
            foreach (MusicFile mf in addList)
            {
                lbMusic.Items.Add(mf);
                PlayList.Add(mf);
            }

            ReIndexVisual();

            lbMusic.ResumeLayout();
            if (usePsycho)
            {
                FormPsycho.UnExecute();
            }
            humanActivity.Enabled = true;
        }

        /// <summary>
        /// Stops the playback, cleans and disposes of the objects used for the playback.
        /// </summary>
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

                // dispose the main memory stream in case one is assigned..
                mainMemoryStream?.Dispose();

                mainMemoryStream = null;
                mainOutputStream = null;
            }
            if (waveOutDevice != null)
            {
                waveOutDevice.Dispose();
                waveOutDevice = null;
            }
        }

        /// <summary>
        /// Plays a song with a given index.
        /// </summary>
        /// <param name="index">The index of the song to play.</param>
        /// <param name="random">A value indicating whether the <paramref name="index"/> was gotten by randomizing.</param>
        /// <param name="addPlayedSong">A value indicating whether to add this song to the played song list.</param>
        private void PlaySong(int index, bool random, bool addPlayedSong = true)
        {
            if (random)
            {
                UpdateRPlayed(MFile, Skipped);
            }
            else
            {
                UpdateNPlayed(MFile, Skipped);
            }

            MFile = PlayList[index];
            if (addPlayedSong)
            {
                playedSongs.Add(index);
            }

            if (random)
            {
                UpdateRPlayed(MFile, false);
            }
            else
            {
                UpdateNPlayed(MFile, false);
            }

            newSong = true;
            DisplayPlayingSong();
        }

        /// <summary>
        /// Gets a value whether the playlist box should be refreshed.
        /// </summary>
        /// <param name="mf">A <see cref="MusicFile"/> class instance to use for comparison.</param>
        /// <returns>True if the playlist box should be refreshed; otherwise false.</returns>
        private bool ShouldRefreshList(MusicFile mf)
        {
            if (mf == null)
            {
                return false;
            }
            List<MusicFile> currentFiles = lbMusic.Items.Cast<MusicFile>().ToList();
            return !currentFiles.Exists(f => f.ID == mf.ID);
        }

        /// <summary>
        /// Displays the currently playing song and refreshes the playlist box if necessary.
        /// </summary>
        private void ShowPlayingSong()
        {
            for (int i = 0; i < lbMusic.Items.Count; i++ )
            {
                if (MFile != null && MFile.ID == ((MusicFile) lbMusic.Items[i]).ID)
                {
                    lbMusic.SetIndex(i);
                    return;
                }
            }

            if (ShouldRefreshList(MFile)) // only do this "jump" if the list is filtered..
            {
                if (Filtered == FilterType.QueueFiltered && QueueCount > 0)
                {
                    ShowQueue();
                }
                else
                {
                    ShowAllSongs();
                }
            }

            for (int i = 0; i < lbMusic.Items.Count; i++)
            {
                if (MFile != null && MFile.ID == ((MusicFile) lbMusic.Items[i]).ID)
                {
                    lbMusic.SetIndex(i);
                    return;
                }
            }
        }

        /// <summary>
        /// List all to songs within the album to the playlist box.
        /// </summary>
        private void ShowAllSongs()
        {
            lbMusic.Items.Clear();
            foreach (MusicFile mf in PlayList) // LOCATION:NOT FILTERED
            {
                lbMusic.Items.Add(mf);
            }
            Filtered = FilterType.NoneFiltered;
        }

        /// <summary>
        /// A method for the playback thread.
        /// </summary>
        private void PlayerThread()
        {
            var previousPaused = waveOutDevice?.PlaybackState == PlaybackState.Paused;
            // prevent total sleep/hibernate mode of the system..

            try
            {

                ThreadExecutionState.SetThreadExecutionState(
                    EsFlags.Continuous | EsFlags.SystemRequired | EsFlags.AwayModeRequired);
                while (!stopped)
                {
                    if (previousPaused != (waveOutDevice?.PlaybackState == PlaybackState.Paused))
                    {
                        // prevent total sleep/hibernate mode of the system..
                        if (previousPaused)
                        {
                            ThreadExecutionState.SetThreadExecutionState(
                                EsFlags.Continuous | EsFlags.SystemRequired | EsFlags.AwayModeRequired);
                        }
                        else
                        {
                            // on pause allow total sleep/hibernate mode of the system..
                            ThreadExecutionState.SetThreadExecutionState(EsFlags.Continuous);
                        }

                        previousPaused = waveOutDevice?.PlaybackState == PlaybackState.Paused;
                    }

                    if (MFile != null)
                    {
                        if (!playing || newSong)
                        {
                            CloseWaveOut();
                            waveOutDevice = new WaveOut
                            {
                                DesiredLatency = LatencyMs, 
                            };
                            try
                            {
                                var (waveStream, memoryStream) = CreateInputStream(MFile.GetFileName());
                                mainOutputStream = waveStream;
                                mainMemoryStream = memoryStream;
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

                            SecondsTotal = mainOutputStream.TotalTime.TotalSeconds;

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
                            ResetAudioVisualizationBars();
                            if (FormSettings.IsQuietHour() && !QuietHoursPause)
                            {
                                volumeStream.Volume = MFile.Volume * (float) QuietHoursVolPercentage;
                            }
                            else
                            {
                                volumeStream.Volume = MFile.Volume;
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
                            newSong = false;
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
                    // ReSharper disable once NonAtomicCompoundOperator, I don't care..
                    calcMs++; // 100 ms * 10 == second, lets make it ten seconds so 10 * 10 = 100;


                    if (mainOutputStream == null)
                    {
                        Seconds = 0;
                        SecondsTotal = 0;
                    }
                    else
                    {
                        Seconds = mainOutputStream.CurrentTime.TotalSeconds;
                    }

                    playerThreadLoaded = true;
                }

                CloseWaveOut();
                // on thread stop allow total sleep/hibernate mode of the system..
                ThreadExecutionState.SetThreadExecutionState(EsFlags.Continuous);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Resets the audio visualization on the bar audio graph thread-safely.
        /// </summary>
        private void ResetAudioVisualizationBars()
        {
            if (!BalancedBars)
            {
                return;
            }

            Thread.Sleep(150); // wait for the playback to stabilize before resetting the view..
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() =>
                {
                    avBars.ResetRelativeView();
                }));
            }
            else
            {
                avBars.ResetRelativeView();
            }
        }

        /// <summary>
        /// Displays the main form title. From thread call with Invoke method; otherwise direct call.
        /// </summary>
        private void TextInvoker()
        {
            if (CurrentAlbum == "tmp")
            {
                Text = @"amp#" + (QuietHours && FormSettings.IsQuietHour()
                           ? " " + DBLangEngine.GetMessage("msgQuietHours",
                                 "[Quiet hours ({0} - {1})]|As in quiet hours defined in the settings are occurring now :-(",
                                 QuietHoursFrom, QuietHoursTo)
                           : string.Empty);
            }
            else
            {
                Text = @"amp# - " + CurrentAlbum + (QuietHours && FormSettings.IsQuietHour()
                           ? " " + DBLangEngine.GetMessage("msgQuietHours",
                                 "[Quiet hours ({0} - {1})]|As in quiet hours defined in the settings are occurring now :-(",
                                 QuietHoursFrom, QuietHoursTo)
                           : string.Empty);
            }
        }

        /// <summary>
        /// Creates a <see cref="NAudio.Wave.WaveStream"/> class instance from a give <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">A file name for a music file to create the <see cref="NAudio.Wave.WaveStream"/> for.</param>
        /// <returns>An instance to the <see cref="NAudio.Wave.WaveStream"/> class if the operation was successful; otherwise false.</returns>
        private (WaveStream waveStream, MemoryStream memoryStream) CreateInputStream(string fileName)
        {
            try
            {
                WaveChannel32 inputStream;

                MemoryStream memoryStream = null;

                try
                {
                    if (UtilityClasses.Settings.Settings.LoadEntireFileSizeLimit > 0 &&
                        UtilityClasses.Settings.Settings.LoadEntireFileSizeLimit * 1000000 > new FileInfo(fileName).Length)
                    {
                        // load the entire file into the memory..
                        memoryStream = new MemoryStream(File.ReadAllBytes(fileName));
                    }
                }
                catch (Exception ex)
                {
                    // log the exception..
                    ExceptionLogger.LogError(ex);
                    memoryStream = null;
                }
                
                // determine the file type by it's extension..
                if (Constants.FileIsMp3(fileName))
                {
                    Mp3FileReader fr = memoryStream != null
                        ? new Mp3FileReader(memoryStream)
                        : new Mp3FileReader(fileName);

                    WaveStream mp3Reader = fr;
                    inputStream = new WaveChannel32(mp3Reader);
                }
                else if (Constants.FileIsOgg(fileName))
                {
                    VorbisWaveReader fr = memoryStream != null
                        ? new VorbisWaveReader(memoryStream)
                        : new VorbisWaveReader(fileName);

                    WaveStream oggReader = fr;
                    inputStream = new WaveChannel32(oggReader);
                }
                else if (Constants.FileIsWav(fileName))
                {
                    WaveFileReader fr = memoryStream != null
                        ? new WaveFileReader(memoryStream)
                        : new WaveFileReader(fileName);

                    WaveStream wavReader = fr;
                    inputStream = new WaveChannel32(wavReader);
                }
                else if (Constants.FileIsFlac(fileName))
                {
                    FlacReader fr = memoryStream != null ? 
                        new FlacReader(memoryStream) : 
                        new FlacReader(fileName);

                    WaveStream wavReader = fr;
                    inputStream = new WaveChannel32(wavReader);
                }
                else if (Constants.FileIsWma(fileName))
                {
                    // now stream constructor on this one..
                    memoryStream?.Dispose();
                    memoryStream = null;

                    WMAFileReader fr = new WMAFileReader(fileName);

                    WaveStream wavReader = fr;
                    inputStream = new WaveChannel32(wavReader);
                }
                else if (Constants.FileIsAacOrM4A(fileName)) // Added: 01.02.2018
                {
                    // now stream constructor on this one..
                    memoryStream?.Dispose();
                    memoryStream = null;

                    MediaFoundationReader fr = new MediaFoundationReader(fileName);
                    WaveStream wavReader = fr;
                    inputStream = new WaveChannel32(wavReader);
                }
                else if (Constants.FileIsAif(fileName)) // Added: 01.02.2018
                {
                    AiffFileReader fr = memoryStream != null
                        ? new AiffFileReader(memoryStream)
                        : new AiffFileReader(fileName);

                    WaveStream wavReader = fr;
                    inputStream = new WaveChannel32(wavReader);
                }
                else // throw for catching furthermore in the code..
                {
                    throw new InvalidOperationException(DBLangEngine.GetMessage("msgUnsupportedExt", "Unsupported file extension.|The file extension is not in the list of supported file types."));
                }

                inputStream.PadWithZeroes = false;
                volumeStream = inputStream;

                // if successful, return the WaveChannel32 instance..
                return (volumeStream, memoryStream);
            }
            catch (Exception ex)
            {
                try 
                {
                    // log the exception..
                    ExceptionLogger.LogError(ex);

                    // try to get the next song..
                    GetNextSong(true);
                }
                catch (Exception ex2)
                {
                    // log the exception..
                    ExceptionLogger.LogError(ex2);

                    // try recursion to create a WaveChannel32 instance..
                    return CreateInputStream(fileName);
                }
            }

            // eek! - failure..
            return default;
        }

        /// <summary>
        /// Displays the currently playing song and a possible album image.
        /// </summary>
        private void UpdateSongName()
        {
            lbSong.Text = MFile.SongName;

            FormAlbumImage.Show(this, MFile, tbFind.PointToScreen(Point.Empty).Y);
            DisplayPlayingSong();
        }

        /// <summary>
        /// A call to either invoke or call directly to refresh the main song list box from a thread. 
        /// </summary>
        private void RefreshListboxFromThread()
        {
            lbMusic.RefreshItems();
        }

        /// <summary>
        /// A call to pause the playback to either be invoked or called directly. 
        /// </summary>
        private void PauseInvoker()
        {
            if (waveOutDevice != null)
            {
                if (waveOutDevice.PlaybackState == PlaybackState.Playing)
                {
                    lastPaused = true;
                    tbPlayNext.Image = Resources.amp_play;
                    tbPlayNext.ToolTipText = DBLangEngine.GetMessage("msgPlay", "Play|Play a song or resume paused");
                    waveOutDevice.Pause();
                }
            }
            TextInvoker();
        }

        /// <summary>
        /// Starts the playback. From thread call with Invoke method; otherwise direct call.
        /// </summary>
        private void PlayInvoker()
        {
            if (waveOutDevice != null && lastPaused)
            {
                if (waveOutDevice.PlaybackState == PlaybackState.Paused)
                {
                    tbPlayNext.Image = Resources.amp_pause;
                    tbPlayNext.ToolTipText = DBLangEngine.GetMessage("msgPause", "Pause|Pause playback");
                    waveOutDevice.Play();
                    ResetAudioVisualizationBars();
                    lastPaused = false;
                }
            }
            TextInvoker();
            lastPaused = false;
        }

        /// <summary>
        /// Sets the playback button to indicate pause. From thread call with Invoke method; otherwise direct call.
        /// </summary>
        private void SetPause()
        {
            tbPlayNext.Image = Resources.amp_pause;
            tbPlayNext.ToolTipText = DBLangEngine.GetMessage("msgPause", "Pause|Pause playback");
        }

        /// <summary>
        /// Sets the audio visualization based on the settings.
        /// </summary>
        private void SetAudioVisualization()
        {
            // set the audio visualization panel row style if used..
            tlpMain.RowStyles[5].Height = AudioVisualizationStyle == 0 ? 0 : AudioVisualizationVisualPercentage;
            tlpMain.RowStyles[4].Height = AudioVisualizationStyle == 0 ? 100 : 100 - AudioVisualizationVisualPercentage;

            if (AudioVisualizationStyle == 0)
            {
                avBars.Visible = false;
                avLine.Visible = false;
            }
            else if (AudioVisualizationStyle == 1)
            {
                avBars.Visible = true;
                avLine.Visible = false;
                avBars.Dock = DockStyle.Fill;
                avBars.Start();
                avLine.Stop();
                avBars.CombineChannels = AudioVisualizationCombineChannels;
                avBars.RelativeView = BalancedBars;
                avBars.HertzSpan = BarAmount;
            }
            else if (AudioVisualizationStyle == 2)
            {
                avBars.Visible = false;
                avLine.Visible = true;
                avLine.Dock = DockStyle.Fill;
                avLine.Start();
                avBars.Stop();
                avLine.CombineChannels = AudioVisualizationCombineChannels;
            }
        }

        /// <summary>
        /// Checks the software arguments whether music files are passed as arguments to the software.
        /// </summary>
        private void CheckArguments()
        {
            var args = Environment.GetCommandLineArgs();

            for (int i = 1; i < args.Length; i++)
            {
                string file = args[i];
                        
                ExceptionLogger.LogMessage($"Request file open: '{file}'.");
                if (File.Exists(file))
                {
                    ExceptionLogger.LogMessage($"File exists: '{file}'. Send open request.");
                    OpenFileToTemporaryAlbum(file);
                }
            }
        }

        /// <summary>
        /// Opens a given file to the temporary album.
        /// </summary>
        /// <param name="file">The file name to add.</param>
        private void OpenFileToTemporaryAlbum(string file)
        {
            if (CurrentAlbum != "tmp")
            {
                CurrentAlbum = "tmp";
                Database.ClearTmpAlbum(ref PlayList, Connection);
                tbShuffle.Checked = true;
                tbRand.Checked = false;
                tbFind.Text = string.Empty;
            }
            CurrentAlbum = "tmp";
            DoAddFileList(new List<string>(new [] {file}), false);

            GetAlbum(CurrentAlbum, false);
            ListAlbums(0);
        }
        #endregion

        #region Fields
        // a class monitoring if the user is idle..
        private HumanActivity humanActivity;

        /// <summary>
        /// The current playback position in seconds.
        /// </summary>
        public double Seconds;

        /// <summary>
        /// The current song's length in seconds.
        /// </summary>
        public double SecondsTotal;

        /// <summary>
        /// A flag indicating whether the playback is stopped.
        /// </summary>
        private volatile bool stopped;

        /// <summary>
        /// A flag indicating if a song is currently playing.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        internal volatile bool playing;

        /// <summary>
        /// A flag indicating whether a new song has been selected compared to previously playing song.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        internal volatile bool newSong;

        /// <summary>
        /// The latest song index which was played or is being played.
        /// </summary>
        private volatile int latestSongIndex = -1;

        /// <summary>
        /// A flag indicating if a next song should be selected with a call to the <see cref="GetNextSong(bool)"/> method.
        /// </summary>
        private bool pendNextSong; 

        /// <summary>
        /// A delegate to be used for calling <see cref="System.Windows.Forms.Control.Invoke(Delegate)"/> method for thread safety.
        /// </summary>
        private delegate void VoidDelegate();

        /// <summary>
        /// A general randomization class instance.
        /// </summary>
        internal Random Random = new Random();
        #endregion

        #region NAudioPlayBack
        /// <summary>
        /// The current <see cref="NAudio.Wave.WaveOut"/> class instance for the playback.
        /// </summary>
        private volatile WaveOut waveOutDevice;

        /// <summary>
        /// The current <see cref="NAudio.Wave.WaveStream"/> class instance for the playback.
        /// </summary>
        private volatile WaveStream mainOutputStream;

        /// <summary>
        /// The current <see cref="MemoryStream"/> used by the <see cref="NAudio.Wave.WaveStream"/> class instance in case the file is entirely loaded into the memory.
        /// </summary>
        private volatile MemoryStream mainMemoryStream;

        /// <summary>
        /// The current <see cref="NAudio.Wave.WaveChannel32"/> class instance for the playback.
        /// </summary>
        private volatile WaveChannel32 volumeStream;
        #endregion

        #region InternalProperties
        /// <summary>
        /// Gets a value indicating whether the stack queue is enabled.
        /// </summary>
        /// <value><c>true</c> if stack queue is enabled; otherwise, <c>false</c>.</value>
        internal bool StackQueueEnabled => tsbQueueStack.Checked;

        /// <summary>
        /// Gets the value whether a playback is considered as skipped; Only 15 percentage of the song was played.
        /// </summary>
        internal bool Skipped
        {
            get
            {
                double percentagePlayed;
                if (mainOutputStream != null)
                {
                    try
                    {
                        percentagePlayed = 100.0 - ((mainOutputStream.TotalTime - mainOutputStream.CurrentTime).TotalSeconds / mainOutputStream.TotalTime.TotalSeconds * 100.0);
                    }
                    catch
                    {
                        percentagePlayed = 100;
                    }
                }
                else
                {
                    percentagePlayed = 100;
                }

                return percentagePlayed < 15.0;
            }
        }
        

        #endregion

        #region PrivateProperties
        /// <summary>
        /// Gets the count of currently queued songs.
        /// </summary>
        private int QueueCount
        {
            get
            {
                return PlayList.Count(f => f.QueueIndex > 0);
            }
        }
        #endregion

        #region PublicMethods
        /// <summary>
        /// Scrambles the queue between the selected songs within the queue.
        /// </summary>
        /// <returns>True if any songs were affected; otherwise false.</returns>
        public bool ScrambleQueueSelected()
        {
            var selectedFiles = SelectedMusicFiles;
            humanActivity.Enabled = false;
            bool affected = MusicFile.ScrambleQueueSelected(SelectedMusicFiles); // if any songs in the play list was affected..

            if (affected)
            {
                ShowQueue();
                SelectMusicFiles(selectedFiles);
            }

            humanActivity.Enabled = true;
            return affected;
        }

        /// <summary>
        /// Moves the selected files to the top of the queue.
        /// </summary>
        /// <returns>True if any songs were affected; otherwise false.</returns>
        public bool MoveQueueTop()
        {
            var selectedFiles = SelectedMusicFiles;
            humanActivity.Enabled = false;
            bool affected = MusicFile.MoveQueueTop(ref PlayList, SelectedMusicFiles); // if any songs in the play list was affected..

            if (affected)
            {
                ShowQueue();
                SelectMusicFiles(selectedFiles);
            }

            humanActivity.Enabled = true;
            return affected;
        }

        /// <summary>
        /// Scrambles the queue to have new random indices.
        /// </summary>
        /// <returns>True if any songs were affected; otherwise false.</returns>
        public bool ScrambleQueue()
        {
            humanActivity.Enabled = false;
            bool affected = MusicFile.ScrambleQueue(ref PlayList); // if any songs in the play list was affected..

            if (affected)
            {
                ShowQueue();
            }
            humanActivity.Enabled = true;
            return affected;
        }

        /// <summary>
        /// Sets a user given rating for a song to the database.
        /// </summary>
        /// <param name="mf">A <see cref="MusicFile"/> class instance to update to the database.</param>
        public void SaveRating(MusicFile mf)
        {
            if (mf == null)
            {
                return;
            }

            if (mf.RatingChanged)
            {
                int mfIdx = PlayList.FindIndex(f => f.ID == mf.ID);
                if (mfIdx != -1)
                {
                    PlayList[mfIdx].Rating = mf.Rating;
                }

                using (SQLiteCommand command = new SQLiteCommand(Connection))
                {
                    command.CommandText = $"UPDATE SONG SET RATING = {mf.Rating} WHERE ID = {mf.ID} ";
                    command.ExecuteNonQuery();
                    mf.RatingChanged = false;
                }
            }
        }

        /// <summary>
        /// Plays the previous song.
        /// </summary>
        public void GetPrevSong()
        {
            if (playedSongs.Count < 2)
            {
            }
            else
            {
                int tmpInt = playedSongs[playedSongs.Count - 2];
                playedSongs.RemoveAt(playedSongs.Count - 1);
                PlaySong(tmpInt, false, false);
            }
        }

        /// <summary>
        /// Updates the time of how many times the file has been played via user selection to the program database.
        /// </summary>
        /// <param name="mf">A <see cref="MusicFile"/> class instance to update to the database.</param>
        /// <param name="skipped">A value indicating whether the song was skipped; I.e. less than 15 percent played.</param>
        public void UpdateNPlayed(MusicFile mf, bool skipped)
        {
            if (mf == null)
            {
                return;
            }

            int mfIdx = PlayList.FindIndex(f => f.ID == mf.ID);
            if (mfIdx != -1)
            {
                PlayList[mfIdx].NPLAYED_RAND++;
                PlayList[mfIdx].SKIPPED_EARLY += skipped ? 1 : 0;
            }

            using (SQLiteCommand command = new SQLiteCommand(Connection))
            {
                command.CommandText =
                    $"UPDATE SONG SET NPLAYED_USER = IFNULL(NPLAYED_USER, 0) + 1, SKIPPED_EARLY = IFNULL(SKIPPED_EARLY, 0) + {(skipped ? "1" : "0")} WHERE ID = {mf.ID} ";
                command.ExecuteNonQuery();
            }
        }
        #endregion

        // File drag drop operation hangs the Windows Explorer (the event duration) so do it in a thread..
        #region DragDropThread
        // the thread to handle the dropped files and/or directories..
        private Thread fileAddThread;

        // the file list to be added to the current album..
        private List<string> fileAddList;

        /// <summary>
        /// A flag indicating whether a thread is currently adding songs to a an album (a lengthy operation).
        /// </summary>
        private volatile bool addFiles;

        // a synchronization context for the ThreadFilesAdd thread..
        private SynchronizationContext context;

        /// <summary>
        /// Lists a given files from a call via a <see cref="SynchronizationContext"/> instance.
        /// </summary>
        /// <param name="state">A list of MusicFile class instances.</param>
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
            Database.SaveQueue(PlayList, Connection, CurrentAlbum);
            GetAlbum(CurrentAlbum, false);
            FormPsycho.UnExecute();
            Enabled = true;
        }

        /// <summary>
        /// Starts a file add thread after a file drop operation.
        /// </summary>
        private void StartFileAddThread()
        {
            if (fileAddThread != null)
            {
                while (!fileAddThread.Join(1000))
                {
                    Application.DoEvents();
                }
                fileAddThread = null;
            }
            Enabled = false;
            FormPsycho.Execute(this);
            FormPsycho.SetStatusText(DBLangEngine.GetMessage("msgWorking", "Working...|The program is loading something"));
            context = SynchronizationContext.Current ?? new SynchronizationContext();
            fileAddThread = new Thread(ThreadFilesAdd);
            fileAddThread.Start();
        }

        /// <summary>
        /// A thread method for the add files via drag &amp; drop.
        /// </summary>
        private void ThreadFilesAdd()
        {
            if (addFiles)
            {
                humanActivity.Enabled = false;
                List<MusicFile> addList = new List<MusicFile>();
                foreach (string filePath in fileAddList)
                {
                    if (Constants.Extensions.Contains(Path.GetExtension(filePath)?.ToUpper()))
                    {
                        if (!File.Exists(filePath))
                        {
                            continue;
                        }

                        var mf = new MusicFile(filePath);
                        addList.Add(mf);
                    }
                }

                Database.AddFileToDb(addList, Connection);

                Database.GetIDsForSongs(ref addList, Connection);
                Database.AddSongToAlbum(CurrentAlbum, addList, Connection);
                context.Send(ListFiles, addList);
                humanActivity.Enabled = true;
            }
            addFiles = false;
        }
        #endregion

        #region InternalMethods
        /// <summary>
        /// Displays the queued songs within the playlist.
        /// </summary>
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

            Filtered = FilterType.QueueFiltered;
        }

        /// <summary>
        /// Displays the alternate queue within the playlist.
        /// </summary>
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
                Filtered = FilterType.AlternateFiltered;
            }));
        }
        #endregion

        #region InternalEvents
        // a user is dragging files and/or directories to the software..
        private void lbMusic_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        // a user is dropped files and/or directories to the software, so handle it..
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

        /// <summary>
        /// Gets the status message for the progress dialog in case of the <see cref="Database.DatabaseProgress"/> event.
        /// </summary>
        /// <param name="e">A DatabaseEventArgs instance containing the event data.</param>
        void Database_DatabaseProgress(DatabaseEventArgs e)
        {
            if (e.EventType == DatabaseEventType.UpdateSongDb)
            {                
                FormPsycho.SetStatusText(DBLangEngine.GetMessage("msgUpdateDB", "Updating song list: {0} / {1}...|A conditional database update is in progress.", e.Progress, e.ProgressEnd));
            }
            else if (e.EventType == DatabaseEventType.InsertSongDb)
            {
                FormPsycho.SetStatusText(DBLangEngine.GetMessage("msgAddDB", "Adding songs: {0} / {1}...|A conditional database add is in progress.", e.Progress, e.ProgressEnd));
            }
            else if (e.EventType == DatabaseEventType.InsertSongAlbum)
            {
                FormPsycho.SetStatusText(DBLangEngine.GetMessage("msgAddDBAlbum", "Adding songs to album: {0} / {1}...|A conditional database album add is in progress.", e.Progress, e.ProgressEnd));
            }
            else if (e.EventType == DatabaseEventType.GetSongId)
            {
                FormPsycho.SetStatusText(DBLangEngine.GetMessage("msgIDSong", "Identifying songs: {0} / {1}...|Songs are identified based on the database song data.", e.Progress, e.ProgressEnd));
            }
            else if (e.EventType == DatabaseEventType.LoadMeta)
            {
                FormPsycho.SetStatusText(DBLangEngine.GetMessage("msgLoadMeta", "Metadata loading: {0} / {1}...|Song metadata(tags) is being loaded.", e.Progress, e.ProgressEnd));
            }
        }

        // a user is dragging files and/or directories over the playlist..
        private void lbMusic_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        // sets the scroll bar value indicating the current playback position..
        private void tmSeek_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!progressUpdating)
                {
                    scProgress.Maximum = (int)SecondsTotal;
                    scProgress.Value = (int)Seconds;
                    TimeSpan ts = TimeSpan.FromSeconds(SecondsTotal - Seconds);
                    lbTime.Text = @"-" + ts.ToString(@"mm\:ss");
                }
            }
            catch
            {
                // ignored..
            }
        }

        // a user double-clicked a file in the playlist, so do play it..
        private void lbMusic_DoubleClick(object sender, EventArgs e)
        {
            if (lbMusic.SelectedItem != null)
            {
                PlaySong(((MusicFile) lbMusic.SelectedItem).VisualIndex, false);
            }
        }

        // the main form is closing, so dispose of the used data and join the threads..
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            tmIPCFiles.Enabled = false;
            tmSeek.Stop();
            tmPendOperation.Stop();
            CloseWaveOut();
            stopped = true;
            while (!thread.Join(1000))
            {
                Application.DoEvents();
            }

            if (!RestartRequired)
            {
                Database.SaveQueue(PlayList, Connection, CurrentAlbum);
            }

            using (Connection)
            {
                Connection.Close();
            }
        }

        // handles the playback stopped event..
        void waveOutDevice_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            GetNextSong(true);
        }

        // the search text was changed, so do the search thing..
        private void tbFind_TextChanged(object sender, EventArgs e)
        {
            Find();
        }

        // handles the key down events within the search text box to avoid a focus change..
        private void tbFind_KeyDown(object sender, KeyEventArgs e)
        {
            // the media keys are handled in a separate method..
            if (HandleMediaKey(ref e)) 
            {
                return;
            }

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

        // the user wanted to play the next song, so do obey..
        private void tbNext_Click(object sender, EventArgs e)
        {
            humanActivity.Stop();
            tbPlayNext.Image = Resources.amp_pause;
            tbPlayNext.ToolTipText = DBLangEngine.GetMessage("msgPause", "Pause|Pause playback");
            GetNextSong();
        }

        // the user wanted to play the previous song, so do obey..
        private void tbPrevious_Click(object sender, EventArgs e)
        {
            humanActivity.Stop();
            tbPlayNext.Image = Resources.amp_pause;
            tbPlayNext.ToolTipText = DBLangEngine.GetMessage("msgPause", "Pause|Pause playback");
            GetPrevSong();
        }

        // a user wants to add a new album to the software..
        private void mnuNewAlbum_Click(object sender, EventArgs e)
        {
            string name =
                FormAddAlbum.Execute(DBLangEngine.GetMessage("msgNewAlbum",
                    "New album|A dialog title to add a new album"));
            if (name != string.Empty)
            {
                ListAlbums(Database.AddNewAlbum(name, Connection));
            }
        }

        // the user wants to play the next song, so do obey..
        private void tbPlayNext_Click(object sender, EventArgs e)
        {
            TogglePause();
        }

        // the main form is shown; enable few timers, update the database, possibly load the default album and create the necessary thread(s)..
        private void MainWindow_Shown(object sender, EventArgs e)
        {
            // ReSharper disable once StringLiteralTypo
            Connection = new SQLiteConnection("Data Source=" + DBLangEngine.DataDir + "amp.sqlite;Pooling=true;FailIfMissing=false;Cache Size=10000;"); // PRAGMA synchronous=OFF;PRAGMA journal_mode=OFF
            Connection.Open();

            if (!ScriptRunner.RunScript(Path.Combine(DBLangEngine.DataDir, "amp.sqlite"), Path.Combine(Paths.AppInstallDir, "SQLiteDatabase", "Script.sql_script")))
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
            Database.AddDefaultAlbum(DBLangEngine.GetMessage("msgDefault", "Default|Default as in default album"), Connection);

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo.GetVersionInfo(assembly.Location);

            thread = new Thread(PlayerThread);
            thread.Start();
            while (!playerThreadLoaded)
            {
                Thread.Sleep(500);
            }

            ListAlbums(1);

            CheckArguments();

            if (CurrentAlbum != "tmp" && RemoteFiles.Count == 0)
            {
                GetAlbum(CurrentAlbum);
            }

            albumChanged = false;
            remote.InitAmpRemote();

            // check for a new version from the internet..
            CheckForNewVersion();

            tmIPCFiles.Enabled = true;
        }

        // a user scrolls the song playback position; set the position to the user given value..
        private void scProgress_Scroll(object sender, ScrollEventArgs e)
        {
            tmSeek.Stop();
            mainOutputStream.CurrentTime = new TimeSpan(0, 0, e.NewValue);
            tmSeek.Start();
        }

        // a user changes the per-song based volume; set the volume and update it to the database..
        private void pnStars0_MouseClick(object sender, MouseEventArgs e)
        {
            if (MFile != null || lbMusic.SelectedIndices.Count > 0)
            {
                int stars = 0;
                if (sender == pnStars0)
                {
                    stars = (int)(500 * (e.X / 88.0));
                    pnStars1.Left = e.X;
                }
                else if (sender == pnStars1)
                {
                    if (pnStars1 != null)
                    {
                        pnStars1.Left += e.X;
                        stars = (int) (500 * (pnStars1.Left / 88.0));
                    }
                }

                if (MFile != null)
                {
                    MFile.Rating = stars;
                    MFile.RatingChanged = true;
                    SaveRating(MFile);
                }

                for (int i = 0; i < lbMusic.SelectedIndices.Count; i++)
                {
                    MusicFile mf = (MusicFile)lbMusic.Items[lbMusic.SelectedIndices[i]];
                    mf.Rating = stars;
                    mf.RatingChanged = true;
                    SaveRating(mf);
                    lbMusic.Items[lbMusic.SelectedIndices[i]] = mf;
                }
            }
        }

        // a user wants to see only queued songs..
        private void tbShowQueue_Click(object sender, EventArgs e)
        {
            ShowQueue();
        }

        // a user wants to clear the queue..
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

        // a user wants to scramble the current queue..
        private void mnuScrambleQueue_Click(object sender, EventArgs e)
        {
            ScrambleQueue(); // scramble the queue to have new random indices..
        }

        // a user wants to select all songs within the playlist box..
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

        // the main form has loaded it self; start the user idle monitor..
        private void MainWindow_Load(object sender, EventArgs e)
        {
            humanActivity = new HumanActivity(15);
            humanActivity.UserSleep += humanActivity_OnUserSleep;
        }

        // the user is idle; update the GUI..
        void humanActivity_OnUserSleep(object sender, UserSleepEventArgs e)
        {
            DisplayPlayingSong();
        }

        // opens a m3u/m3u8 playlist file and adds the songs within the playlist a new user given album..
        private void mnuPlayListM3UNewAlbum_Click(object sender, EventArgs e)
        {
            if (odM3U.ShowDialog() == DialogResult.OK)
            {
                string name = FormAddAlbum.Execute(Path.GetFileNameWithoutExtension(odM3U.FileName));
                if (name != string.Empty)
                {
                    int albumIndex = Database.AddNewAlbum(name, Connection);
                    // ReSharper disable once InconsistentNaming
                    M3U m3u = new M3U(odM3U.FileName);
                    // ReSharper disable once InconsistentNaming
                    List<MusicFile> m3uAdd = new List<MusicFile>();
                    foreach(M3UEntry m in m3u.M3UFiles)
                    {
                        MusicFile addMusicFile = new MusicFile(m.FileName)
                        {
                            OverrideName = m.FileDesc
                        };
                        m3uAdd.Add(addMusicFile);
                    }

                    Database.AddFileToDb(m3uAdd, Connection);
                    Database.GetIDsForSongs(ref m3uAdd, Connection);
                    Database.AddSongToAlbum(name, m3uAdd, Connection);
                    ListAlbums(albumIndex);
                    GetAlbum(name);
                }
            }
        }

        // opens a m3u/m3u8 playlist file and adds the songs within the playlist to the current album..
        private void mnuPlayListM3UToCurrentAlbum_Click(object sender, EventArgs e)
        {
            if (odM3U.ShowDialog() == DialogResult.OK)
            {
                // ReSharper disable once InconsistentNaming
                M3U m3u = new M3U(odM3U.FileName);
                // ReSharper disable once InconsistentNaming
                List<MusicFile> m3uAdd = new List<MusicFile>();
                foreach (M3UEntry m in m3u.M3UFiles)
                {
                    MusicFile addMusicFile = new MusicFile(m.FileName)
                    {
                        OverrideName = m.FileDesc
                    };
                    m3uAdd.Add(addMusicFile);
                }
                Database.GetIDsForSongs(ref m3uAdd, Connection);
                Database.AddSongToAlbum(CurrentAlbum, m3uAdd, Connection);
                GetAlbum(CurrentAlbum);
            }
        }

        // saves the current album into a m3u/m3u8 playlist file..
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
                            // ReSharper disable once StringLiteralTypo
                            sw.WriteLine("#EXTM3U");
                            foreach (MusicFile mf in PlayList)
                            {
                                // ReSharper disable once StringLiteralTypo
                                sw.WriteLine("#EXTINF:" + mf.Duration + "," + ((mf.OverrideName == string.Empty) ? mf.ToString() : mf.OverrideName));
                                sw.WriteLine(mf.FullFileName);
                                sw.WriteLine();
                            }
                        }
                    }
                }
            }
        }

        // the search box was clicked; search if not empty..
        private void tbFind_Click(object sender, EventArgs e)
        {
            Find(true);
        }

        // the search box was focused; search if not empty..
        private void tbFind_Enter(object sender, EventArgs e)
        {
            Find(true);
        }

        // the user adjusts the volume of currently playing song; save the user given volume to the database..
        private void pnVol1_MouseClick(object sender, MouseEventArgs e)
        {
            if ((MFile != null && volumeStream != null) || lbMusic.SelectedIndices.Count > 0)
            {
                float volume = 1;
                if (sender == pnVol1)
                {
                    pnVol2.Left = e.X;
                    volume = 1.0F * (e.X / 50F);
                }
                else if (sender == pnVol2)
                {
                    if (pnVol2 != null)
                    {
                        pnVol2.Left += e.X;
                        volume = 1.0F * (pnVol2.Left / 50F);
                    }
                }

                if (volumeStream != null)
                {
                    volumeStream.Volume = volume;
                }

                if (MFile != null)
                {
                    if (volumeStream != null)
                    {
                        MFile.Volume = volumeStream.Volume;
                    }
                    Database.SaveVolume(MFile, Connection);
                }

                for (int i = 0; i < lbMusic.SelectedIndices.Count; i++)
                {
                    int idx = lbMusic.SelectedIndices[i];
                    int index = PlayList.FindIndex(f => f.ID == ((MusicFile)lbMusic.Items[idx]).ID);
                    if (index >= 0)
                    {
                        PlayList[index].Volume = volume;
                        lbMusic.Items[idx] = PlayList[index];
                        Database.SaveVolume(PlayList[index], Connection);
                    }
                }
            }
        }

        // gets the next song if the flag is set and no new files
        // are currently being added to the database..
        private void tmPendOperation_Tick(object sender, EventArgs e)
        {
            if (pendNextSong && !addFiles)
            {
                pendNextSong = false;
                GetNextSong(true);
            }
        }

        // displays the about dialog which also allows the
        // user to check for a new version of the software..
        private void mnuAbout_Click(object sender, EventArgs e)
        {
            // ReSharper disable once ObjectCreationAsStatement
            using (new FormAbout(this,  Assembly.GetEntryAssembly(), "MIT",
                "https://raw.githubusercontent.com/VPKSoft/amp/master/LICENSE",
                "https://www.vpksoft.net/versions/version.php")) { }
        }

        // reposition the album image if the main window is moved..
        private void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            FormAlbumImage.Reposition(this, tbFind.PointToScreen(Point.Empty).Y);
        }

        // saves the current queue snapshot into the database..
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
                Database.SaveQueueSnapshot(PlayList, Connection, CurrentAlbum, queueName);
            }

            // the alternate queue must go away..
            if (PlayList.Exists(f => f.AlternateQueueIndex >= 0))
            {
                foreach (var item in PlayList)
                {
                    item.AlternateQueueIndex = 0;
                }

                lbMusic.RefreshItems();
            }
        }

        // loads a queue snapshot from the database..
        private void mnuLoadQueue_Click(object sender, EventArgs e)
        {
            int qId = FormSavedQueues.Execute(CurrentAlbum, Connection, out bool append);
            if (qId != -1)
            {
                Database.LoadQueue(ref PlayList, Connection, qId, append);
            }
            lbMusic.RefreshItems();
            GetQueueCount();
        }

        // opens the settings dialog form..
        private void mnuSettings_Click(object sender, EventArgs e)
        {
            using (var formSettings = new FormSettings())
            {
                formSettings.ShowDialog();
            }

            if (RestartRequired)
            {
                Close();
            }
            else
            {
                lbMusic.RefreshItems(); // the naming might have been changed..
                TextInvoker();
                SetAudioVisualization();
            }
        }

        // displays information about the current song..
        private void mnuSongInfo_Click(object sender, EventArgs e)
        {
            if (lbMusic.SelectedItem != null)
            {
                MusicFile mf = lbMusic.SelectedItem as MusicFile;
                FormTagInfo.Execute(mf, this);
            }
        }

        // displays the alternate queue..
        private void mnuShowAlternateQueue_Click(object sender, EventArgs e)
        {
            ShowAlternateQueue();
        }

        // displays the help..
        private void mnuHelpItem_Click(object sender, EventArgs e)
        {
            FormHelp.ShowSingleton();
        }

        // displays all the songs in the current album..
        private void mnuShowAllSongs_Click(object sender, EventArgs e)
        {
            ShowAllSongs();
        }

        // the IPC may start pushing songs in to the static list
        // before any forms are created within the application, so
        // this timer processes the possible list of songs queued
        // to be added to the playlist via the shell context menu..
        private void TmIPCFiles_Tick(object sender, EventArgs e)
        {
            if (StopIpcTimer)
            {
                // songs are currently being pushed to the list..
                return; // ..so just return..
            }

            // stop the timer..
            tmIPCFiles.Enabled = false;

            // set the flag whether remotely pushed files are being processed within the timer..
            RemoteFileBeingProcessed = RemoteFiles.Count > 0;

            // open all the files in the list to a temporary album..
            foreach (var remoteFile in RemoteFiles)
            {
                OpenFileToTemporaryAlbum(remoteFile);
            }
            // clear the list as all the files are handled..
            RemoteFiles.Clear();

            // set the flag whether remotely pushed files are being processed within the timer to false..
            RemoteFileBeingProcessed = false;

            // restart the timer..
            tmIPCFiles.Enabled = true;
        }

        // saves the album as..
        private void MnuSaveAlbumAs_Click(object sender, EventArgs e)
        {
            string name = FormAddAlbum.Execute(DBLangEngine.GetMessage("msgSaveAlbumAs",
                "Save album as|A dialog title to save an existing album with a new name"));
            if (name != string.Empty)
            {
                int id;
                if ((id = Database.AddNewAlbum(name, Connection)) != -1)
                {
                    ListAlbums(id);
                    Database.AddSongToAlbum(name, PlayList, Connection);
                    CurrentAlbum = name;
                    GetAlbum(CurrentAlbum);
                }
            }
        }

        // deletes the current album with a confirmation query..
        private void MnuDeleteAlbum_Click(object sender, EventArgs e)
        {
            if (CurrentAlbum != "tmp" &&
                CurrentAlbum != Database.GetDefaultAlbumName(Connection))
            {
                if (MessageBox.Show(
                        DBLangEngine.GetMessage("msgQueryDeleteAlbum",
                            "Really delete album named: '{0}'?|A confirmation query for the user that a deletion of an album is intended.", CurrentAlbum),
                        DBLangEngine.GetMessage("msgConfirmation",
                            "Confirm|Used in a dialog title to ask for a confirmation to do something"),
                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) ==
                    DialogResult.Yes)
                {
                    Database.DeleteAlbum(CurrentAlbum, Connection);
                    ListAlbums();
                    GetAlbum(Database.GetDefaultAlbumName(Connection));
                }
            }
        }

        // enables/disables the delete current album menu item
        // based on the current album..
        private void MnuFile_DropDownOpening(object sender, EventArgs e)
        {
            mnuDeleteAlbum.Enabled = CurrentAlbum != "tmp" &&
                                     CurrentAlbum != Database.GetDefaultAlbumName(Connection);
        }

        // move the selected song to the top of the queue..
        private void mnuQueueMoveToTop_Click(object sender, EventArgs e)
        {
            MoveQueueTop();
        }

        // scramble the selected songs in the queue..
        private void mnuScrambleQueueSelected_Click(object sender, EventArgs e)
        {
            ScrambleQueueSelected();
        }

        // a user selected an album, so do open the album the user selected..
        private void SelectAlbumClick(object sender, EventArgs e)
        {
            List<Album> albums = Database.GetAlbums(Connection);
            foreach (Album album in albums)
            {
                ToolStripMenuItem item = (ToolStripMenuItem) sender;
                if (item != null && ((int)item.Tag == album.Id && album.AlbumName != CurrentAlbum))
                {
                    DisableChecks();
                    item.Checked = true;
                    Database.SaveQueue(PlayList, Connection, CurrentAlbum);
                    GetAlbum(album.AlbumName);
                    return;
                }
            }
        }
        #endregion
    }
}
