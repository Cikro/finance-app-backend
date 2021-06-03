using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finance_app.Types.Requests.Accounts
{
    public class GetAccountsRequests
    {
        [FromRoute(Name ="userId")]
        public uint UserId { get; set; }

        
        public PaginationInfo PageInfo { get; set; }

    }
}
