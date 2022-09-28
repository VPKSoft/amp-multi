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

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using amp.EtoForms.Settings.Enumerations;
using amp.EtoForms.Utilities;
using amp.Shared.Classes;
using amp.Shared.Localization;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom.Utilities;
using ManagedBass.FftSignalProvider;
using VPKSoft.Utils.Common.Classes;
using static EtoForms.Controls.Custom.Utilities.TableLayoutHelpers;

namespace amp.EtoForms.Forms;

/// <summary>
/// A settings dialog for the software.
/// Implements the <see cref="Dialog" />
/// </summary>
/// <seealso cref="Dialog" />
public class FormSettings : Dialog<bool>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FormSettings"/> class.
    /// </summary>
    public FormSettings()
    {
        Title = $"amp# {UI._} {Shared.Localization.Settings.SettingsLower}";
        ShowInTaskbar = false;
        MinimumSize = new Size(500, 500);
        Content = new Panel { Content = tbcSettings, Padding = Globals.DefaultPadding, };
        NegativeButtons.Add(btnCancel);
        PositiveButtons.Add(btnOk);
        CreateSettingsTabCommon();
        CreateSettingsTabRandom();
        CreateSettingsTabTrackNaming();
        CreateVisualizationSettingsTab();
        CreateMiscellaneousSettings();
        LoadSettings();

        btnCancel.Click += delegate
        {
            Close(false);
        };

        btnOk.Click += delegate
        {
            SaveSettings();
            Close(true);
        };

        DefaultButton = btnOk;
        AbortButton = btnCancel;
    }

    private void LoadSettings()
    {
        // Quiet hours.
        suspendQuietHoursSet = true;
        cbEnableQuietHours.Checked = Globals.Settings.QuietHours;
        cbDecreaseVolumeOnQuietHours.Checked = !Globals.Settings.QuietHoursPause;
        nsQuietHourSilenceAmount.Value = Globals.Settings.QuietHoursVolumePercentage;
        cbPauseOnQuietHours.Checked = Globals.Settings.QuietHoursPause;
        dtpStartQuietHours.Value = DateTime.ParseExact(Globals.Settings.QuietHoursFrom, "HH':'mm", CultureInfo.InvariantCulture);
        dtpEndQuietHours.Value = DateTime.ParseExact(Globals.Settings.QuietHoursTo, "HH':'mm", CultureInfo.InvariantCulture);
        suspendQuietHoursSet = false;

        // Weighted randomization.
        cbWeightedRandomEnabled.Checked = Globals.Settings.BiasedRandom;
        cbWeightedRating.Checked = Globals.Settings.BiasedRatingEnabled;
        sldRating.Value = (int)Globals.Settings.BiasedRating;
        cbWeightedPlayedCount.Checked = Globals.Settings.BiasedPlayedCountEnabled;
        sldPlayedCount.Value = (int)Globals.Settings.BiasedPlayedCount;
        cbWeightedRandomizedCount.Checked = Globals.Settings.BiasedRandomizedCountEnabled;
        sldRandomizedCount.Value = (int)Globals.Settings.BiasedRandomizedCount;
        cbWeightedSkippedCount.Checked = Globals.Settings.BiasedSkippedCountEnabled;
        sldSkippedCount.Value = (int)Globals.Settings.BiasedSkippedCount;
        sldWeightedTolerance.Value = (int)Globals.Settings.Tolerance;

        // Misc.
        cmbUiLocale.SelectedValue =
            new CultureExtended(string.IsNullOrWhiteSpace(Globals.Settings.Locale) ? "en" : Globals.Settings.Locale, true);

        cmbQueueFinishActionFirst.SelectedValue =
            dataStoreQueueFinishAction.First(f => f.First == Globals.Settings.QueueFinishActionFirst);
        cmbQueueFinishActionSecond.SelectedValue =
            dataStoreQueueFinishAction.First(f => f.First == Globals.Settings.QueueFinishActionSecond);

        cbCheckUpdates.Checked = Globals.Settings.AutoCheckUpdates;
        nsStackQueue.Value = Globals.Settings.StackQueueRandomPercentage;
        cbDisplayColumnHeaders.Checked = Globals.Settings.DisplayPlaylistHeader;
        nsRetryCount.Value = Globals.Settings.PlaybackRetryCount;

        // Audio visualization
        cbDisplayAudioVisualization.Checked = Globals.Settings.DisplayAudioVisualization;
        cmbFftWindowSelect.SelectedValue = ((WindowType)Globals.Settings.FftWindow).ToString();
        cbAudioVisualizationBars.Checked = Globals.Settings.AudioVisualizationBars;
        cbAudioVisualizationBars.Checked = Globals.Settings.AudioVisualizationBars;
        cbVisualizeAudioLevels.Checked = Globals.Settings.DisplayAudioLevels;
        cbLevelsVertical.Checked = Globals.Settings.AudioLevelsHorizontal;

        // Album image
        cbShowAlbumImage.Checked = Globals.Settings.ShowAlbumImage;
        cbAutoHideAlbumImage.Checked = Globals.Settings.AutoHideEmptyAlbumImage;

        // Track title naming.
        tbTrackNamingFormula.Text = Globals.Settings.TrackNameFormula;
        tbRenamedTrackNamingFormula.Text = Globals.Settings.TrackNameFormulaRenamed;
        nsTitleMinimumLength.Value = Globals.Settings.TrackNamingMinimumTitleLength;
        cbFallBackToFileNameIfNoLetters.Checked = Globals.Settings.TrackNamingFallbackToFileNameWhenNoLetters;
        tbHelpFolder.Text = Globals.Settings.HelpFolder;
    }

    private void SaveSettings()
    {
        // Quiet hours.
        Globals.Settings.QuietHours = cbEnableQuietHours.Checked == true;
        Globals.Settings.QuietHoursPause = cbDecreaseVolumeOnQuietHours.Checked != true;
        Globals.Settings.QuietHoursVolumePercentage = nsQuietHourSilenceAmount.Value;
        Globals.Settings.QuietHoursPause = cbPauseOnQuietHours.Checked == true;
        Globals.Settings.QuietHoursFrom = dtpStartQuietHours.Value!.Value.ToString("HH':'mm");
        Globals.Settings.QuietHoursTo = dtpEndQuietHours.Value!.Value.ToString("HH':'mm");

        // Weighted randomization.
        Globals.Settings.BiasedRandom = cbWeightedRandomEnabled.Checked == true;
        Globals.Settings.BiasedRatingEnabled = cbWeightedRating.Checked == true;
        Globals.Settings.BiasedRating = sldRating.Value;
        Globals.Settings.BiasedPlayedCountEnabled = cbWeightedPlayedCount.Checked == true;
        Globals.Settings.BiasedPlayedCount = sldPlayedCount.Value;
        Globals.Settings.BiasedRandomizedCountEnabled = cbWeightedRandomizedCount.Checked == true;
        Globals.Settings.BiasedRandomizedCount = sldRandomizedCount.Value;
        Globals.Settings.BiasedSkippedCountEnabled = cbWeightedSkippedCount.Checked == true;
        Globals.Settings.BiasedSkippedCount = sldSkippedCount.Value;
        Globals.Settings.Tolerance = sldWeightedTolerance.Value;

        // Misc.
        if (cmbUiLocale.SelectedValue != null)
        {
            var culture = (CultureInfo)cmbUiLocale.SelectedValue;
            Globals.Settings.Locale = culture.Name;
        }

        Globals.Settings.QueueFinishActionFirst =
            ((Pair<QueueFinishActionType, string>)cmbQueueFinishActionFirst.SelectedValue).First;

        Globals.Settings.QueueFinishActionSecond =
            ((Pair<QueueFinishActionType, string>)cmbQueueFinishActionSecond.SelectedValue).First;

        Globals.Settings.AutoCheckUpdates = cbCheckUpdates.Checked == true;
        Globals.Settings.StackQueueRandomPercentage = (int)nsStackQueue.Value;
        Globals.Settings.DisplayPlaylistHeader = cbDisplayColumnHeaders.Checked == true;

        // Album image
        Globals.Settings.AutoHideEmptyAlbumImage = cbAutoHideAlbumImage.Checked == true;
        Globals.Settings.ShowAlbumImage = cbShowAlbumImage.Checked == true;

        Globals.Settings.PlaybackRetryCount = (int)nsRetryCount.Value;

        // Audio visualization
        Globals.Settings.DisplayAudioVisualization = cbDisplayAudioVisualization.Checked == true;
        if (Enum.TryParse<WindowType>(cmbFftWindowSelect.SelectedValue.ToString() ?? WindowType.Hanning.ToString(), out var value))
        {
            Globals.Settings.FftWindow = (int)value;
        }

        Globals.Settings.AudioVisualizationBars = cbAudioVisualizationBars.Checked == true;
        Globals.Settings.AudioVisualizationBars = cbAudioVisualizationBars.Checked == true;
        Globals.Settings.DisplayAudioLevels = cbVisualizeAudioLevels.Checked == true;
        Globals.Settings.AudioLevelsHorizontal = cbLevelsVertical.Checked == true;

        // Track title naming.
        Globals.Settings.TrackNameFormula = tbTrackNamingFormula.Text;
        Globals.Settings.TrackNameFormulaRenamed = tbRenamedTrackNamingFormula.Text;
        Globals.Settings.TrackNamingMinimumTitleLength = (int)nsTitleMinimumLength.Value;
        Globals.Settings.TrackNamingFallbackToFileNameWhenNoLetters = cbFallBackToFileNameIfNoLetters.Checked == true;

        Globals.Settings.HelpFolder = tbHelpFolder.Text;

        Globals.SaveSettings();
    }

    [MemberNotNull(nameof(tabCommon),
        nameof(cbEnableQuietHours),
        nameof(dtpStartQuietHours),
        nameof(dtpEndQuietHours),
        nameof(cbPauseOnQuietHours),
        nameof(cbDecreaseVolumeOnQuietHours),
        nameof(nsQuietHourSilenceAmount),
        nameof(cmbUiLocale),
        nameof(cbCheckUpdates),
        nameof(nsStackQueue),
        nameof(cbAutoHideAlbumImage),
        nameof(cbShowAlbumImage)
        )]
    private void CreateSettingsTabCommon()
    {
        cbEnableQuietHours = new CheckBox { Text = Shared.Localization.Settings.EnableQuietHours, };
        dtpStartQuietHours = new DateTimePicker { Mode = DateTimePickerMode.Time, };
        dtpEndQuietHours = new DateTimePicker { Mode = DateTimePickerMode.Time, };
        cbPauseOnQuietHours = new CheckBox { Text = Shared.Localization.Settings.PauseOnQuietHours, };
        cbDecreaseVolumeOnQuietHours = new CheckBox { Text = Shared.Localization.Settings.DecreaseVolumeOnQuietHoursBy, };
        nsQuietHourSilenceAmount = new NumericStepper { MinValue = 0, MaxValue = 100, };
        nsStackQueue = new NumericStepper { MinValue = 0, MaxValue = 100, };
        cbCheckUpdates = new CheckBox { Text = Shared.Localization.Settings.CheckForUpdatesUponStartup, };
        cbAutoHideAlbumImage = new CheckBox { Text = Shared.Localization.Settings.AutoHideAlbumImageWindow, };
        cbShowAlbumImage = new CheckBox { Text = UI.UseTrackImageWindow, };

        cmbUiLocale = new ComboBox
        {
            AutoComplete = true,
            ItemTextBinding = new PropertyBinding<string>(nameof(CultureInfo.DisplayName)),
        };

        cmbUiLocale.DataStore = Shared.Globals.Languages.Select(f => new CultureExtended(f, true));

        var spacing = new Size(Globals.DefaultPadding, Globals.DefaultPadding);

        var quietHourSettings =
            CreateRowTable(true, spacing, dtpStartQuietHours, new Label { Text = UI._, }, dtpEndQuietHours);

        var quietHourPercentageRow =
            CreateRowTable(true, spacing, cbDecreaseVolumeOnQuietHours, nsQuietHourSilenceAmount,
                new Label { Text = Shared.Localization.Settings.Percentage, });

        var stackQueue =
            CreateRowTable(true, spacing, new Label { Text = Shared.Localization.Settings.RandomizeStackFromTopBy, }, nsStackQueue,
                new Label { Text = Shared.Localization.Settings.Percentage, });

        var retryCountRow = CreateRowTable(true, spacing, new Label { Text = UI.RetryCountOnPlaybackFailure, },
            nsRetryCount);

        var selectHelpFolder = CreateRowTable(false, spacing, new Label { Text = UI.HelpFolder, },
            new TableCell(tbHelpFolder, true), btnSelectFolder);

        tabCommon = new TabPage
        {
            Text = Shared.Localization.Settings.Common,
            Content = new TableLayout
            {
                Rows =
                {
                    cbEnableQuietHours,
                    quietHourSettings,
                    cbPauseOnQuietHours,
                    quietHourPercentageRow,
                    new Label { Text = Shared.Localization.Settings.LanguageRequiresRestart,},
                    cmbUiLocale,
                    cbCheckUpdates,
                    new Label { Text = Shared.Localization.Settings.StackQueue,},
                    stackQueue,
                    cbShowAlbumImage,
                    cbAutoHideAlbumImage,
                    cbDisplayColumnHeaders,
                    retryCountRow,
                    selectHelpFolder,
                    new TableRow { ScaleHeight = true,}, // Keep this to the last!
                },
                Spacing = new Size(Globals.DefaultPadding, Globals.DefaultPadding),
                Padding = Globals.DefaultPadding,
            },
        };

        tbcSettings.Pages.Add(tabCommon);
        cbDecreaseVolumeOnQuietHours.CheckedChanged += CbDecreaseVolumeOnQuietHours_CheckedChanged;
        cbPauseOnQuietHours.CheckedChanged += CbPauseOnQuietHours_CheckedChanged;
        cbEnableQuietHours.CheckedChanged += CbEnableQuietHours_CheckedChanged;
        btnSelectFolder.Click += BtnSelectFolder_Click;
    }

    private void BtnSelectFolder_Click(object? sender, EventArgs e)
    {
        using var dialog = new SelectFolderDialog();
        if (dialog.ShowDialog(this) == DialogResult.Ok)
        {
            tbHelpFolder.Text = dialog.Directory;
        }
    }

    private bool suspendQuietHoursSet;

    private void CbEnableQuietHours_CheckedChanged(object? sender, EventArgs e)
    {
        if (suspendQuietHoursSet)
        {
            return;
        }

        QuitHoursSetOneOption();
    }


    private void QuitHoursSetOneOption()
    {
        var previous = suspendQuietHoursSet;
        suspendQuietHoursSet = false;

        if (cbPauseOnQuietHours.Checked == false && cbDecreaseVolumeOnQuietHours.Checked == false &&
            cbEnableQuietHours.Checked == true)
        {
            cbPauseOnQuietHours.Checked = true;
        }

        suspendQuietHoursSet = previous;
    }

    private void CbPauseOnQuietHours_CheckedChanged(object? sender, EventArgs e)
    {
        if (suspendQuietHoursSet)
        {
            return;
        }

        if (cbPauseOnQuietHours.Checked == true)
        {
            cbDecreaseVolumeOnQuietHours.Checked = false;
        }

        QuitHoursSetOneOption();
    }

    private void CbDecreaseVolumeOnQuietHours_CheckedChanged(object? sender, EventArgs e)
    {
        if (suspendQuietHoursSet)
        {
            return;
        }

        if (cbDecreaseVolumeOnQuietHours.Checked == true)
        {
            cbPauseOnQuietHours.Checked = false;
        }

        QuitHoursSetOneOption();
    }

    [MemberNotNull(nameof(tabWeightedRandom))]
    private void CreateSettingsTabRandom()
    {
        var spacing = new Size(Globals.DefaultPadding, Globals.DefaultPadding);

        var randomSliders = new TableLayout
        {
            Rows =
            {
                new TableRow
                {
                    Cells =
                    {
                        new Label { Text = UI.Rating, },
                        new TableCell(sldRating, true),
                        cbWeightedRating,
                    },
                },
                new TableRow
                {
                    Cells =
                    {
                        new Label { Text = Shared.Localization.Settings.PlayedCount, },
                        new TableCell(sldPlayedCount, true),
                        cbWeightedPlayedCount,
                    },
                },
                new TableRow
                {
                    Cells =
                    {
                        new Label { Text = Shared.Localization.Settings.RandomizedCount, },
                        new TableCell(sldRandomizedCount, true),
                        cbWeightedRandomizedCount,
                    },
                },
                new TableRow
                {
                    Cells =
                    {
                        new Label { Text = Shared.Localization.Settings.SkippedCount, },
                        new TableCell(sldSkippedCount, true),
                        cbWeightedSkippedCount,
                    },
                },
                new TableRow
                {
                    Cells =
                    {
                        new Label { Text = Shared.Localization.Settings.Tolerance, },
                        new TableCell(sldWeightedTolerance, true),
                        lbWeightedToleranceValue,
                    },
                },
            },
            Spacing = spacing,
        };

        sldWeightedTolerance.ValueChanged += delegate
        {
            lbWeightedToleranceValue.Text = $"{sldWeightedTolerance.Value}";
        };

        tabWeightedRandom = new TabPage
        {
            Text = Shared.Localization.Settings.ModifiedRandom,
            Content = new TableLayout
            {
                Rows =
                {
                    randomSliders,
                    cbWeightedRandomEnabled,
                    CreateRowTable(true, spacing, btnRandomDefaults),
                    new TableRow { ScaleHeight = true,}, // Keep this to the last!
                },
                Spacing = new Size(Globals.DefaultPadding, Globals.DefaultPadding),
                Padding = Globals.DefaultPadding,
            },
        };

        btnRandomDefaults.Click += delegate { WeightedRandomDefaults(); };

        tbcSettings.Pages.Add(tabWeightedRandom);
    }

    /// <summary>
    /// Creates the settings tab for track naming.
    /// </summary>
    [MemberNotNull(nameof(tabTrackNaming))]
    private void CreateSettingsTabTrackNaming()
    {
        tabTrackNaming = new TabPage
        {
            Text = UI.TrackNaming,
            Content = new TableLayout
            {
                Rows =
                {
                    new Label { Text = UI.TrackNamingFormula, },
                    tbTrackNamingFormula,
                    new Label { Text = UI.RenamedTrackNamingFormula, },
                    tbRenamedTrackNamingFormula,
                    CreateRowTable(false, Globals.DefaultSpacing, new Label { Text = UI.MinimumNameLength, },
                        nsTitleMinimumLength, new Panel()),
                    cbFallBackToFileNameIfNoLetters,
                    new TableRow(new Label { Text = UI.FormulaInstructions, }),
                    btFormulaDefaults,
                    new TableRow { ScaleHeight = true,},
                },
                Spacing = new Size(Globals.DefaultPadding, Globals.DefaultPadding),
                Padding = Globals.DefaultPadding,
            },
        };

        btFormulaDefaults.Click += BtFormulaDefaults_Click;

        tbcSettings.Pages.Add(tabTrackNaming);
    }

    [MemberNotNull(nameof(tabVisualizationSettings))]
    private void CreateVisualizationSettingsTab()
    {
        tabVisualizationSettings = new TabPage
        {
            Text = UI.AudioVisualization,
            Content = new TableLayout
            {
                Rows =
                {
                    new Label { Text = amp.Shared.Localization.Settings.AudioVisualizer, },
                    cbDisplayAudioVisualization,
                    EtoHelpers.LabelWrap(amp.Shared.Localization.Settings.AudioVisualizerFFTWindowFunction, cmbFftWindowSelect),
                    cbAudioVisualizationBars,
                    cbVisualizeAudioLevels,
                    cbLevelsVertical,
                    new TableRow { ScaleHeight = true,},
                },
                Spacing = new Size(Globals.DefaultPadding, Globals.DefaultPadding),
                Padding = Globals.DefaultPadding,
            },
        };

        tbcSettings.Pages.Add(tabVisualizationSettings);
    }

    private void CreateMiscellaneousSettings()
    {
        cmbQueueFinishActionFirst.ItemTextBinding = new PropertyBinding<string>("Second");
        cmbQueueFinishActionSecond.ItemTextBinding = new PropertyBinding<string>("Second");
        cmbQueueFinishActionFirst.DataStore = dataStoreQueueFinishAction;
        cmbQueueFinishActionSecond.DataStore = dataStoreQueueFinishAction;

        tabMiscellaneous.Content = new TableLayout
        {
            Rows =
            {
                new TableRow(lbQueueFinishActionFirst, new TableCell(cmbQueueFinishActionFirst, true)),
                new TableRow(lbQueueFinishActionSecond, new TableCell(cmbQueueFinishActionSecond, true)),
                new TableRow { ScaleHeight = true,},
            },
            Spacing = new Size(Globals.DefaultPadding, Globals.DefaultPadding),
            Padding = Globals.DefaultPadding,
        };

        tbcSettings.Pages.Add(tabMiscellaneous);
    }

    private void BtFormulaDefaults_Click(object? sender, EventArgs e)
    {
        tbTrackNamingFormula.Text = TrackDisplayNameGenerate.FormulaDefault;
        tbRenamedTrackNamingFormula.Text = TrackDisplayNameGenerate.FormulaTrackRenamedDefault;
    }

    private void WeightedRandomDefaults()
    {
        sldRating.Value = 500;
        sldPlayedCount.Value = 0;
        sldRandomizedCount.Value = 0;
        sldSkippedCount.Value = 0;
        sldWeightedTolerance.Value = 10;
        cbWeightedRating.Checked = true;
        cbWeightedPlayedCount.Checked = false;
        cbWeightedRandomizedCount.Checked = false;
        cbWeightedSkippedCount.Checked = false;
        cbWeightedRandomEnabled.Checked = false;
    }

    #region UiElements
    // Common controls
    private readonly TabControl tbcSettings = new();
    private readonly Button btnOk = new() { Text = UI.OK, };
    private readonly Button btnCancel = new() { Text = UI.Cancel, };

    // Common settings tab page
    private TabPage tabCommon;
    private CheckBox cbEnableQuietHours;
    private DateTimePicker dtpStartQuietHours;
    private DateTimePicker dtpEndQuietHours;
    private CheckBox cbPauseOnQuietHours;
    private CheckBox cbDecreaseVolumeOnQuietHours;
    private NumericStepper nsQuietHourSilenceAmount;
    private ComboBox cmbUiLocale;
    private CheckBox cbCheckUpdates;
    private NumericStepper nsStackQueue;
    private CheckBox cbShowAlbumImage;
    private CheckBox cbAutoHideAlbumImage;
    private readonly CheckBox cbDisplayColumnHeaders = new() { Text = Shared.Localization.Settings.DisplayPlaylistColumnHeaders, };
    private readonly NumericStepper nsRetryCount = new() { MinValue = 5, MaxValue = 1000, Value = 20, };
    private readonly Button btnSelectFolder = new() { Text = UI.ThreeDots, };
    private readonly TextBox tbHelpFolder = new() { ReadOnly = true, };

    // Weighted random settings
    private TabPage tabWeightedRandom;
    private readonly Slider sldRating = new() { MinValue = 0, MaxValue = 1000, TickFrequency = 100, };
    private readonly CheckBox cbWeightedRating = new();
    private readonly Slider sldPlayedCount = new() { MinValue = 0, MaxValue = 1000, TickFrequency = 100, };
    private readonly CheckBox cbWeightedPlayedCount = new();
    private readonly Slider sldRandomizedCount = new() { MinValue = 0, MaxValue = 1000, TickFrequency = 100, };
    private readonly CheckBox cbWeightedRandomizedCount = new();
    private readonly Slider sldSkippedCount = new() { MinValue = 0, MaxValue = 1000, TickFrequency = 100, };
    private readonly CheckBox cbWeightedSkippedCount = new();
    private readonly Slider sldWeightedTolerance = new() { MinValue = 0, MaxValue = 100, TickFrequency = 10, };
    private readonly Label lbWeightedToleranceValue = new() { Text = "0", };
    private readonly CheckBox cbWeightedRandomEnabled = new() { Text = Shared.Localization.Settings.WeightedRandomizationEnabled, };
    private readonly Button btnRandomDefaults = new() { Text = Shared.Localization.Settings.Defaults, };

    // Track naming tab page
    private TabPage tabTrackNaming;
    private readonly TextBox tbTrackNamingFormula = new();
    private readonly TextBox tbRenamedTrackNamingFormula = new();
    private readonly NumericStepper nsTitleMinimumLength = new() { MinValue = 3, MaxValue = 100, };
    private readonly CheckBox cbFallBackToFileNameIfNoLetters = new() { Text = UI.IfGeneratedNameContainsNoLettersFallBackToFileName, };
    private readonly Button btFormulaDefaults = new() { Text = Shared.Localization.Settings.Defaults, };

    // Audio visualization tab page
    private TabPage tabVisualizationSettings;
    private readonly CheckBox cbDisplayAudioVisualization = new() { Text = UI.DisplayAudioVisualization, };
    private readonly ComboBox cmbFftWindowSelect = new()
    { DataStore = Enum.GetValues<WindowType>().OrderBy(f => (int)f).Select(f => f.ToString()).ToList(), };
    private readonly CheckBox cbAudioVisualizationBars = new() { Text = UI.BarVisualizationMode, };
    private readonly CheckBox cbVisualizeAudioLevels = new() { Text = UI.VisualizeAudioLevels, };
    private readonly CheckBox cbLevelsVertical = new() { Text = amp.Shared.Localization.Settings.HorizontalLevelVisualization, };

    // Miscellaneous settings tab page
    private readonly TabPage tabMiscellaneous = new() { Text = UI.Miscellaneous, };
    private readonly Label lbQueueFinishActionFirst = new() { Text = UI.FirstActionWhenQueueIsFinished, };
    private readonly ComboBox cmbQueueFinishActionFirst = new();
    private readonly Label lbQueueFinishActionSecond = new() { Text = UI.SecondActionWhenQueueIsFinished, };
    private readonly ComboBox cmbQueueFinishActionSecond = new();

    private const string LocalizationActionPrefix = "QueueAction";


    private readonly List<Pair<QueueFinishActionType, string>> dataStoreQueueFinishAction = Enum.GetValues<QueueFinishActionType>().OrderBy(f => (int)f)
        .Select(f => new Pair<QueueFinishActionType, string> { First = f, Second = Shared.Localization.Settings.ResourceManager.GetString(LocalizationActionPrefix + f) ?? string.Empty }).ToList();
    #endregion
}