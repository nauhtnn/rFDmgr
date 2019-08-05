﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace DsktopRec
{
    class Program
    {

        System.Timers.Timer timer = new System.Timers.Timer();

        static void Main(string[] args)
        {
            Program p = new Program();
            p.OnStart(null);
        }

        int Counter = 0;
        bool IsError = false;

        string SavDir { get { return System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DtRec\\"; } }

        void InitCounter()
        {
            System.IO.File.WriteAllText(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DtReLog.txt", "DtRe start");
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
                    System.IO.File.WriteAllText(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DtReErr.txt", e.ToString());
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

        protected void OnStart(string[] args)
        {
            InitCounter();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 60000;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        protected void OnStop()
        {
            System.IO.File.WriteAllText(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DtReLog.txt", "DtRe stop");
        }

        private void OnElapsedTime(object source, System.Timers.ElapsedEventArgs e)
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
                        System.IO.File.WriteAllText(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DtReErr.txt", ex.ToString());
                    }
                }
                try
                {
                    bmpScreenCapture.Save(SavDir + Counter + ".bmp");
                }
                catch (System.Runtime.InteropServices.ExternalException ex)
                {
                    IsError = true;
                    System.IO.File.WriteAllText(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DtReErr.txt", ex.ToString());
                }
                ++Counter;
            }
        }
    }
}
