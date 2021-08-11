using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace finance_app.Types.Repositories.Account
{
    public class AccountContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public AccountContext(DbContextOptions<AccountContext> options) : base(options) {}

    }
}
