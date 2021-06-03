using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using finance_app.Types.Interfaces;
using finance_app.Types;
using finance_app.Types.Responses;
using finance_app.Types.Responses.Dtos;
using finance_app.Types.Requests.Accounts;

namespace finance_app.Controllers
{
    [ApiController]
    [Route("Users/{userId}/[controller]")]
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
        public async Task<ApiResponse<ListResponse<AccountDto>>> Get([FromQuery]GetAccountsRequests request)
        {
            List<AccountDto> accounts;
            if (request.PageInfo != null) {
                
                accounts = _accountService.GetPaginatedAccounts(request.UserId, request.PageInfo);

            } else {
                accounts = _accountService.GetAccounts(request.UserId);
            }

            var ret = new ApiResponse<ListResponse<AccountDto>>
            {
                Data = new ListResponse<AccountDto>(accounts),
                ResponseMessage = "Success"
            };

            return ret;
        }
    }
}
