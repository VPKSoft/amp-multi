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
using amp.Shared.Interfaces;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom.Utilities;
using Microsoft.EntityFrameworkCore;
using Form = Eto.Forms.Form;

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
        Application.Instance.LocalizeString += Instance_LocalizeString;

        MinimumSize = new Size(550, 650);

        Application.Instance.UnhandledException += Program.Instance_UnhandledException;

        playbackOrder = new PlaybackOrder<Song, AlbumSong>(Globals.Settings, UpdateQueueFunc);

        // ReSharper disable once StringLiteralTypo
        var databaseFile = Path.Combine(Globals.DataFolder, "amp_ef_core.sqlite");

        WindowsMigrateCheck.ConvertOldCommandLine(this);
        WindowsMigrateCheck.ConvertOld(this, databaseFile);

        var migration = new Migrate($"Data Source={databaseFile}");
        migration.RunMigrateUp();

        Database.Globals.ConnectionString = $"Data Source={databaseFile}";

        playbackManager = new PlaybackManager<Song, AlbumSong>(Globals.Logger, GetNextSongFunc, GetSongById,
            () => Application.Instance.RunIteration());

        CreateButtons();
        toolBar = CreateToolbar();
        mainVolumeSlider = CreateVolumeSliders();
        Content = CreateMainContent();

        context = new AmpContext();
        // There must always be the default album.
        if (!context.Albums.Any(f => f.Id == 1))
        {
            context.Albums.Add(new Album { Id = 1, AlbumName = "Default", CreatedAtUtc = DateTime.UtcNow, });
            context.SaveChanges();
        }

        songs = context.AlbumSongs.Include(f => f.Song).Where(f => f.AlbumId == 1).AsNoTracking().ToList();

        ToStringFunc<AlbumSong>.StringFunc = song => song.GetSongName(true);

        songs = songs.OrderBy(f => f.GetSongName()).ToList();

        playbackManager.PlaybackStateChanged += PlaybackManager_PlaybackStateChanged;
        playbackManager.SongChanged += PlaybackManager_SongChanged;
        playbackManager.PlaybackPositionChanged += PlaybackManager_PlaybackPositionChanged;

        gvSongs.DataStore = songs;

        commandPlayPause.MenuText = Localization.UI.Play;
        commandPlayPause.Image = EtoHelpers.ImageFromSvg(Colors.SteelBlue,
            FluentIcons.Resources.Filled.Size16.ic_fluent_play_16_filled, Globals.ButtonDefaultSize);

        playbackManager.ManagerStopped = false;
        AssignEventListeners();
        CreateMenu();
    }

    private void TestStuff_Executed(object? sender, EventArgs e)
    {
        // Test stuff here:
        Globals.LoggerSafeInvoke(() => { _ = 1 / int.Parse("0"); });
    }

    private long currentAlbumId = 1;
    private readonly List<AlbumSong> songs;
    private readonly PlaybackManager<Song, AlbumSong> playbackManager;
    private readonly PlaybackOrder<Song, AlbumSong> playbackOrder;
    private readonly AmpContext context;
}