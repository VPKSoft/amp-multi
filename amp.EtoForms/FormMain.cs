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
using amp.Playback.Enumerations;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom;
using EtoForms.Controls.Custom.Utilities;
using Microsoft.EntityFrameworkCore;
using Form = Eto.Forms.Form;
using ListBox = Eto.Forms.ListBox;

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
        MinimumSize = new Size(550, 650);

        Application.Instance.UnhandledException += Program.Instance_UnhandledException;

        playbackOrder = new PlaybackOrder<Song, AlbumSong>(Globals.Settings);

        // ReSharper disable once StringLiteralTypo
        var databaseFile = Path.Combine(Globals.DataFolder, "amp_ef_core.sqlite");

        WindowsMigrateCheck.ConvertOldCommandLine(this);
        WindowsMigrateCheck.ConvertOld(this, databaseFile);

        var migration = new Migrate($"Data Source={databaseFile}");
        migration.RunMigrateUp();

        Database.Globals.ConnectionString = $"Data Source={databaseFile}";

        playbackManager = new PlaybackManager<Song, AlbumSong>(Globals.Logger, GetNextSongFunc, GetSongById);

        btnPlayPause = new CheckedButton
        {
            Size = new Size(32, 32),
            CheckedSvgImage = FluentIcons.Resources.Filled.Size16.ic_fluent_pause_16_filled,
            UncheckedSvgImage = FluentIcons.Resources.Filled.Size16.ic_fluent_play_16_filled,
            CheckedImageColor = Colors.Purple,
            UncheckedImageColor = Colors.Purple,
        };

        btnPlayPause.CheckedChange += PlayPauseToggle;

        var toolBar = new StackLayout
        {
            Orientation = Orientation.Horizontal,
            Items =
            {
                new Button((_, _) => commandPlayPause.Execute()) { Image = EtoHelpers.ImageFromSvg(Colors.Teal, FluentIcons.Resources.Filled.Size16.ic_fluent_previous_16_filled, new Size(32, 32)), Size = new Size(32, 32), },
                btnPlayPause,
                new Button(PlayNextSongClick) { Image = EtoHelpers.ImageFromSvg(Colors.Teal, FluentIcons.Resources.Filled.Size16.ic_fluent_next_16_filled, new Size(32, 32)), Size = new Size(32, 32), },
                new Panel {Width =  Globals.DefaultPadding,},
                new Button((_, _) => { }) { Image = EtoHelpers.ImageFromSvg(Color.Parse("#502D16"), amp.EtoForms.Properties.Resources.queue_three_dots, new Size(32, 32)), Size = new Size(32, 32), },
                new Panel {Width =  Globals.DefaultPadding,},
                new Button((_, _) => { }) { Image = EtoHelpers.ImageFromSvg(Color.Parse("#D4AA00"), amp.EtoForms.Properties.Resources.shuffle_random_svgrepo_com_modified, new Size(32, 32)), Size = new Size(32, 32), },
                new Button((_, _) => { }) { Image = EtoHelpers.ImageFromSvg(Color.Parse("#FF5555"), amp.EtoForms.Properties.Resources.repeat_svgrepo_com_modified, new Size(32, 32)), Size = new Size(32, 32), },
                new Panel {Width =  Globals.DefaultPadding,},
                new Button((_, _) => { }) { Image = EtoHelpers.ImageFromSvg(Colors.Navy, amp.EtoForms.Properties.Resources.stack_queue_three_dots, new Size(32, 32)), Size = new Size(32, 32), },
            },
        };

        songVolumeSlider = new VolumeSlider((_, args) =>
        {
            playbackManager!.PlaybackVolume = args.Value / 100.0;
        })
        { Maximum = 300, };

        var mainVolumeSlider =
            new TableLayout
            {
                Rows =
                {
                    new TableRow
                    {
                        Cells =
                        {
                            new TableCell(new Label { Text = "Volume", VerticalAlignment = VerticalAlignment.Center, Height = 40,}),
                            new Panel { Width = Globals.DefaultPadding,},
                            new TableCell(songVolumeSlider, true),
                        },
                    },
                    new Panel {Height = Globals.DefaultPadding,},
                    new TableRow
                    {
                        Cells =
                        {
                            new TableCell(new Label { Text = "Song volume", VerticalAlignment = VerticalAlignment.Center, Height = 40,}),
                            new Panel { Width = Globals.DefaultPadding,},
                            new TableCell(new VolumeSlider(), true),
                        },
                    },
                },
                Height = 80 + Globals.DefaultPadding * 3,
                Padding = Globals.DefaultPadding,
            };

        Content = new StackLayout
        {
            Items =
            {
                new StackLayoutItem(mainVolumeSlider, HorizontalAlignment.Stretch),
//                new StackLayoutItem(new Panel { Height = 40, Content = new VolumeSlider(), Padding = new Padding(6, 2),}, HorizontalAlignment.Stretch),
                new StackLayoutItem(lbSongsTitle),
                new StackLayoutItem(new Panel { Content = toolBar, Padding = new Padding(Globals.DefaultPadding, 2),}, HorizontalAlignment.Stretch),
                new StackLayoutItem(new Panel { Content = tbSearch, Padding = new Padding(Globals.DefaultPadding, 2),}, HorizontalAlignment.Stretch),
                new StackLayoutItem(new Panel { Content = lbSongs, Padding = new Padding(Globals.DefaultPadding, 2), }, HorizontalAlignment.Stretch) { Expand = true,},
            },
            Padding = new Padding(Globals.WindowBorderWidth, Globals.DefaultPadding),
        };

        context = new AmpContext();

        songs = context.AlbumSongs.Include(f => f.Song).Where(f => f.AlbumId == 1).ToList();

        playbackManager.PlaybackStateChanged += PlaybackManager_PlaybackStateChanged;
        playbackManager.SongChanged += PlaybackManager_SongChanged;

        lbSongs.Items.AddRange(context.AlbumSongs.Include(f => f.Song).Where(f => f.AlbumId == 1).Select(f => new ListItem { Text = f.GetSongName(), Key = f.Id.ToString(), }));

        commandPlayPause.Executed += CommandPlayPause_Executed;

        commandPlayPause.MenuText = Localization.UI.Play;
        commandPlayPause.Image = EtoHelpers.ImageFromSvg(Colors.SteelBlue,
            FluentIcons.Resources.Filled.Size16.ic_fluent_play_16_filled, new Size(32, 32));

        nextSongCommand.Executed += NextSongCommand_Executed;

        tbSearch.TextChanged += TbSearch_TextChanged;
        lbSongs.MouseDoubleClick += LbSongs_MouseDoubleClick;

        playbackManager.ManagerStopped = false;
        Closing += FormMain_Closing;
        KeyDown += FormMain_KeyDown;
        lbSongs.KeyDown += FormMain_KeyDown;
    }

    private async void PlayNextSongClick(object? sender, EventArgs e)
    {
        await playbackManager.PlayNextSong();
    }

    private async Task<AlbumSong?> GetSongById(long songId)
    {
        return await Application.Instance.InvokeAsync(AlbumSong? () =>
        {
            return songs.FirstOrDefault(f => f.SongId == songId);
        });
    }

    private readonly ListBox lbSongs = new() { Height = 650, Width = 550, };
    private readonly TextBox tbSearch = new();
    private readonly List<AlbumSong> songs;
    private readonly CheckedButton btnPlayPause;
    private readonly Label lbSongsTitle = new();
    private readonly VolumeSlider songVolumeSlider;

    #region MenuCommands
    private readonly PlaybackManager<Song, AlbumSong> playbackManager;
    private readonly PlaybackOrder<Song, AlbumSong> playbackOrder;
    private readonly Command commandPlayPause = new();
    private readonly Command nextSongCommand = new();
    private readonly AmpContext context;
    #endregion
}