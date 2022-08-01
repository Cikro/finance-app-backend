using Microsoft.AspNetCore.Mvc;
using finance_app.Types.DataContracts.V1.Dtos;
using System.Collections.Generic;
using finance_app.Types.Repositories.Transaction;

namespace finance_app.Types.DataContracts.V1.Requests.JournalEntries
{
    public class CorrectJournalEntryRequest
    {   
        public List<TransactionForJournalEntryRequests> Transactions { get; set; }
        
    }
}
