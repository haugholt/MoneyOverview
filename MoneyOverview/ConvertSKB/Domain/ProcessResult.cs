using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertSKB.Domain
{
    public class ProcessResult
    {
        private List<AccountLine> list;
        private AccountLine item;
        public bool AnyChange { get; private set; }

        public ProcessResult(List<AccountLine> list, bool anyChange)
        {
            // TODO: Complete member initialization
            this.list = list;
            this.AnyChange = anyChange;
        }

        public ProcessResult(AccountLine item, bool anyChange)
        {
            this.list = new List<AccountLine>();
            this.list.Add(item);
            this.AnyChange = anyChange;
        }

        public List<AccountLine> UnMatched { get { return new List<AccountLine>(list); } }

        public bool HasItems { get { return list.Count > 0; } }

        public static ProcessResult End
        {
            get
            {
                return new ProcessResult(new List<AccountLine>(), false);
            }
        }

        public static ProcessResult SomethingChanged
        {
            get
            {
                return new ProcessResult(new List<AccountLine>(), true);
            }
        }

    }
}
