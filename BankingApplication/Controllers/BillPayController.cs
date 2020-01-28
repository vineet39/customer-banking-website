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
using RepositoryWrapper;

namespace BankingApplication.Controllers
{
    public class BillPayController : Controller
    {
        private readonly Wrapper repo;
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
        public BillPayController(BankAppContext context) {
            repo = new Wrapper(context);
        }
        public async Task<IActionResult> CreateBill(int ?billID = null)
        {
            var customer = await repo.Customer.GetByID(x => x.CustomerID == CustomerID).Include(x => x.Accounts).FirstOrDefaultAsync();
            var billviewmodel = new BillViewModel { Customer = customer };
            if (billID.HasValue)
            {
                var bill = await repo.BillPay.GetByID(x => x.BillPayID == billID).FirstOrDefaultAsync();
                billviewmodel.Billpay = bill;
            }
            var list = await repo.Payee.GetAll().ToListAsync();
            billviewmodel.SetPayeeDictionary(list);
            return View(billviewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBill(BillViewModel bill)
        {
            bill.Billpay.FKAccountNumber = await repo.Account
                .GetByID(x => x.AccountNumber == bill.SelectedAccount).FirstOrDefaultAsync();
            bill.Billpay.FKPayeeID = await repo.Payee.GetByID(x => x.PayeeID == bill.SelectedPayee).FirstOrDefaultAsync();
            repo.BillPay.Update(bill.Billpay);
            await repo.SaveChanges();
            var list = await repo.Payee.GetAll().ToListAsync();
            var customer = await repo.Customer.GetByID(x => x.CustomerID == CustomerID).Include(x => x.Accounts).FirstOrDefaultAsync();
            var billviewmodel = new BillViewModel { Customer = customer };
            billviewmodel.SetPayeeDictionary(list);
            return View(billviewmodel);
        }

        public async Task<IActionResult> SelectAccount()
        {
            var accounts = await repo.Account.GetByID(x => x.CustomerID == CustomerID).ToListAsync();

            return View(accounts);
        }

        public async Task<IActionResult> BillSchedule(int accountNumber)
        {
            var account = await repo.Account.GetByID(x => x.AccountNumber == accountNumber).Include(x => x.Bills).FirstOrDefaultAsync();
            var list = await repo.Payee.GetAll().ToListAsync();
            BillScheduleViewModel bills = new BillScheduleViewModel(account, list);
            return View(bills);
        }

        public async Task<IActionResult> SeeMyBalance(int id)
        {
            
            var account = await repo.Account.GetByID(x => x.AccountNumber == id).FirstOrDefaultAsync();
            return PartialView(account);
            
        }
    }
}