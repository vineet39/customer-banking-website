using System;
using System.Collections.Generic;
using System.Text;
using BankingApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class CustomerRepository : Repository<Customer>, IRepository<Customer>
    {
        public CustomerRepository(DbContext context) : base(context)
        {

        }
    }
}
