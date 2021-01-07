#region license
/*
Public domain. Free to be used in any purpose.
*/
#endregion

using System.Collections.Generic;
using System.Windows.Forms;

namespace amp.UtilityClasses.Controls
{
    /// <summary>
    /// An inherited class from the list box which allows the items to be refreshed.
    /// Implements the <see cref="System.Windows.Forms.ListBox" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.ListBox" />
    public class RefreshDungeonListBox : ReaLTaiizor.Controls.DungeonListBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshListBox"/> class.
        /// </summary>
        public void RefreshListBox()
        {
            DoubleBuffered = true;
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
