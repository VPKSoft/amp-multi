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
    public static async Task<List<QueueStash>> GetStashForAlbum(long albumId, AmpContext context,
        IExceptionReporter reporter)
    {
        var result = new List<QueueStash>();

        try
        {
            var dto = await context.QueueStashes.Where(f => f.AlbumId == albumId).AsNoTracking()
                .Select(f => Globals.AutoMapper.Map<QueueStash>(f))
                .ToListAsync();
            result.AddRange(dto);
        }
        catch (Exception ex)
        {
            reporter.RaiseExceptionOccurred(ex, nameof(QueueHandling), nameof(DeleteStashFromAlbum));
        }

        return result;
    }

    /// <summary>
    /// Saves the queue stash specified by track identifier - queue index dictionary.
    /// </summary>
    /// <param name="album">The album reference entity.</param>
    /// <param name="idQueueOrderPairs">The track identifier - queue index pairs.</param>
    /// <param name="context">The amp# database context.</param>
    /// <param name="reporter">An instance to a class implementing the <see cref="IExceptionReporter"/> interface for error reporting and logging.</param>
    /// <returns><c>true</c> if the queue stash was saved successfully, <c>false</c> otherwise.</returns>
    public static async Task<bool> SaveQueueStash(IEntity album, Dictionary<long, int> idQueueOrderPairs,
        AmpContext context,
        IExceptionReporter reporter)
    {
        return await SaveQueueStash(album.Id, idQueueOrderPairs, context, reporter);
    }

    /// <summary>
    /// Saves the queue stash specified by track identifier - queue index dictionary.
    /// </summary>
    /// <param name="albumId">The album reference identifier.</param>
    /// <param name="idQueueOrderPairs">The track identifier - queue index pairs.</param>
    /// <param name="context">The amp# database context.</param>
    /// <param name="reporter">An instance to a class implementing the <see cref="IExceptionReporter"/> interface for error reporting and logging.</param>
    /// <returns><c>true</c> if the queue stash was saved successfully, <c>false</c> otherwise.</returns>
    public static async Task<bool> SaveQueueStash(long albumId, Dictionary<long, int> idQueueOrderPairs,
        AmpContext context,
        IExceptionReporter reporter)
    {
        try
        {
            await DeleteStashFromAlbum(albumId, context, reporter);
            var toSave = idQueueOrderPairs.Select(f => new Database.DataModel.QueueStash
            {
                AlbumId = albumId,
                AudioTrackId = f.Key,
                QueueIndex = f.Value,
                CreatedAtUtc = DateTime.UtcNow,
            }).ToList();
            context.QueueStashes.AddRange(toSave);
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
    /// Gets the queue stash count for the specified album.
    /// </summary>
    /// <param name="album">The album reference entity.</param>
    /// <param name="context">The amp# database context.</param>
    /// <param name="reporter">An instance to a class implementing the <see cref="IExceptionReporter"/> interface for error reporting and logging.</param>
    /// <returns>The amount of tracks in the queue stash.</returns>
    public static int GetQueueStashCount(IEntity album, AmpContext context,
        IExceptionReporter reporter)
    {
        return GetQueueStashCount(album.Id, context, reporter);
    }

    /// <summary>
    /// Gets the queue stash count for the specified album.
    /// </summary>
    /// <param name="albumId">The album reference identifier.</param>
    /// <param name="context">The amp# database context.</param>
    /// <param name="reporter">An instance to a class implementing the <see cref="IExceptionReporter"/> interface for error reporting and logging.</param>
    /// <returns>The amount of tracks in the queue stash.</returns>
    public static int GetQueueStashCount(long albumId, AmpContext context,
        IExceptionReporter reporter)
    {
        try
        {
            var stashCount = context.QueueStashes.Count(f => f.AlbumId == albumId);
            return stashCount;
        }
        catch (Exception ex)
        {
            reporter.RaiseExceptionOccurred(ex, nameof(QueueHandling), nameof(DeleteStashFromAlbum));
            return 0;
        }
    }
}
