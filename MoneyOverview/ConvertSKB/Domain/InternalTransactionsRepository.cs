using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertSKB.Domain
{
    public class InternalTransactionsRepository
    {
        List<InternalTransaction> lines = new List<InternalTransaction>();
        internal void Add(AccountLine item, AccountLine candidate)
        {
            lines.Add(new InternalTransaction(item, candidate));
        }

        internal List<AccountLine> GetAll()
        {
            var flattened = new List<AccountLine>();
            foreach (var pair in lines)
            {
                flattened.Add(pair.Item1);
                flattened.Add(pair.Item2);
            }
            return flattened;
        }

        internal class InternalTransaction
        {
            internal AccountLine Item1;
            internal AccountLine Item2;
            internal InternalTransaction(AccountLine item1, AccountLine item2) {
                Item1 = item1;
                Item2 = item2;
            }
        }
    }
}
