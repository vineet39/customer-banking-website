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

            return View(customer);
        } 

        private async Task<Account> ReturnAccountData(int accountNumber) 
        {
            var account = await repo.Account.GetByID(x => x.AccountNumber == accountNumber).Include(x => x.Transactions).
                FirstOrDefaultAsync();

            return account;
        }

        [HttpPost]
        public async Task<IActionResult> PerformTransaction(string transactionType,string accountNumber,string destinationAccountNumber,string amount,string comment = null){
            
            IActionResult newView = null;
            
            switch (transactionType) 
            {
                case "W":
                    newView = await WithDraw(int.Parse(accountNumber),decimal.Parse(amount));
                    break;
                case "D":
                    newView = await Deposit(int.Parse(accountNumber),decimal.Parse(amount));
                    break;
                case "T":
                    newView = await Transfer(int.Parse(accountNumber),int.Parse(destinationAccountNumber),decimal.Parse(amount),comment);
                    Console.WriteLine("Test");
                    break;
            }

            return newView;
        }

        public async Task<IActionResult> WithDraw(int accountNumber,decimal amount)
        {
            var account = await ReturnAccountData(accountNumber);

            // if(amount <= 0)
            //     ModelState.AddModelError(nameof(amount), "Amount must be positive.");
            // if(amount.HasMoreThanTwoDecimalPlaces())
            //     ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
            // if(!ModelState.IsValid)
            // {
            //     ViewBag.Amount = amount;
            //     return View(account);
            // }

            account.Withdraw(amount);
            await repo.SaveChanges();
            
            return RedirectToAction ("Index", "Bank");           
        }

        public async Task<IActionResult> Deposit(int accountNumber,decimal amount){
            
            var account = await ReturnAccountData(accountNumber);

            // if(amount <= 0)
            //     ModelState.AddModelError(nameof(amount), "Amount must be positive.");
            // if(amount.HasMoreThanTwoDecimalPlaces())
            //     ModelState.AddModelError(nameof(amount), "Amount cannot have more than 2 decimal places.");
            // if(!ModelState.IsValid)
            // {
            //     ViewBag.Amount = amount;
            //     return View(account);
            // }

            account.Deposit(amount);
            await repo.SaveChanges();

            return RedirectToAction ("Index", "Bank");           
            

        }

        public async Task<IActionResult> Transfer(int accountNumber,int destinationAccountNumber, decimal amount,string comment)
        {   
            var senderAccount = await ReturnAccountData(accountNumber);
            var receiverAccount = await ReturnAccountData(destinationAccountNumber);

            senderAccount.Transfer(amount,receiverAccount,comment);

            await repo.SaveChanges();
            
            return RedirectToAction ("Index", "Bank");
        }

    }
}