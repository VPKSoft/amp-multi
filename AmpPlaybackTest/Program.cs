using System;
using System.Windows.Forms;
using NAudio.Wave;

namespace AmpPlaybackTest
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }

        #region MockSettings
        internal static class Settings
        {
            /// <summary>
            /// Gets or sets the limit of a file size to be loaded into the memory for smoother playback.
            /// </summary>
            public static int LoadEntireFileSizeLimit { get; set; } = -1;

            /// <summary>
            /// The latency in milliseconds for the <see cref="WaveOut.DesiredLatency"/>.
            /// </summary>
            public static int LatencyMs { get; set; } = 300;
        }
        #endregion

    }
}
