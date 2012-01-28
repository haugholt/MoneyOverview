using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ConvertSKB
{
    public class PathAnalyst
    {
        private string path;
        private List<Account> accounts;
        private bool hasNotParsedYet = true;
        private AccountReader accountReader;

        public PathAnalyst(string path, AccountReader accountReader)
        {
            this.path = path;
            this.accountReader = accountReader;
        }

        public string Bank {
            get { return "Skandiabanken"; }
        }

        public IEnumerable<Account> Accounts { 
            get {
                if (hasNotParsedYet) ParseAccounts();
                return new List<Account>(accounts);
            } 
        }

        private void ParseAccounts()
        {
            hasNotParsedYet = false;
            accounts = new List<Account>();

            System.IO.DirectoryInfo folder = new DirectoryInfo(path);
            foreach (var file in folder.GetFiles("*.csv"))
            {
                Account next = AccountReader.Read(file);
                accounts.Add(next);
            }
        }
    }
}
