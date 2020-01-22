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
        public const decimal withdrawServiceCharge = 0.1M;
        public const decimal transferServiceCharge = 0.2M;

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

         public void GenerateTransaction(char type, decimal amount, int destinationAccountNumber = 0, string comment = null)
         {
            Transaction transaction = new Transaction(){
            AccountNumber = this.AccountNumber,
            TransactionType = type,
            Amount = amount,
            ModifyDate = DateTime.UtcNow,
            Comment = comment
            };

            if(destinationAccountNumber == 0)
            {
                transaction.DestinationAccountNumber = null;
            }
            else
            {
                transaction.DestinationAccountNumber = destinationAccountNumber;
            }
        }

        public void Withdraw(decimal amount)
        {
            Balance = Balance - amount;
            
            var filteredList =  Transactions.Where(t => t.TransactionType != Transaction.ServiceChargeTransaction);
            
            if(filteredList.Count() >= 5)
            {
                Balance -= withdrawServiceCharge;
                GenerateTransaction(Transaction.TransferTransaction,transferServiceCharge);
            }
            
            GenerateTransaction(Transaction.WithdrawTransaction,amount);
        }

        public void Deposit(decimal amount)
        {
            Balance = Balance + amount;
            
            GenerateTransaction(Transaction.DepositTransaction,amount);
        }
    }
}
