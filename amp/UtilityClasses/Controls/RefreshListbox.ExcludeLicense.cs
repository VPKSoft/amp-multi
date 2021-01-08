#region license
/*
Public domain. Free to be used in any purpose.
*/
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace amp.UtilityClasses.Controls
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
            base.WndProc(ref m);
        }

        /// <summary>
        /// Occurs when the <see cref="ListBox.Items"/> collection changes.
        /// </summary>
        public event EventHandler ItemsChanged;

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
