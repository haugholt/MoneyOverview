using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertSKB
{
    public class AccountLine
    {
        private string date;
        private string reference;
        private string type;
        private string desc;
        private string amount;

        public AccountLine(string date, string reference, string type, string p_4, string amount)
        {
            // TODO: Complete member initialization
            this.date = date;
            this.reference = reference;
            this.type = type;
            this.desc = p_4;
            this.amount = amount;
        }
    }
}
