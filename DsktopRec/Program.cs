using System;
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
        static void Main(string[] args)
        {
            Program.InitCounter();
            while (true)
            {
                System.Threading.Thread.Sleep(60000);
                CaptureDsktop();
            }
        }

        static int Counter;

        static string SavDir { get { return System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DsktopRec\\"; } }

        static void InitCounter()
        {
            Counter = 0;
            while (System.IO.File.Exists(SavDir + Counter + ".bmp"))
            {
                ++Counter;
            }
        }

        static void CaptureDsktop()
        {
            using (Bitmap bmpScreenCapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                            Screen.PrimaryScreen.Bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bmpScreenCapture))
                {
                    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                     Screen.PrimaryScreen.Bounds.Y,
                                     0, 0,
                                     bmpScreenCapture.Size,
                                     CopyPixelOperation.SourceCopy);
                }
                bmpScreenCapture.Save(SavDir + Counter + ".bmp");
                ++Counter;
            }
        }
    }
}
