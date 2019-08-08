using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace DtRec
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.Init();
			p.Counter = 0;
			while (System.IO.File.Exists(p.SavDir + p.Counter + ".jpg"))
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

        string ExeDir { get { return System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DtRec\\"; } }
        string SavDir { get { return System.IO.Path.GetTempPath() + "DtRec\\"; } }

        void Init()
        {
            IsError = false;
            if (!Directory.Exists(SavDir))
            {
                try
                {
                    Directory.CreateDirectory(SavDir);
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
				using(MemoryStream ms = new MemoryStream())
				{
					try
					{
						// Save to memory using the Jpeg format
						bmpScreenCapture.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
					}
					catch (System.Runtime.InteropServices.ExternalException ex)
					{
						IsError = true;
						System.IO.File.AppendAllText(SavDir + "log.txt", ex.ToString());
						return;
					}
					// read to end
					byte[] bmpBytes = ms.GetBuffer();
					try
					{
						System.IO.File.WriteAllBytes(SavDir + idx + ".jpg", bmpBytes);
					}
					catch(IOException)
					{
						IsError = true;
						return;
					}
					catch(UnauthorizedAccessException)
					{
						IsError = true;
						return;
					}
					ms.Close();
				}
            }
        }
    }
}
