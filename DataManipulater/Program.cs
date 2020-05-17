using MKModel;
using System;
using System.IO;
using System.Linq;

namespace DataManipulater
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessDirectory("C:\\MageKnightDatabase\\MKData\\RebellionDialsFormattedData");
        }

        
        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        // Insert logic for processing found files here.
        public static void ProcessFile(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);

            int i = 0;
            string newText = string.Empty;
            foreach (string line in lines)
            {
                i++;
                if (i <= 2)
                {
                    newText += line;
                }
                else if (i == 3)
                {
                    newText += line +'\n';
                    i = 0;
                }
            }

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            fs.Seek(0, SeekOrigin.Begin);
            fs.SetLength((long)newText.Length);
            StreamWriter sw = new StreamWriter(fs);
            
            sw.Write(string.Empty);
            sw.Write(newText);
            sw.Close();
            fs.Close();
        }
    }
}
