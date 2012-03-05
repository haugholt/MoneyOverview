using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoneyOverview.Core.Infrastructure;

namespace ConvertSKB
{
    public class FilePersistor
    {
        private string pathOut;
        FileWriter writer;

        public FilePersistor(string pathOut)
        {
            this.pathOut = pathOut;
        }

        public void Persist(IEnumerable<AccountLine> lines)
        {
            List<string> toWrite = new List<string>();
            toWrite.Add("AccountName;Date;ActualAmount;Type;Reference;Desc");
            //consoleReporter.WriteLine("AccountName;Date;ActualAmount;Type;Reference;Desc");
            foreach (var resitem in lines)
            {
                toWrite.Add(string.Format("{0};{1};{2};{3};{4};{5}"
                , resitem.AccountName, resitem.Date, resitem.ActualAmount, resitem.Type, resitem.Reference, resitem.Desc));
            }
            
            FileWriter writer = new FileWriter();
            writer.WriteLines(pathOut, toWrite);
        }
    }
}
