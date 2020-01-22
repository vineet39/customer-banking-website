using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApplication.Data;
using BankingApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleHashing;

namespace BankingApplication.Controllers
{
    public class LoginController : Controller
    {
        private readonly BankAppContext context;
        public LoginController(BankAppContext Context)
        {
            context = Context;
        }
        public IActionResult Login()
        {
            return View();
        }

        //Login Form post logic referencing Web Dev Tutorial 7

        [HttpPost]
        public IActionResult Login(string userID, string password)
        {
            //LINQ query for eager loading login
            var login = context.Login.Where(a => a.UserID == userID).Include(a => a.Customer).Single();
            if (login == null || !PBKDF2.Verify(login.Password, password))
            {
                ModelState.AddModelError("LoginFailed", "Login failed, please try again.");
                return View(new LoginViewModel { UserID = userID });
            }

            

            // Set customer session variables
            HttpContext.Session.SetInt32(nameof(Customer.CustomerID), login.CustomerID);
            HttpContext.Session.SetString(nameof(Customer.CustomerName), login.Customer.CustomerName);

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