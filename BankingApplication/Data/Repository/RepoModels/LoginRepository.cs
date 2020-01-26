using BankingApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class LoginRepository : Repository<Login>, IRepository<Login>
    {
        public LoginRepository(DbContext context) : base(context)
        {

        }
    }
}
