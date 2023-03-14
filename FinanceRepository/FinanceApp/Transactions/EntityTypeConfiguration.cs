
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace finance_app.Types.Repositories.Transactions {

    public class TransactionEntityConfig : DatabaseObjectEntityConfig<Transaction>
    {
        public TransactionEntityConfig(): base("transactions"){ }

        new public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            base.Configure(builder);
            builder.HasOne(p => p.JournalEntry)
                .WithMany(j => j.Transactions)
                .HasForeignKey(t => t.JournalEntryId);
            
        }
    }  
}