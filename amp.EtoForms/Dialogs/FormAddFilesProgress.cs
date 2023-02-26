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

using amp.Database;
using amp.Database.DataModel;
using amp.Shared.Classes;
using amp.Shared.Constants;
using amp.Shared.Localization;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom.Helpers;
using EtoForms.Controls.Custom.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace amp.EtoForms.Dialogs;

/// <summary>
/// A dialog which adds and updates music files into the database.
/// Implements the <see cref="Eto.Forms.Dialog{T}" />
/// </summary>
/// <seealso cref="Eto.Forms.Dialog{T}" />
public class FormAddFilesProgress : Form
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FormAddFilesProgress"/> class.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/> for the database to update the added files into.</param>
    /// <param name="directory">The directory to search for audio files to add into the database.</param>
    /// <param name="albumId">The album identifier to initially to add the audio tracks to.</param>
    private FormAddFilesProgress(AmpContext context, string directory, long albumId) : this(context, albumId)
    {
        this.directory = directory;
        singeFileBatch = false;
        saveDatabaseThread.Start();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FormAddFilesProgress"/> class.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/> for the database to update the added files into.</param>
    /// <param name="albumId">The album identifier to initially to add the audio tracks to.</param>
    /// <param name="files">The audio files to add into the database.</param>
    private FormAddFilesProgress(AmpContext context, long albumId, params string[] files) : this(context, albumId)
    {
        directoryCrawled = true;
        singeFileBatch = true;
        this.files = files;
        saveDatabaseThread.Start();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FormAddFilesProgress"/> class.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/> for the database to update the added files into.</param>
    /// <param name="albumId">The album identifier to initially to add the audio tracks to.</param>
    private FormAddFilesProgress(AmpContext context, long albumId)
    {
        Closed += FormAddFilesProgress_Closed;

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
                new StackLayoutItem(new TableLayout
                {
                    Rows =
                    {
                        new TableRow
                        {
                            Cells =
                            {
                                new TableCell(new Panel(), true),
                                btnCancel,
                                btnOk,
                            },
                        },
                    },
                    Padding = Globals.DefaultPadding, Spacing = Globals.DefaultSpacing,
                }),
            },
            Padding = Globals.DefaultPadding,
        };

        userAbortToken = source.Token;
        saveDatabaseThread = new Task(ThreadMethod, userAbortToken);

        base.Size = new Size(500, 200);
        Shown += DialogAddFilesProgress_Shown;
        btnOk.Click += BtnOk_Click;
        btnCancel.Click += CancelClick;
        defaultCancelButtonHandler = DefaultCancelButtonHandler.WithWindow(this).WithCancelButton(btnCancel).WithDefaultButton(btnOk);
    }

    #region InternalEvents

    private async void FormAddFilesProgress_Closed(object? sender, EventArgs e)
    {
        if (!closedViaButton)
        {
            await Abort();
        }

        defaultCancelButtonHandler?.Dispose();

        if (!closedViaButton)
        {
            resultAction?.Invoke(false);
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
        Close();
        resultAction?.Invoke(true);
    }

    private async Task Abort()
    {
        source.Cancel();

        await Globals.LoggerSafeInvokeAsync(async () =>
        {
            await saveDatabaseThread.WaitAsync(TimeSpan.FromMilliseconds(2000), userAbortToken);
        });

        await TryRollback();
        context.ChangeTracker.Clear();
    }

    private async void CancelClick(object? sender, EventArgs e)
    {
        await Abort();
        closedViaButton = true;
        Close();
        resultAction?.Invoke(false);
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

        this.CenterForm(owner);
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
    private Action<bool>? resultAction;
    private readonly CancellationTokenSource source = new();
    private volatile Queue<List<FileInfo>> fileUpdateQueue = new();
    private IDbContextTransaction? transaction;
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
    private readonly Task saveDatabaseThread;
    private volatile bool directoryCrawled;
    private readonly TimeEstimateCalculator estimateCalculator = new(500);
    private readonly DefaultCancelButtonHandler? defaultCancelButtonHandler;
    private Window? owner;
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
            while (!userAbortToken.IsCancellationRequested)
            {
                transaction ??= await context.Database.BeginTransactionAsync(userAbortToken);

                if (fileUpdateQueue.Any())
                {
                    var fileInfos = fileUpdateQueue.Dequeue();
                    await HandleFiles(fileInfos);
                    if (singeFileBatch)
                    {
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
                var track = await context.AudioTracks.FirstOrDefaultAsync(f => f.FileName == fileInfo.Name && f.FilePath == fileInfo.DirectoryName, cancellationToken: userAbortToken) ?? new AudioTrack();
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
                context.ChangeTracker.Clear();
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
    /// <param name="resultAction">An action to call when the dialog is either finished adding the files or the operation was canceled.</param>
    /// <remarks>The <paramref name="owner" /> specifies the control on the window that will be blocked from user input until the dialog is closed.</remarks>
    /// <returns>The result of the modal dialog</returns>
    public static void Show(Window owner, AmpContext context, string directory, long albumId, Action<bool> resultAction)
    {
        var dialogProgress = new FormAddFilesProgress(context, directory, albumId) { Owner = owner, Topmost = true, resultAction = resultAction, };
        dialogProgress.owner = owner;
        owner.Enabled = false;
        dialogProgress.Show();
    }

    /// <summary>
    /// Shows the dialog and blocks until the user closes the dialog.
    /// </summary>
    /// <param name="owner">The owner control that is showing the form</param>
    /// <param name="context">The <see cref="DbContext"/> for the database to update the added files into.</param>
    /// <param name="albumId">The album identifier to initially to add the audio tracks to.</param>
    /// <param name="resultAction">An action to call when the dialog is either finished adding the files or the operation was canceled.</param>
    /// <param name="files">The audio files to add into the database.</param>
    /// <remarks>The <paramref name="owner" /> specifies the control on the window that will be blocked from user input until the dialog is closed.</remarks>
    /// <returns>The result of the modal dialog</returns>
    public static void Show(Window owner, AmpContext context, long albumId, Action<bool> resultAction, params string[] files)
    {
        var dialogProgress = new FormAddFilesProgress(context, albumId, files) { Owner = owner, Topmost = true, resultAction = resultAction, };
        dialogProgress.owner = owner;
        dialogProgress.albumId = albumId;
        owner.Enabled = false;
        dialogProgress.Show();
    }
}