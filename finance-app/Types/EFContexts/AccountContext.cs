using Microsoft.Extensions.Configuration;
using finance_app.Types.EFModels;
using Microsoft.EntityFrameworkCore;

namespace finance_app.Types.EFContexts
{
    public class AccountContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public AccountContext(DbContextOptions<AccountContext> options) : base(options) {}

    }
}
