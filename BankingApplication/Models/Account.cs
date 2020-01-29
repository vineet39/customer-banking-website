using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BankingApplication.Models {
    public class Account {
        public const decimal WithdrawServiceCharge = 0.1M;
        public const decimal TransferServiceCharge = 0.2M;
        private const decimal minBalSavings = 0;
        private const decimal minBalCheckings = 200;

        [Key, DatabaseGenerated (DatabaseGeneratedOption.None)]
        [StringLength (4)]
        public int AccountNumber { get; set; }

        [Required]
        public char AccountType { get; set; }

        [Required]
        [StringLength (4)]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        [Column (TypeName = "money")]
        [DataType (DataType.Currency)]
        [Required]
        public decimal Balance { get; set; }

        [Required]
        [StringLength (8)]
        public DateTime ModifyDate { get; set; }
        public List<Transaction> Transactions { get; set; }

        public List<BillPay> Bills { get; set; }

        public void GenerateTransaction (char type, decimal amount, int destinationAccountNumber = 0, string comment = null) {
            Transaction transaction = new Transaction () {
            AccountNumber = this.AccountNumber,
            TransactionType = type,
            Amount = amount,
            ModifyDate = DateTime.UtcNow,
            Comment = comment
            };

            if (destinationAccountNumber == 0) {
                transaction.DestinationAccountNumber = null;
            } else {
                transaction.DestinationAccountNumber = destinationAccountNumber;
            }

            this.Transactions.Add (transaction);
        }

        public bool checkIfFundsSufficient (decimal amount, decimal transactionCharge) {
            var filteredList = Transactions.Where (t => t.TransactionType != Transaction.ServiceChargeTransaction);

            if (filteredList.Count () >= 5) {
                if ((Balance - amount - transactionCharge < minBalSavings && AccountType == 'S') ||
                    (Balance - amount - transactionCharge < minBalCheckings && AccountType == 'C')) {
                    return false;
                }
            } else {
                if ((Balance - amount < minBalSavings && AccountType == 'S') ||
                    (Balance - amount < minBalCheckings && AccountType == 'C')) {
                    return false;
                }
            }

            return true;
        }

        public bool Withdraw (decimal amount) {
            bool haveSufficientFunds = checkIfFundsSufficient (amount, WithdrawServiceCharge);

            if (!haveSufficientFunds) {
                return false;
            }

            var filteredList = Transactions.Where (t => t.TransactionType != Transaction.ServiceChargeTransaction);

            Balance = Balance - amount;

            if (filteredList.Count () >= 5) {
                Balance -= WithdrawServiceCharge;
                GenerateTransaction (Transaction.ServiceChargeTransaction, WithdrawServiceCharge);
            }

            GenerateTransaction (Transaction.WithdrawTransaction, amount);

            return true;
        }

        public void Deposit (decimal amount) {
            Balance = Balance + amount;

            GenerateTransaction (Transaction.DepositTransaction, amount);
        }

        public bool Transfer (decimal amount, Account receiverAccount, string comment = null) {
            bool haveSufficientFunds = checkIfFundsSufficient (amount, TransferServiceCharge);

            if (!haveSufficientFunds) {
                return false;
            }

            var filteredList = Transactions.Where (t => t.TransactionType != Transaction.ServiceChargeTransaction);

            Balance = Balance - amount;

            if (filteredList.Count () >= 5) {
                Balance -= TransferServiceCharge;
                GenerateTransaction (Transaction.ServiceChargeTransaction, TransferServiceCharge);
            }

            GenerateTransaction (Transaction.TransferTransaction, amount, receiverAccount.AccountNumber, comment);

            receiverAccount.Balance = receiverAccount.Balance + amount;
            receiverAccount.GenerateTransaction (Transaction.TransferTransaction, amount, 0, comment);

            return true;
        }

        public bool PayBill(BillPay bill)
        {
            if(!checkIfFundsSufficient(bill.Amount, 0))
            {
                return false;
            }

            Balance -= bill.Amount;
            GenerateTransaction(Transaction.BillPayTransaction, bill.Amount, 0);
            return true;

        }
    }
}