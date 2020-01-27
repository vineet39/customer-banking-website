using BankingApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Repository;

namespace RepositoryWrapper
{
    public class PayeeRepository : Repository<Payee>, IRepository<Payee>
    {
        public PayeeRepository(DbContext context) : base(context)
        {

        }
    }
}
