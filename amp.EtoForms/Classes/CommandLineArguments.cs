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

using CommandLine;
// ReSharper disable UnusedAutoPropertyAccessor.Global, required by the NuGet package

namespace amp.EtoForms.Classes;

/// <summary>
/// The application command line arguments.
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global, Instantiated by CommandLineParser library. 
public class CommandLineArguments
{
    /// <summary>
    /// Gets or sets the PID (Process Identifier) to wait for before starting the application.
    /// </summary>
    /// <value>The PID to wait for before starting the application.</value>
    [Option('p', "pid", Required = false, HelpText = nameof(Shared.Localization.Messages.AProcessIdentifierPIDToWaitForExitBeforeStartingTheApplication), ResourceType = typeof(Shared.Localization.Messages))]
    public int? PidWait { get; set; }

    /// <summary>
    /// Gets or sets the name of the backup file to backup the application data before complete startup.
    /// </summary>
    /// <value>The name of the backup file.</value>
    [Option('b', "backup", Required = false, HelpText = nameof(Shared.Localization.Messages.AFileNameToBackupTheApplicationDataBeforeCompleteStartup), ResourceType = typeof(Shared.Localization.Messages))]
    public string? BackupFileName { get; set; }

    /// <summary>
    /// Gets or sets the name of the archive file to restore the backed up application data settings from.
    /// </summary>
    /// <value>The file name to restore backup the backup from.</value>
    [Option('r', "restore", Required = false, HelpText = nameof(Shared.Localization.Messages.RestoresABackupFromAZippedFileIntoTheProgramApplicationDataFolderOverridingTheExisting), ResourceType = typeof(Shared.Localization.Messages))]
    public string? RestoreBackupFile { get; set; }
}