using BankingApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Repository;

namespace RepositoryWrapper
{
    public class BillPayRepository : Repository<BillPay>, IRepository<BillPay>
    {
        public BillPayRepository(DbContext context) : base(context)
        {

        }
    }
}
