using BankingApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class TransactionRepository : Repository<Transaction>, IRepository<Transaction>
    {
        public TransactionRepository(DbContext context) : base(context)
        { 
        }
    }
}
