using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertSKB
{
    class SkbImporter
    {
        private PathAnalyst pathAnalyst;
        private ConsoleReporter consoleReporter;
        private FilePersistor filePersistor;

        public SkbImporter(PathAnalyst pathAnalyst, ConsoleReporter consoleReporter, FilePersistor filePersistor)
        {
            this.pathAnalyst = pathAnalyst;
            this.consoleReporter = consoleReporter;
            this.filePersistor = filePersistor;
        }

        internal void Execute()
        {
            string bank = pathAnalyst.Bank;
            consoleReporter.WriteLine("Analysing bank: {0}", bank);

            foreach (Account account in pathAnalyst.Accounts)
            {
                consoleReporter.WriteLine(account.Fullname);
                consoleReporter.WriteLine(account.ToString());
            }
            //throw new NotImplementedException();
        }
    }
}
