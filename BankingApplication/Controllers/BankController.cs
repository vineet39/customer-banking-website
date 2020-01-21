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

            return View();
        } 
    }
}