using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApplication.Models;

namespace BankingApplication.Data
{
    public class BankAppContext : DbContext
    {
        public BankAppContext(DbContextOptions<BankAppContext> options) : base(options)
        {
        }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Transaction> Transaction { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Transaction>().
                HasOne(x => x.Account).WithMany(x => x.Transactions).HasForeignKey(x => x.AccountNumber);
        }
    }
}
