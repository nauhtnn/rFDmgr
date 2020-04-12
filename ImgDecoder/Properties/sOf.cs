using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace sOf
{
	enum SOF_Action
	{
		SHOW,
		ENCIP,
		DECIP
	}
	
    class Program
    {
		public const string SOF_EXT = ".l";
		
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("no pw");
                return;
            }
			if(args[0] != "666")
			{
				Console.WriteLine("wrong pw");
				return;
			}
            
			if(args.Length < 2)
				Console.WriteLine("action?");
			else if(args[1] == "show")
				ProcessDirectory(".", SOF_Action.SHOW);
			else if(args[1] == "encip")
				ProcessDirectory(".", SOF_Action.ENCIP);
			else if(args[1] == "decip")
				ProcessDirectory(".", SOF_Action.DECIP);
			
            Console.WriteLine("\ndone");
        }
		
		public static void XOR1(string path)
		{
			//no checking existance here
			
			byte f = 0xff;
            using(BinaryReader r = new BinaryReader(File.OpenRead(path)))
            {
                byte x = r.ReadByte();
                f = (byte)(f ^ x);
                r.Close();
            }
            using(BinaryWriter w = new BinaryWriter(File.OpenWrite(path)))
            {
                w.Write(f);
                w.Close();
            }
		}
		
		//MSDN Directory.GetFiles Method (String)
		// Process all files in the directory passed in, recurse on any directories 
		// that are found, and process the files they contain.
		public static void ProcessDirectory(string targetDirectory, SOF_Action act)
		{
			// Process the list of files found in the directory.
			string [] fileEntries = Directory.GetFiles(targetDirectory);
			switch(act)
			{
				case SOF_Action.SHOW:
					foreach(string fileName in fileEntries)
						ProcessFileSHOW(fileName);
				break;
				case SOF_Action.ENCIP:
					foreach(string fileName in fileEntries)
						ProcessFileENCIP(fileName);
				break;
				case SOF_Action.DECIP:
					foreach(string fileName in fileEntries)
						ProcessFileDECIP(fileName);
				break;
			}

			// Recurse into subdirectories of this directory.
			string [] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
			foreach(string subdirectory in subdirectoryEntries)
				ProcessDirectory(subdirectory, act);
		}

		// Insert logic for processing found files here.
		public static void ProcessFileENCIP(string path) 
		{
			//no checking existance here
			
			if(IsVideo(path))
			{
				XOR1(path);
				string name = Path.GetFileNameWithoutExtension(path);
				string cipName = Encipher(name);
				Console.WriteLine(name + "----" + cipName);
				File.Move(path, Path.GetDirectoryName(path) + "\\" + cipName + SOF_EXT);
			}
		}
		
		public static void ProcessFileDECIP(string path) 
		{
			//no checking existance here
			
			if(IsSOF(path))
			{
				XOR1(path);
				string cipName = Path.GetFileNameWithoutExtension(path);
				string name = Decipher(cipName);
				Console.WriteLine(name + "----" + cipName);
				File.Move(path, Path.GetDirectoryName(path) + "\\" + name + ".mp4");
			}
		}
		
		public static void ProcessFileSHOW(string path) 
		{
			//no checking existance here
			
			if(IsSOF(path))
			{
				string cipName = Path.GetFileNameWithoutExtension(path);
				string name = Decipher(cipName);
				Console.WriteLine(name + "----" + cipName);
			}
		}
		//end MSDN
	
		public static bool IsVideo(string path)
		{
			string ext = Path.GetExtension(path);
			return ext == ".mp4";
		}
		
		public static bool IsSOF(string path)
		{
			string ext = Path.GetExtension(path);
			return ext == SOF_EXT;
		}
		
		//Caesar https://www.c-sharpcorner.com/article/caesar-cipher-in-c-sharp/
		public static char cipher(char ch, int key = 3) {
            if (!char.IsLetter(ch)) {
                return ch;  
            }  
  
            char d = char.IsUpper(ch) ? 'A' : 'a';  
            return (char)((((ch + key) - d) % 26) + d);
        }
  
        public static string Encipher(string input, int key = 3) {  
            string output = string.Empty;  
  
            foreach(char ch in input)  
				output += cipher(ch, key);  
  
            return output;  
        }  
  
        public static string Decipher(string input, int key = 3) {  
            return Encipher(input, 26 - key);  
        }
		//end Caesar
    }
}
