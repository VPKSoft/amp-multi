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
using amp.Playback.Converters;
using amp.Playback.EventArguments;
using amp.Shared.Interfaces;
using ManagedBass;
using VPKSoft.DropOutStack;
using PlaybackState = amp.Playback.Enumerations.PlaybackState;

namespace amp.Playback;

/// <summary>
/// A playback manager for the amp# software.
/// </summary>
/// <typeparam name="TSong">The type of the <see cref="IAlbumSong{TSong}"/> <see cref="IAlbumSong{TSong}.Song"/> member.</typeparam>
/// <typeparam name="TAlbumSong">The type of the <see cref="IAlbumSong{TSong}"/>.</typeparam>
public class PlaybackManager<TSong, TAlbumSong> : IDisposable where TSong : ISong where TAlbumSong : IAlbumSong<TSong>
{
    private readonly Serilog.Core.Logger logger;

    private volatile int currentSongHandle;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaybackManager{TSong, TAlbumSong}"/> class.
    /// </summary>
    /// <param name="logger">The logger to log exceptions, etc.</param>
    /// <param name="getNextSongFunc">A Task&lt;<see cref="Func{TAlbumSong}"/>&gt;, which is executed when requesting a next song for playback.</param>
    /// <param name="getSongById">A Task&lt;<see cref="Func{TIdentity,TAlbumSong}"/>&gt;, which is executed when requesting song data by its reference identifier for playback.</param>
    /// <remarks>The contents of the <paramref name="getNextSongFunc"/> must be thread safe as it gets called from another thread.</remarks>
    /// <remarks>The contents of the <paramref name="getSongById"/> must be thread safe as it gets called from another thread.</remarks>
    public PlaybackManager(Serilog.Core.Logger logger, Func<Task<TAlbumSong?>> getNextSongFunc, Func<long, Task<TAlbumSong?>> getSongById)
    {
        this.logger = logger;
        Bass.Init();
        this.getNextSongFunc = getNextSongFunc;
        this.getSongById = getSongById;
    }

    /// <summary>
    /// Plays the specified song.
    /// </summary>
    /// <param name="song">The song.</param>
    /// <param name="userChanged">A value indicating whether the user selected the song for playback.</param>
    public void PlaySong(IAlbumSong<TSong> song, bool userChanged)
    {
        CheckManagerRunning();

        DisposeCurrentChannel();
        currentSongHandle = Bass.CreateStream(song.Song?.FileName ??
                                              throw new InvalidOperationException(
                                                  "The IAlbumSong.Song must be not null."));


        if (currentSongHandle != 0)
        {
            playedSongIds.Push(song.Id);
            skipPlaybackStateChange = userChanged;

            Bass.ChannelPlay(currentSongHandle);
            PlaybackVolume = song.Song.PlaybackVolume;
            songChanged = previousSongId != song.SongId;
            PreviousSongId = song.SongId;
        }
    }


    /// <summary>
    /// Plays the previous song if one is available.
    /// </summary>
    /// <returns><c>true</c> if the previous song playback was started successfully, <c>false</c> otherwise.</returns>
    public async Task<bool> PreviousSong()
    {
        CheckManagerRunning();
        if (playedSongIds.Count == 0)
        {
            return false;
        }

        var id = playedSongIds.Pop();
        var song = await getSongById(id);

        if (song != null)
        {
            PlaySong(song, true);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if the manager is running and throws an <see cref="InvalidOperationException"/> if not.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>.
    private void CheckManagerRunning()
    {
        if (ManagerStopped)
        {
            throw new InvalidOperationException("The manager must be running by setting the ManagerStopped = false.");
        }
    }

    /// <summary>
    /// Occurs when the playback position changed.
    /// </summary>
    /// <remarks>The event subscription code must be thread-safe as it gets invoked from another thread.</remarks>
    public event EventHandler<PlaybackPositionChangedArgs>? PlaybackPositionChanged;

    /// <summary>
    /// Occurs when the song changed.
    /// </summary>
    /// <remarks>The event subscription code must be thread-safe as it gets invoked from another thread.</remarks>
    public event EventHandler<SongChangedArgs>? SongChanged;

    /// <summary>
    /// Occurs when the playback state changed.
    /// </summary>
    /// <remarks>The event subscription code must be thread-safe as it gets invoked from another thread.</remarks>
    public event EventHandler<PlaybackStateChangedArgs>? PlaybackStateChanged;

    private volatile bool stopThread = true;
    private volatile PlaybackState previousPlaybackState;
    private readonly object lockObject = new();
    private readonly object volumeLockObject = new();
    private volatile bool skipPlaybackStateChange;
    private volatile DropOutStack<long> playedSongIds = new(100);
    private double volume = 1;

    [EditorBrowsable(EditorBrowsableState.Never)]
    private double previousPosition;
    [EditorBrowsable(EditorBrowsableState.Never)]
    private long previousSongId;

    private volatile bool songChanged;

    private Thread? playbackThread;
    private readonly Func<Task<TAlbumSong?>> getNextSongFunc;
    private readonly Func<long, Task<TAlbumSong?>> getSongById;


    /// <summary>
    /// Plays the next song.
    /// </summary>
    public async Task PlayNextSong()
    {
        var nextSong = await getNextSongFunc();
        if (nextSong != null)
        {
            PlaySong(nextSong, false);
        }
    }

    public double PlaybackVolume
    {
        get
        {
            lock (volumeLockObject)
            {
                return Bass.ChannelGetAttribute(currentSongHandle, ChannelAttribute.Volume);
            }
        }

        set
        {
            lock (volumeLockObject)
            {
                volume = value;
                Bass.ChannelSetAttribute(currentSongHandle, ChannelAttribute.Volume, value);
            }
        }
    }

    /// <summary>
    /// Resumes the playback if paused, otherwise plays the next song.
    /// </summary>
    public async Task PlayOrResume()
    {
        if (previousPlaybackState == PlaybackState.Paused)
        {
            Bass.ChannelPlay(currentSongHandle);
        }
        else
        {
            await PlayNextSong();
        }
    }

    /// <summary>
    /// Pauses the playback.
    /// </summary>
    public void Pause()
    {
        if (previousPlaybackState == PlaybackState.Playing)
        {
            Bass.ChannelPause(currentSongHandle);
        }
    }

    /// <summary>
    /// Gets or sets the previous song identifier (Thread safe).
    /// </summary>
    /// <value>The previous song identifier (Thread safe).</value>
    private long PreviousSongId
    {
        get
        {
            lock (lockObject)
            {
                return previousSongId;
            }
        }

        set
        {
            lock (lockObject)
            {
                previousSongId = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the previous playback position (Thread safe).
    /// </summary>
    /// <value>The previous playback position (Thread safe).</value>
    private double PreviousPosition
    {
        get
        {
            lock (lockObject)
            {
                return previousPosition;
            }
        }

        set
        {
            lock (lockObject)
            {
                previousPosition = value;
            }
        }
    }

    /// <summary>
    /// Gets the state of the playback.
    /// </summary>
    /// <value>The state of the playback.</value>
    public PlaybackState PlaybackState => previousPlaybackState;

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="PlaybackManager{TSong, TAlbum}"/> thread is stopped.
    /// </summary>
    /// <value><c>true</c> if this <see cref="PlaybackManager{TSong, TAlbum}"/> thread is stopped; otherwise, <c>false</c>.</value>
    public bool ManagerStopped
    {
        get => stopThread;

        set
        {
            if (stopThread != value)
            {
                stopThread = value;

                if (stopThread && playbackThread != null)
                {
                    playbackThread.Join();
                    FreeBass();
                    playbackThread = null;
                }

                if (!stopThread && playbackThread == null)
                {
                    playbackThread = new Thread(PlaybackThreadMethod);
                    playbackThread.Start();
                }
            }
        }
    }

    /// <summary>
    /// Disposes the current playback channel if any.
    /// </summary>
    private void DisposeCurrentChannel()
    {
        try
        {
            if (currentSongHandle != 0)
            {
                Bass.ChannelStop(currentSongHandle);
                Bass.StreamFree(currentSongHandle);
                currentSongHandle = 0;
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex, "");
        }
    }

    /// <summary>
    /// The thread method handling the audio playback.
    /// </summary>
    private async void PlaybackThreadMethod()
    {
        while (!stopThread)
        {
            double position = 0, duration = 0;
            var playbackState = ManagedBass.PlaybackState.Stopped;

            if (currentSongHandle != 0)
            {
                var bytes = Bass.ChannelGetLength(currentSongHandle);
                duration = Bass.ChannelBytes2Seconds(currentSongHandle, bytes);
                var positionBytes = Bass.ChannelGetPosition(currentSongHandle);
                position = Bass.ChannelBytes2Seconds(currentSongHandle, positionBytes);
                playbackState = Bass.ChannelIsActive(currentSongHandle);

                if (Math.Abs(PreviousPosition - position) > Globals.FloatingPointTolerance || songChanged)
                {
                    PlaybackPositionChanged?.Invoke(this,
                        new PlaybackPositionChangedArgs
                        {
                            CurrentPosition = position,
                            PlaybackLength = duration,
                            PlaybackState = AmpPlaybackStateConverter.ConvertFrom(playbackState),
                            SongId = PreviousSongId,
                        });
                }

                PreviousPosition = position;
            }

            if (songChanged)
            {
                songChanged = false;

                SongChanged?.Invoke(this,
                    new SongChangedArgs
                    {
                        CurrentPosition = position,
                        PlaybackLength = duration,
                        PlaybackState = AmpPlaybackStateConverter.ConvertFrom(playbackState),
                        SongId = PreviousSongId,
                    });
            }

            if (previousPlaybackState != AmpPlaybackStateConverter.ConvertFrom(playbackState))
            {
                PlaybackStateChanged?.Invoke(this,
                    new PlaybackStateChangedArgs
                    {
                        CurrentPosition = position,
                        PlaybackLength = duration,
                        PlaybackState = AmpPlaybackStateConverter.ConvertFrom(playbackState),
                        SongId = PreviousSongId,
                        PreviousPlaybackState = previousPlaybackState,
                    });

                if (!skipPlaybackStateChange && previousPlaybackState == PlaybackState.Stopped)
                {
                    await PlayNextSong();
                }
                else if (skipPlaybackStateChange)
                {
                    skipPlaybackStateChange = false;
                }
            }

            previousPlaybackState = AmpPlaybackStateConverter.ConvertFrom(playbackState);

            Thread.Sleep(100);
        }
    }

    /// <summary>
    /// Releases the resources used by the ManagedBass library.
    /// </summary>
    private void FreeBass()
    {
        try
        {
            DisposeCurrentChannel();
            Bass.Free();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "");
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        ManagerStopped = true;
        FreeBass();
    }
}