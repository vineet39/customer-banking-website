using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApplication.Data;
using BankingApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication.Controllers
{
    public class BillPayController : Controller
    {
        private readonly BankAppContext _context;
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
        public BillPayController(BankAppContext context) => _context = context;
        public async Task<IActionResult> CreateBill()
        {
            var list = _context.Payee.ToList();
            Console.WriteLine(list[0].PayeeName);
            var customer = await _context.Customer.Include(x => x.Accounts).
                FirstOrDefaultAsync(x => x.CustomerID == CustomerID);
            var billviewmodel = new BillViewModel { Customer = customer};
            billviewmodel.SetPayeeDictionary(list);
            return View(billviewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBill(BillViewModel bill)
        {
            bill.Billpay.FKAccountNumber = await _context.Account
                .FirstOrDefaultAsync(x => x.AccountNumber == bill.SelectedAccount);
            bill.Billpay.FKPayeeID = await _context.Payee.FirstOrDefaultAsync(x => x.PayeeID == bill.SelectedPayee);
            _context.BillPay.Update(bill.Billpay);
            await _context.SaveChangesAsync();
            var list = _context.Payee.ToList();
            var customer = await _context.Customer.Include(x => x.Accounts).
                FirstOrDefaultAsync(x => x.CustomerID == CustomerID);
            var billviewmodel = new BillViewModel { Customer = customer };
            billviewmodel.SetPayeeDictionary(list);

            return View(billviewmodel);
        }

        public IActionResult BillSchedule()
        {
            return View();
        }
    }
}