using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertSKB
{
    public class PotentialMatches
    {
        Dictionary<AccountLine, List<PotentialMatch>> potMatches;
        private AccountLine item;

        public PotentialMatches(AccountLine item)
        {
            this.item = item;
            potMatches = new Dictionary<AccountLine, List<PotentialMatch>>();
        }

        internal void Add(PotentialMatch potentialMatch)
        {
            if (!potMatches.ContainsKey(item)) potMatches.Add(item, new List<PotentialMatch>());
            potMatches[item].Add(potentialMatch);
        }

        public bool HasMatches { get { return potMatches.ContainsKey(item); } }

        public int Count { get { return potMatches[item].Count; } }

        public bool HasBestMatch
        {
            get
            {
                try
                {
                    var result = BestMatch;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public AccountLine Source { get { return item; } }

        public PotentialMatch BestMatch { get { 
            if(Count == 1)
                return potMatches[item][0];

            PotentialMatch toReturn = null;
            bool foundBest = false;
            foreach(var candidate in potMatches[item]){
                if(candidate.MatchesOn("DDAmount+")){
                    if(foundBest) throw new NotImplementedException("Multiple best candidates found!");
                    foundBest = true;
                    toReturn = candidate;
                }
            }

            if (toReturn == null) throw new NotImplementedException("No BestMatch found! "+this.ToString());
            if(!foundBest) throw new NotImplementedException("No BestMatch found!");
            return toReturn;
        } }

        public AccountLine BestCandidate { get { 
            return BestMatch.Candidate; 
        } }

        public IEnumerable<PotentialMatch> Matches { get { return new List<PotentialMatch>(potMatches[item]);} }

        public override string ToString()
        {
            StringBuilder bu = new StringBuilder();

            foreach (var pot in potMatches[item])
            {
                bu.AppendFormat("\n{0}", pot);
            }
            string potentials = bu.ToString();
            return string.Format("Potential Matches for {0}:\n{1}", item, potentials);
        }



        public MoneyOverview.Core.Domain.SimpleMoney AbsoluteAmount { get 
        {
            if (item.ActualAmount.IsPositiveNumber) return item.ActualAmount;
            return BestCandidate.ActualAmount;
            }
        }
    }
}
