using System;
using Microsoft.EntityFrameworkCore;

namespace finance_app.EFContexts
{
    public class AccountContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseMySql("Data Source=blogging.db");
    }
}
