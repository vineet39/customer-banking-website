using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApplication.Models
{
    public class BillScheduleViewModel
    {
        public Account account;
        public List<Payee> payees;
        public Dictionary<string, BillPay> bills;

        public BillScheduleViewModel(Account a, List<Payee> p)
        {
            bills =
                (from bill in a.Bills
                 join payee in p
                 on bill.PayeeID equals payee.PayeeID
                 select new { payee.PayeeName, bill })
                 .ToDictionary(x => x.PayeeName, x => x.bill);
        }

        public void Join(Account a, List<Payee> p)
        {
            bills =
                (from bill in a.Bills
                 join payee in p
                 on bill.PayeeID equals payee.PayeeID
                 select new { payee.PayeeName, bill }).ToDictionary(x => x.PayeeName, x => x.bill);
        }
    }
}
