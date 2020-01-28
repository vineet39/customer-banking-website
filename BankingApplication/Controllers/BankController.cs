using BankingApplication.Data;
using BankingApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System;
using RepositoryWrapper;

namespace BankingApplication.Controllers
{
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

        [HttpPost]
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

        public async Task<RedirectToActionResult> Deposit(int accountNumber,decimal amount){
            
            var account = await ReturnAccountData(accountNumber);
            account.Deposit(amount);
            
            ModelState.AddModelError("TransactionSuccess", "Transaction Successful.");

            await repo.SaveChanges();

            return RedirectToAction ("Index", "Bank");               

        }

        public async Task Transfer(int accountNumber,int destinationAccountNumber, decimal amount,string comment)
        {   
            if(accountNumber == destinationAccountNumber )
            {
                ModelState.AddModelError("TransactionFailed", "Sender and receiver account number can't be same.Transaction Failed.");
                return;
            }

            var senderAccount = await ReturnAccountData(accountNumber);
            var receiverAccount = await ReturnAccountData(destinationAccountNumber);
            
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
