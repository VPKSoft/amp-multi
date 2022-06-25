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
using amp.Database.Interfaces;
using amp.Playback.Converters;
using amp.Playback.EventArguments;
using ManagedBass;
using PlaybackState = amp.Playback.Enumerations.PlaybackState;

namespace amp.Playback;

/// <summary>
/// A playback manager for the amp# software.
/// </summary>
/// <typeparam name="TSong">The type of the <see cref="IAlbumSong{TSong}"/> <see cref="IAlbumSong{TSong}.Song"/> member.</typeparam>
/// <typeparam name="TAlbum">The type of the <see cref="IAlbumSong{TSong}"/>.</typeparam>
public class PlaybackManager<TSong, TAlbum> : IDisposable where TSong : ISong where TAlbum : IAlbumSong<TSong>
{
    private readonly Serilog.Core.Logger logger;

    private volatile int currentSongHandle;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaybackManager{TSong, TAlbum}"/> class.
    /// </summary>
    /// <param name="logger">The logger to log exceptions, etc.</param>
    /// <param name="getNextSongFunc">A <see cref="Func{TResult}"/>, which is executed when requesting a next song for playback.</param>
    public PlaybackManager(Serilog.Core.Logger logger, Func<TAlbum?> getNextSongFunc)
    {
        this.logger = logger;
        Bass.Init();
        this.getNextSongFunc = getNextSongFunc;
    }

    /// <summary>
    /// Plays the specified song.
    /// </summary>
    /// <param name="song">The song.</param>
    public void PlaySong(IAlbumSong<TSong> song)
    {
        if (ManagerStopped)
        {
            throw new InvalidOperationException("The manager must be running by setting the ManagerStopped = false.");
        }

        DisposeCurrentChannel();

        currentSongHandle = Bass.CreateStream(song.Song?.FileName ??
                                              throw new InvalidOperationException(
                                                  "The IAlbumSong.Song must be not null."));
        Bass.ChannelPlay(currentSongHandle);
        songChanged = previousSongId != song.SongId;
        PreviousSongId = song.SongId;
    }

    /// <summary>
    /// Occurs when the playback position changed.
    /// </summary>
    public event EventHandler<PlaybackPositionChangedArgs>? PlaybackPositionChanged;

    /// <summary>
    /// Occurs when the song changed.
    /// </summary>
    public event EventHandler<SongChangedArgs>? SongChanged;

    /// <summary>
    /// Occurs when the playback state changed.
    /// </summary>
    public event EventHandler<PlaybackStateChangedArgs>? PlaybackStateChanged;

    private volatile bool stopThread = true;
    private volatile PlaybackState previousPlaybackState;
    private readonly object lockObject = new();

    [EditorBrowsable(EditorBrowsableState.Never)]
    private double previousPosition;
    [EditorBrowsable(EditorBrowsableState.Never)]
    private long previousSongId;

    private volatile bool songChanged;

    private Thread? playbackThread;
    private readonly Func<TAlbum?> getNextSongFunc;

    public void PlayNextSong()
    {
        var nextSong = getNextSongFunc();
        if (nextSong != null)
        {
            PlaySong(nextSong);
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

    private void PlaybackThreadMethod()
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

                if (previousPlaybackState == PlaybackState.Stopped)
                {
                    PlayNextSong();
                }
            }

            previousPlaybackState = AmpPlaybackStateConverter.ConvertFrom(playbackState);

            Thread.Sleep(100);
        }
    }

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