using Microsoft.AspNetCore.Mvc;
using finance_app.Types.DataContracts.V1.Dtos;

namespace finance_app.Types.DataContracts.V1.Requests.Accounts
{
    public class GetAccountsRequests
    {
        [FromRoute(Name ="userId")]
        public uint UserId { get; set; }

        
        public PaginationInfo PageInfo { get; set; }

    }
}
