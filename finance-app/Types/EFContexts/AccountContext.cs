using Microsoft.Extensions.Configuration;
using System.Data.Entity;
using finance_app.Types.EFModels;

namespace finance_app.Types.EFContexts
{
    public class AccountContext : EFContext
    {
        public DbSet<Account> Accounts { get; set; }

        public AccountContext(IConfiguration configuration) : base(configuration) {}

    }
}
