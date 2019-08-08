using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace DtRec
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.Init();
			p.Counter = 0;
			while (System.IO.File.Exists(p.SavDir + p.Counter + ".bmp"))
				++p.Counter;
			while (!p.IsError)
			{
				p.Rec(p.Counter);
				if(1440 < ++p.Counter)
					p.Counter = 0;
				System.Threading.Thread.Sleep(60000);
            }
        }

        public int Counter = 0;
        public bool IsError { get; private set; }

        string SavDir { get { return System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DtRec\\"; } }

        void Init()
        {
            IsError = false;
            if (!System.IO.Directory.Exists(SavDir))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(SavDir);
                }
                catch (Exception)
                {
                    IsError = true;
                }
            }
        }

        private void Rec(int idx)
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
                    catch (System.ComponentModel.Win32Exception)
                    {
                        IsError = true;
                        return;
                    }
                }
                try
                {
                    bmpScreenCapture.Save(SavDir + idx + ".bmp");
                }
                catch (System.Runtime.InteropServices.ExternalException ex)
                {
                    IsError = true;
					System.IO.File.AppendAllText(SavDir + "log.txt", ex.ToString());
                    return;
                }
            }
        }
    }
}
