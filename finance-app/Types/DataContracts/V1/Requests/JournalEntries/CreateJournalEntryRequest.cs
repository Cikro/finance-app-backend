using System.Collections.Generic;
using finance_app.Types.DataContracts.V1.Dtos;

namespace finance_app.Types.DataContracts.V1.Requests.JournalEntries
{
    public class CreateJournalEntryRequest
    {   
        
        public List<TransactionForJournalEntryRequests> Transactions { get; set; }
        
    }

}
