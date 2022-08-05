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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using amp.EtoForms.Settings;
using amp.Shared.Extensions;
using amp.Shared.Localization;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom;
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
        MinimumSize = new Size(500, 500);
        AutoSize = false;
        NegativeButtons.Add(btnCancel);
        PositiveButtons.Add(btnOk);
        Shown += FormColorSettings_Shown;
    }

    private void FormColorSettings_Shown(object? sender, EventArgs e)
    {
        CreateTabColors();
    }

    private void CreateTabColors()
    {
        var tableLayoutContent = new TableLayout
        {
            Spacing = new Size(Globals.DefaultPadding, Globals.DefaultPadding),
            Padding = Globals.DefaultPadding,
        };

        var colorConfig = new ColorConfiguration();
        colorConfig.Load(Path.Join(Globals.DataFolder, "colorSettings.json"));

        foreach (var propertyInfo in colorConfig.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            var nullable = propertyInfo.IsNullableProperty();
            if (propertyInfo.PropertyType == typeof(string))
            {
                var text = ColorsDescriptions.ResourceManager.GetString(propertyInfo.Name, new CultureInfo(Globals.Settings.Locale)) ?? UI.ERRORNOTEXT;
                var propertyValue = propertyInfo.GetValue(colorConfig);
                var colorString = propertyValue?.ToString() ?? "";
                var color = propertyValue == null ? null : (Color?)Color.Parse(colorString);

                var row = new LabelColorPickerRow<string>(text, nullable, color, Size20.ic_fluent_eraser_20_filled, new Size(20, 20))
                {
                    DataObject = propertyInfo.Name,
                };

                tableLayoutContent.Rows.Add(row);
            }
        }

        var scrollable = new Scrollable
        {
            Content = tableLayoutContent,
            ExpandContentWidth = true,
            ExpandContentHeight = false,
        };

        tableLayoutContent.Rows.Add(new TableRow { ScaleHeight = true, });
        Content = scrollable;
    }

    private readonly Button btnOk = new() { Text = UI.OK, };
    private readonly Button btnCancel = new() { Text = UI.Cancel, };
}