using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ConvertSKB.Domain.Payees
{
    public class PayeeExtractor
    {
        private SkbRepository SkbRepo;
        private ConsoleReporter Reporter;

        public PayeeExtractor(SkbRepository skbRepo, PayeeRepository payeeRepo, ConsoleReporter consoleReporter)
        {
            this.SkbRepo = skbRepo;
            this.Reporter = consoleReporter;
        }

        internal void ExtractPayees()
        {
            StripNonRepeatingData();
            //Extract();
        }

        private void StripNonRepeatingData()
        {
            StripNonRepeatingData("^[0-9]{2}\\.[0-9]{2}");
            
            StripNonRepeatingData("^TIL: *");
            StripNonRepeatingData(" *BETNR: *[0-9]{1,5}$");

            StripNonRepeatingData("^\\*[0-9]{4} [0-9]{2}\\.[0-9]{2} (NOK|GBP|SEK) [0-9]*,[0-9]{2} ");
            StripNonRepeatingData(" KURS: [0-9]*,[0-9]*$");
            
            StripNonRepeatingData("^Fra: ");
            StripNonRepeatingData(" Betalt: [0-9]{2}\\.[0-9]{2}\\.[0-9]{2}");
            StripNonRepeatingData(" (Jan|Feb|Mar|Apr|Mai|Jun|Jul|Aug|Sep|Okt|Nov|Des|May|Oct|Dec) ");
            StripNonRepeatingData(" (Januar|Februar|Mars|April|Mai|Juni|Juli|August|September|Oktober|November|Desember) ");
            StripNonRepeatingData("2011");
            StripNonRepeatingData("(  )*");//Double spaces
        }

        private void StripNonRepeatingData(string regexstring) {
            Regex regex = new Regex(regexstring);
            var res = SkbRepo.GetAll().FindAll((AccountLine item) => regex.IsMatch(item.Desc));
            //res.ForEach(item => Reporter.WriteLine("{0}", item.Desc));
            res.ForEach(item => StripDesc(item, regex));
        }


        private void StripDesc(AccountLine item, Regex regex)
        {
            string original = item.Desc;
            string updated = regex.Replace(original, "");

            AccountLine res = new AccountLine(item.Date, item.Reference, item.Type, updated, item.Amount, item.ActualAmount, item.AccountName);
            //Reporter.WriteLine("{0}\n{1}\n", item.Desc, res.Desc);
            //Reporter.WriteLine("{0}", res.Desc);
            SkbRepo.Remove(item);
            SkbRepo.Add(res);
        }
    }
}
