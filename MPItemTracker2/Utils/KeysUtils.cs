using System;
using System.Windows.Input;

namespace MPItemTracker2.Utils
{
    class KeysUtils
    {
        public static Key ConvertFromString(string keystr)
        {
            return (Key)Enum.Parse(typeof(Key), keystr);
        }
    }
}
