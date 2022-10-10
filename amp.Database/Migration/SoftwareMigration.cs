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
using amp.Database.DataModel;

namespace amp.Database.Migration;

/// <summary>
/// A class to run data alter migrations on Entity Framework.
/// </summary>
public static class SoftwareMigration
{
    /// <summary>
    /// Runs the software migration.
    /// </summary>
    /// <param name="context">The database context.</param>
    public static void RunSoftwareMigration(AmpContext context)
    {
        var methods = typeof(SoftwareMigration).GetMethods(BindingFlags.Static | BindingFlags.Public).ToList();
        methods = methods.Where(f => f.CustomAttributes.Any(a => a.AttributeType == typeof(SoftwareMigrationAttribute))).ToList();

        foreach (var methodInfo in methods)
        {
            var attribute =
                (SoftwareMigrationAttribute?)methodInfo.GetCustomAttribute(typeof(SoftwareMigrationAttribute));


            if (attribute != null && !context.SoftwareMigrationVersion.Any(f => f.Version == attribute.Version))
            {
                methodInfo.Invoke(null, new object?[] { context, attribute.Version, });
            }
        }
    }

    /// <summary>
    /// Separates the file name from the file path to different columns.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="version">The the migration version.</param>
    [SoftwareMigration(Version = 1)]
    public static void SeparateFileNamePath(AmpContext context, int version)
    {
        using var transaction = context.Database.BeginTransaction();
        try
        {
            var tracks = context.AudioTracks.ToList();
            foreach (var audioTrack in tracks)
            {
                var fileName = Path.GetFileName(audioTrack.FileName);
                var filePath = Path.GetDirectoryName(audioTrack.FileName) ?? "";
                audioTrack.FileName = fileName;
                audioTrack.FilePath = filePath;
                audioTrack.ModifiedAtUtc = DateTime.UtcNow;
            }

            context.SoftwareMigrationVersion.Add(new SoftwareMigrationVersion { Version = version, });

            context.SaveChanges();
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
        }
    }
}