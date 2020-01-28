using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApplication.Models {
    public class BillPay {
        public int BillPayID { get; set; }

        [Required]
        [ForeignKey ("FKAccountNumber")]
        public int AccountNumber { get; set; }
        public Account FKAccountNumber { get; set; }

        [Required]
        [ForeignKey ("FKPayeeID")]
        public int PayeeID { get; set; }
        public Payee FKPayeeID { get; set; }

        [Required]
        [Column (TypeName = "money")]
        [DataType (DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        [DataType (DataType.DateTime)]
        public DateTime ScheduleDate { get; set; }

        public enum Periods {
            [Display (Name = "Once Off")]
            OnceOff = 'S',
            Monthly = 'M',
            Quarterly = 'Q',
            Annually = 'Y'
        }

        [Required]
        public Periods Period { get; set; }

        [Required]
        public DateTime ModifyDate { get; set; }

    }
}