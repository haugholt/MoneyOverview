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
            string bank = pathAnalyst.Bank;
            consoleReporter.WriteLine("Analysing bank: {0}", bank);
            Dictionary<string, int> countTypes = new Dictionary<string, int>();

            foreach (Account account in pathAnalyst.Accounts)
            {
                consoleReporter.WriteLine(account.Fullname);
                consoleReporter.WriteLine(account.ToString());

                foreach (var item in account.Items)
                {
                    if (!countTypes.ContainsKey(item.Type)) countTypes.Add(item.Type, 0);
                    countTypes[item.Type]++;
                }
            }

            foreach (var typecount in countTypes)
            {
                consoleReporter.WriteLine("{0}: {1}", typecount.Key, typecount.Value);
            }


            InternalTransfersMatcher itm = new InternalTransfersMatcher(pathAnalyst.Accounts, consoleReporter);
            var results = itm.GetMatches();

            var innSaldo = new SaldoItem("0,00", "0,00");
            var utSaldo = new SaldoItem("0,00", "0,00");
            Account felles = new Account("Skandiabanken", "felles", innSaldo, utSaldo);

            SkbRepository skbRepo = new SkbRepository();
            foreach (var account in pathAnalyst.Accounts)
            {
                foreach (var line in account.Items)
                {
                    felles.AddLine(line);
                    skbRepo.Add(line);
                    consoleReporter.WriteLine(line.Desc);
                }
            }

            Func<AccountLine, SkbRepository, MatchResult> convertMethod;
//             public delegate MatchResult MyDelegateType(AccountLine item, SkbRepository skbRepo);
            //InternalMatcherService.Macho(skbRepo, convertMethod);
            //var matchon = (AccountLine item, SkbRepository skandiaRepo) => { return new MatchResult(); };
            InternalMatcherService.Match(skbRepo, (AccountLine item, SkbRepository skandiaRepo) => { return new MatchResult(); });
        }

        public MatchResult MatchOn(AccountLine item, SkbRepository skbRepo)
        {
            throw new NotImplementedException();
        }
        //   // List<AccountLine> 
        //    ProcessResult pRes = new ProcessResult(skbRepo.GetAll(), true);
        //    do
        //    {
        //        pRes = ProcessMatches(pRes.UnMatched);
        //    } while (pRes.AnyChange);

        //}

        //private ProcessResult ProcessMatches(List<AccountLine> toProcess)
        //{
        //    if (toProcess.Count < 1) return ProcessResult.End;
        //    bool anythingChanged = false;
        //    List<AccountLine> unMatched = new List<AccountLine>();
        //    foreach (var item in toProcess)
        //    {
        //        ProcessResult res = ProcessItem(item);
        //        if (res.HasItems)
        //        {
        //            unMatched.AddRange(res.UnMatched);
        //        }
        //        if (res.AnyChange) anythingChanged = true;
        //    }
        //    return new ProcessResult(unMatched, anythingChanged);
        //}

        //private ProcessResult ProcessItem(AccountLine item)
        //{
        //    bool anythingChanged = false;
        //    throw new NotImplementedException();
        //}

        
    }
}
