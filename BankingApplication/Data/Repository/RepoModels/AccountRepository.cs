using BankingApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class AccountRepository : Repository<Account>, IRepository<Account>
    {
        public AccountRepository(DbContext context) : base(context)
        {
        }
    }
}
