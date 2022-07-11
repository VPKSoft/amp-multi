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
using amp.Shared.Classes;
using amp.Shared.Localization;
using Eto.Drawing;
using Eto.Forms;

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
        cbEnableQuietHours.Checked = Globals.Settings.QuietHours;
        cbDecreaseVolumeOnQuietHours.Checked = !Globals.Settings.QuietHoursPause;
        nsQuietHourSilenceAmount.Value = Globals.Settings.QuietHoursVolumePercentage;
        cbPauseOnQuietHours.Checked = Globals.Settings.QuietHoursPause;
        dtpStartQuietHours.Value = DateTime.ParseExact(Globals.Settings.QuietHoursFrom!, "HH':'mm", CultureInfo.InvariantCulture);
        dtpEndQuietHours.Value = DateTime.ParseExact(Globals.Settings.QuietHoursTo!, "HH':'mm", CultureInfo.InvariantCulture);

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

        cbCheckUpdates.Checked = Globals.Settings.AutoCheckUpdates;
        nsStackQueue.Value = Globals.Settings.StackQueueRandomPercentage;
        cbAutoHideAlbumImage.Checked = Globals.Settings.AutoHideEmptyAlbumImage;
        cbDisplayColumnHeaders.Checked = Globals.Settings.DisplayPlaylistHeader;
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

        Globals.Settings.AutoCheckUpdates = cbCheckUpdates.Checked == true;
        Globals.Settings.StackQueueRandomPercentage = nsStackQueue.Value;
        Globals.Settings.AutoHideEmptyAlbumImage = cbAutoHideAlbumImage.Checked == true;
        Globals.Settings.DisplayPlaylistHeader = cbDisplayColumnHeaders.Checked == true;
        Globals.SaveSettings();
    }


    private TableLayout CreateRowTable(bool lastScaleWidth, Size spacing, params object[] controls)
    {
        var tableRow = new TableRow();
        var result = new TableLayout
        {
            Rows =
            {
                tableRow,
            },
            Spacing = spacing,
        };

        foreach (var control in controls)
        {
            if (control is Control c)
            {
                tableRow.Cells.Add(new TableCell(c));

            }

            if (control is TableCell tc)
            {
                tableRow.Cells.Add(tc);
            }
        }

        if (lastScaleWidth)
        {
            tableRow.Cells.Add(new TableCell { ScaleWidth = true, });
        }

        return result;
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
        nameof(cbAutoHideAlbumImage)
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
                    cbAutoHideAlbumImage,
                    cbDisplayColumnHeaders,
                    new TableRow { ScaleHeight = true,}, // Keep this to the last!
                },
                Spacing = new Size(Globals.DefaultPadding, Globals.DefaultPadding),
                Padding = Globals.DefaultPadding,
            },
        };

        tbcSettings.Pages.Add(tabCommon);
    }

    [MemberNotNull(
        nameof(tabWeightedRandom)
    )]
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
    private readonly CheckBox cbWeightedRandomEnabled = new() { Text = Shared.Localization.Settings.WeightedRandomizationEnabled, };
    private readonly CheckBox cbWeightedRating = new();
    private readonly Slider sldRating = new() { MinValue = 0, MaxValue = 1000, TickFrequency = 100, };
    private readonly CheckBox cbWeightedPlayedCount = new();
    private readonly Slider sldPlayedCount = new() { MinValue = 0, MaxValue = 1000, TickFrequency = 100, };
    private readonly CheckBox cbWeightedRandomizedCount = new();
    private readonly Slider sldRandomizedCount = new() { MinValue = 0, MaxValue = 1000, TickFrequency = 100, };
    private readonly CheckBox cbWeightedSkippedCount = new();
    private readonly Slider sldSkippedCount = new() { MinValue = 0, MaxValue = 1000, TickFrequency = 100, };
    private readonly Slider sldWeightedTolerance = new() { MinValue = 0, MaxValue = 100, TickFrequency = 10, };
    private readonly Label lbWeightedToleranceValue = new() { Text = "0", };
    private readonly Button btnRandomDefaults = new() { Text = Shared.Localization.Settings.Defaults, };

    private readonly CheckBox cbDisplayColumnHeaders = new() { Text = Shared.Localization.Settings.DisplayPlaylistColumnHeaders, };
    private CheckBox cbAutoHideAlbumImage;
    private CheckBox cbCheckUpdates;
    private ComboBox cmbUiLocale;
    private NumericStepper nsQuietHourSilenceAmount;
    private NumericStepper nsStackQueue;
    private DateTimePicker dtpStartQuietHours;
    private DateTimePicker dtpEndQuietHours;
    private CheckBox cbEnableQuietHours;
    private CheckBox cbPauseOnQuietHours;
    private CheckBox cbDecreaseVolumeOnQuietHours;
    private TabPage tabCommon;
    private TabPage tabWeightedRandom;
    private readonly TabControl tbcSettings = new();
    private readonly Button btnOk = new() { Text = UI.OK, };
    private readonly Button btnCancel = new() { Text = UI.Cancel, };
    #endregion
}