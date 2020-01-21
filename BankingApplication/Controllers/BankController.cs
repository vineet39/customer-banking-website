using BankingApplication.Data;
using BankingApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BankingApplication.Controllers
{
    public class BankController : Controller
    {
        private readonly BankAppContext _context;
        public BankController(BankAppContext context) => _context = context;
        public async Task<IActionResult> Index() {
            
           var customer =  await _context.Customer.Include(x => x.Accounts).
                FirstOrDefaultAsync(x => x.CustomerID == 2100);

            return View(customer);
        } 

        [HttpPost]
        public void PerformTransaction(string transactionType,string accountNumber,string destinationAccountNumber,string amount,string comment){
            
            switch (transactionType) 
            {
                case "W":
                    WithDraw(int.Parse(accountNumber),decimal.Parse(amount));
                    break;
                case "T":
                    Deposit(int.Parse(accountNumber),decimal.Parse(amount));
                    break;
                case "D":
                    Transfer(int.Parse(accountNumber),int.Parse(destinationAccountNumber),decimal.Parse(amount),comment);
                    break;
            }

        }

        public void WithDraw(int accountNumber,decimal amount){

        }

        public void Deposit(int accountNumber,decimal amount){

        }

        public void Transfer(int accountNumber,int destinationAccountNumber, decimal amount,string comment){

        }

    }
}