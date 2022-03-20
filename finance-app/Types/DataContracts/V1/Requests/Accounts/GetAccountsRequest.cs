using Microsoft.AspNetCore.Mvc;
using finance_app.Types.DataContracts.V1.Dtos;

namespace finance_app.Types.DataContracts.V1.Requests.Accounts
{
    public class GetAccountsRequest
    {   
        // TODO: Considering add Depth property to fetch only accounts without children, 
        // and their childrens  to a certian depth
        public PaginationInfo PageInfo { get; set; }
    }
}
