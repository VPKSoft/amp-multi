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

namespace amp.Database.LegacyConvert;

/// <summary>
/// Event arguments for the <see cref="MigrateOld.ReportProgress"/> event.
/// Implements the <see cref="EventArgs" />
/// </summary>
/// <seealso cref="EventArgs" />
public class ConvertProgressArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the songs handled count.
    /// </summary>
    /// <value>The songs handled count.</value>
    public int SongsHandledCount { get; init; }

    /// <summary>
    /// Gets or sets the songs count total.
    /// </summary>
    /// <value>The songs count total.</value>
    public int SongsCountTotal { get; init; }

    /// <summary>
    /// Gets or sets the albums handled count.
    /// </summary>
    /// <value>The albums handled count.</value>
    public int AlbumsHandledCount { get; init; }

    /// <summary>
    /// Gets or sets the albums count total.
    /// </summary>
    /// <value>The albums count total.</value>
    public int AlbumsCountTotal { get; init; }

    /// <summary>
    /// Gets or sets the album entries handled count.
    /// </summary>
    /// <value>The album entries handled count.</value>
    public int AlbumEntriesHandledCount { get; init; }

    /// <summary>
    /// Gets or sets the album entry count total.
    /// </summary>
    /// <value>The album entry count total.</value>
    public int AlbumEntryCountTotal { get; init; }

    /// <summary>
    /// Gets or sets the queue entries handled count.
    /// </summary>
    /// <value>The queue entries handled count.</value>
    public int QueueEntriesHandledCount { get; init; }

    /// <summary>
    /// Gets or sets the queue entry count total.
    /// </summary>
    /// <value>The queue entry count total.</value>
    public int QueueEntryCountTotal { get; init; }

    /// <summary>
    /// Gets the count of total entries handled in the conversion.
    /// </summary>
    /// <value>The count of total entries handled in the conversion.</value>
    public int HandledCountTotal => SongsHandledCount + AlbumsHandledCount + AlbumEntriesHandledCount + QueueEntriesHandledCount;

    /// <summary>
    /// Gets or sets the count of total entries to handle in the conversion.
    /// </summary>
    /// <value>The count of total entries to handle in the conversion.</value>
    public int CountTotal { get; init; }
}