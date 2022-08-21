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

using System.ComponentModel;
using amp.Database;
using amp.Database.DataModel;
using amp.Shared.Classes;
using amp.Shared.Constants;
using amp.Shared.Localization;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace amp.EtoForms.Dialogs;

/// <summary>
/// A dialog which adds and updates music files into the database.
/// Implements the <see cref="Eto.Forms.Dialog{T}" />
/// </summary>
/// <seealso cref="Eto.Forms.Dialog{T}" />
public class DialogAddFilesProgress : Dialog<bool>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DialogAddFilesProgress"/> class.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/> for the database to update the added files into.</param>
    /// <param name="directory">The directory to search for audio files to add into the database.</param>
    /// <param name="albumId">The album identifier to initially to add the audio tracks to.</param>
    private DialogAddFilesProgress(AmpContext context, string directory, long albumId) : this(context, albumId)
    {
        this.directory = directory;
        singeFileBatch = false;
        saveDatabaseThread.Start();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DialogAddFilesProgress"/> class.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/> for the database to update the added files into.</param>
    /// <param name="albumId">The album identifier to initially to add the audio tracks to.</param>
    /// <param name="files">The audio files to add into the database.</param>
    private DialogAddFilesProgress(AmpContext context, long albumId, params string[] files) : this(context, albumId)
    {
        directoryCrawled = true;
        singeFileBatch = true;
        this.files = files;
        saveDatabaseThread.Start();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DialogAddFilesProgress"/> class.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/> for the database to update the added files into.</param>
    /// <param name="albumId">The album identifier to initially to add the audio tracks to.</param>
    private DialogAddFilesProgress(AmpContext context, long albumId)
    {
        if (albumId == 0)
        {
            Title = "amp# - " + UI.AddMusic;
        }
        else
        {
            var album = context.Albums.FirstOrDefault(f => f.Id == albumId)?.AlbumName;
            Title = "amp# - " + string.Format(UI.AddMusicToAlbum0, album);
        }

        this.context = context;
        this.albumId = albumId;

        var topTable = new TableLayout
        {
            Rows =
            {
                new TableRow
                {
                    Cells =
                    {
                        new Label { Text = UI.DirectoriesCrawled + UI.ColonDelimiter, },
                        new Panel { Width = Globals.DefaultPadding, },
                        lbDirectoryCount,
                    },
                },
                new TableRow
                {
                    Cells =
                    {
                        new Label { Text = UI.FilesFound + UI.ColonDelimiter, },
                        new Panel { Width = Globals.DefaultPadding, },
                        lbFilesCount,
                    },
                },
                new TableRow
                {
                    Cells =
                    {
                        new Label { Text = UI.DatabaseUpdated + UI.ColonDelimiter, },
                        new Panel { Width = Globals.DefaultPadding, },
                        lbDatabaseUpdatedCount,
                    },
                },
                new TableRow
                {
                    Cells =
                    {
                        new Label { Text = albumId == 0 ? UI.ColonDelimiter : UI.TracksAddedToAlbum + UI.ColonDelimiter, },
                        new Panel { Width = Globals.DefaultPadding, },
                        lbTrackAddedToAlbumCount,
                    },
                },
                new TableRow
                {
                    Cells =
                    {
                        new Label { Text = UI.TimeEstimateReady + UI.ColonDelimiter, },
                        new Panel { Width = Globals.DefaultPadding, },
                        lbTimeEstimateReady,
                    },
                },
            },
            Padding = Globals.DefaultPadding,
        };

        Content = new StackLayout
        {
            Items =
            {
                new StackLayoutItem(topTable),
                new StackLayoutItem(progressDbUpdate, HorizontalAlignment.Stretch),
            },
            Padding = Globals.DefaultPadding,
        };

        userAbortToken = source.Token;
        saveDatabaseThread = new Thread(ThreadMethod);

        base.Size = new Size(500, 200);
        Shown += DialogAddFilesProgress_Shown;
        NegativeButtons.Add(btnCancel);
        btnOk.Click += BtnOk_Click;
        btnCancel.Click += CancelClick;
        PositiveButtons.Add(btnOk);
        Closing += DialogAddFilesProgress_Closing;
        defaultCancelButtonHandler = DefaultCancelButtonHandler.WithWindow(this).WithCancelButton(btnCancel).WithDefaultButton(btnOk);
        Closed += DialogAddFilesProgress_Closed;
    }

    private void DialogAddFilesProgress_Closed(object? sender, EventArgs e)
    {
        defaultCancelButtonHandler?.Dispose();
    }

    #region InternalEvents
    private async void DialogAddFilesProgress_Closing(object? sender, CancelEventArgs e)
    {
        if (!closedViaButton)
        {
            await Abort();
        }
    }

    private async void BtnOk_Click(object? sender, EventArgs e)
    {
        try
        {
            await transaction!.CommitAsync(CancellationToken.None);
        }
        catch (Exception ex)
        {
            Globals.Logger?.Error(ex, "");
        }

        closedViaButton = true;
        Close(true);
    }

    private async Task Abort()
    {
        source.Cancel();
        threadStopped = true;

        while (!saveDatabaseThread.Join(500))
        {
            Application.Instance.RunIteration();
        }

        await TryRollback();
    }

    private async void CancelClick(object? sender, EventArgs e)
    {
        await Abort();
        closedViaButton = true;
        Close(false);
    }

    private async void DialogAddFilesProgress_Shown(object? sender, EventArgs e)
    {
        if (directory != null)
        {
            await DirectoryFileCrawler.CrawlDirectory(directory, userAbortToken, FilesCallbackFunc, true,
                MusicConstants.SupportedExtensionArray);
            directoryCrawled = true;
        }

        if (files != null)
        {
            List<FileInfo> updateFiles = new();
            foreach (var file in files)
            {
                try
                {
                    updateFiles.Add(new FileInfo(file));
                }
                catch (Exception ex)
                {
                    Globals.Logger?.Error(ex, "");
                }
            }
            await FilesCallbackFunc(updateFiles);
        }
    }
    #endregion

    #region UiElements
    private readonly Label lbDirectoryCount = new() { Text = UI._, };
    private readonly Label lbFilesCount = new() { Text = UI._, };
    private readonly Label lbDatabaseUpdatedCount = new() { Text = UI._, };
    private readonly Label lbTrackAddedToAlbumCount = new() { Text = UI._, };
    private readonly Label lbTimeEstimateReady = new() { Text = UI._, };
    private readonly ProgressBar progressDbUpdate = new();
    private readonly Button btnOk = new() { Text = UI.OK, Enabled = false, };
    private readonly Button btnCancel = new() { Text = UI.Cancel, };
    #endregion

    #region PrivateFields
    private readonly CancellationTokenSource source = new();
    private volatile Queue<List<FileInfo>> fileUpdateQueue = new();
    private IDbContextTransaction? transaction;
    private volatile bool threadStopped;
    private int fileCounter;
    private int directoryCounter;
    private int databaseCounter;
    private bool closedViaButton;
    private long albumId;
    private volatile bool singeFileBatch;
    private readonly AmpContext context;
    private readonly string? directory;
    private readonly string[]? files;
    private readonly CancellationToken userAbortToken;
    private readonly Thread saveDatabaseThread;
    private volatile bool directoryCrawled;
    private readonly TimeEstimateCalculator estimateCalculator = new(500);
    private readonly DefaultCancelButtonHandler? defaultCancelButtonHandler;
    #endregion

    /// <summary>
    /// A callback method to enqueue files to be inserted or updated into the database.
    /// </summary>
    /// <param name="fileInfos">The music files to insert or update.</param>
    /// <returns><see cref="Task.CompletedTask"/></returns>
    private Task FilesCallbackFunc(List<FileInfo> fileInfos)
    {
        fileCounter += fileInfos.Count;
        directoryCounter++;
        lbFilesCount.Text = $"{fileCounter}";
        lbDirectoryCount.Text = $"{directoryCounter}";
        fileUpdateQueue.Enqueue(fileInfos);
        return Task.CompletedTask;
    }

    /// <summary>
    /// The thread contents to update the database.
    /// </summary>
    private async void ThreadMethod()
    {
        try
        {
            while (!threadStopped)
            {
                if (transaction == null)
                {
                    transaction = await context.Database.BeginTransactionAsync(userAbortToken);
                }

                if (fileUpdateQueue.Any())
                {
                    var fileInfos = fileUpdateQueue.Dequeue();
                    await HandleFiles(fileInfos);
                    if (singeFileBatch)
                    {
                        threadStopped = true;
                        await Application.Instance.InvokeAsync(() =>
                        {
                            btnOk.Enabled = true;
                            btnCancel.Enabled = false;
                        });
                    }
                }
                else if (!singeFileBatch)
                {
                    if (directoryCrawled)
                    {
                        await transaction.CommitAsync(CancellationToken.None);
                        threadStopped = true;
                        await Application.Instance.InvokeAsync(() =>
                        {
                            btnOk.Enabled = true;
                            btnCancel.Enabled = false;
                        });
                    }
                }

                Thread.Sleep(200);
            }
        }
        catch (Exception ex)
        {
            Globals.Logger?.Error(ex, "");
            await TryRollback();
        }
    }

    /// <summary>
    /// Tries the rollback the transaction for the database.
    /// </summary>
    private async Task TryRollback()
    {
        try
        {
            await transaction!.RollbackAsync(CancellationToken.None);
        }
        catch (Exception ex)
        {
            Globals.Logger?.Error(ex, "");
        }
    }

    /// <summary>
    /// Inserts or updates the music files into the database.
    /// </summary>
    /// <param name="fileInfos">The file infos.</param>
    private async Task HandleFiles(List<FileInfo> fileInfos)
    {
        var start = DateTime.Now;
        foreach (var fileInfo in fileInfos)
        {
            if (userAbortToken.IsCancellationRequested)
            {
                return;
            }

            try
            {
                var track = await context.AudioTracks.FirstOrDefaultAsync(f => f.FileName == fileInfo.Name, cancellationToken: userAbortToken) ?? new AudioTrack();
                track.UpdateTrackInfo(fileInfo);
                if (track.Id == 0)
                {
                    await context.AudioTracks.AddAsync(track, userAbortToken);
                }
                else
                {
                    context.Update(track);
                }

                await context.SaveChangesAsync(userAbortToken);

                if (albumId != 0)
                {
                    var albumTrack = await context.AlbumTracks.FirstOrDefaultAsync(f => f.AlbumId == albumId && f.AudioTrackId == track.Id,
                        cancellationToken: userAbortToken) ?? new AlbumTrack { AudioTrackId = track.Id, AlbumId = albumId, CreatedAtUtc = DateTime.UtcNow, };

                    if (albumTrack.Id == 0)
                    {
                        await context.AlbumTracks.AddAsync(albumTrack, userAbortToken);
                    }
                    else
                    {
                        context.AlbumTracks.Update(albumTrack);
                    }
                }

                await context.SaveChangesAsync(userAbortToken);
            }
            catch (Exception ex)
            {
                Globals.Logger?.Error(ex, "");
                await TryRollback();
            }
        }

        var end = DateTime.Now;
        estimateCalculator.AddData(start, end, fileInfos.Count);

        databaseCounter += fileInfos.Count;
        await Application.Instance.InvokeAsync(() =>
        {
            lbDatabaseUpdatedCount.Text =
                    string.Format(UI.NoOfNo, databaseCounter, fileCounter);

            if (albumId != 0)
            {
                lbTrackAddedToAlbumCount.Text =
                    string.Format(UI.NoOfNo, databaseCounter, fileCounter);
            }

            var eta = estimateCalculator.Estimate(fileCounter);
            lbTimeEstimateReady.Text = eta == null ? UI._ : $"{eta.Value.ToShortDateString()} {eta.Value.ToLongTimeString()}";

            progressDbUpdate.MaxValue = fileCounter;
            progressDbUpdate.Value = databaseCounter;
        });
    }

    /// <summary>
    /// Shows the dialog and blocks until the user closes the dialog.
    /// </summary>
    /// <param name="owner">The owner control that is showing the form</param>
    /// <param name="context">The <see cref="DbContext"/> for the database to update the added files into.</param>
    /// <param name="directory">The directory to search for audio files to add into the database.</param>
    /// <param name="albumId">The album identifier to initially to add the audio tracks to.</param>
    /// <remarks>The <paramref name="owner" /> specifies the control on the window that will be blocked from user input until the dialog is closed.</remarks>
    /// <returns>The result of the modal dialog</returns>
    public static bool ShowModal(Control owner, AmpContext context, string directory, long albumId)
    {
        var dialogProgress = new DialogAddFilesProgress(context, directory, albumId);

        return dialogProgress.ShowModal(owner);
    }

    /// <summary>
    /// Shows the dialog and blocks until the user closes the dialog.
    /// </summary>
    /// <param name="owner">The owner control that is showing the form</param>
    /// <param name="context">The <see cref="DbContext"/> for the database to update the added files into.</param>
    /// <param name="albumId">The album identifier to initially to add the audio tracks to.</param>
    /// <param name="files">The audio files to add into the database.</param>
    /// <remarks>The <paramref name="owner" /> specifies the control on the window that will be blocked from user input until the dialog is closed.</remarks>
    /// <returns>The result of the modal dialog</returns>
    public static bool ShowModal(Control owner, AmpContext context, long albumId, params string[] files)
    {
        var dialogProgress = new DialogAddFilesProgress(context, albumId, files);
        dialogProgress.albumId = albumId;

        return dialogProgress.ShowModal(owner);
    }
}