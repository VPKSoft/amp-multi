#region license
/*
Public domain. Free to be used in any purpose.
*/
#endregion

using System.Collections.Generic;
using System.Windows.Forms;

namespace amp
{
    public partial class RefreshListbox : ListBox
    {
        public void RefreshListBox()
        {
            DoubleBuffered = true;
        }

        public new void RefreshItem(int index)
        {
            int t = TopIndex;
            base.RefreshItem(index);
            try
            {
                TopIndex = t;
            } catch
            {

            }
        }

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

            }
        }

        private List<int> pushedSelection = new List<int>();
        public void PushSelection()
        {
            pushedSelection.Clear();
            for (int i = 0; i < SelectedIndices.Count; i++)
            {
                pushedSelection.Add(SelectedIndices[i]);
            }
        }

        public void PopSelection()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                SetSelected(i, pushedSelection.IndexOf(i) != -1);
            }
        }


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

            }
        }
    }
}
