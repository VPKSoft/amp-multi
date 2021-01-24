#region License
/*
MIT License

Copyright(c) 2021 Petteri Kautonen

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
using System.ComponentModel;
using System.Windows.Forms;

namespace AmpControls
{
    /// <summary>
    /// An inherited class from the list box which allows the items to be refreshed.
    /// Implements the <see cref="System.Windows.Forms.ListBox" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.ListBox" />
    public class RefreshListbox : ListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshListBox"/> class.
        /// </summary>
        public void RefreshListBox()
        {
            DoubleBuffered = true;
        }

        // ReSharper disable four times InconsistentNaming, WinApi constant..
        // ReSharper disable four times IdentifierTypo, WinApi constant..
        private const int LB_ADDSTRING = 0x180;
        private const int LB_INSERTSTRING = 0x181;
        private const int LB_DELETESTRING = 0x182;
        private const int LB_RESETCONTENT = 0x184;
        private const int WM_VSCROLL = 0x0115;
        private const int WM_MOUSEWHEEL = 0x020A;
        private const int WM_KEYDOWN = 0x0100;


        /// <summary>
        /// The list's window procedure.
        /// </summary>
        /// <param name="m">A Windows Message Object.</param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == LB_ADDSTRING ||
                m.Msg == LB_INSERTSTRING ||
                m.Msg == LB_DELETESTRING ||
                m.Msg == LB_RESETCONTENT)
            {
                ItemsChanged?.Invoke(this, new EventArgs());
            }

            // if the message indicates that the TopIndex property of the list box might have changed, raise the VScrollChanged event..
            if (m.Msg == WM_VSCROLL || m.Msg == WM_MOUSEWHEEL || m.Msg == WM_KEYDOWN)
            {
                // ..if not denied and the TopIndex property was actually changed..
                if (lastTopIndex != this.TopIndex)
                {
                    lastTopIndex = TopIndex; // save the top index so it's changes can be monitored..

                    // if the VScrollChanged event is subscribed the raise the event with (FromControl = true)..
                    VScrollChanged?.Invoke(this, new VScrollChangedEventArgs()
                        { Minimum = 0, Maximum = Items.Count, Value = VScrollPosition, FromControl = true });
                }
            }

            base.WndProc(ref m);
        }

        // a latest saved TopIndex property value to avoid unnecessary event raising. Initialize with a ridiculous value..
        internal int lastTopIndex = int.MinValue;


        /// <summary>
        /// Occurs when the <see cref="ListBox.Items"/> collection changes.
        /// </summary>
        public event EventHandler ItemsChanged;

        /// <summary>
        /// An event that is raised if the vertical scroll position was changed either by code or by the control.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public event EventHandler<VScrollChangedEventArgs> VScrollChanged;

        /// <summary>
        /// Gets or sets the VScrollPosition property value.
        /// </summary>
        [Browsable(false)] // hide from the designer..
        public int VScrollPosition
        {
            get
            {
                if (Items.Count == 0) // do nothing if there are no items..
                {
                    return 0; // .. except return zero..
                }

                return TopIndex;
            }

            set
            {
                if (value < 0) // do not handle stupid values..
                {
                    return;
                }

                if (Items.Count == 0) // do nothing if there are no items..
                {
                    TopIndex = 0; // .. except set the value to zero..
                }
                else if (value <= Items.Count) // set the TopIndex (VScrollPosition), if the value is valid..
                {
                    TopIndex = value;
                }
                else // ..otherwise complain via an exception..
                {
                    throw new ArgumentOutOfRangeException(nameof(VScrollPosition));
                }
            }
        }

        /// <summary>
        /// Refreshes the item contained at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to refresh.</param>
        public new void RefreshItem(int index)
        {
            int t = TopIndex;
            base.RefreshItem(index);
            try
            {
                TopIndex = t;
            }
            catch
            {
                // ignored..
            }
        }

        /// <summary>
        /// Selects an item from the list box with specific index.
        /// </summary>
        /// <param name="index">The index.</param>
        public void SetIndex(int index)
        {
            try
            {
                if (SelectedIndices.Count > 1 && SelectedIndex != index)
                {
                    ClearSelected();
                }
                SelectedIndex = index;
            }
            catch
            {
                // ignored..
            }
        }

        private readonly List<int> pushedSelection = new List<int>();

        /// <summary>
        /// Pushes the selection in to an internal list which can be then restored by using <see cref="PopSelection"/> method.
        /// </summary>
        public void PushSelection()
        {
            pushedSelection.Clear();
            for (int i = 0; i < SelectedIndices.Count; i++)
            {
                pushedSelection.Add(SelectedIndices[i]);
            }
        }

        /// <summary>
        /// Pops the selection saved by using the <see cref="PushSelection"/> method.
        /// </summary>
        public void PopSelection()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                SetSelected(i, pushedSelection.IndexOf(i) != -1);
            }
        }

        /// <summary>
        /// Refreshes all <see cref="T:System.Windows.Forms.ListBox" /> items and retrieves new strings for them.
        /// </summary>
        public new void RefreshItems()
        {
            int t = TopIndex;
            base.RefreshItems();
            try
            {
                TopIndex = t;
            }
            catch
            {
                // ignored..
            }
        }
    }
}
