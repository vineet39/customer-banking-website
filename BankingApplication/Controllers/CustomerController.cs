using System.Threading.Tasks;
using BankingApplication.Data;
using BankingApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication.Controllers
{
    public class CustomerController : Controller
    {
        private readonly BankAppContext _context;
        public CustomerController(BankAppContext context) => _context = context;
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
        public async Task<IActionResult> EditProfile(int customerid) {
            
           var customer =  await _context.Customer.FirstOrDefaultAsync(x => x.CustomerID == CustomerID);

            return View(customer);
        } 
        
        [HttpPost]
        public async Task<IActionResult> SaveChanges(int customerid,string customerName,string TFN,string address,string city,string postcode,string state,string phone){

            var customer = await _context.Customer.FindAsync(customerid);
            
            customer.CustomerName = customerName;
            customer.TFN = TFN;
            customer.Address = address;
            customer.City = city;
            customer.State = state;
            customer.PostCode = postcode;
            customer.Phone = phone;
            
            await _context.SaveChangesAsync();
    
            return RedirectToAction(nameof(EditProfile));
            
        }
    }
}