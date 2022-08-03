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
using amp.Database.ExtensionClasses;
using Eto.Forms;

namespace amp.EtoForms.Layout;
internal static class ReusableControls
{
    public static ComboBox CreateAlbumSelectCombo(Func<long?, Task>? selectedValueChanged, AmpContext context, long selectAlbumId)
    {
        var cmbAlbumSelect = new ComboBox { ReadOnly = false, AutoComplete = true, };
        cmbAlbumSelect.ItemTextBinding = new PropertyBinding<string>(nameof(Album.AlbumName));

        cmbAlbumSelect.SelectedValueChanged += async (_, _) =>
        {
            var id = ((amp.EtoForms.Models.Album?)cmbAlbumSelect.SelectedValue)?.Id;

            if (selectedValueChanged != null)
            {
                await selectedValueChanged.Invoke(id);
            }
        };

        UpdateAlbumDataSource(cmbAlbumSelect, context, selectAlbumId).Wait();

        return cmbAlbumSelect;
    }

    public static async Task UpdateAlbumDataSource(ComboBox cmbAlbumSelect, AmpContext context, long currentAlbumId = 0)
    {
        var albumsEntity = await context.Albums.GetUnTrackedList(f => f.AlbumName, new long[] { 1, });

        var albums = albumsEntity.Select(f => Globals.AutoMapper.Map<amp.EtoForms.Models.Album>(f)).ToList();

        cmbAlbumSelect.DataStore = albums;
        if (albums.Any(f => f.Id == currentAlbumId))
        {
            cmbAlbumSelect.SelectedValue = albums.FirstOrDefault(f => f.Id == currentAlbumId);
        }
        else
        {
            cmbAlbumSelect.SelectedValue = albums.FirstOrDefault(f => f.Id == 1);
        }
    }
}