using BankingApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Repository;

namespace RepositoryWrapper
{
    public class AccountRepository : Repository<Account>, IRepository<Account>
    {
        public AccountRepository(DbContext context) : base(context)
        {
        }
    }
}
