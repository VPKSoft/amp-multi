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

using System.Reflection;
using amp.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace amp.Database.ExtensionClasses;

/// <summary>
/// Extension methods for the <see cref="DbContext"/> class.
/// </summary>
public static class DbContextExtensions
{
    /// <summary>
    /// Upserts the specified entity range into the database and saves the changes.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="context">The database context.</param>
    /// <param name="entities">The entities to upsert.</param>
    public static async Task UpsertRange<TEntity>(this DbContext context, params TEntity[] entities)
        where TEntity : class, IEntity
    {
        var toInsert = entities.Where(f => f.Id == 0).ToList();
        var toUpdate = entities.Where(f => f.Id != 0).ToList();

        foreach (var entity in toUpdate)
        {
            var update = await context.Set<TEntity>().FirstOrDefaultAsync(f => f.Id == entity.Id);
            if (update != null)
            {
                UpdateEntity(update, entity);
                await context.SaveChangesAsync();
            }
        }

        if (toInsert.Count > 0)
        {
            context.Set<TEntity>().AddRange(toInsert);
            await context.SaveChangesAsync();
        }
    }

    private static void UpdateEntity(IEntity destination, IEntity source)
    {
        var propertyInfos = destination.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

        foreach (var propertyInfo in propertyInfos)
        {
            if (destination.Id != 0 && propertyInfo.Name == nameof(IEntity.Id) || propertyInfo.Name == nameof(IRowVersionEntity.RowVersion))
            {
                continue;
            }

            var sourceValue = propertyInfo.GetValue(source);
            propertyInfo.SetValue(destination, sourceValue);
        }
    }
}