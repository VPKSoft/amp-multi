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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using amp.Database;
using amp.Database.DataModel;
using Eto.Forms;
using Microsoft.EntityFrameworkCore;
using amp.Shared.Localization;

namespace amp.EtoForms.Forms;

/// <summary>
/// A form to manage the albums within the the database.
/// Implements the <see cref="Form" />
/// </summary>
/// <seealso cref="Form" />
public class FormAlbums : Dialog
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FormAlbums"/> class.
    /// </summary>
    /// <param name="context">The amp# EF Core database context.</param>
    public FormAlbums(AmpContext context)
    {
        this.context = context;

        dataSource = GetAlbumDataSource();

        gvAlbums = new GridView
        {
            Columns =
            {
                new GridColumn
                {
                    DataCell = new TextBoxCell(nameof(Album.AlbumName)), Expand = true,
                    Editable = true,
                },
            },
        };

        Content = new TableLayout
        {
            Rows =
            {
                new TableRow(gvAlbums) {ScaleHeight = true,},
            },
        };

        gvAlbums.DataStore = new ObservableCollection<Album>(dataSource);

        PositiveButtons.Add(btnClose);
    }

    private List<Album> GetAlbumDataSource()
    {
        var albums = new List<Album> { context.Albums.First(f => f.Id == 1), };
        albums.AddRange(context.Albums.Where(f => f.Id != 1).AsNoTracking().ToList());
        return albums;
    }


    private readonly Button btnClose = new() { Text = UI.Close, };
    private readonly List<Album> dataSource;
    private readonly AmpContext context;
    private readonly GridView gvAlbums;
}