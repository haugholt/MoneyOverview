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
            if (args.Length != 1)
            {
                Usage();
                return;
            }
            
            string path = args[0];
            Console.Out.WriteLine("Path: "+path);
        }

        private static void Usage()
        {
            Console.Out.WriteLine("Provide a path");
            
        }
    }
}
