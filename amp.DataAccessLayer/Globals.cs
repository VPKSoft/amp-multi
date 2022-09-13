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

using amp.Database.DataModel;
using AutoMapper;

namespace amp.DataAccessLayer;

/// <summary>
/// Global data for the amp# application.
/// </summary>
public static class Globals
{
    /// <summary>
    /// Gets or sets the floating point comparison tolerance.
    /// </summary>
    /// <value>The floating point comparison tolerance.</value>
    public static double FloatingPointTolerance { get; set; } = 0.000000001;

    /// <summary>
    /// Gets or sets the floating point comparison tolerance for the single-precision floating point values.
    /// </summary>
    /// <value>The floating point comparison tolerance.</value>
    public static float FloatingPointSingleTolerance { get; set; } = 0.00001f;

    private static MapperConfiguration? mapperConfiguration;
    private static IMapper? mapper;

    /// <summary>
    /// Gets the automatic mapper.
    /// </summary>
    /// <value>The automatic mapper.</value>
    public static IMapper AutoMapper
    {
        get
        {
            mapperConfiguration ??= new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AlbumTrack, DtoClasses.AlbumTrack>();
                cfg.CreateMap<AudioTrack, DtoClasses.AudioTrack>();
                cfg.CreateMap<Album, DtoClasses.Album>();
                cfg.CreateMap<QueueTrack, DtoClasses.QueueTrack>();
                cfg.CreateMap<QueueSnapshot, DtoClasses.QueueSnapshot>();
                cfg.CreateMap<QueueStash, DtoClasses.QueueStash>();

                cfg.CreateMap<DtoClasses.AlbumTrack, AlbumTrack>();
                cfg.CreateMap<DtoClasses.AudioTrack, AudioTrack>();
                cfg.CreateMap<DtoClasses.Album, Album>();
                cfg.CreateMap<DtoClasses.QueueTrack, QueueTrack>();
                cfg.CreateMap<DtoClasses.QueueSnapshot, QueueSnapshot>();
                cfg.CreateMap<DtoClasses.QueueStash, QueueStash>();
            });

            mapper ??= mapperConfiguration.CreateMapper();

            return mapper;
        }
    }
}