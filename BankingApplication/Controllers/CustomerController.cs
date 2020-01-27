using System.Threading.Tasks;
using BankingApplication.Data;
using BankingApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using RepositoryWrapper;

namespace BankingApplication.Controllers
{
    public class CustomerController : Controller
    {
        private readonly BankAppContext _context;
        private readonly Wrapper repo;
        public CustomerController(BankAppContext context)
        {
            repo = new Wrapper(context);
        }
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        private async Task<Customer> GetCustomerData() 
        {
            var customer = await repo.Customer.GetByID(x => x.CustomerID == CustomerID).Include(x => x.Accounts).
                FirstOrDefaultAsync();

            return customer;
        }
        public async Task<IActionResult> EditProfile() {
            
           var customer =  await GetCustomerData();

            return View(customer);
        } 
        
        [HttpPost]
        public async Task<IActionResult> SaveChanges(int customerid,string customerName,string TFN,string address,string city,string postcode,string state,string phone){

            var customer = await GetCustomerData();
            
            customer.CustomerName = customerName;
            customer.TFN = TFN;
            customer.Address = address;
            customer.City = city;
            customer.State = state;
            customer.PostCode = postcode;
            customer.Phone = phone;
            
            await repo.SaveChanges();
    
            return RedirectToAction(nameof(EditProfile));
            
        }

    }
}