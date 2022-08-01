using Microsoft.AspNetCore.Mvc;
using finance_app.Types.DataContracts.V1.Dtos;

namespace finance_app.Types.DataContracts.V1.Requests.Accounts
{
    public class GetAccountsRequest
    {   
        // TODO: Considering add Depth property to fetch only accounts without children, 
        // and their children's to a certain depth
        public PaginationInfo PageInfo { get; set; }
    }
}
