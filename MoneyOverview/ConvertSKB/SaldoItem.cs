using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertSKB
{
    public class SaldoItem
    {
        private string utSaldo;
        private string utSaldoDato;

        public SaldoItem(string utSaldo, string utSaldoDato)
        {
            // TODO: Complete member initialization
            this.utSaldo = utSaldo;
            this.utSaldoDato = utSaldoDato;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", utSaldoDato, utSaldo);
        }
    }
}
