using System;
using System.Collections.Generic;
using System.Text;
using BankingApplication.Models;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace RepositoryWrapper
{
    public class CustomerRepository : Repository<Customer>, IRepository<Customer>
    {
        public CustomerRepository(DbContext context) : base(context)
        {

        }
    }
}
