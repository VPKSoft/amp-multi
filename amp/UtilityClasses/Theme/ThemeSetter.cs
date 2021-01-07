using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReaLTaiizor.Forms;

namespace amp.UtilityClasses.Theme
{
    internal class ThemeSetter
    {
        private void SetTheme(CrownForm form)
        {

        }

        internal static void ColorControls(Color foreColor, Color backColor, params Control[] controls)
        {
            foreach (var label in controls)
            {
                label.ForeColor = foreColor;
                label.BackColor = backColor;
            }
        }

        internal static void FixMenuTheme(MenuStrip menuStrip)
        {
            menuStrip.BackColor = Color.Transparent;
            foreach (ToolStripItem item in menuStrip.Items)
            {
                if (item.GetType().IsAssignableFrom(typeof(ToolStripMenuItem)))
                {
                    FixMenuTheme(item as ToolStripMenuItem);
                }
                item.BackColor = Color.Transparent;
            }
        }

        internal static void FixMenuTheme(ToolStripMenuItem menuStrip)
        {
            menuStrip.BackColor = Color.Transparent;
            foreach (ToolStripItem item in menuStrip.DropDownItems)
            {
                if (item.GetType().IsAssignableFrom(typeof(ToolStripMenuItem)))
                {
                    FixMenuTheme(item as ToolStripMenuItem);
                }
                item.BackColor = Color.Transparent;
            }
        }
    }
}
