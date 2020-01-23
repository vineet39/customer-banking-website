using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApplication.Data;
using BankingApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication.Controllers
{
    public class BillController : Controller
    {
        private readonly BankAppContext _context;
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
        public BillController(BankAppContext context) => _context = context;
        public async Task<IActionResult> CreateBill()
        {
            var customer = await _context.Customer.Include(x => x.Accounts).
                FirstOrDefaultAsync(x => x.CustomerID == CustomerID);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BillPost()
        {
            return View();
        }

        public IActionResult BillSchedule()
        {
            return View();
        }
    }
}