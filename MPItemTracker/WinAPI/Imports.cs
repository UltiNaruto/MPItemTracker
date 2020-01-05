using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace Prime.WinAPI
{
    internal class Imports
    {
        #region C Defines
        internal static int WS_EX_TRANSPARENT = 0x20;
        internal static int WS_EX_LAYERED = 0x80000;
        internal static int WS_CHILD = 0x40000000;
        internal static int GWL_EXSTYLE = (-20);
        internal static int GWL_STYLE = (-16);
        #endregion

        #region C Structs
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

            public int X
            {
                get { return Left; }
                set { Right -= (Left - value); Left = value; }
            }

            public int Y
            {
                get { return Top; }
                set { Bottom -= (Top - value); Top = value; }
            }

            public int Height
            {
                get { return Bottom - Top; }
                set { Bottom = value + Top; }
            }

            public int Width
            {
                get { return Right - Left; }
                set { Right = value + Left; }
            }

            public System.Drawing.Point Location
            {
                get { return new System.Drawing.Point(Left, Top); }
                set { X = value.X; Y = value.Y; }
            }

            public System.Drawing.Size Size
            {
                get { return new System.Drawing.Size(Width, Height); }
                set { Width = value.Width; Height = value.Height; }
            }

            public static implicit operator System.Drawing.Rectangle(RECT r)
            {
                return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
            }

            public static implicit operator RECT(System.Drawing.Rectangle r)
            {
                return new RECT(r);
            }

            public static bool operator ==(RECT r1, RECT r2)
            {
                return r1.Equals(r2);
            }

            public static bool operator !=(RECT r1, RECT r2)
            {
                return !r1.Equals(r2);
            }

            public bool Equals(RECT r)
            {
                return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
            }

            public override bool Equals(object obj)
            {
                if (obj is RECT)
                    return Equals((RECT)obj);
                else if (obj is System.Drawing.Rectangle)
                    return Equals(new RECT((System.Drawing.Rectangle)obj));
                return false;
            }

            public override int GetHashCode()
            {
                return ((System.Drawing.Rectangle)this).GetHashCode();
            }

            public override string ToString()
            {
                return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
            }
        }
        #endregion

        #region C Imports
        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        static extern int SetWindowLongPtr32(IntPtr hWnd, int nIndex, int value);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr value);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        #endregion

        static IntPtr[] GetAllChildWindowHandles(Process process)
        {
            var handles = new List<IntPtr>();

            foreach (ProcessThread thread in process.Threads)
                EnumThreadWindows(thread.Id, (hWnd, lParam) => { handles.Add(hWnd); return true; }, IntPtr.Zero);

            return handles.ToArray();
        }

        internal static Dictionary<IntPtr, String> GetAllChildWindowHandlesByClassNames(Process process)
        {
            Dictionary<IntPtr, String> result = new Dictionary<IntPtr, String>();
            IntPtr[] windowHandles = GetAllChildWindowHandles(process);
            StringBuilder className = null;
            foreach(var windowHandle in windowHandles)
            {
                className = new StringBuilder(512);
                GetClassName(windowHandle, className, 512);
                result.Add(windowHandle, className.ToString());
            }
            return result;
        }

        internal static Dictionary<IntPtr, String> GetAllChildWindowHandlesByWindowTitles(Process process)
        {
            Dictionary<IntPtr, String> result = new Dictionary<IntPtr, String>();
            IntPtr[] windowHandles = GetAllChildWindowHandles(process);
            foreach (var windowHandle in windowHandles)
                result.Add(windowHandle, GetWindowTitle(windowHandle));
            return result;
        }

        internal static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8)
                return GetWindowLongPtr64(hWnd, nIndex);
            else
                return GetWindowLongPtr32(hWnd, nIndex);
        }

        internal static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr value)
        {
            if (IntPtr.Size == 8)
                return SetWindowLongPtr64(hWnd, nIndex, value);
            else
                return new IntPtr(SetWindowLongPtr32(hWnd, nIndex, value.ToInt32()));
        }

        internal static void SetParentWindow(IntPtr childHwnd, IntPtr newParentHwnd)
        {
            SetParent(childHwnd, newParentHwnd);
        }

        internal static KeyValuePair<Point, Size>  GetWindowPosAndSize(IntPtr hWnd)
        {
            RECT rect;
            GetWindowRect(hWnd, out rect);
            return new KeyValuePair<Point, Size>(new Point(rect.X, rect.Y), new Size(rect.Width, rect.Height));
        }

        internal static IntPtr GetTopMostWindow()
        {
            return GetForegroundWindow();
        }

        internal static String GetWindowTitle(IntPtr hwnd)
        {
            StringBuilder windowText = new StringBuilder(512);
            GetWindowText(hwnd, windowText, 512);
            return windowText.ToString();
        }
    }
}
