using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertSKB
{
    public class Money
    {
        private string amount;
        private decimal p;

        // Declare which operator to overload (+), the types 
        // that can be added (two Complex objects), and the 
        // return type (Complex):
        public static Money operator +(Money c1, Money c2)
        {
            return new Money(c1.Value+ c2.Value);
        }

        public Money(string amount)
        {
            // TODO: Complete member initialization
            this.amount = amount;
        }

        public Money(decimal p)
        {
            // TODO: Complete member initialization
            this.amount = p.ToString();
        }

        decimal Value
        {
            get {
                return decimal.Parse(amount);
                //decimal ci = 0;
                //var bits = amount.Split(new char[] { ',' }, 2);
                //ci = decimal.Parse(bits[0]);
                ////Console.Out.WriteLine("Got here");
                ////Console.Out.WriteLine("0," + bits[1]);
                //ci = ci + decimal.Parse("0," + bits[1]);
                //return ci;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}", Value);
        }

        public override bool Equals(object obj)
        {
            Console.Out.WriteLine("Woot");
            Money other = obj as Money;
            if (other == null )return base.Equals(obj);

            return Value == other.Value;
        }

    }
}
