using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertSKB.Domain.Payees
{
    public class PayeeRepository
    {
        List<Payee> list = new List<Payee>();
        public PayeeRepository()
        {
            list = new List<Payee>();
        }
        public IEnumerable<Payee> GetAll()
        {
            return new List<Payee>(list);
        }
    }
}
