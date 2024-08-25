using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace MPItemTracker2
{
    class Program
    {
        static string _ExecutableDir = string.Empty;
        public static string ExecutableDir
        {
            get
            {
                if (_ExecutableDir == string.Empty)
                    _ExecutableDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) ?? String.Empty;
                return _ExecutableDir;
            }
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Forms.MainForm());
        }
    }
}
