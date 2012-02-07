using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoneyOverview.Core.Domain;

namespace ConvertSKB
{
    public class SimpleMoneyParser
    {
        internal SimpleMoney Parse(string amount)
        {
            if (amount.Contains(',')) {
                string[] parts = amount.Split(new char[]{','}, 2);
                int kroner = Rinse(parts[0]);
                int ears = Rinse(parts[1]);

                return new SimpleMoney(kroner, ears);
            }
            throw new NotImplementedException("Only handles commas for now");
        }

        private int Rinse(string p)
        {
            string res = p.Replace(".", "");
            res = p.Replace(" ", "");

            return int.Parse(res);
        }
    }
}
