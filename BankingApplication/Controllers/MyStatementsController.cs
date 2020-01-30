using System.Collections.Generic;
using System.Threading.Tasks;
using BankingApplication.Attributes;
using BankingApplication.Data;
using BankingApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using RepositoryWrapper;

namespace BankingApplication.Controllers {
    [AuthorizeCustomer]
    public class MyStatementsController : Controller {
        private readonly Wrapper repo;
        public MyStatementsController (BankAppContext context) {
            repo = new Wrapper (context);
        }
        private int CustomerID => HttpContext.Session.GetInt32 (nameof (Customer.CustomerID)).Value;
        public async Task<IActionResult> SelectAccount () {
            
            var accounts = await repo.Account.GetByID (x => x.CustomerID == CustomerID).ToListAsync ();

            return View (accounts);
        }

        public IActionResult IndexToMyDetails (int accountNumber) {
            HttpContext.Session.SetInt32 ("selectedAccountNumber", accountNumber);
            return RedirectToAction (nameof (MyDetails));
        }

        // Paging code referenced from web dev tutorials.
        public async Task<IActionResult> MyDetails (int page = 1) {
            // Number of transactions to be shown at a time.
            const int pageSize = 4;
            
            var selectedAccountNumber = HttpContext.Session.GetInt32 ("selectedAccountNumber");
            
            // List of transactions to be paged.
            var pagedList = await repo.Transaction.GetByID (x => x.AccountNumber == selectedAccountNumber).ToPagedListAsync (page, pageSize);

            return View (pagedList);
        }

        // Returning a partial view showing balance and button to view transctions once account
        // is selected on select account page.
        public async Task<IActionResult> SeeMyBalance (int id) {
            var account = await repo.Account.GetByID (x => x.AccountNumber == id).FirstOrDefaultAsync ();
            return PartialView (account);
        }

    }

}