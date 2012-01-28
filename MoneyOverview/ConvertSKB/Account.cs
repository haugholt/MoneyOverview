using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertSKB
{
    public class Account
    {
        private List<AccountLine> lines;
        private SaldoItem innSaldo;
        private SaldoItem utSaldo;

        public Account(string filename, string name, SaldoItem innSaldo, SaldoItem utSaldo)
        {
            // TODO: Complete member initialization
            this.Fullname = filename;
            this.Name = name;
            this.innSaldo = innSaldo;
            this.utSaldo = utSaldo;
            lines = new List<AccountLine>();
        }

        public string Fullname { get; private set; }

        internal void AddLine(AccountLine aLine)
        {
            lines.Add(aLine);
        }

        public string Name { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}: {1} lines : {2} : {3}", Name, lines.Count, innSaldo, utSaldo);
        }

        public List<AccountLine> Items {
            get { return new List<AccountLine>(this.lines); } 
        }
    }
}
