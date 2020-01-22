using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApplication.Models
{
    public class Transaction
    {
        public const char WithdrawTransaction= 'W';
        public const char DepositTransaction = 'D';
        public const char TransferTransaction = 'T';
        public const char BillPayTransaction= 'B';

        public const char ServiceChargeTransaction = 'S';

        [StringLength(4)]
        public int TransactionID { get; set; }

        [Required]
        public char TransactionType { get; set; }

        [Required]
        [StringLength(4)]
        public int AccountNumber { get; set; }
        public virtual Account Account { get; set; }

        [StringLength(4)]
        [ForeignKey("DestinationAccount")]
        public int? DestinationAccountNumber { get; set; }
        public virtual Account DestinationAccount { get; set; }

        [StringLength(8)]
        [Required]
        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [StringLength(255)]
        public string Comment { get; set; }

        [StringLength(15)]
        public DateTime ModifyDate { get; set; }
    }
}
