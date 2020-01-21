using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApplication.Models
{
    public class BillPay
    {
        [StringLength(4)]
        public int BillPayID { get; set; }

        [Required,StringLength(4)]
         [ForeignKey("FKAccountNumber")]
        public int AccountNumber { get; set; }
        public Account FKAccountNumber { get; set; }

        [Required,StringLength(4)]
        [ForeignKey("FKPayeeID")]
        public int PayeeID { get; set; }
        public Payee FKPayeeID { get; set; }

        [Required,StringLength(8)]
        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required,StringLength(15)]
        public DateTime ScheduleDate { get; set; }

        [Required]
        public char Period { get; set; }

        [Required,StringLength(8)]
        public DateTime ModifyDate { get; set; }
        
    }
}