using System.Threading.Tasks;
using BankingApplication.Data;
using BankingApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using X.PagedList;
using System.Collections.Generic;
using System;
using RepositoryWrapper;
using BankingApplication.Attributes;

namespace BankingApplication.Controllers
{   
    [AuthorizeCustomer]
    public class MyStatementsController : Controller
    {
        //private readonly BankAppContext _context;
        private readonly Wrapper repo;
        public MyStatementsController(BankAppContext context)
        {
            repo = new Wrapper(context);
        }
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
        private List<Transaction> transactions;
        public async Task<IActionResult> SelectAccount() 
        {
            var accounts = await repo.Account.GetByID(x => x.CustomerID == CustomerID).ToListAsync();

            return View(accounts);
        } 
        
        
         public async Task<IActionResult> IndexToMyDetails(int accountNumber) 
        {
            // const int pageSize = 3;
            // var transactions = await _context.Transaction.Where(x => x.AccountNumber == accountNumber).ToPagedListAsync(page, pageSize);

            // return PartialView(transactions);

            transactions = await repo.Transaction.GetByID(x => x.AccountNumber == accountNumber).ToListAsync();

             HttpContext.Session.SetInt32("selectedAccountNumber", accountNumber);

            return RedirectToAction(nameof(MyDetails));
        } 

        public async Task<IActionResult> MyDetails(int page = 1)
        {
            const int pageSize = 4;
            var selectedAccountNumber = HttpContext.Session.GetInt32("selectedAccountNumber");
            Console.WriteLine(selectedAccountNumber);
            var pagedList = await repo.Transaction.GetByID(x => x.AccountNumber == selectedAccountNumber).ToPagedListAsync(page, pageSize);

            return View(pagedList);
        }

        public async Task<IActionResult> SeeMyBalance(int id) 
        {
            var account = await repo.Account.GetByID(x => x.AccountNumber == id).FirstOrDefaultAsync();
            return PartialView(account);
        } 



        
    }

    

}