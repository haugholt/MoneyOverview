using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoneyOverview.Core.Domain;

namespace ConvertSKB.Domain
{
    public static class AccountLineAggregatorService
    {
        public static SimpleMoney Sum(List<AccountLine> lines)
        {
            SimpleMoney sum = SimpleMoney.Zero;
            foreach (var item in lines)
            {
                sum += item.ActualAmount;
            }
            return sum;
        }
    }
}
