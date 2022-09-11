using amp.DataAccessLayer.DtoClasses;
using amp.Database;
using amp.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
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


    /// <summary>
    /// Gets the queue stashes for an album.
    /// </summary>
    /// <param name="album">The album to get the queue stashes for.</param>
    /// <param name="context">The amp# database context.</param>
    /// <param name="reporter">An instance to a class implementing the <see cref="IExceptionReporter"/> interface for error reporting and logging.</param>
    /// <returns>A <see cref="List{T}"/> of <see cref="QueueStash"/> instances belonging to the specified album.</returns>
    public static async Task<List<QueueStash>> GetStashForAlbum(IEntity album, AmpContext context,
        IExceptionReporter reporter)
    {
        return await GetStashForAlbum(album.Id, context, reporter);
    }

    /// <summary>
    /// Gets the queue stashes for an album.
    /// </summary>
    /// <param name="albumId">The album reference identifier.</param>
    /// <param name="context">The amp# database context.</param>
    /// <param name="reporter">An instance to a class implementing the <see cref="IExceptionReporter"/> interface for error reporting and logging.</param>
    /// <returns>A <see cref="List{T}"/> of <see cref="QueueStash"/> instances belonging to the specified album.</returns>
    public static async Task<List<QueueStash>> GetStashForAlbum(long albumId, AmpContext context, IExceptionReporter reporter)
    {
        var result = new List<QueueStash>();

        try
        {
            var dto = await context.QueueStashes.Where(f => f.AlbumId == albumId).AsNoTracking().Select(f => Globals.AutoMapper.Map<QueueStash>(f))
                .ToListAsync();
            result.AddRange(dto);
        }
        catch (Exception ex)
        {
            reporter.RaiseExceptionOccurred(ex, nameof(QueueHandling), nameof(DeleteStashFromAlbum));
        }

        return result;
    }
}
