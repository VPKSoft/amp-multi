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

using amp.DataAccessLayer.DtoClasses;
using amp.EtoForms.Forms.EventArguments;
using amp.Shared.Classes;
using amp.Shared.Localization;
using ATL;
using Eto.Drawing;
using Eto.Forms;
using EtoForms.Controls.Custom.Utilities;
using FluentIcons.Resources.Filled;

namespace amp.EtoForms.Forms;

/// <summary>
/// A dialog to view and edit audio trackTag data.
/// Implements the <see cref="Dialog" />
/// </summary>
/// <seealso cref="Dialog" />
public partial class FormDialogTrackInfo : Dialog
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FormDialogTrackInfo"/> class.
    /// </summary>
    /// <param name="audioTrack">The audio trackTag data.</param>
    /// <param name="audioTrackChanged">The event handler for the <see cref="AudioTrackChanged"/> event.</param>
    public FormDialogTrackInfo(AudioTrack audioTrack,
        EventHandler<AudioTrackChangedEventArgs>? audioTrackChanged = null)
    {
        Title = $"amp# - {audioTrack.DisplayName}";
        MinimumSize = new Size(800, 600);
        AudioTrackChanged += audioTrackChanged;
        this.audioTrack = audioTrack;
        imageOff = EtoHelpers.ImageFromSvg(Color.Parse(Globals.ColorConfiguration.ColorTrackMissingImage), Size16.ic_fluent_image_multiple_off_16_filled,
            new Size(300, 300));

        LayoutTagData();
        LayoutAmpData();
        LayoutRest();

        trackTag = new Track(audioTrack.FileName);
        SetData();
        AttachEvents();
        defaultCancelButtonHandler = DefaultCancelButtonHandler.WithWindow(this).WithBoth(btnClose);
        Closed += FormDialogTrackInfo_Closed;
    }

    private void FormDialogTrackInfo_Closed(object? sender, EventArgs e)
    {
        defaultCancelButtonHandler.Dispose();
    }

    private readonly Image imageOff;
    private readonly List<Image> embeddedImages = new();
    private int embeddedImageIndex = -1;
    private readonly AudioTrack audioTrack;
    private readonly Track trackTag;
    private bool ampDataChanged;
    private bool tagDataChanged;
    private Bitmap? copiedFromTag;
    private readonly DefaultCancelButtonHandler defaultCancelButtonHandler;

    private bool TagDataChanged
    {
        set
        {
            if (value != tagDataChanged)
            {
                tagDataChanged = value;
                UpdateButtonsEnabled();
            }
        }
    }

    private bool AmpDataChanged
    {
        set
        {
            if (value != ampDataChanged)
            {
                ampDataChanged = value;
                UpdateButtonsEnabled();
            }
        }
    }

    private void BtnSaveChanges_Click(object? sender, EventArgs e)
    {
        if (tcTagTabs.SelectedIndex == 0)
        {
            if (MessageBox.Show(this, Messages.SaveTheTagChangedThisWillModifyTheActualAudioFile, Messages.Confirmation,
                    MessageBoxButtons.OKCancel, MessageBoxType.Question) == DialogResult.Ok)
            {
                ModifyTagData();
                if (!trackTag.Save())
                {
                    MessageBox.Show(this, UI.TagSaveFailed, UI.SaveFailed, MessageBoxButtons.OK);
                }
                else
                {
                    TagDataChanged = false;
                }
            }
        }

        if (tcTagTabs.SelectedIndex == 1)
        {
            ModifyAmpData();
            audioTrack.TagFindString = new Track(audioTrack.FileName).GetTagString();
            var args = new AudioTrackChangedEventArgs(audioTrack);
            AudioTrackChanged?.Invoke(this, args);
            if (!args.SaveSuccess)
            {
                MessageBox.Show(this, UI.TagSaveFailed, UI.SaveFailed, MessageBoxButtons.OK);
            }
            else
            {
                AmpDataChanged = false;
            }
        }

        UpdateButtonsEnabled();
    }

    /// <summary>
    /// Occurs when audio trackTag data has been changed.
    /// </summary>
    public event EventHandler<AudioTrackChangedEventArgs>? AudioTrackChanged;

    private readonly ButtonToolItem btnCopyToAmp = new() { ToolTip = UI.CopyTagDataToAmp, };
    private readonly ButtonToolItem btnCopyToTag = new() { ToolTip = UI.CopyAmpDataToTag, Visible = false, };
    private readonly ButtonToolItem btnSaveChanges = new() { ToolTip = UI.SaveActiveTabChanges, Visible = false, };
    private readonly ButtonToolItem btnLoadImage = new() { ToolTip = UI.LoadImageFromFile, Visible = false, };
    private readonly ButtonToolItem btnClearImage = new() { ToolTip = UI.RemoveTrackImage, Visible = false, };

    private void TcTagTabs_SelectedIndexChanged(object? sender, EventArgs e)
    {
        btnCopyToAmp.Visible = tcTagTabs.SelectedIndex == 0;
        btnCopyToTag.Visible = tcTagTabs.SelectedIndex == 1;
        btnLoadImage.Visible = tcTagTabs.SelectedIndex == 1;
        UpdateButtonsEnabled();
    }

    private void CopyToTagClick(object? sender, EventArgs e)
    {
        CopyToTag();
    }

    private void CopyToAmpClick(object? sender, EventArgs e)
    {
        CopyToAmp();
    }

    private void AmpDataTextChanged(object? sender, EventArgs e)
    {
        AmpDataChanged = true;
    }

    private void TagDataTextChanged(object? sender, EventArgs e)
    {
        TagDataChanged = true;
    }
}