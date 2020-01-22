using BankingApplication.Data;
using BankingApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BankingApplication.Controllers
{
    public class BankController : Controller
    {
        private readonly BankAppContext _context;
        public BankController(BankAppContext context) => _context = context;
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
        public async Task<IActionResult> Index() {
            
           var customer =  await _context.Customer.Include(x => x.Accounts).
                FirstOrDefaultAsync(x => x.CustomerID == CustomerID);

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