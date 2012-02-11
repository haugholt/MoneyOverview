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
        private SimpleMoney TotalInternal;

        public InternalTransfersMatcher(IEnumerable<Account> iEnumerable, ConsoleReporter consoleReporter)
        {
            this.Reporter = consoleReporter;
            this.iEnumerable = iEnumerable;
            rest = new List<Account>(iEnumerable);
            TotalInternal = new SimpleMoney(0, 0);
        }

        public Dictionary<AccountLine, AccountLine> GetMatches()
        {
            Dictionary<AccountLine, AccountLine> results = new Dictionary<AccountLine, AccountLine>();

            var result = HeadTail(rest, results);
            Reporter.WriteLine("\n\nLE TOTAL INTERNAL TRANSFERS: {0}", TotalInternal);
            return result;
        }

        private Dictionary<AccountLine, AccountLine> HeadTail(List<Account> rest, Dictionary<AccountLine, AccountLine> results)
        {
            if (rest.Count <= 1) return results;
            var input = rest[0];
            Reporter.WriteLine("Walking account: {0}", input.Name);
            rest.RemoveAt(0);

            FindMatches(input, rest, results);

            return HeadTail(rest, results);
        }

        private void FindMatches(Account input, List<Account> rest, Dictionary<AccountLine, AccountLine> results)
        {

            foreach (var item in input.Items)
            {
                if (item.Type.Equals("Kreditrente"))
                {
                    continue;
                }

                PotentialMatches potMatches = new PotentialMatches(item);

                foreach (var account in rest)
                {
                    foreach (var candidate in account.Items)
                    {
                        InternalTransfer candidateMatch = new InternalTransfer(item, candidate);
                        PotentialMatch potentialMatch = new PotentialMatch(candidateMatch);

                        SimpleMoney combined = item.ActualAmount + candidate.ActualAmount;

                        if (candidateMatch.HasMatchingDates
                            && candidateMatch.HasMatchingAmounts
                            && candidateMatch.HasMatchingDescriptions
                            && candidateMatch.HasMatchingReference
                            )
                        {
                            potentialMatch.AddMethod("DDAmount+");
                        }

                        if (candidateMatch.HasMatchingDates
                            && candidateMatch.HasMatchingAmounts
                            && candidateMatch.HasMatchingDescriptions
                            )
                        {
                            potentialMatch.AddMethod("DDAmount");
                        }

                        if (potentialMatch.HasAnyMatches)
                        {
                            potMatches.Add(potentialMatch);

                        }
                    } //candidate
                } //Account in rest

                if (potMatches.HasMatches)
                {
                    if (!potMatches.HasBestMatch)
                    {
                        Reporter.WriteLine("WARN: No best match, total Amount WILL Be off!");
                    }
                    
                    if (potMatches.HasBestMatch)
                    {
                        TotalInternal += potMatches.AbsoluteAmount;
                        foreach (var account in rest)
                        {
                            if (account.Items.Contains(potMatches.BestCandidate))
                            {
                                account.Remove(potMatches.BestCandidate);
                                account.Remove(potMatches.Source);
                                Reporter.WriteLine("\nRemoved A: {0}", potMatches.BestCandidate);
                                Reporter.WriteLine("Removed B: {0}", potMatches.Source);
                            }
                        }
                    }
                }

            }// each item in input
            //Reporter.WriteLine("\n\nTOTAL INTERNAL TRANSFERS: {0}", totalInternal);    
        }


    }
}
