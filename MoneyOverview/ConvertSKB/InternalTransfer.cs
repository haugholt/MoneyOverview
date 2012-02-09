using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoneyOverview.Core.Domain;

namespace ConvertSKB
{
    public class InternalTransfer
    {
        private AccountLine item;
        private AccountLine candidate;

        public InternalTransfer(AccountLine item, AccountLine candidate)
        {
            // TODO: Complete member initialization
            this.item = item;
            this.candidate = candidate;
        }

        public AccountLine Candidate { get { return this.candidate; } }

        public bool HasMatchingDates { get { return item.Date.Equals(candidate.Date); } }

        public bool HasMatchingDescriptions { get { return item.Desc.Equals(candidate.Desc); } }

        public bool HasMatchingAmounts
        {
            get
            {
                SimpleMoney combined = item.ActualAmount + candidate.ActualAmount;
                return combined.Equals(new SimpleMoney(0, 0));
            }
        }

        public bool HasMatchingReference { get { return item.Reference.Equals(candidate.Reference); } }

        public override string ToString()
        {
            return string.Format(" {1,10}: \t{0}", candidate, "");
        }

        
    }
}
