using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoneyOverview.Core.Domain
{
    public class SimpleMoney
    {
        public int Kroner { get; private set; }
        public int Ears { get; private set; }

        public SimpleMoney(int kroner, int ears)
        {
            Kroner = kroner;
            Ears = ears;
        }

        public SimpleMoney(int kroner) : this(kroner, 0)
        {
        }

        // Declare which operator to overload (+), the types 
        // that can be added (two Complex objects), and the 
        // return type (Complex):
        public static SimpleMoney operator +(SimpleMoney c1, SimpleMoney c2)
        {
            return new SimpleMoney(c1.Kroner + c2.Kroner, c1.Ears + c2.Ears);
        }

        public override string ToString()
        {
            return string.Format("{0:### ### ##0},{1:00}", Kroner, Ears);
        }

        public override bool Equals(object obj)
        {
            SimpleMoney other = obj as SimpleMoney;
            if (other == null) return base.Equals(obj);

            return Kroner == other.Kroner && Ears == other.Ears;
        }


        public bool IsPositiveNumber { get { 
            return Kroner >= 0 && Ears >= 0; 
        } }
    }
}
