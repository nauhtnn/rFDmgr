using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace DsktopRec
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.InitCounter();
			while(!p.IsError)
			{
				p.Rec();
				System.Threading.Thread.Sleep(60000);
			}
        }

        int Counter = 0;
        public bool IsError { get; private set; }

        string SavDir { get { return System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DtRec\\"; } }

        void InitCounter()
        {
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
					return;
                }
            }
            Counter = 0;
            while (System.IO.File.Exists(SavDir + Counter + ".bmp"))
                ++Counter;
        }

        private void Rec()
        {
			if(IsError)
				return;
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
                        return;
                    }
                }
                try
                {
                    bmpScreenCapture.Save(SavDir + Counter + ".bmp");
                }
                catch (System.Runtime.InteropServices.ExternalException ex)
                {
                    IsError = true;
                    return;
                }
                ++Counter;
            }
        }
    }
}
