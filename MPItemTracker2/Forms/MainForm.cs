using MPItemTracker2.Utils;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Wrapper;

namespace MPItemTracker2.Forms
{
    public partial class MainForm : Form
    {
        private bool emuInit = false;
        private bool gameInit = false;
        private bool formInit = false;
        private BackgroundWorker bW = null;

        public MainForm()
        {
            Version version = new AssemblyName(Assembly.GetExecutingAssembly().FullName).Version;
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Application.ExecutablePath));
            FormUtils.Init(this);
            InitializeComponent();
            this.Text += $" v{version.Major}.{version.Minor}";
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Pink, e.ClipRectangle);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            try
            {
                if (!formInit)
                {
                    e.Graphics.DrawString("Waiting for Metroid Prime 1/2/3...", new Font("Arial", 10), Brushes.Black, new Point(10, 5));
                }
                if (emuInit && gameInit && formInit)
                {
                    Dolphin.UpdateTracker(e.Graphics);
                }
            }
            catch
            {
                emuInit = false;
                gameInit = false;
                formInit = false;
                FormUtils.Close();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
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
                        FormUtils.Refresh();
                    }
                    catch
                    {
                        emuInit = false;
                        gameInit = false;
                        formInit = false;
                        FormUtils.Close();
                        break;
                    }
                    Thread.Sleep(200);
                }
            };
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            FormUtils.Refresh();
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
                FormUtils.Close();
            }
        }
    }
}
