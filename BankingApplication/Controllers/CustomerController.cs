using System.Threading.Tasks;
using BankingApplication.Data;
using BankingApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BankingApplication.Controllers
{
    public class CustomerController : Controller
    {
        private readonly BankAppContext _context;
        public CustomerController(BankAppContext context) => _context = context;
        public ViewResult EditProfile(int customerid) {
            
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
        
        [HttpPost]
        public async Task<IActionResult> SaveChanges(int customerid,string name,string TFN,string address,string city,string postcode,string state,string phone){

            var customer = await _context.Customer.FindAsync(customerid);
            customer.CustomerName = name;
            await _context.SaveChangesAsync();
            Console.WriteLine("Changesss");
            return RedirectToAction(nameof(EditProfile));
            
        }
    }
}