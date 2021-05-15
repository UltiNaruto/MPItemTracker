using Imports;
using System;
using System.Drawing;

namespace MPItemTracker2.Utils
{
    class FormUtils
    {
        static Forms.MainForm mainForm;

        public static void Init(Forms.MainForm form)
        {
            mainForm = form;
            ImportsMgr.Init();
        }

        public static void Refresh()
        {
            if (mainForm == null)
                throw new Exception("Call FormUtils.Init() first!");

            if (mainForm.InvokeRequired)
                mainForm.Invoke(new Action(() => Refresh()));
            else
                mainForm.Refresh();
        }

        public static void Close()
        {
            if (mainForm == null)
                throw new Exception("Call FormUtils.Init() first!");

            if (mainForm.InvokeRequired)
                mainForm.Invoke(new Action(() => Close()));
            else
            {
                ImportsMgr.DeInit();
                mainForm.Close();
            }
        }

        public static void SetWindowPosition(Point p)
        {
            if (mainForm == null)
                throw new Exception("Call FormUtils.Init() first!");

            if (mainForm.InvokeRequired)
                mainForm.Invoke(new Action(() => SetWindowPosition(p)));
            else
                mainForm.Location = p;
        }

        public static void SetWindowSize(Size s)
        {
            if (mainForm == null)
                throw new Exception("Call FormUtils.Init() first!");

            if (mainForm.InvokeRequired)
                mainForm.Invoke(new Action(() => SetWindowSize(s)));
            else
                mainForm.Size = s;
        }

        public static void SetFormBGColor(Color color)
        {
            if (mainForm == null)
                throw new Exception("Call FormUtils.Init() first!");

            if (mainForm.InvokeRequired)
                mainForm.Invoke(new Action(() => SetFormBGColor(color)));
            else
                mainForm.BackColor = color;
        }
    }
}
