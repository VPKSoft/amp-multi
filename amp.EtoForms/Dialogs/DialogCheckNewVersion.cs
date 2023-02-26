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

using System.Globalization;
using System.Text;
using amp.Shared.Classes;
using amp.Shared.Localization;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom.Utilities;
using VPKSoft.Utils.Common.UpdateCheck;

namespace amp.EtoForms.Dialogs;
internal class DialogCheckNewVersion : Dialog
{
    private void CreateLayout()
    {
        Title = $"amp# {UI._} {UI.CheckForNewVersion}";

        MinimumSize = new Size(500, 300);
        var versionInfo = versionData.MaxBy(f => f.ReleaseDateTime);

        var linkButton = new LinkButton { Text = versionInfo?.DownloadUrl, };
        linkButton.Click += (_, _) => Application.Instance.Open(linkButton.Text);

        var historyBuilder = new StringBuilder();

        foreach (var data in versionData.OrderBy(f => f.ReleaseDateTime))
        {
            historyBuilder.AppendLine(
                $"{UpdateChecker.VersionAndTagToString(data.Version, UI._, data.VersionTag, UI.VersionPrefix)}, {data.ReleaseDateTime.DateTime.ToString(CultureInfo.CurrentUICulture)}");
            historyBuilder.AppendLine("---------------------");
            historyBuilder.AppendLine(data.ChangeLog);
        }

        var cbForget = new CheckBox();
        cbForget.CheckedChanged += (_, _) =>
        {
            if (cbForget.Checked == true)
            {
                if (versionInfo != null)
                {
                    Globals.Settings.ForgetVersionUpdate =
                        $"{versionInfo.Version}|{versionInfo.VersionTag ?? string.Empty}";
                }
            }
            else
            {
                Globals.Settings.ForgetVersionUpdate = string.Empty;
            }
        };

        var forgetSettingRow = !showForgetSetting
            ? new TableRow()
            : new TableRow
            {
                Cells =
                {
                    new Label { Text = UI.DoNotRemindOfThisVersionAgain, },
                    cbForget,
                },
            };

        var layout = new TableLayout
        {
            Rows =
            {
                new TableRow
                {
                    Cells =
                    {
                        new Label { Text = UI.NewVersion, },
                        new Label { Text = $"{UpdateChecker.VersionAndTagToString(versionInfo?.Version, UI._, versionInfo?.VersionTag)}" , },
                    },
                },
                new TableRow
                {
                    Cells =
                    {
                        new Label { Text = UI.CurrentVersion, },
                        new Label { Text = $"{UpdateChecker.VersionAndTagToString(currentVersion, UI._, currentVersionTag)}" , },
                    },
                },
                new TableRow
                {
                    Cells =
                    {
                        new Label { Text = UI.Download, },
                        linkButton,
                    },
                },
                forgetSettingRow,
                new TableRow
                {
                    Cells =
                    {
                        new Label { Text = UI.Changes, },
                        new TextArea { Text = historyBuilder.ToString(), ReadOnly = true, },
                    },
                    ScaleHeight = true,
                },
            },
            Padding = Globals.DefaultPadding,
            Spacing = Globals.DefaultSpacing,
        };

        Content = new Panel { Content = layout, Padding = Globals.DefaultPadding, };

        PositiveButtons.Add(btnClose);

        btnClose.Click += (_, _) => Close();

        defaultCancelButtonHandler = DefaultCancelButtonHandler.WithWindow(this).WithBoth(btnClose);
        Closed += DialogCheckNewVersion_Closed;
    }

    private void DialogCheckNewVersion_Closed(object? sender, EventArgs e)
    {
        defaultCancelButtonHandler?.Dispose();
    }

    private DefaultCancelButtonHandler? defaultCancelButtonHandler;
    private const string CheckUri = "https://www.vpksoft.net/versions/amp_version.json";

    /// <summary>
    /// Initializes a new instance of the <see cref="DialogCheckNewVersion"/> class.
    /// </summary>
    /// <param name="versionData">The new version data.</param>
    /// <param name="currentVersion">The current version of the application.</param>
    /// <param name="showForgetSetting">A value indicating whether to display a check box to not remind of the current new version again.</param>
    /// <param name="currentVersionTag">The current version tag of the application.</param>
    private DialogCheckNewVersion(List<VersionData> versionData, Version currentVersion, bool showForgetSetting, string? currentVersionTag)
    {
        this.versionData = versionData;
        this.currentVersionTag = currentVersionTag;
        this.currentVersion = currentVersion;
        this.showForgetSetting = showForgetSetting;
        CreateLayout();
    }

    /// <summary>
    /// Checks the new version and displays an information dialog if a new version was found.
    /// </summary>
    /// <param name="owner">The owner for the information dialog.</param>
    /// <param name="version">The current application version.</param>
    /// <param name="showForgetSetting">if set to <c>true</c> show a checkbox allowing user to forget reminder of the current new version.</param>
    /// <param name="versionTag">The version tag.</param>
    /// <returns><c>true</c> if the information dialog was displayed, <c>false</c> otherwise.</returns>
    public static async Task<bool> CheckNewVersion(Control owner, Version version, bool showForgetSetting, string? versionTag)
    {
        var checker = new UpdateChecker(CheckUri, version, versionTag);
        var versionData = (await checker.CheckUpdates()).ToList();
        if (versionData.Count == 0)
        {
            return false;
        }

        using var dialog = new DialogCheckNewVersion(versionData, version, showForgetSetting, versionTag);
        if (UtilityOS.IsMacOS)
        {
            // ReSharper disable once MethodHasAsyncOverload, Eto.Forms bug in MacOS implementation prevents this
            dialog.ShowModal(owner);
        }
        else
        {
            await dialog.ShowModalAsync(owner);
        }

        return true;
    }

    private readonly List<VersionData> versionData;
    private readonly Version currentVersion;
    private readonly string? currentVersionTag;
    private readonly Button btnClose = new() { Text = UI.Close, };
    private readonly bool showForgetSetting;
}