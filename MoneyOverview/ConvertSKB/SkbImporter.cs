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
            Dictionary<string, int> countTypes = new Dictionary<string,int>();

            foreach (Account account in pathAnalyst.Accounts)
            {
                consoleReporter.WriteLine(account.Fullname);
                consoleReporter.WriteLine(account.ToString());

                foreach(var item in account.Items){
                    if (!countTypes.ContainsKey(item.Type)) countTypes.Add(item.Type, 0);
                    countTypes[item.Type]++;
                }
            }

            foreach (var typecount in countTypes) {
                consoleReporter.WriteLine("{0}: {1}", typecount.Key, typecount.Value);
            }

            //Matching internal transfers
            List<Account> rest = new List<Account>(pathAnalyst.Accounts);
            var input = rest[0];
            rest.RemoveAt(0);

            int hitCounter = 0;
            foreach (var item in input.Items)
            {
                consoleReporter.WriteLine("\n\nLooking for item: \t{0} - {1}", item, item.Type);
                if(item.Type.Equals("Kreditrente")) {
                    consoleReporter.WriteLine(" - Skipped!");
                    continue;
                }

                foreach (var account in rest)
                {
                    foreach (var candidate in account.Items)
                    {
                        if (item.Date.Equals(candidate.Date)
                            && item.Desc.Equals(candidate.Desc)
                            )
                        {
                            hitCounter++;
                            consoleReporter.WriteLine(" Potential match {1:0000}: \t{0}", candidate, hitCounter);
                            if (hitCounter > 100) return; //TODO: Don't do this!
                        }

                    }
                }
            }
        }
    }
}
