using BankingApplication.Data;
using BankingApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using RepositoryWrapper;
using BankingApplication.Attributes;


namespace BankingApplication.Controllers
{   
    [AuthorizeCustomer]
    public class BankController : Controller
    {
        //private readonly BankAppContext _context;
        private readonly Wrapper repo;
        public BankController(BankAppContext context) 
        {
            repo = new Wrapper(context);
        }
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
        private Customer customer;

        // Method to go to atm page.
        public async Task<IActionResult> Index() 
        {
            customer =  await repo.Customer.GetByID(x => x.CustomerID == CustomerID).Include(x => x.Accounts).
                FirstOrDefaultAsync();

            IndexViewModel indexViewModel = new IndexViewModel();
            indexViewModel.Customer = customer;

            return View(indexViewModel);
        } 

        private async Task<Account> ReturnAccountData(int accountNumber) 
        {
            var account = await repo.Account.GetByID(x => x.AccountNumber == accountNumber).Include(x => x.Transactions).
                FirstOrDefaultAsync();

            return account;
        }

        // Redirecting to type of transaction based on user input,withdraw if 'W', deposit if 'D', tranfer if 'T'.
        public async Task<IActionResult> PerformTransaction(string transactionType,string accountNumber,string destinationAccountNumber,string amount,string comment = null){
            
             switch (transactionType) 
            {
                case "W":
                    await WithDraw(int.Parse(accountNumber),decimal.Parse(amount));
                    break;
                case "D":
                    await Deposit(int.Parse(accountNumber),decimal.Parse(amount));
                    break;
                case "T":
                    await Transfer(int.Parse(accountNumber),int.Parse(destinationAccountNumber),decimal.Parse(amount),comment);
                    Console.WriteLine("Test");
                    break;
            }
            
            customer =  await repo.Customer.GetByID(x => x.CustomerID == CustomerID).Include(x => x.Accounts).
                FirstOrDefaultAsync();
            return View("Index",new IndexViewModel { Customer = customer });

        }

        // Performing withdraw operation.
        public async Task WithDraw(int accountNumber,decimal amount)
        {
            var account = await ReturnAccountData(accountNumber);

            bool transactionSuccessful = account.Withdraw(amount);
            
            if(!transactionSuccessful)
            {
                ModelState.AddModelError("TransactionFailed", "Insufficient balance.Transaction Failed");
                return;
            }
            
            ModelState.AddModelError("TransactionSuccess", "Transaction Successful.");

            await repo.SaveChanges();
                    
        }

        // Performing deposit operation.
        public async Task<RedirectToActionResult> Deposit(int accountNumber,decimal amount){
            
            var account = await ReturnAccountData(accountNumber);
            account.Deposit(amount);
            
            ModelState.AddModelError("TransactionSuccess", "Transaction Successful.");

            await repo.SaveChanges();

            return RedirectToAction ("Index", "Bank");               

        }

        // Performing transfer operation.
        public async Task Transfer(int accountNumber,int destinationAccountNumber, decimal amount,string comment)
        {   
            // Returning from method if sender and receiver account number entered are the same.
            if(accountNumber == destinationAccountNumber )
            {
                ModelState.AddModelError("TransactionFailed", "Sender and receiver account number can't be same.Transaction Failed.");
                return;
            }

            var senderAccount = await ReturnAccountData(accountNumber);
            var receiverAccount = await ReturnAccountData(destinationAccountNumber);
            
            // Returning from method if incorrect receiver account number is entered.
            if(receiverAccount == null)
            {
                ModelState.AddModelError("TransactionFailed", "Invalid receiver account number.Transaction Failed.");
                return;
            }

            bool transactionSuccessful = senderAccount.Transfer(amount,receiverAccount,comment);
            
            if(!transactionSuccessful)
            {
                ModelState.AddModelError("TransactionFailed", "Insufficient balance.Transaction Failed");
                return;
            }
            
            ModelState.AddModelError("TransactionSuccess", "Transaction Successful.");

            await repo.SaveChanges();
            
        }

    }
}
