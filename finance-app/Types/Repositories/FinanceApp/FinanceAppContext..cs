using finance_app.Types.Repositories.Accounts;
using finance_app.Types.Repositories.ApplicationAccounts;
using finance_app.Types.Repositories.JournalEntries;
using finance_app.Types.Repositories.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace finance_app.Types.Repositories.FinanceApp {
    public class AuthenticationContext : DbContext {
        private readonly IConfiguration _configuration;

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<JournalEntry> JournalEntries { get; set; }
        public DbSet<ApplicationAccount> ApplicationAccounts { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }


        public AuthenticationContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            if (!options.IsConfigured) {
                options.UseMySql(_configuration.GetConnectionString("MainDB"));
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            new AccountEntityConfig().Configure(modelBuilder.Entity<Account>());
            new JournalEntryEntityConfig().Configure(modelBuilder.Entity<JournalEntry>());
            new TransactionEntityConfig().Configure(modelBuilder.Entity<Transaction>());
            new ApplicationAccountEntityConfig().Configure(modelBuilder.Entity<ApplicationAccount>());
            new ApplicationUserEntityConfig().Configure(modelBuilder.Entity<ApplicationUser>());
        }

    }
}
