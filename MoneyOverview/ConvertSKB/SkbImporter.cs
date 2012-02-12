using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoneyOverview.Core.Domain;
using ConvertSKB.Domain;

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
            SkbRepository skbRepo = new SkbRepository();

            string bank = pathAnalyst.Bank;
            consoleReporter.WriteLine("Analysing bank: {0}", bank);
            Dictionary<string, int> countTypes = new Dictionary<string, int>();

            foreach (Account account in pathAnalyst.Accounts)
            {
                consoleReporter.WriteLine(account.Fullname);
                consoleReporter.WriteLine(account.ToString());

                foreach (var item in account.Items)
                {
                    skbRepo.Add(item);
                    if (!countTypes.ContainsKey(item.Type)) countTypes.Add(item.Type, 0);
                    countTypes[item.Type]++;
                }
            }

            foreach (var typecount in countTypes)
            {
                consoleReporter.WriteLine("{0}: {1}", typecount.Key, typecount.Value);
            }

            consoleReporter.WriteLine("\n\nBegin: {0} lines", skbRepo.GetAll().Count);
            
            InternalTransactionsRepository internalRepo = new InternalTransactionsRepository();
            InternalMatcher internalMatcher = new InternalMatcher(skbRepo, internalRepo);

            internalMatcher.Match(InternalMatcher.MatchOnAll);
            consoleReporter.WriteLine("\nAfter 1 run: {0} lines, {1} matches", skbRepo.GetAll().Count, internalRepo.GetAll().Count);

            internalMatcher.Match(InternalMatcher.MatchOnMost);
            consoleReporter.WriteLine("\nAfter 2 run: {0} lines, {1} matches", skbRepo.GetAll().Count, internalRepo.GetAll().Count);

            List<AccountLine> positiveLines = AccountLineFilterService.GetPositiveLines(internalRepo.GetAll());
            var internalSum = AccountLineAggregatorService.Sum(positiveLines);
            consoleReporter.WriteLine("\nTotal internal transactions: {0}", internalSum);
        }
    }
}
