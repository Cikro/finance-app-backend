using finance_app.Types.Repositories.Transaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace finance_app.Types.Repositories
{
    public class FinanceAppContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DbSet<Account.Account> Accounts { get; set; }
        public DbSet<Transaction.Transaction> Transactions { get; set; }
        public DbSet<JournalEntry.JournalEntry> JournalEntries { get; set; }
        

        public FinanceAppContext(DbContextOptions options) : base(options){}
        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            if (!options.IsConfigured){                
                options.UseMySql(_configuration.GetConnectionString("MainDB"));
            }
        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JournalEntry.JournalEntry>().ToTable("journal_entries");
            modelBuilder.Entity<Transaction.Transaction>().ToTable("transactions")
                .HasOne(p => p.JournalEntry)
                .WithMany()
                .HasForeignKey(t => t.JournalEntryId);
        }
    }
}
