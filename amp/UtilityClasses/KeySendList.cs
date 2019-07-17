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

namespace amp.UtilityClasses
{
    public static class KeySendList
    {
        private static List<KeyValuePair<Keys, string>> keys = new List<KeyValuePair<Keys, string>>();

        static KeySendList()
        {
            keys.Add(new KeyValuePair<Keys, string>(Keys.Back, "{BACKSPACE}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.Pause, "{BREAK}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.CapsLock, "{CAPSLOCK}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.Delete, "{DELETE}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.Down, "{DOWN}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.End, "{END}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.Return, "{ENTER}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.Escape, "{ESC}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.Help, "{HELP}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.Home, "{HOME}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.Insert, "{INSERT}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.Left, "{LEFT}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.NumLock, "{NUMLOCK}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.PageDown, "{PGDN}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.PageUp, "{PGUP}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.PrintScreen, "{PRTSC}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.Right, "{RIGHT}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.Scroll, "{SCROLLLOCK}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.Tab, "{TAB}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.Up, "{UP}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.F1, "{F1}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.F2, "{F2}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.F3, "{F3}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.F4, "{F4}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.F5, "{F5}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.F6, "{F6}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.F7, "{F7}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.F8, "{F8}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.F9, "{F9}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.F10, "{F10}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.F11, "{F11}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.F12, "{F12}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.F13, "{F13}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.F14, "{F14}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.F15, "{F15}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.F16, "{F16}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.Add, "{ADD}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.Subtract, "{SUBTRACT}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.Multiply, "{MULTIPLY}"));
            keys.Add(new KeyValuePair<Keys, string>(Keys.Divide, "{DIVIDE}"));            
        }

        public static bool HasKey(Keys key)
        {
            return GetKeyString(key) != null;
        }

        public static bool HasKey(string key)
        {
            return GetKeyKeys(key) != null;
        }

        public static string GetKeyString(Keys key)
        {
            foreach (KeyValuePair<Keys, string> k in keys)
            {
                if (k.Key == key)
                {
                    return k.Value;
                }
            }
            return null;
        }

        public static Keys? GetKeyKeys(string key)
        {
            foreach (KeyValuePair<Keys, string> k in keys)
            {
                if (k.Value == key)
                {
                    return k.Key;
                }
            }
            return null;
        }
    }
}
