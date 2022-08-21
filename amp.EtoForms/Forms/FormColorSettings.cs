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
using System.Reflection;
using amp.EtoForms.Settings;
using amp.Shared.Extensions;
using amp.Shared.Localization;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom;
using EtoForms.Controls.Custom.EventArguments;
using FluentIcons.Resources.Filled;

namespace amp.EtoForms.Forms;

internal class FormColorSettings : Dialog
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FormSettings"/> class.
    /// </summary>
    public FormColorSettings()
    {
        Title = $"amp# {UI._} {UI.ColorSettings}";
        ShowInTaskbar = false;
        MinimumSize = new Size(600, 500);
        AutoSize = false;
        NegativeButtons.Add(btnCancel);
        PositiveButtons.Add(btnOk);
        PositiveButtons.Add(btnDefaults);
        Shown += FormColorSettings_Shown;
        btnOk.Click += BtnOk_Click;
        btnDefaults.Click += BtnDefaults_Click;
        btnCancel.Click += BtnCancel_Click;
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void BtnDefaults_Click(object? sender, EventArgs e)
    {
        tableLayoutContent.Rows.Clear();
        Globals.ColorConfiguration.Defaults();
        CreateTabColors();
    }

    private void BtnOk_Click(object? sender, EventArgs e)
    {
        var colorConfig = Globals.ColorConfiguration;

        if (configRows.Any(f => f.ColorChanged))
        {
            foreach (var row in configRows.Where(f => f.ColorChanged))
            {
                var info = colorConfig.GetType().GetProperty(row.DataObject!.ColorPropertyName);

                if (info != null)
                {
                    if (row.DataObject!.ColorPropertyName ==
                        nameof(ColorConfiguration.ColorsSpectrumVisualizerChannels))
                    {
                        colorConfig.ColorsSpectrumVisualizerChannels[row.DataObject.ArrayIndex] = row.SelectedColor!.ToString()!;
                    }
                    else
                    {
                        info.SetValue(colorConfig, row.SelectedColor?.ToString());
                    }
                }
            }
        }

        Globals.SaveColorConfiguration();
        Close();
    }

    private void FormColorSettings_Shown(object? sender, EventArgs e)
    {
        CreateTabColors();
    }

    private static LabelColorPickerRow<ColorUiData> CreateRow(PropertyInfo propertyInfo, ColorConfiguration colorConfig, EventHandler<ColorChangedEventArgs> colorChangedHandler)
    {
        var nullable = propertyInfo.IsNullableProperty();
        var text = ColorsDescriptions.ResourceManager.GetString(propertyInfo.Name,
            new CultureInfo(Globals.Settings.Locale)) ?? UI.ERRORNOTEXT;
        var propertyValue = propertyInfo.GetValue(colorConfig);
        var colorString = propertyValue?.ToString() ?? "";
        var color = propertyValue == null ? null : (Color?)Color.Parse(colorString);

        var row = new LabelColorPickerRow<ColorUiData>(text, nullable, color, Size20.ic_fluent_eraser_20_filled,
            new Size(20, 20), Color.Parse(Globals.ColorConfiguration.ButtonImageDefaultColor))
        {
            DataObject = new ColorUiData(propertyInfo.Name, text, -1),
        };

        row.ColorValueChanged += colorChangedHandler;

        return row;
    }

    private void Row_ColorValueChanged(object? sender, ColorChangedEventArgs e)
    {
        if (cbSynchronizeColors.Checked == true)
        {
            foreach (var row in configRows)
            {
                if (!Equals(sender, row))
                {
                    row.ColorPickerValue = e.Color;
                }
            }
        }
    }

    private static LabelColorPickerRow<ColorUiData> CreateRow(int listIndex, string colorValue, EventHandler<ColorChangedEventArgs> colorChangedHandler)
    {
        var index = listIndex / 2 + 1;

        var text = listIndex % 2 == 0
            ? string.Format(ColorsDescriptions.SpectrumChannel0StartColor, index)
            : string.Format(ColorsDescriptions.SpectrumChannel0EndColor, index);

        var color = Color.Parse(colorValue);

        var row = new LabelColorPickerRow<ColorUiData>(text, false, color, Size20.ic_fluent_eraser_20_filled,
            new Size(20, 20), Color.Parse(Globals.ColorConfiguration.ButtonImageDefaultColor))
        {
            DataObject = new ColorUiData(nameof(ColorConfiguration.ColorsSpectrumVisualizerChannels), text, listIndex),
        };

        row.ColorValueChanged += colorChangedHandler;

        return row;
    }

    private List<LabelColorPickerRow<ColorUiData>> configRows = new();

    [MemberNotNull(nameof(tableLayoutContent))]
    private void CreateTabColors()
    {
        tableLayoutContent = new TableLayout
        {
            Spacing = new Size(Globals.DefaultPadding, Globals.DefaultPadding),
            Padding = Globals.DefaultPadding,
        };

        tableLayoutContent.Rows.Add(cbSynchronizeColors);

        var colorConfig = Globals.ColorConfiguration;

        configRows = new List<LabelColorPickerRow<ColorUiData>>();

        foreach (var propertyInfo in colorConfig.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (propertyInfo.PropertyType == typeof(string))
            {
                configRows.Add(CreateRow(propertyInfo, colorConfig, Row_ColorValueChanged));
            }
        }

        var i = 0;
        while (i < colorConfig.ColorsSpectrumVisualizerChannels.Length)
        {
            var color = colorConfig.ColorsSpectrumVisualizerChannels[i];

            var row = CreateRow(i, color, Row_ColorValueChanged);
            configRows.Add(row);
            i++;


            color = colorConfig.ColorsSpectrumVisualizerChannels[i];
            row = CreateRow(i, color, Row_ColorValueChanged);
            configRows.Add(row);
            i++;
        }

        configRows = configRows.OrderBy(f => f.DataObject?.ColorDescription).ToList();

        foreach (var row in configRows)
        {
            tableLayoutContent.Rows.Add(row);
        }

        var scrollable = new Scrollable
        {
            Content = tableLayoutContent,
            ExpandContentWidth = true,
            ExpandContentHeight = false,
            Size = Size,
        };

        tableLayoutContent.Rows.Add(new TableRow { ScaleHeight = true, });
        Content = scrollable;
    }

    private readonly Button btnOk = new() { Text = UI.OK, };
    private readonly Button btnCancel = new() { Text = UI.Cancel, };
    private readonly Button btnDefaults = new() { Text = Shared.Localization.Settings.Defaults, };
    private TableLayout tableLayoutContent = new();
    private readonly CheckBox cbSynchronizeColors = new() { Text = UI.SynchronizeColors, };
}