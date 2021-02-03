using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Windows.Forms;
using amp.Remote;

namespace AmpRESTfulTest
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            ampRestClient = new AmpRestHttpClient(tbRemoteUrl.Text, (int) nudRemotePort.Value);
            ampRestClient.LogMessage += LogMessage;
        }

        private void LogMessage(object sender, AmpRestHttpClientLogEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => Log(e.LogMessage)));
            }
            else
            {
                Log(e.LogMessage);
            }
        }

        private readonly AmpRestHttpClient ampRestClient;


        private readonly HttpClient client = new HttpClient();

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var result = await client.PostAsJsonAsync(new Uri("http://localhost:12345/api/queue/true"),
                    new List<AlbumSongRemote>(new[] {new AlbumSongRemote {Album = "Testing.."}}),
                    CancellationToken.None);
            }
            catch (Exception exception)
            {
                Log(exception);
            }
        }

        private void Log(Exception exception)
        {
            Log(exception.Message);
        }

        private void Log(string text)
        {
            tbLog.AppendText(DateTime.Now.ToString("[HH':'mm':'ss] "));
            tbLog.AppendText(text);
            tbLog.AppendText(Environment.NewLine);
        }

        private async void tsbPrevious_Click(object sender, EventArgs e)
        {
            await ampRestClient.PreviousSong();
        }

        private async void tsbNext_Click(object sender, EventArgs e)
        {
            await ampRestClient.NextSong();
        }

        private async void tsbPlayPause_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton) sender;
            if ((bool?)button.Tag == true)
            {
                button.Tag = false;
                button.Image = Properties.Resources.Play;
                await ampRestClient.Pause();
            }
            else
            {
                button.Tag = true;
                button.Image = Properties.Resources.Pause;
                await ampRestClient.Play();
            }
        }

        private async void FormMain_Shown(object sender, EventArgs e)
        {
            var songs = await ampRestClient.GetAlbumSongs();
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => { lbSongs.Items.AddRange(songs.ToArray()); }));
            }
            else
            {
                var songList = songs.ToArray();
                lbSongs.Items.AddRange(songList.ToArray());
                Log($"Fetch count: {songList.Length}");
            }
        }
    }
}
