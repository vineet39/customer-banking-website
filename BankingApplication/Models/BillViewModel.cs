using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankingApplication.Models
{
    public class BillViewModel {
        public Customer Customer { get; set; }
        public BillPay Billpay { get; set; } = new BillPay ();

        [Required]
        public int SelectedAccount { get; set; }

        public int SelectedPayee { get; set; }

        [Required]
        [Display (Name = "Payee")]
        public Dictionary<int, string> Payees = new Dictionary<int, string> ();

        public void SetPayeeDictionary (List<Payee> pList) {
            foreach (var payee in pList) {
                Payees.Add (payee.PayeeID, payee.PayeeName);
            }
        }
    }
}