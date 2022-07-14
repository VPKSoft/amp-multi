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

using amp.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace amp.Database.ExtensionClasses;

/// <summary>
/// Helper classes for <see cref="DbSet{TEntity}"/>.
/// </summary>
public static class DbSetHelpers
{
    /// <summary>
    /// Gets the un-tracked list of entities.
    /// </summary>
    /// <typeparam name="T">The TEntity type.</typeparam>
    /// <typeparam name="TKey">The type of the key for the <paramref name="orderFunc"/>.</typeparam>
    /// <param name="entities">The entities of type of <see cref="DbSet{TEntity}"/>.</param>
    /// <param name="orderFunc">The orderFunc.</param>
    /// <param name="topListItems">The top list item keys.</param>
    /// <param name="selectFunc">The optional selection filtering function.</param>
    /// <returns>List&lt;T&gt;.</returns>
    public static async Task<List<T>> GetUnTrackedList<T, TKey>(this DbSet<T> entities, Func<T, TKey> orderFunc, long[] topListItems, Func<T, bool>? selectFunc = null) where T : class, IEntity
    {
        var result = new List<T>();
        foreach (var topListItem in topListItems)
        {
            var item = entities.FirstOrDefault(f => f.Id == topListItem);
            if (item != null)
            {
                result.Add(item);
            }
        }

        var addItems = await entities.Where(f => !topListItems.Contains(f.Id)).AsNoTracking().AsQueryable().ToListAsync();

        if (selectFunc != null)
        {
            addItems = entities.Where(selectFunc).ToList();
        }

        addItems = addItems.OrderBy(orderFunc).ToList();

        result.AddRange(addItems);

        return result;
    }
}