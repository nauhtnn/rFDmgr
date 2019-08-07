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

        int Counter = -1;

        //string SavDir { get { return System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DtRec\\"; } }
        string SavDir { get { return "C:\\DtRec\\"; } }

        protected override void OnStart(string[] args)
        {
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 1000;
            timer.AutoReset = true;
            timer.Start();
        }

        void InitCounter()
        {
            Counter = 0;
            while (System.IO.File.Exists(SavDir + Counter + ".bmp"))
                ++Counter;
        }

        protected override void OnStop()
        {
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            if (File.Exists(SavDir + "DtRec.exe"))
            {
                if (Counter < 0)
                    InitCounter();
                try
                {
                    using (System.Diagnostics.Process pProcess = new System.Diagnostics.Process())
                    {
                        pProcess.StartInfo.FileName = SavDir + "DtRec.exe";
                        pProcess.StartInfo.Arguments = Counter.ToString();
                        ++Counter;
                        pProcess.StartInfo.UseShellExecute = false;
                        pProcess.StartInfo.RedirectStandardOutput = true;
                        pProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        pProcess.StartInfo.CreateNoWindow = true; //not diplay a windows
                        pProcess.Start();
                        string errorCode = pProcess.StandardOutput.ReadToEnd(); //The output result
                        pProcess.WaitForExit();
                        if (int.Parse(errorCode) != 0)
                            StopPlease();
                    }
                }
                catch(Exception ex)
                {
                    StreamWriter s = System.IO.File.AppendText("c:\\dtrec.txt");
                    s.Write(ex.ToString());
                    s.Close();
                    StopPlease();
                }
            }
            else
            {
                StopPlease();
            }
        }

        void StopPlease()
        {
            timer.Stop();
            Stop();
        }
    }
}
