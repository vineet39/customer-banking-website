using BankingApplication.Data;
using BankingApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System;

namespace BankingApplication.Controllers
{
    public class BankController : Controller
    {
        private readonly BankAppContext _context;
        public BankController(BankAppContext context) => _context = context;
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
        private Customer customer;
        public async Task<IActionResult> Index() 
        {
            customer =  await _context.Customer.Include(x => x.Accounts).
                FirstOrDefaultAsync(x => x.CustomerID == CustomerID);

            IndexViewModel indexViewModel = new IndexViewModel();
            indexViewModel.Customer = customer;

            return View(indexViewModel);
        } 

        private async Task<Account> ReturnAccountData(int accountNumber) 
        {
            var account = await _context.Account.Include(x => x.Transactions).
                FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);

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
            
            customer =  await _context.Customer.Include(x => x.Accounts).
                FirstOrDefaultAsync(x => x.CustomerID == CustomerID);
            return View("Index",new IndexViewModel { Customer = customer });

        }

        public async Task WithDraw(int accountNumber,decimal amount)
        {
            var account = await ReturnAccountData(accountNumber);

            bool transactionSuccessful = account.Withdraw(amount);
            
            if(!transactionSuccessful)
                ModelState.AddModelError("TransactionFailed", "Insufficient balance.Transaction Failed");
            
            await _context.SaveChangesAsync();
                    
        }

        public async Task Deposit(int accountNumber,decimal amount){
            
            var account = await ReturnAccountData(accountNumber);

            account.Deposit(amount);
            await _context.SaveChangesAsync();
        
        }

        public async Task Transfer(int accountNumber,int destinationAccountNumber, decimal amount,string comment)
        {   
            var senderAccount = await ReturnAccountData(accountNumber);
            var receiverAccount = await ReturnAccountData(destinationAccountNumber);

            bool transactionSuccessful = senderAccount.Transfer(amount,receiverAccount,comment);
            
            if(!transactionSuccessful)
                ModelState.AddModelError("TransactionFailed", "Insufficient balance.Transaction Failed");

            senderAccount.Transfer(amount,receiverAccount,comment);

            await _context.SaveChangesAsync();
            
        }

    }
}