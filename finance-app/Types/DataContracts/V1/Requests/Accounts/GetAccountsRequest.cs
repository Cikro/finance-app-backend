using Microsoft.AspNetCore.Mvc;
using finance_app.Types.DataContracts.V1.Dtos;

namespace finance_app.Types.DataContracts.V1.Requests.Accounts
{
    public class GetAccountsRequest
    {   
        public PaginationInfo PageInfo { get; set; }
    }
}
