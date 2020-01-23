using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApplication.Models
{
    public class BillViewModel
    {
        public Customer Customer { get; set; }
        public BillPay Billpay { get; set; }

        [Required]
        public int SelectedAccount { get; set; }
    }
}
