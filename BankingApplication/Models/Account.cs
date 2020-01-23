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
        public const decimal WithdrawServiceCharge = 0.1M;
        public const decimal TransferServiceCharge = 0.2M;

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

            this.Transactions.Add(transaction);
        }

        public void Withdraw(decimal amount)
        {
            Balance = Balance - amount;
            
            var filteredList =  Transactions.Where(t => t.TransactionType != Transaction.ServiceChargeTransaction);
            
            if(filteredList.Count() >= 5)
            {
                Balance -= WithdrawServiceCharge;
                GenerateTransaction(Transaction.ServiceChargeTransaction,WithdrawServiceCharge);
            }
            
            GenerateTransaction(Transaction.WithdrawTransaction,amount);
        }

        public void Deposit(decimal amount)
        {
            Balance = Balance + amount;
            
            GenerateTransaction(Transaction.DepositTransaction,amount);
        }

        public void Transfer(decimal amount,Account receiverAccount,string comment = null)
        {
            Balance = Balance - amount;
            
            var filteredList =  Transactions.Where(t => t.TransactionType != Transaction.ServiceChargeTransaction);
            
            if(filteredList.Count() >= 5)
            {
                Balance -= TransferServiceCharge;
                GenerateTransaction(Transaction.ServiceChargeTransaction,TransferServiceCharge);
            }
            
            GenerateTransaction(Transaction.TransferTransaction,amount,receiverAccount.AccountNumber,comment);

            receiverAccount.Balance = receiverAccount.Balance + amount;
            GenerateTransaction(Transaction.TransferTransaction,amount,0,comment);
        }
    }
}
