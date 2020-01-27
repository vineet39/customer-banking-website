using BankingApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Repository;

namespace RepositoryWrapper
{
    public class LoginRepository : Repository<Login>, IRepository<Login>
    {
        public LoginRepository(DbContext context) : base(context)
        {

        }
    }
}
