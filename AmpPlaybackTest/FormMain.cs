using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using amp.UtilityClasses;
using NAudio.Vorbis;
using NAudio.Wave;

namespace AmpPlaybackTest
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            playerThread = new Thread(PlayerThreadSimulation);
            playerThread.Start();
        }

        private readonly Thread playerThread;

        private void PlayerThreadSimulation()
        {
            while (!stopped)
            {
                if (!string.IsNullOrWhiteSpace(fileName) && File.Exists(fileName))
                {
                    //Invoke(new MethodInvoker(StartPlayback));
                    StartPlayback2();
                    
                    fileName = null;
                }

                Thread.Sleep(100);
            }
        }

        #region AlternateNAudioPlayBack
        private volatile WaveOutEvent outputDevice;
        private volatile WaveStream audioFile;
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

        /// <summary>
        /// The file name for the music file to play.
        /// </summary>
        private volatile string fileName;

        /// <summary>
        /// A flag indicating whether the playback is stopped.
        /// </summary>
        private volatile bool stopped;
        #endregion

        /// <summary>
        /// Stops the playback, cleans and disposes of the objects used for the playback.
        /// </summary>
        private void CloseWaveOut()
        {
            waveOutDevice?.Stop();

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
        /// Stops the playback, cleans and disposes of the objects used for the playback.
        /// </summary>
        private void CloseWaveOut2()
        {
            outputDevice?.Stop();
            outputDevice?.Dispose();
            outputDevice = null;
            audioFile?.Dispose();
            audioFile = null;
        }

        private void StartPlayback()
        {
            CloseWaveOut();
            waveOutDevice = new WaveOut
            {
                //DesiredLatency = Program.Settings.LatencyMs,
            };


            try
            {
                var (waveStream, memoryStream) = CreateInputStream(fileName);
                mainOutputStream = waveStream;
                mainMemoryStream = memoryStream;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return;
            }

            if (mainOutputStream == null)
            {
                return;
            }


            waveOutDevice.Init(mainOutputStream);
            waveOutDevice.Play();
        }

        private void StartPlayback2()
        {
            CloseWaveOut2();
            outputDevice = new WaveOutEvent();

            if (Path.GetExtension(fileName).Equals(".ogg", StringComparison.InvariantCultureIgnoreCase))
            {
                audioFile = new NAudio.Vorbis.VorbisWaveReader(fileName);
            }
            else
            {
                audioFile = new AudioFileReader(fileName);
            }

            outputDevice.Init(audioFile);
            outputDevice.Play();
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
                    if (Program.Settings.LoadEntireFileSizeLimit > 0 &&
                        Program.Settings.LoadEntireFileSizeLimit * 1000000 > new FileInfo(fileName).Length)
                    {
                        // load the entire file into the memory..
                        memoryStream = new MemoryStream(File.ReadAllBytes(fileName));
                    }
                }
                catch (Exception ex)
                {
                    // log the exception..
                    Debug.WriteLine(ex);
                    memoryStream = null;
                }
                
                // determine the file type by it's extension..
                if (Constants.FileIsMp3(fileName))
                {
                    AudioFileReader fr = new AudioFileReader(fileName);

                    WaveStream mp3Reader = fr;
                    inputStream = new WaveChannel32(mp3Reader);
                }
                else if (Constants.FileIsOgg(fileName))
                {
                    // special handling for ogg/vorbis..
                    VorbisWaveReader fr = new VorbisWaveReader(fileName);

                    WaveStream oggReader = fr;
                    inputStream = new WaveChannel32(oggReader);
                }
                else if (Constants.FileIsWav(fileName))
                {
                    AudioFileReader fr = new AudioFileReader(fileName);

                    WaveStream wavReader = fr;
                    inputStream = new WaveChannel32(wavReader);
                }
                else if (Constants.FileIsFlac(fileName))
                {
                    AudioFileReader fr = new AudioFileReader(fileName);

                    WaveStream wavReader = fr;
                    inputStream = new WaveChannel32(wavReader);
                }
                else if (Constants.FileIsWma(fileName))
                {
                    // now stream constructor on this one..
                    memoryStream?.Dispose();
                    memoryStream = null;

                    AudioFileReader fr = new AudioFileReader(fileName);

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
                    AudioFileReader fr = new AudioFileReader(fileName);

                    WaveStream wavReader = fr;
                    inputStream = new WaveChannel32(wavReader);
                }
                else // throw for catching furthermore in the code..
                {
                    throw new InvalidOperationException("Unsupported file extension.");
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
                    Debug.WriteLine(ex);
                }
                catch (Exception ex2)
                {
                    // log the exception..
                    Debug.WriteLine(ex2);

                    // try recursion to create a WaveChannel32 instance..
                    return CreateInputStream(fileName);
                }
            }

            // eek! - failure..
            return default;
        }

        private void mnuOpenMusicFile_Click(object sender, EventArgs e)
        {
            if (odMusicFile.ShowDialog() == DialogResult.OK)
            {
                fileName = odMusicFile.FileName;
                //StartPlayback();
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopped = true;
            while (!playerThread.Join(100))
            {
                Application.DoEvents();
            }
        }
    }
}
