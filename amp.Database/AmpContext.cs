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

using System.Diagnostics;
using amp.Database.DataModel;
using amp.Database.ExtensionClasses;
using Microsoft.EntityFrameworkCore;

namespace amp.Database;

/// <summary>
/// An EF Core database context for the amp# software.
/// Implements the <see cref="DbContext" />
/// </summary>
/// <seealso cref="DbContext" />
public class AmpContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AmpContext"/> class.
    /// </summary>
    /// <remarks>See <see href="https://aka.ms/efcore-docs-dbcontext">DbContext lifetime, configuration, and initialization</see>
    /// for more information.</remarks>
    public AmpContext()
    {
        Songs = base.Set<Song>();
        Albums = base.Set<Album>();
        AlbumSongs = base.Set<AlbumSong>();
        QueueSnapshots = base.Set<QueueSnapshot>();
        QueueSongs = base.Set<QueueSong>();
    }

    // ReSharper disable five times MemberCanBePrivate.Global, these entities will be used.
    // ReSharper disable five times UnusedAutoPropertyAccessor.Global, these entities will be used.

    /// <summary>
    /// Gets the songs.
    /// </summary>
    /// <value>The songs.</value>
    public DbSet<Song> Songs { get; }

    /// <summary>
    /// Gets the albums.
    /// </summary>
    /// <value>The albums.</value>
    public DbSet<Album> Albums { get; }

    /// <summary>
    /// Gets the album songs.
    /// </summary>
    /// <value>The album songs.</value>
    public DbSet<AlbumSong> AlbumSongs { get; }

    /// <summary>
    /// Gets the queue snapshots.
    /// </summary>
    /// <value>The queue snapshots.</value>
    public DbSet<QueueSnapshot> QueueSnapshots { get; }

    /// <summary>
    /// Gets the queued songs.
    /// </summary>
    /// <value>The queued songs.</value>
    public DbSet<QueueSong> QueueSongs { get; }

    /// <inheritdoc cref="DbContext.OnConfiguring"/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(Globals.ConnectionString);
        optionsBuilder.EnableSensitiveDataLogging();
#if DEBUG
        optionsBuilder.LogTo(s =>
        {
            Debug.WriteLine(s);
        });
#endif
    }

    // ReSharper disable once RedundantOverriddenMember, probably will be needed.
    /// <inheritdoc cref="DbContext.OnModelCreating"/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Song>().SpecifyUtcKind();
        modelBuilder.Entity<Album>().SpecifyUtcKind();
        modelBuilder.Entity<AlbumSong>().SpecifyUtcKind();
        modelBuilder.Entity<QueueSnapshot>().SpecifyUtcKind();
        modelBuilder.Entity<QueueSong>().SpecifyUtcKind();

        base.OnModelCreating(modelBuilder);
    }
}