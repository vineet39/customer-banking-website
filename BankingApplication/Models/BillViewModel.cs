using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApplication.Models
{
    public class BillViewModel
    {
        public Customer Customer;
        public BillPay Billpay;

        [Required]
        public int selectedAccount;

        public

        [Required]
        public decimal amount;

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime scheduledDate;
    }
}
