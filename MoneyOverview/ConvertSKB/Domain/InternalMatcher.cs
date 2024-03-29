﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoneyOverview.Core.Domain;

namespace ConvertSKB.Domain
{
    public class InternalMatcher
    {
        private SkbRepository skbRepo;
        private InternalTransactionsRepository internalRepo;
        private Func<AccountLine, AccountLine, bool> match;

        public InternalMatcher(SkbRepository skbRepo, InternalTransactionsRepository internalRepo)
        {
            this.skbRepo = skbRepo;
            this.internalRepo = internalRepo;
        }

        public void Match(Func<AccountLine, AccountLine, bool> matchmethod)
        {
            this.match = matchmethod;
            ProcessResult pRes = new ProcessResult(skbRepo.GetAll(), true);
            do
            {
                pRes = ProcessMatches(pRes.UnMatched);
            } while (pRes.AnyChange);
        }

        public static bool MatchOnAll(AccountLine item, AccountLine candidate)
        {
            return MatchOnMost(item, candidate) && item.Reference.Equals(candidate.Reference);
        }

        public static bool MatchOnMost(AccountLine item, AccountLine candidate)
        {
            SimpleMoney total = item.ActualAmount + candidate.ActualAmount;
            return item.Date.Equals(candidate.Date)
                && total.Equals(SimpleMoney.Zero)
                && item.Desc.Equals(candidate.Desc);
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
