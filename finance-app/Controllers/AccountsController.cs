using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using finance_app.Types.EFModels;
using finance_app.Types.Interfaces;
using finance_app.Types;

namespace finance_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        
        private readonly ILogger<AccountsController> _logger;
        private readonly IAccountService _accountService;

        public AccountsController(ILogger<AccountsController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IEnumerable<Account>> Get([FromQuery] uint userId, [FromQuery] PaginationInfo pageInfo)
        {
            //TODO: Add API return Class
            // result
            //  Message
            //  Code
            // Data<t>
            // TODO: Figure out how to add search terms / filter terms?
            IEnumerable<Account> ret;
            if (pageInfo.PageNumber != null && pageInfo.ItemsPerPage != null) {
                ret = _accountService.GetPaginatedAccounts(userId, (int) pageInfo.ItemsPerPage, (int) pageInfo.PageNumber);

            } else {
                ret = _accountService.GetAccounts(userId);
            }
            
            return ret;
        }
    }
}
