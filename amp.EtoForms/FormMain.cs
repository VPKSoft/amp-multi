#region License
/*
MIT License

Copyright(c) 2022 Petteri Kautonen

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

using amp.Database;
using amp.Database.DataModel;
using amp.EtoForms.Utilities;
using amp.Playback;
using amp.Shared.Localization;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom.UserIdle;
using Microsoft.EntityFrameworkCore;
using AlbumTrack = amp.EtoForms.Models.AlbumTrack;
using AudioTrack = amp.EtoForms.Models.AudioTrack;

namespace amp.EtoForms;

/// <summary>
/// The application main form.
/// Implements the <see cref="Form" />
/// </summary>
/// <seealso cref="Form" />
public partial class FormMain : Form
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FormMain"/> class.
    /// </summary>
    public FormMain()
    {
        Application.Instance.UnhandledException += Program.Instance_UnhandledException;
        Application.Instance.LocalizeString += Instance_LocalizeString;
        Title = "amp#";

        MinimumSize = new Size(550, 650);

        playbackOrder = new PlaybackOrder<AudioTrack, AlbumTrack, Models.Album>(Globals.Settings, UpdateQueueFunc);

        // ReSharper disable once StringLiteralTypo
        var databaseFile = Path.Combine(Globals.DataFolder, "amp_ef_core.sqlite");

        WindowsMigrateCheck.ConvertOldCommandLine(this);
        WindowsMigrateCheck.ConvertOld(this, databaseFile);

        var migration = new Migrate($"Data Source={databaseFile}");
        migration.RunMigrateUp();

        Database.Globals.ConnectionString = $"Data Source={databaseFile}";

        playbackManager = new PlaybackManager<AudioTrack, AlbumTrack, Models.Album>(Globals.Logger, GetNextSongFunc, GetSongById,
            () => Application.Instance.RunIteration(), Globals.Settings.PlaybackRetryCount);

        context = new AmpContext();
        CreateButtons();
        toolBar = CreateToolbar();
        songAdjustControls = CreateValueSliders();
        Content = CreateMainContent();

        totalVolumeSlider.Value = Globals.Settings.MasterVolume * 100;

        // There must always be the default album.
        if (!context.Albums.Any(f => f.Id == 1))
        {
            context.Albums.Add(new Album { Id = 1, AlbumName = UI.DefaultAlbumName, CreatedAtUtc = DateTime.UtcNow, });
            context.SaveChanges();
        }

        songs = context.AlbumTracks.Include(f => f.AudioTrack).Where(f => f.AlbumId == CurrentAlbumId).AsNoTracking().Select(f => Globals.AutoMapper.Map<AlbumTrack>(f)).ToList();

        playbackManager.ManagerStopped = false;

        idleChecker = new UserIdleChecker(this);

        AssignSettings();
        AssignEventListeners();
        CreateMenu();
    }

    private void TestStuff_Executed(object? sender, EventArgs e)
    {
        // Test stuff here:
        Globals.LoggerSafeInvoke(() => { _ = 1 / int.Parse("0"); });
    }

    private List<AlbumTrack> songs;
    private List<AlbumTrack> filteredSongs = new();
    private readonly PlaybackManager<AudioTrack, AlbumTrack, Models.Album> playbackManager;
    private readonly PlaybackOrder<AudioTrack, AlbumTrack, Models.Album> playbackOrder;
    private readonly AmpContext context;
    private readonly UserIdleChecker idleChecker;
}