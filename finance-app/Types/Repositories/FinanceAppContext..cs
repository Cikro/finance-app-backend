using finance_app.Types.Repositories.Accounts;
using finance_app.Types.Repositories.JournalEntries;
using finance_app.Types.Repositories.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace finance_app.Types.Repositories
{
    public class FinanceAppContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<JournalEntry> JournalEntries { get; set; }
        

        public FinanceAppContext(DbContextOptions options) : base(options){}

        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            if (!options.IsConfigured){                
                options.UseMySql(_configuration.GetConnectionString("MainDB"));
            }
        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Account
            modelBuilder.Entity<Account>().ToTable("accounts");
            modelBuilder.Entity<Account>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<JournalEntry>()
                .Property(e => e.DateCreated)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<JournalEntry>()
                .Property(e => e.DateLastEdited)
                .ValueGeneratedOnUpdate();
            #endregion Account

            #region JournalEntry
            modelBuilder.Entity<JournalEntry>().ToTable("journal_entries");
            modelBuilder.Entity<JournalEntry>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<JournalEntry>()
                .Property(e => e.DateCreated)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<JournalEntry>()
                .Property(e => e.DateLastEdited)
                .ValueGeneratedOnUpdate();
            #endregion JournalEntry

            #region Transactions
            modelBuilder.Entity<Transaction>().ToTable("transactions")
                .HasOne(p => p.JournalEntry)
                .WithMany(j => j.Transactions)
                .HasForeignKey(t => t.JournalEntryId);
            modelBuilder.Entity<Transaction>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Transaction>()
                .Property(e => e.DateCreated)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Transaction>()
                .Property(e => e.DateLastEdited)
                .ValueGeneratedOnUpdate();
            #endregion Transactions
        }
        
    }
}
