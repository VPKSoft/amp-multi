using System;
using System.Runtime.InteropServices;

namespace amp.UtilityClasses.WindowsPowerSave
{
    /// <summary>
    /// A class for the <see cref="SetThreadExecutionState"/> P/Invoke call.
    /// </summary>
    public class ThreadExecutionState
    {
        /// <summary>
        /// Enables an application to inform the system that it is in use,
        /// thereby preventing the system from entering sleep or turning off the
        /// display while the application is running.
        /// </summary>
        /// <param name="esFlags">The thread's execution requirements.</param>
        /// <returns>If the function succeeds, the return value is the previous thread execution state.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        // ReSharper disable once UnusedMember.Global
        public static extern EsFlags SetThreadExecutionState(EsFlags esFlags);
    }

    /// <summary>
    /// The thread's execution requirements.
    /// </summary>
    [Flags]
    public enum EsFlags: uint
    {
        /// <summary>
        /// Enables away mode. This value must be specified with <see cref="Continuous"/>.
        /// Away mode should be used only by media-recording and media-distribution applications
        /// that must perform critical background processing on desktop computers while the
        /// computer appears to be sleeping.
        /// </summary>
        AwayModeRequired = 0x00000040,

        /// <summary>
        /// Informs the system that the state being set should remain in effect until the next call that uses
        /// <see cref="Continuous"/> and one of the other state flags is cleared.
        /// </summary>
        Continuous = 0x80000000,

        /// <summary>
        /// Forces the display to be on by resetting the display idle timer.
        /// </summary>
        DisplayRequired = 0x00000002,

        /// <summary>
        /// Forces the system to be in the working state by resetting the system idle timer.
        /// </summary>
        SystemRequired = 0x00000001,

        /// <summary>
        /// This value is not supported.
        /// If <see cref="UserPresent"/> is combined with other <see cref="EsFlags"/> values,
        /// the call will fail and none of the specified states will be set.
        /// </summary>
        [Obsolete("Legacy flag, should not be used.")]
        UserPresent = 0x00000004,
    }
}
