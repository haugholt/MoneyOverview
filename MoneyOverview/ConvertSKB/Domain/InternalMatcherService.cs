using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertSKB.Domain
{
    public delegate MatchResult MyDelegateType(AccountLine item, SkbRepository skbRepo);

    public class InternalMatcherService
    {
//        delegate MatchResult MyDelegateType(AccountLine item, SkbRepository skbRepo);

        //public static void Match(SkbRepository skbRepo, MyDelegateType fop)
        public static void Match(SkbRepository skbRepo, Func<AccountLine, SkbRepository, MatchResult> convertMethod)
        {
            ProcessResult pRes = new ProcessResult(skbRepo.GetAll(), true);
            do
            {
                pRes = ProcessMatches(pRes.UnMatched);
            } while (pRes.AnyChange);
        }

        private static ProcessResult ProcessMatches(List<AccountLine> toProcess)
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

        private static ProcessResult ProcessItem(AccountLine item)
        {
            bool anythingChanged = false;
            throw new NotImplementedException();
        }

        internal static void Macho(SkbRepository skbRepo, Func<AccountLine, SkbRepository, MatchResult> convertMethod)
        {
            throw new NotImplementedException();
        }
    }
}
