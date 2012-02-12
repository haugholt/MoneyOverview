using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertSKB.Domain
{
    public static class AccountLineFilterService
    {
        internal static List<AccountLine> GetPositiveLines(List<AccountLine> list)
        {
            List<AccountLine> found = new List<AccountLine>();
            foreach (var item in list)
            {
                if (item.ActualAmount.IsPositiveNumber) found.Add(item);
            }
            return found;
        }
    }
}
