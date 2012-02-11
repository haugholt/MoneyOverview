using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoneyOverview.Core.Domain;

namespace ConvertSKB
{
    public class AccountLine
    {
        //private string date;

        public string Date { get; private set; }
        public string Reference { get; private set; }
        public string Type { get; private set; }
        public string Desc { get; private set; }
        public string Amount { get; private set; }
        public SimpleMoney ActualAmount { get; private set; }
        public string AccountName { get; private set; }

        public AccountLine(string date, string reference, string type, string p_4, string amount, SimpleMoney actualAmount, string accountName)
        {
            // TODO: Complete member initialization
            this.Date = date;
            this.Reference = reference;
            this.Type = type;
            this.Desc = p_4;
            this.Amount = amount;
            this.ActualAmount = actualAmount;
            this.AccountName = accountName;
        }

        public override string ToString()
        {
            return string.Format("AccountLine [{0}, {1}, {2}, {3}, {4}/{5}]", Date ,Reference, Type, Amount, Desc, ActualAmount);
        }
    }
}
