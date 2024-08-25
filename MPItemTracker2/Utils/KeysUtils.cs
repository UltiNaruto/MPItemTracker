using Dapplo.Windows.Input.Enums;
using System;

namespace MPItemTracker2.Utils
{
    class KeysUtils
    {
        public static VirtualKeyCode ConvertFromString(string keystr)
        {
            return (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), keystr);
        }
    }
}
