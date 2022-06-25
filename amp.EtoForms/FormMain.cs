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
using amp.EtoForms.Playback;
using amp.EtoForms.Utilities;
using amp.Playback;
using Eto.Drawing;
using Eto.Forms;
using Microsoft.EntityFrameworkCore;
using Form = Eto.Forms.Form;
using ListBox = Eto.Forms.ListBox;

namespace amp.EtoForms;

/// <summary>
/// The application main form.
/// Implements the <see cref="Form" />
/// </summary>
/// <seealso cref="Form" />
public class FormMain : Form
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

        var toolBar = new StackLayout
        {
            Orientation = Orientation.Vertical,
            Items =
            {
                new Button((_, _) => commandPlayPause.Execute()) { Image = EtoHelpers.ImageFromSvg(Colors.SteelBlue, FluentIcons.Resources.Filled.Size16.ic_fluent_play_16_filled, new Size(32, 32)), Size = new Size(32, 32), },
                new Button((_, _) => nextSongCommand.Execute()) { Image = EtoHelpers.ImageFromSvg(Colors.SteelBlue, FluentIcons.Resources.Filled.Size16.ic_fluent_arrow_circle_right_16_filled, new Size(32, 32)), Size = new Size(32, 32), }
            },
        };

        Content = new StackLayout
        {
            Items =
            {
                new StackLayoutItem(new Panel { Content = toolBar, Padding = new Padding(6, 2),}, HorizontalAlignment.Stretch),
                new StackLayoutItem(new Panel { Content = tbSearch, Padding = new Padding(6, 2),}, HorizontalAlignment.Stretch),
                new StackLayoutItem(new Panel { Content = lbSongs, Padding = new Padding(6, 2), }, HorizontalAlignment.Stretch) { Expand = true,},
            }
        };

        var context = new AmpContext();

        var songs = context.AlbumSongs.Include(f => f.Song).Where(f => f.AlbumId == 1).ToList();

        playbackManager = new PlaybackManager<Song, AlbumSong>(Globals.Logger, () => songs[playbackOrder.NextSongIndex(songs)]);

        lbSongs.Items.AddRange(context.AlbumSongs.Include(f => f.Song).Where(f => f.AlbumId == 1).Select(f => new ListItem { Text = f.GetAlbumName(), Key = f.Id.ToString(), }));

        commandPlayPause.Executed += (_, _) =>
        {
            var song = context.AlbumSongs.First(f => f.Id == long.Parse(lbSongs.SelectedKey));
            playbackManager.PlaySong(song);
        };

        commandPlayPause.MenuText = Localization.UI.Play;
        commandPlayPause.Image = EtoHelpers.ImageFromSvg(Colors.SteelBlue,
            FluentIcons.Resources.Filled.Size16.ic_fluent_play_16_filled, new Size(32, 32));

        nextSongCommand.Executed += delegate
        {
            playbackManager.PlayNextSong();
        };

        playbackManager.ManagerStopped = false;
        Closing += FormMain_Closing;
    }

    private void FormMain_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        playbackManager.Dispose();
    }

    private readonly ListBox lbSongs = new() { Height = 650, Width = 550, };
    private readonly TextBox tbSearch = new();

    #region MenuCommands

    private readonly PlaybackManager<Song, AlbumSong> playbackManager;
    private readonly PlaybackOrder<Song, AlbumSong> playbackOrder;
    private readonly Command commandPlayPause = new();
    private readonly Command nextSongCommand = new();
    #endregion
}