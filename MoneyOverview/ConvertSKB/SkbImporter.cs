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


            //InternalTransfersMatcher itm = new InternalTransfersMatcher(pathAnalyst.Accounts, consoleReporter);
            //var results = itm.GetMatches();

            //var innSaldo = new SaldoItem("0,00", "0,00");
            //var utSaldo = new SaldoItem("0,00", "0,00");
            //Account felles = new Account("Skandiabanken", "felles", innSaldo, utSaldo);

            
            //foreach (var account in pathAnalyst.Accounts)
            //{
            //    foreach (var line in account.Items)
            //    {
            //        felles.AddLine(line);
            //        skbRepo.Add(line);
            //        consoleReporter.WriteLine(line.Desc);
            //    }
            //}

            consoleReporter.WriteLine("\n\nBegin: {0} lines", skbRepo.GetAll().Count);
            

            InternalTransactionsRepository internalRepo = new InternalTransactionsRepository();

            new InternalMatcherService(skbRepo, internalRepo, MatchOnAll).Match();
            consoleReporter.WriteLine("\nAfter 1 run: {0} lines, {1} matches", skbRepo.GetAll().Count, internalRepo.GetAll().Count);
            
            InternalMatcherService internalMatcher = new InternalMatcherService(skbRepo, internalRepo, MatchOnMost);
            internalMatcher.Match();

            consoleReporter.WriteLine("\nAfter 2 run: {0} lines, {1} matches", skbRepo.GetAll().Count, internalRepo.GetAll().Count);

            SimpleMoney totalInternal = SimpleMoney.Zero;
            foreach (var inter in internalRepo.GetAll())
            {
                if (inter.ActualAmount.IsPositiveNumber) totalInternal += inter.ActualAmount;
            }
            consoleReporter.WriteLine("total internal transactions: {0}", totalInternal);
            //InternalMatcherService.Match(skbRepo, (AccountLine item, SkbRepository skandiaRepo) => { return new MatchResult(); });
        }

        //public MatchResult MatchOn(AccountLine item, SkbRepository skbRepo)
        //{
        //    throw new NotImplementedException();
        //}
        public bool MatchOnAll(AccountLine item, AccountLine candidate)
        {
            SimpleMoney total = item.ActualAmount + candidate.ActualAmount;
            // Console.Out.WriteLine("Matchonmost: {0}", total); 

            var res = item.Date.Equals(candidate.Date)
                && total.Equals(SimpleMoney.Zero)
                && item.Desc.Equals(candidate.Desc)
                && item.Reference.Equals(candidate.Reference);

            //if (res) Console.Out.WriteLine("Found match: {0}\n           : {1}", item, candidate);

            return res;
        }
        public bool MatchOnMost(AccountLine item, AccountLine candidate)
        {
            SimpleMoney total = item.ActualAmount + candidate.ActualAmount;
           // Console.Out.WriteLine("Matchonmost: {0}", total); 

            var res = item.Date.Equals(candidate.Date)
                && total.Equals(SimpleMoney.Zero)
                && item.Desc.Equals(candidate.Desc);

            //if (res) Console.Out.WriteLine("Found match: {0}\n           : {1}", item, candidate);

            return res;
        }
    }
}
