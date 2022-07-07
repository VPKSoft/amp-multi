namespace amp.Remote.DataClasses;

/// <summary>
/// An enumeration describing the filter status of the playlist.
/// </summary>
public enum FilterType
{
    /// <summary>
    /// The playlist is filter with a search string.
    /// </summary>
    SearchFiltered,

    /// <summary>
    /// The playlist is showing queued songs.
    /// </summary>
    QueueFiltered,

    /// <summary>
    /// The playlist is showing song in the alternate queue.
    /// </summary>
    AlternateFiltered,

    /// <summary>
    /// The playlist is not filtered.
    /// </summary>
    NoneFiltered
}