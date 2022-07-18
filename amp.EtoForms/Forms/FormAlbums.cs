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

using System.Collections.ObjectModel;
using amp.Database;
using amp.Database.DataModel;
using amp.Database.ExtensionClasses;
using Eto.Forms;
using amp.Shared.Localization;
using Eto.Drawing;
using EtoForms.Controls.Custom.Utilities;
using FluentIcons.Resources.Filled;
using Microsoft.EntityFrameworkCore;

namespace amp.EtoForms.Forms;

/// <summary>
/// A form to manage the albums within the the database.
/// Implements the <see cref="Form" />
/// </summary>
/// <seealso cref="Form" />
public class FormAlbums : Dialog<bool>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FormAlbums"/> class.
    /// </summary>
    /// <param name="context">The amp# EF Core database context.</param>
    public FormAlbums(AmpContext context)
    {
        MinimumSize = new Size(400, 500);
        Title = $"amp# {UI._} {UI.Albums}";

        this.context = context;
        Padding = Globals.DefaultPadding;

        ToolBar = new ToolBar
        {
            Items =
            {
                new ButtonToolItem(DeleteClick)
                {
                    Image = EtoHelpers.ImageFromSvg(Colors.SteelBlue, Size16.ic_fluent_delete_16_filled,
                        Globals.MenuImageDefaultSize),
                    ToolTip = UI.DeleteAlbum,
                },
                new ButtonToolItem(AddClick)
                {
                    Image = EtoHelpers.ImageFromSvg(Colors.SteelBlue, Size16.ic_fluent_add_16_filled,
                        Globals.MenuImageDefaultSize),
                    ToolTip = UI.AddNewAlbum,
                },
            },
        };

        gvAlbums = new GridView
        {
            Columns =
            {
                new GridColumn
                {
                    DataCell = new TextBoxCell(nameof(Album.AlbumName)), Expand = true,
                    Editable = true,
                    HeaderText = UI.AlbumName,
                },
            },
        };

        Content = new TableLayout
        {
            Rows =
            {
                new TableRow(gvAlbums) {ScaleHeight = true,},
            },
            Padding = Globals.DefaultPadding,
            Spacing = Globals.DefaultSpacing,
        };

        gvAlbums.CellEdited += GvAlbums_CellEdited;

        NegativeButtons.Add(btnCancel);
        PositiveButtons.Add(btnSaveAndClose);

        btnCancel.Click += BtnCancel_Click;
        btnSaveAndClose.Click += BtnSaveAndClose_Click;

        Shown += FormAlbums_Shown;
    }

    private void GvAlbums_CellEdited(object? sender, GridViewCellEventArgs e)
    {
        var album = (Album)e.Item;
        album.ModifiedAtUtc = DateTime.UtcNow;
    }

    private async void BtnSaveAndClose_Click(object? sender, EventArgs e)
    {
        if (dataSource!.Any(f => string.IsNullOrWhiteSpace(f.AlbumName)))
        {
            MessageBox.Show(this, Messages.AlbumNameCanNotBeAnEmptyString, Messages.Notification);
            return;
        }

        await using var transaction = await context.Database.BeginTransactionAsync();

        await Globals.LoggerSafeInvokeAsync(async () =>
        {
            var toAdd = dataSource!.Where(f => f.Id == 0);

            await context.Albums.AddRangeAsync(toAdd);

            await context.SaveChangesAsync();

            var ids = dataSource!.Where(f => f.Id != 0).Select(f => f.Id).Distinct().ToList();
            var toRemove = context.Albums.Where(f => !ids.Contains(f.Id));
            var removeIds = await toRemove.Select(f => f.Id).ToListAsync();

            var toRemoveSongs = context.AlbumTracks.Where(f => removeIds.Contains(f.AlbumId));

            context.AlbumTracks.RemoveRange(toRemoveSongs);
            await context.SaveChangesAsync();

            context.Albums.RemoveRange(toRemove);
            await context.SaveChangesAsync();

            var existingIds = await context.Albums.Where(f => ids.Contains(f.Id)).Select(f => f.Id).ToListAsync();

            var toUpdate = dataSource!.Where(f => existingIds.Contains(f.Id)).ToList();

            context.Albums.UpdateRange(toUpdate);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }, async (_) => await transaction.RollbackAsync());

        Close(true);
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        Close(false);
    }

    private void AddClick(object? sender, EventArgs e)
    {
        dataSource!.Add(new Album { AlbumName = Messages.Untitled, CreatedAtUtc = DateTime.Now, });
    }

    private void DeleteClick(object? sender, EventArgs e)
    {
        if (gvAlbums.SelectedItem != null)
        {
            var album = (Album)gvAlbums.SelectedItem;

            if (album.Id == 1)
            {
                MessageBox.Show(this,
                    Messages.TheDefaultAlbumCanNotBeRemovedOnlyRenamingIsPossible,
                    Messages.Notification, MessageBoxButtons.OK);

                return;
            }

            dataSource!.Remove(album);
        }
    }

    private async void FormAlbums_Shown(object? sender, EventArgs e)
    {
        dataSource = new ObservableCollection<Album>(await GetAlbumDataSource());
        gvAlbums.DataStore = dataSource;
    }

    private async Task<List<Album>> GetAlbumDataSource()
    {
        var albums = await context.Albums.GetUnTrackedList(f => f.AlbumName, new long[] { 1, });
        return albums;
    }

    private readonly Button btnCancel = new() { Text = UI.Close, };
    private readonly Button btnSaveAndClose = new() { Text = UI.SaveClose, };
    private ObservableCollection<Album>? dataSource;
    private readonly AmpContext context;
    private readonly GridView gvAlbums;
}