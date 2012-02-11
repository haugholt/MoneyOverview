using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertSKB.Domain
{
    public class InternalMatcherService
    {
        private SkbRepository skbRepo;
        private InternalTransactionsRepository internalRepo;
        private Func<AccountLine, AccountLine, bool> match;

        public InternalMatcherService(SkbRepository skbRepo, InternalTransactionsRepository internalRepo, Func<AccountLine, AccountLine, bool> match)
        {
            this.skbRepo = skbRepo;
            this.match = match;
            this.internalRepo = internalRepo;
        }

        public void Match()
        {
            ProcessResult pRes = new ProcessResult(skbRepo.GetAll(), true);
            do
            {
                pRes = ProcessMatches(pRes.UnMatched);
            } while (pRes.AnyChange);
        }

        private ProcessResult ProcessMatches(List<AccountLine> toProcess)
        {
            if (toProcess.Count < 1) return ProcessResult.End;
            bool anythingChanged = false;
            List<AccountLine> unMatched = new List<AccountLine>();
            foreach (var item in toProcess)
            {
                ProcessResult res = ProcessItem(item);
                if (res.HasItems)
                {
                    unMatched.AddRange(res.UnMatched);
                }
                if (res.AnyChange) anythingChanged = true;
            }
            return new ProcessResult(unMatched, anythingChanged);
        }

        private ProcessResult ProcessItem(AccountLine item)
        {
            bool anythingChanged = false;
            MatchResult result = FindCandidateMatches(item);
            if (result.HasMultipleCandidates) return new ProcessResult(item, false);
            if (result.HasNoResults) return ProcessResult.End;

            TransferInternal(item, result.BestCandidate);
            return ProcessResult.SomethingChanged;
        }

        private void TransferInternal(AccountLine item, AccountLine candidate)
        {
            skbRepo.Remove(item);
            skbRepo.Remove(candidate);
            internalRepo.Add(item, candidate);
        }

        private MatchResult FindCandidateMatches(AccountLine item)
        {
            List<AccountLine> foundCandidates = new List<AccountLine>();
            foreach (var candidate in skbRepo.GetAll())
            {
                if (this.match(item, candidate)) foundCandidates.Add(candidate);
            }
            return new MatchResult(foundCandidates);
        }

    }
}
