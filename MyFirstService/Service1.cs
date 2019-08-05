using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Drawing;

namespace DtRec
{
    public partial class Service1 : ServiceBase
    {

        Timer timer = new Timer();
        public Service1()
        {
            InitializeComponent();
        }

        int Counter = 0;
        bool IsError = false;

        //string SavDir { get { return System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DtRec\\"; } }
        string SavDir { get { return "C:\\DtRec\\"; } }

        void InitCounter()
        {
            //System.IO.File.WriteAllText(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DtReLog.txt", "DtRe start");
            System.IO.File.WriteAllText("C:\\DtReLog.txt", "DtRe start");
            IsError = false;
            if (!System.IO.Directory.Exists(SavDir))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(SavDir);
                }
                catch (Exception e)
                {
                    IsError = true;
                    //System.IO.File.WriteAllText(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DtReErr.txt", e.ToString());
                    System.IO.File.WriteAllText("C:\\DtReErr.txt", e.ToString());
                }
            }
            if (IsError)
                return;
            Counter = 0;
            while (System.IO.File.Exists(SavDir + Counter + ".bmp"))
            {
                ++Counter;
            }
        }

        protected override void OnStart(string[] args)
        {
            InitCounter();
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 60000;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            //System.IO.File.WriteAllText(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DtReLog.txt", "DtRe stop");
            System.IO.File.WriteAllText("C:\\DtReLog.txt", "DtRe stop");
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            if (IsError)
            {
                timer.Enabled = false;
                return;
            }
            using (Bitmap bmpScreenCapture = new Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width,
                                            System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bmpScreenCapture))
                {
                    try
                    {
                        g.CopyFromScreen(System.Windows.Forms.Screen.PrimaryScreen.Bounds.X,
                                     System.Windows.Forms.Screen.PrimaryScreen.Bounds.Y,
                                     0, 0,
                                     bmpScreenCapture.Size,
                                     CopyPixelOperation.SourceCopy);
                    }
                    catch (System.ComponentModel.Win32Exception ex)
                    {
                        IsError = true;
                        //System.IO.File.WriteAllText(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DtReErr.txt", ex.ToString());
                        System.IO.File.WriteAllText("C:\\DtReErr.txt", ex.ToString());
                    }
                }
                try
                {
                    bmpScreenCapture.Save(SavDir + Counter + ".bmp");
                }
                catch (System.Runtime.InteropServices.ExternalException ex)
                {
                    IsError = true;
                    //System.IO.File.WriteAllText(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DtReErr.txt", ex.ToString());
                    System.IO.File.WriteAllText("C:\\DtReErr.txt", ex.ToString());
                }
                ++Counter;
            }
        }
    }
}
