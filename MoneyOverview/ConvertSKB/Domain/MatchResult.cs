using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertSKB.Domain
{
    public class MatchResult
    {
        private List<AccountLine> foundCandidates;

        public MatchResult(List<AccountLine> foundCandidates)
        {
            this.foundCandidates = foundCandidates;
        }

        public bool HasMultipleCandidates { get { return foundCandidates.Count > 1; } }
        public bool HasNoResults { get { return foundCandidates.Count < 1; } }
        public AccountLine BestCandidate
        {
            get
            {
                if (HasMultipleCandidates || HasNoResults) throw new Exception("This has never happened before!");
                return foundCandidates[0];
            }
        }
    }
}
