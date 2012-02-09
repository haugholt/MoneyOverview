using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoneyOverview.Core.Domain;

namespace ConvertSKB
{
    public class InternalTransfersMatcher
    {
        private IEnumerable<Account> iEnumerable;
        List<Account> rest;
        private ConsoleReporter Reporter;

        public InternalTransfersMatcher(IEnumerable<Account> iEnumerable, ConsoleReporter consoleReporter)
        {
            this.Reporter = consoleReporter;
            this.iEnumerable = iEnumerable;
            rest = new List<Account>(iEnumerable);
        }

        public Dictionary<AccountLine, AccountLine> GetMatches()
        {
            Dictionary<AccountLine, AccountLine> results = new Dictionary<AccountLine, AccountLine>();

            return HeadTail(rest, results);
        }

        private Dictionary<AccountLine, AccountLine> HeadTail(List<Account> rest, Dictionary<AccountLine, AccountLine> results)
        {
            if (rest.Count <= 1) return results;
            var input = rest[0];
            rest.RemoveAt(0);

            FindMatches(input, rest, results);

            return HeadTail(rest, results);
        }

        private void FindMatches(Account input, List<Account> rest, Dictionary<AccountLine, AccountLine> results)
        {
            int hitCounter = 0;
            foreach (var item in input.Items)
            {
                if (item.Type.Equals("Kreditrente"))
                {
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
                            && combined.Equals(new SimpleMoney(0, 0))
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
                    } //candidate
                } //Account in rest

                if (potentialMatchCounter == 0) continue;
                if (potentialMatchCounter != 3)
                {
                    Reporter.WriteLine("\nLooking for: \t{0} - found {1}", item, potentialMatchCounter);
                    foreach (var type in potentialMatches)
                    {
                        //consoleReporter.WriteLine(" Type: {0}, Count: {1}", type.Key, type.Value.Count);
                        if (type.Value.Count != 0)
                        {
                            foreach (var res in type.Value)
                            {
                                var calculated = res.ActualAmount + item.ActualAmount;
                                var zero = new SimpleMoney(0, 0);
                                Reporter.WriteLine(" {1,10}: \t{0} \t \t== {2} / {3}", res, type.Key, calculated, zero);
                                //if (calculated == zero) consoleReporter.WriteLine("YAY");
                                //if (calculated.Equals(zero)) consoleReporter.WriteLine("YAY2");
                            }
                        }
                    }
                }
            }// each item in input
        }


    }
}
