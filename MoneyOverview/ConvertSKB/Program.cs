using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertSKB
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Usage();
                return;
            }
            
            string path = args[0];
            Console.Out.WriteLine("Path: "+path);

            string pathOut = args[1];
            AccountReader accountReader = new AccountReader();
            PathAnalyst pathAnalyst = new PathAnalyst(path, accountReader);

            ConsoleReporter consoleReporter = new ConsoleReporter();
            FilePersistor filePersistor = new FilePersistor(pathOut);

            SkbImporter skbImporter = new SkbImporter(pathAnalyst, consoleReporter, filePersistor);

            skbImporter.Execute();
        }

        private static void Usage()
        {
            Console.Out.WriteLine("Provide two paths");
            
        }
    }
}
