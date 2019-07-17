#region License
/*
MIT License

Copyright(c) 2019 Petteri Kautonen

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

using System.Collections.Generic;
using System.Windows.Forms;

namespace amp.UtilityClasses.Controls
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
