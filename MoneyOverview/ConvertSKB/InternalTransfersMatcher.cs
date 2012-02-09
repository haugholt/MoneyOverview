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

                Dictionary<AccountLine, List<PotentialMatch>> potMatches = new Dictionary<AccountLine, List<PotentialMatch>>();

                foreach (var account in rest)
                {
                    foreach (var candidate in account.Items)
                    {
                        InternalTransfer candidateMatch = new InternalTransfer(item, candidate);
                        PotentialMatch potentialMatch = new PotentialMatch(candidateMatch);

                        bool foundAlready = false;
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

                        //if (candidateMatch.HasMatchingDates && candidateMatch.HasMatchingDescriptions && candidateMatch.HasMatchingReference)
                        //{
                        //    potentialMatch.AddMethod("DateDescRef");
                        //}

                        //if (candidateMatch.HasMatchingDates && candidateMatch.HasMatchingDescriptions)
                        //{
                        //    potentialMatch.AddMethod("DateDesc");
                        //}

                        //if (candidateMatch.HasMatchingReference)
                        //{
                        //    potentialMatch.AddMethod("Reference");
                        //}

                        if (potentialMatch.HasAnyMatches)
                        {
                            if (!potMatches.ContainsKey(item)) potMatches.Add(item, new List<PotentialMatch>());
                            potMatches[item].Add(potentialMatch);
                        }
                    } //candidate
                } //Account in rest

                if(potMatches.ContainsKey(item)){
                    int potCount = potMatches[item].Count;
                    //if (potCount == 1) continue;
                    if (potCount == 1)
                    {
                        if (potMatches[item][0].OnlyMatchesOn("DDAmount"))
                        {
                            continue;
                            Reporter.WriteLine("\nOnly DDAmount\nLooking for: \t{0} - found {1}", item, potCount);
                            foreach (var pot in potMatches[item])
                            {
                                Reporter.WriteLine("{0}", pot);
                            }
                        }
                        foreach (var account in rest)
                        {
                            if (account.Items.Contains(potMatches[item][0].Candidate)){
                                account.Items.Remove(potMatches[item][0].Candidate);
                                account.Items.Remove(potMatches[item][0].Candidate);
                            }
                        }
                    }else{

                    Reporter.WriteLine("\nLooking for: \t{0} - found {1}", item, potCount);
                    foreach(var pot in potMatches[item]){
                        Reporter.WriteLine("{0}", pot);
                    }
                }
                }

            }// each item in input
        }


    }
}
