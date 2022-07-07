using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using amp.Remote.DataClasses;
using AmpRESTfulTest.ClientImplementation;
using Newtonsoft.Json;

namespace AmpRESTfulTest;

public partial class FormMain : Form
{
    public FormMain()
    {
        InitializeComponent();

        ampRestClient = new AmpRestHttpClient(tbRemoteUrl.Text, (int)nudRemotePort.Value);
        ampRestClient.LogMessage += LogMessage;
        HttpClientHelpers.LogMessages.LogMessage += LogMessage;
    }

    #region Log
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
    #endregion

    #region InternalEvents
    private async void button1_Click(object sender, EventArgs e)
    {
        try
        {
            var result = await client.PostAsJsonAsync(new Uri("http://localhost:12345/api/queue/true"),
                new List<AlbumSongRemote>(new[] { new AlbumSongRemote { Album = "Testing.." } }),
                CancellationToken.None);
        }
        catch (Exception exception)
        {
            Log(exception);
        }
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
        var button = (ToolStripButton)sender;
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

    private List<AlbumSongRemote> PlayList { get; set; }

    private async Task RefreshPlayList()
    {
        var songs = await ampRestClient.GetAlbumSongs();
        var songList = songs.ToArray();
        PlayList = new List<AlbumSongRemote>(songList);
        lbSongs.Items.AddRange(songList.Cast<object>().ToArray());
        Log($"Fetch count: {songList.Length}");
    }

    private async void FormMain_Shown(object sender, EventArgs e)
    {
        await RefreshPlayList();
        var state = await GetPlayerState();
        SetUiState(state);
        tmPlayerState.Enabled = true;
    }

    private async void tmPlayerState_Tick(object sender, EventArgs e)
    {
        if (PlayList.Count == 0)
        {
            await RefreshPlayList();
        }
        var state = await GetPlayerState();
        SetUiState(state);
    }

    private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
    {
        tmPlayerState.Enabled = false;
    }
    #endregion

    #region UIMethods
    private bool suspendPositionUpdate;

    private PlayerStateRemote PlayerState { get; set; }

    /// <summary>
    /// Sets the state of the UI.
    /// </summary>
    /// <param name="stateRemote">The state of the remote amp# music player.</param>
    private void SetUiState(PlayerStateRemote stateRemote)
    {
        if (stateRemote == null)
        {
            Log($"private void SetUiState(PlayerStateRemote stateRemote): Argument null.");
            return;
        }

        tsbPlayPause.Image = stateRemote.Paused ? Properties.Resources.Play : Properties.Resources.Pause;
        tsbPlayPause.ToolTipText = stateRemote.Paused ? "Play" : "Pause";
        tsbPrevious.Enabled = stateRemote.CanGoPrevious;
        tsbShuffle.Checked = stateRemote.Shuffle;
        tsbRepeat.Checked = stateRemote.Random;
        lbSongName.Text = stateRemote.CurrentSongName ?? "-";
        var songLength = (int)Math.Round(stateRemote.CurrentSongLength + 0.5);
        var songPosition = (int)Math.Round(stateRemote.CurrentSongPosition + 0.5);

        if (songLength != tbSongPosition.Maximum)
        {
            tbSongPosition.Maximum = songLength;
        }

        if (songPosition != tbSongPosition.Value)
        {
            if (!suspendPositionUpdate)
            {
                tbSongPosition.Value = songPosition;
            }
        }

        var span = new TimeSpan(0, 0, 0, 0, songLength * 1000 - songPosition * 1000);
        lbMinusTime.Text = @"- " + span.ToString(@"hh\:mm\:ss");
    }
    #endregion

    #region RESTfulAPI
    private readonly AmpRestHttpClient ampRestClient;

    private readonly HttpClient client = new();

    private async Task<PlayerStateRemote> GetPlayerState()
    {
        var state = await ampRestClient.GetPlayerState();

        Log("Player state JSON:");
        Log(JsonConvert.SerializeObject(state));

        return state;
    }
    #endregion

    #region Search
    /// <summary>
    /// Finds the songs with the text in the search box.
    /// </summary>
    /// <param name="onlyIfText">if set to <c>true</c> an empty or white space in the search box doesn't affect the filtering.</param>
    /// <param name="alternateSearch">A search text to override the default search box text.</param>
    private void Find(bool onlyIfText = false, string alternateSearch = null)
    {
        var findText = alternateSearch ?? tbFind.Text;

        if (onlyIfText)
        {
            if (string.IsNullOrWhiteSpace(findText))
            {
                return;
            }
        }
        lbSongs.Items.Clear();
        foreach (var songRemote in PlayList)
        {
            if (songRemote.Match(findText))
            {
                lbSongs.Items.Add(songRemote);
            }
        }
    }

    private void tbFind_TextChanged(object sender, EventArgs e)
    {
        Find();
    }
    #endregion

    #region PlaybackPosition
    private void tbSongPosition_MouseDown(object sender, MouseEventArgs e)
    {
        suspendPositionUpdate = true;
    }

    private void tbSongPosition_MouseUp(object sender, MouseEventArgs e)
    {
        suspendPositionUpdate = false;
    }

    private void tbSongPosition_MouseLeave(object sender, EventArgs e)
    {
        suspendPositionUpdate = false;
    }

    private async void tbSongPosition_Scroll(object sender, EventArgs e)
    {
        await ampRestClient.SetPositionSeconds((double)tbSongPosition.Value);
    }
    #endregion

    #region Keyboard
    private async void FormMain_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Add || e.KeyValue == 187)
        {
            var tasks = new List<Task<List<AlbumSongRemote>>>();
            foreach (AlbumSongRemote songRemote in lbSongs.SelectedItems)
            {
                tasks.Add(
                    e.Control ? ampRestClient.Queue(false, songRemote) : ampRestClient.Queue(true, songRemote));
            }

            await Task.WhenAll(tasks.Cast<Task>().ToArray());

            e.SuppressKeyPress = true;
        }
    }
    #endregion
}