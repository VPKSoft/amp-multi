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
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace amp.Database.ExtensionClasses;

/// <summary>
/// Some EF Core extension methods.
/// </summary>
public static class EntityHelper
{
    /// <summary>
    /// Specifies the entity implementing the <see cref="IEntity"/> interface <see cref="DateTime"/> properties as <see cref="DateTimeKind.Utc"/>.
    /// </summary>
    /// <param name="typeBuilder">The <see cref="EntityTypeBuilder"/> instance.</param>
    public static void SpecifyUtcKind<T>(this EntityTypeBuilder<T> typeBuilder) where T : class, IEntity
    {
        typeBuilder.Property(f => f.CreatedAtUtc).HasConversion(x => x.ToUniversalTime(), x => DateTime.SpecifyKind(x, DateTimeKind.Utc));
        typeBuilder.Property(f => f.ModifiedAtUtc).HasConversion(x => x!.Value.ToUniversalTime(), x => DateTime.SpecifyKind(x, DateTimeKind.Utc));
    }

    /// <summary>
    /// Specifies the entity implementing the <see cref="IRowVersionEntity"/> interface <see cref="IRowVersionEntity.RowVersion"/> column conversion.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="typeBuilder">The <see cref="EntityTypeBuilder"/> instance.</param>
    public static void SpecifyRowVersion<T>(this EntityTypeBuilder<T> typeBuilder) where T : class, IEntity, IRowVersionEntity
    {
        typeBuilder.Property(f => f.RowVersion).HasConversion(x => x == null ? 0 : BitConverter.ToInt64(x), x => BitConverter.GetBytes(x));
    }
}