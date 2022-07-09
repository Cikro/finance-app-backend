using System.Collections.Generic;
using finance_app.Types.Repositories.Transaction;

namespace finance_app.Types.DataContracts.V1.Requests.JournalEntries
{
    public class CreateJournalEntryRequest
    {   
        public List<Transaction> Transactions { get; set; }
        
    }
}
