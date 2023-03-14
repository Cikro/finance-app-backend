
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace finance_app.Types.Repositories.JournalEntries {

    public class JournalEntryEntityConfig : DatabaseObjectEntityConfig<JournalEntry>
    {
        public JournalEntryEntityConfig(): base("journal_entries"){ }

    }  
}