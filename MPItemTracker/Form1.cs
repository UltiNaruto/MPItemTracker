using Prime.Memory;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace MPItemTracker
{
    public partial class Form1 : Form
    {
        private bool emuInit = false;
        private bool gameInit = false;
        private bool formInit = false;
        private BackgroundWorker bW = null;

        public Form1()
        {
            InitializeComponent();
        }

        void SafeClose()
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(() => SafeClose()));
            else
                this.Close();
        }

        void SafeRefresh()
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(() => SafeRefresh()));
            else
                this.Refresh();
        }

        internal void SetWindowPosAndSize(Point p, Size s)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(() => SetWindowPosAndSize(p, s)));
            else
            {
                this.Location = p;
                this.Size = s;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            try
            {
                if (!formInit)
                {
                    e.Graphics.DrawString("Waiting for Metroid Prime...", new Font("Arial", 10), Brushes.Black, new Point(10, 5));
                }
                if (emuInit && gameInit && formInit)
                {
                    e.Graphics.Clear(this.TransparencyKey);
                    Dolphin.UpdateTracker(e.Graphics);
                }
            }
            catch
            {
                emuInit = false;
                gameInit = false;
                formInit = false;
                SafeClose();
            }
        }

        private void FormSizeChanged(object sender, EventArgs e)
        {
            SafeRefresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bW = new BackgroundWorker();
            bW.WorkerSupportsCancellation = true;
            bW.DoWork += (s, ev) =>
            {
                while (true)
                {
                    try
                    {
                        if (emuInit && gameInit && formInit)
                        {
                            Dolphin.UpdateTrackerInfo(this);
                        }
                        SafeRefresh();
                    }
                    catch
                    {
                        emuInit = false;
                        gameInit = false;
                        formInit = false;
                        SafeClose();
                        break;
                    }
                    Thread.Sleep(100);
                }
            };
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!emuInit)
                    emuInit = Dolphin.Init();
                if (emuInit && !gameInit)
                    gameInit = Dolphin.GameInit();
                if (emuInit && gameInit && !formInit)
                {
                    Dolphin.InitMP();
                    Dolphin.InitTracker(this);
                    formInit = true;
                    bW.RunWorkerAsync();
                    this.timer1.Stop();
                }
            }
            catch
            {
                emuInit = false;
                gameInit = false;
                formInit = false;
                SafeClose();
            }
        }
    }
}
