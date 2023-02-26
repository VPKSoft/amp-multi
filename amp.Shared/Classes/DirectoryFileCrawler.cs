#region License
/*
MIT License

Copyright(c) 2023 Petteri Kautonen

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

namespace amp.Shared.Classes;
using static TaskExecutionCatchPatterns;

/// <summary>
/// A class to crawl directories for specified file types.
/// </summary>
public static class DirectoryFileCrawler
{
    /// <summary>
    /// Crawls the directory optionally recursive and reports the found files via an async callback function for each directory.
    /// </summary>
    /// <param name="path">The path to start crawling from.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <param name="filesCallbackFunc">The callback function to report found files via a <see cref="FileInfo"/> class.</param>
    /// <param name="recurse">if set to <c>true</c> crawl the directory recursively.</param>
    /// <param name="fileExtensions">The file extensions.</param>
    public static async Task CrawlDirectory(string path, CancellationToken cancellationToken, Func<List<FileInfo>, Task> filesCallbackFunc, bool recurse, params string[] fileExtensions)
    {
        var extensionsParsed = fileExtensions.Select(f => f.ToLowerInvariant().Replace("*.", ".")).ToList();
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            await TryCatchPatternAsync(async () =>
            {
                var files = Directory.GetFiles(path);
                var collectedFiles = new List<FileInfo>();

                TryCatchPattern(() =>
                {
                    foreach (var file in files)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        if (extensionsParsed.Contains(Path.GetExtension(file).ToLowerInvariant()))
                        {
                            TryCatchPattern(() => collectedFiles.Add(new FileInfo(file)));
                        }
                    }
                });

                cancellationToken.ThrowIfCancellationRequested();

                await TryCatchPatternAsync(async () =>
                {
                    var directories = Directory.GetDirectories(path);

                    foreach (var directory in directories)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        if (recurse)
                        {
                            await CrawlDirectory(directory, cancellationToken, filesCallbackFunc, recurse, fileExtensions);
                        }
                    }

                    cancellationToken.ThrowIfCancellationRequested();
                });

                if (collectedFiles.Any())
                {
                    await filesCallbackFunc(collectedFiles);
                }
            });
        }
        catch (OperationCanceledException)
        {
        }
    }
}