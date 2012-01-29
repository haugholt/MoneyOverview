using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoneyOverview.Core.Infrastructure;

namespace ConvertSKB
{
    public class AccountReader
    {
        internal static Account Read(System.IO.FileInfo file)
        {
            FileReader reader = new FileReader(file.FullName);
            var lines = reader.ReadAllLines();
            string name = ParseFileName(file.Name);
            
            Console.Out.WriteLine("\n\n{0}", name);

            var utSaldo = ParseSaldoLine(lines[0]);
            lines.RemoveAt(0);
            lines.RemoveAt(0); //Headings

            var innSaldo = ParseSaldoLine(lines[lines.Count - 1]);
            lines.RemoveAt(lines.Count -1);

            Account result = new Account(file.Name, name, innSaldo, utSaldo);


            foreach (var line in lines)
            {
                AccountLine aLine = ParseLine(line);
                result.AddLine(aLine);
            }

            return result;
        }

        private static string ParseFileName(string fileName)
        {
            var result = fileName.Split(new char[] { '_' });
            return result[0];
         }

        private static SaldoItem ParseSaldoLine(string line)
        {
            List<string> items = SplitTheLine(line);
            //Console.Out.WriteLine("First Line: {0} items");

            //int i = 0;
            //foreach (string item in items) {
            //    Console.Out.WriteLine(" {0} Line: {1} ", i++, item);
            //}

            string utSaldoDato = items[4]; //TODO: Parse the date
            string utSaldo = items[6];

            Console.Out.WriteLine(" Saldo: {0}: {1}", utSaldoDato, utSaldo);
            return new SaldoItem(utSaldo, utSaldoDato);
        }


        private static List<string> SplitTheLine(string line)
        {
            var result = line.Split(new char[] { ';' });
            var temp = new List<string>(result);
            var endResult = new List<string>(temp.Count);
            foreach (var item in temp)
            {
                string workitem = item;
                if (workitem.StartsWith("\""))
                {
                    workitem = workitem.Substring(1);
                }
                if (workitem.EndsWith("\""))
                {
                    workitem = workitem.Substring(0, workitem.Length-1);
                }
                endResult.Add(workitem);
            }
            return endResult;
            //return new List<string>(endResult);
        }

        private static AccountLine ParseLine(string line)
        {
            //"BOKFØRINGSDATO";"RENTEDATO";"ARKIVREFERANSE";"TYPE";"TEKST";"UT FRA KONTO";"INN PÅ KONTO";
            //Console.Out.WriteLine(" {0}", line);
            List<string> items = SplitTheLine(line);

            //if (!items[0].Equals(items[1])) Console.Out.WriteLine("DateDiff: {0}", line);

            string amount = null;
            if (items[5] != "") amount = "-" +items[5];
            if (items[6] != "") amount = items[6];
            if (amount == null) throw new NotImplementedException("Amount cannot be null!");
            return new AccountLine(items[0], items[2], items[3], items[4], amount);
        }
    }
}
