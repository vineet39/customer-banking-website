using BankingApplication.Data;
using BankingApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankingApplication.Controllers
{
    public class BankController : Controller
    {
        private readonly BankAppContext _context;
        public BankController(BankAppContext context) => _context = context;
        public ViewResult Index() {
            
            var customer = new Customer(){
                CustomerID = 2100,
                CustomerName = "Shekar",
                Address = "73 Vasey Ave",
                City = "Melbourne",
                TFN = "123456",
                PostCode = "3075",
                Phone = "61414092713",
                State = "Victoria"
            };

            return View(customer);
        } 
    }
}