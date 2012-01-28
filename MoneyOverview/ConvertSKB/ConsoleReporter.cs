using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertSKB
{
    public class ConsoleReporter
    {
        public void WriteLine(string format, params object[] arg)
        {
            Console.Out.WriteLine(format, arg);
        }
    }
}
