using System.Threading.Tasks;
using BankingApplication.Data;
using BankingApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryWrapper;
using SimpleHashing;

namespace BankingApplication.Controllers
{
    public class LoginController : Controller
    {
        //Repository object
        private readonly Wrapper repo;
        public LoginController(BankAppContext context)
        {
            repo = new Wrapper(context);
        }
        public IActionResult Login()
        {
            return View();
        }

        //Login Form post logic referencing Web Dev Tutorial
        [HttpPost]
        public async Task<IActionResult> Login(string userID, string password)
        {
            //LINQ query for eager loading login
            var login = await repo.Login.GetByID(a => a.UserID == userID).Include(a => a.Customer).FirstOrDefaultAsync();
            if (login == null || !PBKDF2.Verify(login.Password, password))
            {
                ModelState.AddModelError("LoginFailed", "Login failed, please try again.");
                return View(new LoginViewModel { UserID = userID });
            }

            // Set customer session variables
            HttpContext.Session.SetInt32(nameof(Customer.CustomerID), login.CustomerID);
            HttpContext.Session.SetString(nameof(Customer.CustomerName), login.Customer.CustomerName);
            HttpContext.Session.SetString(nameof(BankingApplication.Models.Login.UserID), login.UserID);

            //Redirect to customer page
            return RedirectToAction("Index", "Bank");
        }

        public IActionResult Logout()
        {
            //Clear Session variables
            HttpContext.Session.Clear();

            //Return to home page
            return RedirectToAction("Index", "Home");

        }
    }
    
}