using Microsoft.AspNetCore.Mvc;
using finance_app.Types.DataContracts.V1.Dtos;

namespace finance_app.Types.DataContracts.V1.Requests.Transactions
{
    public class GetTransactionsRequest
    {   
        public PaginationInfo PageInfo { get; set; }
        public bool IncludeJournals { get; set; } = false;
    }
}
