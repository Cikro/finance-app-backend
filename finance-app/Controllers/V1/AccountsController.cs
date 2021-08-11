using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.CodeDom;
using Microsoft.Extensions.Localization;

using finance_app.Types.Services.V1.Interfaces;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.DataContracts.V1.Requests.Accounts;
using finance_app.Types.DataContracts.V1.Dtos;

namespace finance_app.Controllers.V1
{
    [ApiController]
    [Route("Users/{userId}/[controller]")]
    [ApiVersion("1.0")]
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

        [HttpGet]
        [ApiVersion("1.1")]
        public async Task<ApiResponse<ListResponse<int>>> Get11([FromQuery]GetAccountsRequests request)
        {

            var x = new List<int>();
            x.AddRange(new List<int>{5,6,7,8});
            
            var ret = new ApiResponse<ListResponse<int>>
            {
                Data = new ListResponse<int>(x),
                ResponseMessage = "Success"
            };

            return ret;
        }
    }
}
