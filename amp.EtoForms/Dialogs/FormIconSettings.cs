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

using System.Globalization;
using System.Reflection;
using amp.EtoForms.Classes;
using amp.EtoForms.Properties;
using amp.EtoForms.Settings;
using amp.Shared.Localization;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom.Utilities;
using FluentIcons.Resources.Filled;
using VPKSoft.ApplicationSettingsJson;
using TableLayout = Eto.Forms.TableLayout;

namespace amp.EtoForms.Dialogs;

internal class FormIconSettings : Dialog
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FormIconSettings"/> class.
    /// </summary>
    public FormIconSettings()
    {
        Title = $"amp# {UI._} {UI.IconSettings}";
        CreateLayout();
        _ = DefaultCancelButtonHandler.WithWindow(this).WithCancelButton(btnCancel).WithDefaultButton(btnOk);
    }

    private void CreateLayout()
    {
        MinimumSize = new Size(600, 600);

        var tableLayout = new TableLayout
        {
            Rows =
            {
                new TableRow(
                    new TableLayout
                    {
                        Rows =
                        {
                            new TableRow(
                                new TableCell(listCustomIcons),
                                new TableCell
                                {
                                    Control = new TableLayout
                                    {
                                        Rows =
                                        {
                                            new Label { Text = UI.SingleIconSetting,},
                                            new Label { Text = UI.Icon,},
                                            ivCustomIcon,
                                            new Label { Text = UI.IconColor,},
                                            cpCustomIconSelector,
                                            cbColorizeIcon,
                                            new TableRow { ScaleHeight = true,},
                                        },
                                    }
                                }
                            ) { ScaleHeight = true, },
                        },
                        Spacing = Globals.DefaultSpacing,
                        Padding = Globals.DefaultPadding,
                    }),
            },
            Spacing = Globals.DefaultSpacing,
            Padding = Globals.DefaultPadding,
        };

        Content = tableLayout;

        NegativeButtons.Add(btnCancel);
        PositiveButtons.Add(btnOk);
        PositiveButtons.Add(btnDefaults);

        ListIconSettings(false);
        listCustomIcons.SelectedValueChanged += ListCustomIcons_SelectedValueChanged;
        ivCustomIcon.MouseDown += IvCustomIcon_MouseDown;
        btnDefaults.Click += BtnDefaults_Click;
        btnOk.Click += BtnOk_Click;
        btnCancel.Click += BtnCancel_Click;
        cpCustomIconSelector.ValueChanged += CpCustomIconSelector_ValueChanged;
        cbColorizeIcon.CheckedChanged += CbColorizeIcon_CheckedChanged;
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void BtnOk_Click(object? sender, EventArgs e)
    {
        UpdateIconSettings();
        Globals.SaveCustomIcons();
        Close();
    }

    void RefreshList()
    {
        // Dummy update via data store.
        listCustomIcons.DataStore = listCustomIcons.DataStore;
    }

    private void DisplaySelected()
    {
        SuspendChangedEvents = true;
        var iconData = (IconData?)listCustomIcons.SelectedValue;
        if (iconData != null)
        {
            ivCustomIcon.Image = CreateImage(iconData);
            cpCustomIconSelector.Value = iconData.NoColorChange || iconData.OverrideColor == null
                ? default
                : Color.Parse(iconData.OverrideColor);
            cbColorizeIcon.Checked = !iconData.NoColorChange;
        }
        else
        {
            ivCustomIcon.Image = null;
            cpCustomIconSelector.Value = default;
            cbColorizeIcon.Checked = false;
        }

        SuspendChangedEvents = false;
    }

    private void IvCustomIcon_MouseDown(object? sender, MouseEventArgs e)
    {
        SetIconDataAction(iconData =>
        {
            using var dialog = new OpenFileDialog();
            dialog.Filters.Add(new FileFilter(UI.ScalableVectorGraphicFiles, ".svg"));
            if (dialog.ShowDialog(this) == DialogResult.Ok)
            {
                iconData.SvgData = File.ReadAllBytes(dialog.FileName);
                iconData.IsCustom = true;
                iconData.Image.Dispose();
                iconData.Image = CreateListImage(iconData);
            }
        });
    }

    private void CpCustomIconSelector_ValueChanged(object? sender, EventArgs e)
    {
        SetIconDataAction(iconData =>
        {
            iconData.OverrideColor = cpCustomIconSelector.Value.ToString();
            iconData.Image = CreateListImage(iconData);
        });
    }

    private void CbColorizeIcon_CheckedChanged(object? sender, EventArgs e)
    {
        SetIconDataAction(iconData =>
        {
            iconData.NoColorChange = cbColorizeIcon.Checked == false;
            cpCustomIconSelector.Enabled = cbColorizeIcon.Checked == true;
            iconData.Image = CreateListImage(iconData);
        });
    }

    private int suspendCount;

    private bool SuspendChangedEvents
    {
        get => suspendCount < 0;

        set
        {
            if (value)
            {
                suspendCount--;
            }
            else
            {
                suspendCount++;
            }
        }
    }

    private void SetIconDataAction(Action<IconData> action)
    {
        if (SuspendChangedEvents)
        {
            return;
        }

        var iconData = (IconData?)listCustomIcons.SelectedValue;
        if (iconData != null)
        {
            SuspendChangedEvents = true;
            action(iconData);
            RefreshList();
            DisplaySelected();
            SuspendChangedEvents = false;
        }
    }


    private void BtnDefaults_Click(object? sender, EventArgs e)
    {
        ListIconSettings(true);
    }

    private void ListCustomIcons_SelectedValueChanged(object? sender, EventArgs e)
    {
        DisplaySelected();
    }

    private void UpdateIconSettings()
    {
        var dataSource = (List<IconData>)listCustomIcons.DataStore;
        var publicMembers = typeof(CustomIcons).GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(f => f.CustomAttributes.Any(a => a.AttributeType == typeof(SettingsAttribute))).ToList();

        foreach (var item in dataSource)
        {
            if (item.IsCustom)
            {
                var propertyInfo = publicMembers.FirstOrDefault(f => f.Name == item.Name);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(Globals.CustomIconSettings,
                        new CustomIcon
                        {
                            Color = item.OverrideColor,
                            IconData = item.SvgData,
                            PreserveOriginalColor = item.NoColorChange,
                            IconHeight = item.ImageDisplaySize.Height,
                            IconWidth = item.ImageDisplaySize.Width,
                        });
                }
            }
        }
    }

    private void ListIconSettings(bool defaults)
    {
        var iconList = new List<IconData>();

        var menuColor = Color.Parse(Globals.ColorConfiguration.MenuItemImageColor);
        var menuColorAlternate = Color.Parse(Globals.ColorConfiguration.MenuItemImageAlternateColor);

        if (defaults)
        {
            Globals.CustomIconSettings.Defaults();
        }

        // File menu.
        var item = CreateItemImage(nameof(CustomIcons.AddMusicFiles), Globals.CustomIconSettings.AddMusicFiles,
            menuColorAlternate, Size20.ic_fluent_collections_add_20_filled);
        iconList.Add(item);

        item = CreateItemImage(nameof(CustomIcons.Album), Globals.CustomIconSettings.Album,
            menuColor, Size16.ic_fluent_music_note_2_16_filled);
        iconList.Add(item);

        item = CreateItemImage(nameof(CustomIcons.TrackInformation), Globals.CustomIconSettings.TrackInformation,
            menuColor, Size16.ic_fluent_info_16_filled);
        iconList.Add(item);

        item = CreateItemImage(nameof(CustomIcons.QuitApplication), Globals.CustomIconSettings.QuitApplication,
            menuColor, Size20.ic_fluent_arrow_exit_20_filled);
        iconList.Add(item);

        // Queue menu.
        item = CreateItemImage(nameof(CustomIcons.SaveCurrentQueue), Globals.CustomIconSettings.SaveCurrentQueue,
            menuColor, Size16.ic_fluent_save_16_filled);
        iconList.Add(item);

        item = CreateItemImage(nameof(CustomIcons.SavedQueues), Globals.CustomIconSettings.SavedQueues,
            menuColor, Resources.queue_three_dots);
        iconList.Add(item);

        item = CreateItemImage(nameof(CustomIcons.ClearQueue), Globals.CustomIconSettings.ClearQueue,
            menuColor, Resources.queue_three_dots_clear);
        iconList.Add(item);

        item = CreateItemImage(nameof(CustomIcons.ScrambleQueue), Globals.CustomIconSettings.ScrambleQueue,
            menuColor, Size16.ic_fluent_re_order_dots_vertical_16_filled);
        iconList.Add(item);

        item = CreateItemImage(nameof(CustomIcons.StashQueue), Globals.CustomIconSettings.StashQueue,
            menuColor, Size20.ic_fluent_arrow_down_20_filled);
        iconList.Add(item);

        item = CreateItemImage(nameof(CustomIcons.PopStashedQueue), Globals.CustomIconSettings.PopStashedQueue,
            menuColor, Size20.ic_fluent_arrow_export_up_20_filled);
        iconList.Add(item);

        // Tools menu.
        item = CreateItemImage(nameof(CustomIcons.Settings), Globals.CustomIconSettings.Settings,
            menuColor, Size16.ic_fluent_settings_16_filled);
        iconList.Add(item);

        item = CreateItemImage(nameof(CustomIcons.ColorSettings), Globals.CustomIconSettings.ColorSettings,
            menuColor, Size16.ic_fluent_color_16_filled);
        iconList.Add(item);

        item = CreateItemImage(nameof(CustomIcons.UpdateTrackMetadata), Globals.CustomIconSettings.UpdateTrackMetadata,
            menuColor, Size16.ic_fluent_arrow_clockwise_16_filled);
        iconList.Add(item);

        item = CreateItemImage(nameof(CustomIcons.IconSettings), Globals.CustomIconSettings.IconSettings,
            menuColor, Size20.ic_fluent_icons_20_filled);
        iconList.Add(item);

        // Help menu.
        item = CreateItemImage(nameof(CustomIcons.HelpAbout), Globals.CustomIconSettings.HelpAbout,
            menuColorAlternate, Size20.ic_fluent_question_circle_20_filled);
        iconList.Add(item);

        item = CreateItemImage(nameof(CustomIcons.Help), Globals.CustomIconSettings.Help,
            menuColor, Size20.ic_fluent_book_search_20_filled);
        iconList.Add(item);

        item = CreateItemImage(nameof(CustomIcons.CheckNewVersion), Globals.CustomIconSettings.CheckNewVersion,
            menuColor, Size16.ic_fluent_arrow_download_16_filled);
        iconList.Add(item);

        // Playback toolbar.
        var buttonColor = Color.Parse(Globals.ColorConfiguration.PreviousButtonColor);
        item = CreateItemImage(nameof(CustomIcons.PreviousTrack), Globals.CustomIconSettings.PreviousTrack,
            buttonColor, Size16.ic_fluent_previous_16_filled);
        iconList.Add(item);

        buttonColor = Color.Parse(Globals.ColorConfiguration.PlayButtonPlayColor);
        item = CreateItemImage(nameof(CustomIcons.Play), Globals.CustomIconSettings.Play,
            buttonColor, Size16.ic_fluent_play_16_filled);
        iconList.Add(item);

        buttonColor = Color.Parse(Globals.ColorConfiguration.PlayButtonPauseColor);
        item = CreateItemImage(nameof(CustomIcons.Pause), Globals.CustomIconSettings.Pause,
            buttonColor, Size16.ic_fluent_pause_16_filled);
        iconList.Add(item);

        buttonColor = Color.Parse(Globals.ColorConfiguration.NextButtonColor);
        item = CreateItemImage(nameof(CustomIcons.NextTrack), Globals.CustomIconSettings.NextTrack,
            buttonColor, Size16.ic_fluent_next_16_filled);
        iconList.Add(item);

        buttonColor = Color.Parse(Globals.ColorConfiguration.QueueButtonColor);
        item = CreateItemImage(nameof(CustomIcons.ShowQueue), Globals.CustomIconSettings.ShowQueue,
            buttonColor, Resources.queue_three_dots);
        iconList.Add(item);

        buttonColor = Color.Parse(Globals.ColorConfiguration.ShuffleButtonColor);
        item = CreateItemImage(nameof(CustomIcons.Shuffle), Globals.CustomIconSettings.Shuffle,
            buttonColor, Resources.shuffle_random_svgrepo_com_modified);
        iconList.Add(item);

        buttonColor = Color.Parse(Globals.ColorConfiguration.RepeatButtonColor);
        item = CreateItemImage(nameof(CustomIcons.Repeat), Globals.CustomIconSettings.Repeat,
            buttonColor, Resources.repeat_svgrepo_com_modified);
        iconList.Add(item);

        buttonColor = Color.Parse(Globals.ColorConfiguration.StackQueueButtonColor);
        item = CreateItemImage(nameof(CustomIcons.StackQueue), Globals.CustomIconSettings.StackQueue,
            buttonColor, Resources.stack_queue_three_dots);
        iconList.Add(item);

        // Album, Volume and Rating.
        var itemColor = Color.Parse(Globals.ColorConfiguration.TheMusicNoteColor);
        item = CreateItemImage(nameof(CustomIcons.AlbumLookup), Globals.CustomIconSettings.AlbumLookup,
            itemColor, Size16.ic_fluent_music_note_2_16_filled);
        iconList.Add(item);

        itemColor = Color.Parse(Globals.ColorConfiguration.ColorSpeakerMainVolume);
        item = CreateItemImage(nameof(CustomIcons.MainVolumeSliderSpeaker), Globals.CustomIconSettings.MainVolumeSliderSpeaker,
            itemColor, Size16.ic_fluent_speaker_2_16_filled);
        iconList.Add(item);

        itemColor = Color.Parse(Globals.ColorConfiguration.ColorSpeakerTrackVolume);
        item = CreateItemImage(nameof(CustomIcons.TrackVolumeSliderSpeaker), Globals.CustomIconSettings.TrackVolumeSliderSpeaker,
            itemColor, Size16.ic_fluent_speaker_2_16_filled);
        iconList.Add(item);

        itemColor = Color.Parse(Globals.ColorConfiguration.ColorRatingSlider);
        item = CreateItemImage(nameof(CustomIcons.Rating), Globals.CustomIconSettings.Rating,
            itemColor, Size16.ic_fluent_star_16_filled);
        iconList.Add(item);

        // Sliders, etc.
        itemColor = Color.Parse(Globals.ColorConfiguration.ColorMainVolumeValueIndicator);
        item = CreateItemImage(nameof(CustomIcons.MainVolumeSlider), Globals.CustomIconSettings.MainVolumeSlider,
            itemColor, global::EtoForms.Controls.Custom.Properties.Resources.slider_mark, new Size(15, 30));
        iconList.Add(item);

        itemColor = Color.Parse(Globals.ColorConfiguration.ColorTrackVolumeValueIndicator);
        item = CreateItemImage(nameof(CustomIcons.TrackVolumeSlider), Globals.CustomIconSettings.TrackVolumeSlider,
            itemColor, global::EtoForms.Controls.Custom.Properties.Resources.slider_mark, new Size(15, 30));
        iconList.Add(item);

        itemColor = Color.Parse(Globals.ColorConfiguration.ColorPositionSliderValueIndicator);
        item = CreateItemImage(nameof(CustomIcons.PositionSlider), Globals.CustomIconSettings.PositionSlider,
            itemColor, global::EtoForms.Controls.Custom.Properties.Resources.slider_mark, new Size(15, 30));
        iconList.Add(item);

        // Miscellaneous.
        itemColor = Color.Parse(Globals.ColorConfiguration.ClearSearchButtonColor);
        item = CreateItemImage(nameof(CustomIcons.ClearSearch), Globals.CustomIconSettings.ClearSearch,
            itemColor, Size20.ic_fluent_eraser_20_filled);
        iconList.Add(item);

        listCustomIcons.ItemTextBinding = new PropertyBinding<string>(nameof(IconData.Description));
        listCustomIcons.ItemImageBinding = new PropertyBinding<Image>(nameof(IconData.Image));

        listCustomIcons.DataStore = iconList;
    }

    private static IconData CreateItemImage(
        string name,
        CustomIcon? customIcon,
        Color? itemColor,
        byte[] defaultSvgData, Size? customSize = null)
    {
        var resMan = UiImageNames.ResourceManager;
        var size = customSize ?? new Size(30, 30);
        var iconText = resMan.GetString(name, new CultureInfo(Globals.Settings.Locale)) ?? UI.ERRORNOTEXT;

        var color = customIcon?.Color == null && customIcon?.PreserveOriginalColor != true ? itemColor : Color.Parse(customIcon.Color);

        if (customIcon?.PreserveOriginalColor == true)
        {
            color = null;
        }

        var svg = color == null
        ? EtoHelpers.ImageFromSvg(customIcon?.IconData ?? defaultSvgData, size)
        : EtoHelpers.ImageFromSvg(color.Value,
                customIcon?.IconData ?? defaultSvgData, size);

        return new IconData(name, iconText, svg)
        {
            NoColorChange = customIcon?.PreserveOriginalColor == true,
            OverrideColor = color?.ToString(),
            SvgData = customIcon?.IconData ?? defaultSvgData,
            IsCustom = customIcon != null,
            ImageDisplaySize = size,
        };
    }

    private static Image CreateListImage(IconData iconData)
    {
        return CreateImage(iconData, iconData.ImageDisplaySize);
    }

    private static Image CreateImage(IconData iconData, Size? size = null)
    {
        return iconData.NoColorChange || iconData.OverrideColor == null ?
        EtoHelpers.ImageFromSvg(iconData.SvgData, size ?? imageViewSize)
            : EtoHelpers.ImageFromSvg(Color.Parse(iconData.OverrideColor),
            iconData.SvgData, size ?? imageViewSize);
    }

    private static readonly Size imageViewSize = new(300, 300);
    private readonly ListBox listCustomIcons = new() { Height = 400, Width = 300, };
    private readonly ImageView ivCustomIcon = new() { Width = 300, Height = 300, Cursor = Cursors.Pointer, };
    private readonly ColorPicker cpCustomIconSelector = new();
    private readonly CheckBox cbColorizeIcon = new() { Text = UI.ColorizeIcon, };
    private readonly Button btnOk = new() { Text = UI.OK, };
    private readonly Button btnCancel = new() { Text = UI.Cancel, };
    private readonly Button btnDefaults = new() { Text = Shared.Localization.Settings.Defaults, };
}