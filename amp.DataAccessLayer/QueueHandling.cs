using amp.Database;
using amp.Shared.Interfaces;
using VPKSoft.Utils.Common.Interfaces;

namespace amp.DataAccessLayer;

/// <summary>
/// Methods for queue data.
/// </summary>
public static class QueueHandling
{
    /// <summary>
    /// Deletes the stash from an album.
    /// </summary>
    /// <param name="album">The album to delete the queue stash from.</param>
    /// <param name="context">The amp# database context.</param>
    /// <param name="reporter">An instance to a class implementing the <see cref="IExceptionReporter"/> interface for error reporting and logging.</param>
    /// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise.</returns>
    public static async Task<bool> DeleteStashFromAlbum(IEntity album, AmpContext context, IExceptionReporter reporter)
    {
        return await DeleteStashFromAlbum(album.Id, context, reporter);
    }

    /// <summary>
    /// Deletes the stash from an album.
    /// </summary>
    /// <param name="albumId">The album reference identifier to delete the queue stash from.</param>
    /// <param name="context">The amp# database context.</param>
    /// <param name="reporter">An instance to a class implementing the <see cref="IExceptionReporter"/> interface for error reporting and logging.</param>
    /// <returns><c>true</c> if the operation was successful, <c>false</c> otherwise.</returns>
    public static async Task<bool> DeleteStashFromAlbum(long albumId, AmpContext context, IExceptionReporter reporter)
    {
        try
        {
            var stashes = context.QueueStashes.Where(f => f.AlbumId == albumId);
            context.QueueStashes.RemoveRange(stashes);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            reporter.RaiseExceptionOccurred(ex, nameof(QueueHandling), nameof(DeleteStashFromAlbum));
            return false;
        }
    }
}
