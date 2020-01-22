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

            return View(customer);
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
                    Transfer(int.Parse(accountNumber),int.Parse(destinationAccountNumber),decimal.Parse(amount),comment);
                    break;
            }

            return newView;
        }

        public async Task<IActionResult> WithDraw(int accountNumber,decimal amount)
        {
            var account = await _context.Account.Include(x => x.Transactions).
                FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);

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
            await _context.SaveChangesAsync();
            
            return RedirectToAction ("Index", "Bank");           
        }

        public async Task<IActionResult> Deposit(int accountNumber,decimal amount){
            
            var account = await _context.Account.Include(x => x.Transactions).
                FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);

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
            await _context.SaveChangesAsync();

            return RedirectToAction ("Index", "Bank");           
            

        }

        public void Transfer(int accountNumber,int destinationAccountNumber, decimal amount,string comment)
        {

        }

    }
}