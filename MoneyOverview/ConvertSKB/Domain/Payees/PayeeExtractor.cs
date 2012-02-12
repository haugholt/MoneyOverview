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
            /*
*1065 20.12 NOK 200,00 BUYPASS MINIBAN
*1065 20.12 GBP 28,82 Amazon EU KURS: 9,3948
rjan Haugholt
*1065 05.12 GBP 42,50 WWW.EMMABRIDGEWATE KURS: 9,1788
*1065 30.11 NOK 275,00 ONE.COM
*1065 02.09 NOK 2074,00 RENT-A-WRECK OSLO VEST
*1065 04.09 SEK 217,00 RASTA SVINESUND KURS: 0,8581
*1065 04.09 SEK 50,00 RASTA SVINESUND KURS: 0,8582
*1065 04.09 SEK 1931,80 NORDBY SUPERMARKET KURS: 0,8581
*1065 04.09 SEK 858,00 BABYPROFFSEN KURS: 0,8594
*1065 04.09 SEK 845,00 HAPPYLAND BABYSHOP KURS: 0,8594
Fra: rjan Haugholt Betalt: 24.11.11
Kostnader SMS-bank Juli 2011
             
             * */
            StripNonRepeatingData("^[0-9]{2}\\.[0-9]{2}");
            StripNonRepeatingData("^TIL: *");
            StripNonRepeatingData(" *BETNR: *[0-9]{1,5}$");
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
