using System.Collections.Generic;

namespace amp.UtilityClasses
{
    /// <summary>
    /// A class containing constant data used around this software.
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// The extensions of the audio formats used within the software.
        /// </summary>
        public static readonly List<string> Extensions =
            new List<string>(new []
            {
                ".MP3", 
                ".OGG", 
                // ReSharper disable once StringLiteralTypo
                ".FLAC", 
                ".WMA", 
                ".WAV", 
                ".M4A", 
                ".AAC", 
                ".AIF", 
                // ReSharper disable once StringLiteralTypo
                ".AIFF"
            });

        /// <summary>
        /// Gets a value indicating based on the file name if the file is a MP3 file.
        /// </summary>
        /// <param name="fileName">The name of the file check.</param>
        public static bool FileIsMp3(string fileName)
        {
            return fileName.ToUpper().EndsWith(".mp3".ToUpper());
        }

        /// <summary>
        /// Gets a value indicating based on the file name if the file is an OGG file.
        /// </summary>
        /// <param name="fileName">The name of the file check.</param>
        public static bool FileIsOgg(string fileName)
        {
            return fileName.ToUpper().EndsWith(".ogg".ToUpper());
        }

        /// <summary>
        /// Gets a value indicating based on the file name if the file is a WAV file.
        /// </summary>
        /// <param name="fileName">The name of the file check.</param>
        public static bool FileIsWav(string fileName)
        {
            return fileName.ToUpper().EndsWith(".wav".ToUpper());
        }

        // ReSharper disable once CommentTypo
        /// <summary>
        /// Gets a value indicating based on the file name if the file is a FLAC file.
        /// </summary>
        /// <param name="fileName">The name of the file check.</param>
        // ReSharper disable once IdentifierTypo
        public static bool FileIsFlac(string fileName)
        {
            // ReSharper disable once StringLiteralTypo
            return fileName.ToUpper().EndsWith(".flac".ToUpper());
        }

        /// <summary>
        /// Gets a value indicating based on the file name if the file is a WMA file.
        /// </summary>
        /// <param name="fileName">The name of the file check.</param>
        public static bool FileIsWma(string fileName)
        {
            // ReSharper disable once StringLiteralTypo
            return fileName.ToUpper().EndsWith(".wma".ToUpper());
        }

        /// <summary>
        /// Gets a value indicating based on the file name if the file is an AAC/M4A file.
        /// </summary>
        /// <param name="fileName">The name of the file check.</param>
        public static bool FileIsAacOrM4A(string fileName)
        {
            return fileName.ToUpper().EndsWith(".wma".ToUpper()) ||
                   fileName.ToUpper().EndsWith(".aac".ToUpper());
        }

        /// <summary>
        /// Gets a value indicating based on the file name if the file is an AIF file.
        /// </summary>
        /// <param name="fileName">The name of the file check.</param>
        public static bool FileIsAif(string fileName)
        {
            return fileName.ToUpper().EndsWith(".aif".ToUpper()) ||

                   // ReSharper disable once StringLiteralTypo
                   fileName.ToUpper().EndsWith(".aiff".ToUpper());
        }
    }
}
