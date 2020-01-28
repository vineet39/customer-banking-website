using System.Threading.Tasks;
using BankingApplication.Data;
using BankingApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RepositoryWrapper;
using SimpleHashing;
using BankingApplication.Attributes;

namespace BankingApplication.Controllers
{   
    
    [AuthorizeCustomer]
    public class CustomerController : Controller
    {
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

        public ViewResult ChangePassword() {

            return View();
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
            
            ModelState.AddModelError("EditSuccess", "Profile edited successfully.");
            await repo.SaveChanges();
    
            return RedirectToAction(nameof(EditProfile));
            
        }

        public async Task<IActionResult> SavePassword(string oldpassword,string newpassword,string confirmnewpassword){
           
            var userID = HttpContext.Session.GetString(nameof(Login.UserID));
            var login = await repo.Login.GetByID(a => a.UserID == userID).FirstOrDefaultAsync();
           
            if (!PBKDF2.Verify(login.Password ,oldpassword))
            {
                ModelState.AddModelError("PasswordChangeFailed", "Old password entered is incorrect.");
                return View("ChangePassword");
            }
            if(oldpassword == newpassword)
            {
                ModelState.AddModelError("PasswordChangeFailed", "Old password and new password cannot be same.");
                return View("ChangePassword");
            }

            if(newpassword != confirmnewpassword)
            {
                ModelState.AddModelError("PasswordChangeFailed", "New password and confirmed new password do not match");
                return View("ChangePassword");
            }
            
            login.Password = PBKDF2.Hash(newpassword);
            ModelState.AddModelError("PasswordChangeSuccess", "Password changed successfully.");
            await repo.SaveChanges();

            return View("ChangePassword"); 

        }

    }
}