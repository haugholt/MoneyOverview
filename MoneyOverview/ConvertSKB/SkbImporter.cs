using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoneyOverview.Core.Domain;

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



            MatchInternalTransfers();
        }

        private void MatchInternalTransfers()
        {
            //Matching internal transfers
            List<Account> rest = new List<Account>(pathAnalyst.Accounts);
            var input = rest[0];
            rest.RemoveAt(0);

            int hitCounter = 0;
            foreach (var item in input.Items)
            {
                // consoleReporter.WriteLine("\nLooking for item: \t{0} - {1}", item, item.Type);
                if (item.Type.Equals("Kreditrente"))
                {
                    //   consoleReporter.WriteLine(" - Skipped!");
                    continue;
                }

                int potentialMatchCounter = 0;
                Dictionary<string, List<AccountLine>> potentialMatches = new Dictionary<string, List<AccountLine>>();
                potentialMatches.Add("DateDescAmount", new List<AccountLine>());
                potentialMatches.Add("DateDesc", new List<AccountLine>());
                potentialMatches.Add("Reference", new List<AccountLine>());

                foreach (var account in rest)
                {
                    foreach (var candidate in account.Items)
                    {
                        bool foundAlready = false;

                        SimpleMoney combined = item.ActualAmount + candidate.ActualAmount;

                        if (item.Date.Equals(candidate.Date)
                            && item.Desc.Equals(candidate.Desc)
                            && combined.Equals(new SimpleMoney(0,0))
                            )
                        {
                            foundAlready = true;
                            hitCounter++;
                            potentialMatchCounter++;
                            potentialMatches["DateDescAmount"].Add(candidate);
                            //consoleReporter.WriteLine(" Potential match {1:0000}: \t{0}", candidate, hitCounter);

                        }

                        if (item.Date.Equals(candidate.Date)
                            && item.Desc.Equals(candidate.Desc)
                            )
                        {
                            foundAlready = true;
                            hitCounter++;
                            potentialMatchCounter++;
                            potentialMatches["DateDesc"].Add(candidate);
                            //consoleReporter.WriteLine(" Potential match {1:0000}: \t{0}", candidate, hitCounter);

                        }

                        if (item.Reference.Equals(candidate.Reference))
                        {
                            //if (foundAlready)
                            //{
                            //    consoleReporter.WriteLine(" !!! Found already: {0}", candidate);
                            //}

                            foundAlready = true;
                            hitCounter++;
                            potentialMatchCounter++;
                            potentialMatches["Reference"].Add(candidate);
                        }


                    }

                    //if (potentialMatchCounter == 0) continue;
                    //if (potentialMatchCounter != 2)
                    //{
                    //    consoleReporter.WriteLine("\nLooking for: \t{0} - found {1}", item, potentialMatchCounter);
                    //    foreach (var type in potentialMatches)
                    //    {
                    //        //consoleReporter.WriteLine(" Type: {0}, Count: {1}", type.Key, type.Value.Count);
                    //        if (type.Value.Count != 0)
                    //        {
                    //            foreach (var res in type.Value)
                    //            {
                    //                var calculated = res.ActualAmount + item.ActualAmount;
                    //                var zero = new SimpleMoney(0,0);
                    //                consoleReporter.WriteLine(" {1,10}: \t{0} \n \t== {2} / {3}", res, type.Key, calculated, zero);
                    //                if (calculated == zero) consoleReporter.WriteLine("YAY");
                    //                if (calculated.Equals( zero)) consoleReporter.WriteLine("YAY2");
                    //            }
                    //        }
                    //    }
                    //}

                    ////if (hitCounter > 100) return; //TODO: Don't do this!
                }

                if (potentialMatchCounter == 0) continue;
                if (potentialMatchCounter != 2)
                {
                    consoleReporter.WriteLine("\nLooking for: \t{0} - found {1}", item, potentialMatchCounter);
                    foreach (var type in potentialMatches)
                    {
                        //consoleReporter.WriteLine(" Type: {0}, Count: {1}", type.Key, type.Value.Count);
                        if (type.Value.Count != 0)
                        {
                            foreach (var res in type.Value)
                            {
                                var calculated = res.ActualAmount + item.ActualAmount;
                                var zero = new SimpleMoney(0, 0);
                                consoleReporter.WriteLine(" {1,10}: \t{0} \t \t== {2} / {3}", res, type.Key, calculated, zero);
                                //if (calculated == zero) consoleReporter.WriteLine("YAY");
                                //if (calculated.Equals(zero)) consoleReporter.WriteLine("YAY2");
                            }
                        }
                    }
                }

                //if (hitCounter > 100) return; //TODO: Don't do this!


            }
        }
    }
}
