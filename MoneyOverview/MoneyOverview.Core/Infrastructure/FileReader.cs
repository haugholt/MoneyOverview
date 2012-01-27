using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MoneyOverview.Core.Infrastructure
{
    public class FileReader
    {
        public FileReader(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; private set; }

        public List<string> ReadAllLines()
        {
            return this.ReadAllLines(FileName);
        }

        private List<string> ReadAllLines(string fileName)
        {
            System.IO.FileInfo fifo = new FileInfo(fileName);

            List<string> lines = new List<string>();
            if (!fifo.Exists) return lines;

            //using (StreamReader rea = new StreamReader(fileName, true))
            using (StreamReader rea = new StreamReader(fileName, Encoding.GetEncoding(28591)))
            {
                while (!rea.EndOfStream)
                {
                    var line = rea.ReadLine();
                    if (line.StartsWith("#")) continue;
                    if (line.Trim().Equals("")) continue;
                    lines.Add(
                        line);
                }
            }
            return lines;
        }

    }
}