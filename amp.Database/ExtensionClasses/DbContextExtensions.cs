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

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace amp.Database.ExtensionClasses;

/// <summary>
/// Extension methods for the <see cref="DbContext"/> class.
/// </summary>
public static class DbContextExtensions
{
    /// <summary>
    /// Updates the specified range of entities, saves the possible changes and stops tracking the <paramref name="entities"/> if the <paramref name="clearTracking"/> is set to <c>true</c>.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity being operated on by this set.</typeparam>
    /// <param name="context">The database context the entities belong to.</param>
    /// <param name="entities">The entities to update.</param>
    /// <param name="clearTracking">if set to <c>true</c> the <see cref="ChangeTracker.Clear()"/> is called.</param>
    /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
    public static async Task<int> UpdateRangeAndSave<TEntity>(this DbContext context, IEnumerable<TEntity> entities, bool clearTracking = true) where TEntity : class
    {
        return await context.UpdateRangeAndSave(entities.ToArray(), clearTracking);
    }

    /// <summary>
    /// Updates the specified range of entities, saves the possible changes and stops tracking the <paramref name="entities"/> if the <paramref name="clearTracking"/> is set to <c>true</c>.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity being operated on by this set.</typeparam>
    /// <param name="context">The database context the entities belong to.</param>
    /// <param name="entities">The entities to update.</param>
    /// <param name="clearTracking">if set to <c>true</c> the <see cref="ChangeTracker.Clear()"/> is called.</param>
    /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
    public static async Task<int> UpdateRangeAndSave<TEntity>(this DbContext context, TEntity[] entities, bool clearTracking = true) where TEntity : class
    {
        context.Set<TEntity>().UpdateRange(entities);
        var result = await context.SaveChangesAsync();

        if (clearTracking)
        {
            context.ChangeTracker.Clear();
        }

        return result;
    }
}