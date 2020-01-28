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
        public List<NameBill> bills;

        public BillScheduleViewModel(Account a, List<Payee> p)
        {
            bills =
                (from bill in a.Bills
                 join payee in p
                 on bill.PayeeID equals payee.PayeeID
                 select new NameBill { PayeeName = payee.PayeeName, Bill = bill })
                 .ToList();
        }

        public class NameBill
        {
            public string PayeeName { get; set; }
            public BillPay Bill { get; set; }
        }
    }
}
