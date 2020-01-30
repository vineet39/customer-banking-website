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
using BankingApplication.Attributes;

namespace BankingApplication.Controllers
{ 
    [AuthorizeCustomer]
    public class BillPayController : Controller
    {
        private readonly Wrapper repo;
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
        public BillPayController(BankAppContext context) {
            repo = new Wrapper(context);
        }
        public async Task<IActionResult> CreateBill(int billID = 0)
        {
            HttpContext.Session.SetInt32("Mod", 0);
            ViewData["Mod"] = 0;
            var customer = await repo.Customer.GetByID(x => x.CustomerID == CustomerID).Include(x => x.Accounts).FirstOrDefaultAsync();
            var billviewmodel = new BillViewModel { Customer = customer };
            if (billID != 0)
            {
                var bill = await repo.BillPay.GetByID(x => x.BillPayID == billID).FirstOrDefaultAsync();
                billviewmodel.Billpay = bill;
                HttpContext.Session.SetInt32("Mod", billID);
                ViewData["Mod"] = billID;
            }
            var list = await repo.Payee.GetAll().ToListAsync();
            billviewmodel.SetPayeeDictionary(list);
            return View(billviewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBill(BillViewModel billv)
        {
            var customer = await repo.Customer.GetByID(x => x.CustomerID == CustomerID).Include(x => x.Accounts).FirstOrDefaultAsync();
            var list = await repo.Payee.GetAll().ToListAsync();
            billv.Customer = customer;
            billv.SetPayeeDictionary(list);
            if (!ModelState.IsValid)
            {
                return View(billv);
            }
            var mod = HttpContext.Session.GetInt32("Mod");
            billv.Billpay.FKAccountNumber = await repo.Account
                .GetByID(x => x.AccountNumber == billv.SelectedAccount).FirstOrDefaultAsync();
            billv.Billpay.FKPayeeID = await repo.Payee.GetByID(x => x.PayeeID == billv.SelectedPayee).FirstOrDefaultAsync();
            if (mod != 0)
            {
                var bill = await repo.BillPay.GetByID(x => x.BillPayID == mod).FirstOrDefaultAsync();
                bill.UpdateBill(billv.Billpay);
                repo.BillPay.Update(bill);
            }
            else { repo.BillPay.Update(billv.Billpay);}
            await repo.SaveChanges();
            ModelState.AddModelError("BillCreatedSuccess", "Bill has been saved.");
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

        public async Task<IActionResult> DeleteBill(int id)
        {
            var bill = await repo.BillPay.GetByID(x => x.BillPayID == id).FirstOrDefaultAsync();
            repo.BillPay.Delete(bill);
            await repo.SaveChanges();
            return RedirectToAction("CreateBill", "BillPay");
        }
    }
}