using BankingApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class BillPayRepository : Repository<BillPay>, IRepository<BillPay>
    {
        public BillPayRepository(DbContext context) : base(context)
        {

        }
    }
}
