using Microsoft.AspNetCore.Mvc;
using finance_app.Types.DataContracts.V1.Dtos;

namespace finance_app.Types.DataContracts.V1.Requests.Accounts
{
    public class CreateAccountsRequests
    {
        [FromRoute(Name ="userId")]
        public uint UserId { get; set; }
        
        [FromBody]
        public AccountDto Account { get; set; }

    }
}
