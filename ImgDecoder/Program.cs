using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgDecoder
{
    class Program
    {
        static void Main(string[] args)
        {
            string pattern;
            if (0 < args.Length)
                pattern = args[0];
            else
                pattern = "*.jpg";
            foreach(string file in System.IO.Directory.GetFiles(System.IO.Directory.GetCurrentDirectory(), pattern))
            {
                System.IO.FileStream s = new System.IO.FileStream(file, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
                byte x = (byte)s.ReadByte();
                x = (byte)(x ^ 0xff);
                s.Seek(0, System.IO.SeekOrigin.Begin);
                s.WriteByte(x);
                s.Close();
            }
        }
    }
}
