using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApplication.Models
{
    public class Account
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(4)]
        public int AccountNumber { get; set; }

        [Required]
        public char AccountType { get; set; }

        [Required]
        [StringLength(4)]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Balance { get; set; }

        [Required]
        [StringLength(8)]
        public DateTime ModifyDate { get; set; }


        public List<Transaction> Transactions { get; set; } 
    }
}
