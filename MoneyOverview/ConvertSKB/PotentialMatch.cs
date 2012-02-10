using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertSKB
{
    public class PotentialMatch
    {
        private InternalTransfer candidateMatch;
        List<string> matchMethods = new List<string>();
        public PotentialMatch(InternalTransfer candidateMatch)
        {
            this.candidateMatch = candidateMatch;
        }

        internal void AddMethod(string p)
        {
            matchMethods.Add(p);
        }

        public bool HasAnyMatches { get { return matchMethods.Count > 0; } }

        public override string ToString()
        {
            var arr = matchMethods.ToArray();
            var summary= string.Join(", ", arr);
            return string.Format("{0} [{1}]", candidateMatch, summary);
        }

        internal bool OnlyMatchesOn(string p)
        {
            return matchMethods.Count == 1 && MatchesOn(p);
        }

        internal bool MatchesOn(string p)
        {
            return matchMethods.Contains(p);
        }

        public AccountLine Candidate { get { return candidateMatch.Candidate; } }
    }
}
