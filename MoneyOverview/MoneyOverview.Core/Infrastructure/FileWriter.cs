using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MoneyOverview.Core.Infrastructure
{
    public class FileWriter
    {
        public void WriteLines(string fileName, IEnumerable<string> lines)
        {
            System.IO.FileInfo fifo = new FileInfo(fileName);
            MakeSureDirectoryExists(fifo);

            using (StreamWriter woit = new StreamWriter(fileName, true, Encoding.GetEncoding(28591)))
            {
                foreach (var line in lines)
                {
                    woit.WriteLine(line);
                }
                woit.Flush(); // not needed?
            }
        }

        public void AppendLineToFile(string fileName, string line)
        {
            System.IO.FileInfo fifo = new FileInfo(fileName);
            MakeSureDirectoryExists(fifo);
            //FileStream stream = fifo.OpenWrite();
            //StreamWriter writer = new StreamWriter(stream);
            //writer.AutoFlush = true;

            using (StreamWriter woit = new StreamWriter(fileName, true, Encoding.Unicode))
            {
                woit.WriteLine(line);
                woit.Flush(); // not needed?
            }

        }
        public bool Exists(string fileName)
        {
            System.IO.FileInfo fifo = new FileInfo(fileName);
            return fifo.Exists;
        }

        public bool DirectoryExists(string folderName)
        {
            System.IO.DirectoryInfo fifo = new DirectoryInfo(folderName);
            return fifo.Exists;
        }
        private static void MakeSureDirectoryExists(FileInfo info)
        {
            if (info.Directory == null) throw new DirectoryNotFoundException("File without Directory: " + info.FullName);
            if (!info.Directory.Exists) info.Directory.Create();

        }

    }
}
