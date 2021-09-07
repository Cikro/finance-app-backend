using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using finance_app.Types.Services.V1.Interfaces;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.DataContracts.V1.Requests.Accounts;
using finance_app.Types.DataContracts.V1.Dtos;

namespace finance_app.Controllers.V1
{
    [ApiController]
    [Route("api/Users/{userId}/[controller]")]
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
                ResponseMessage = "Success",
                StatusCode = System.Net.HttpStatusCode.OK,
                ResponseCode = ApiResponseCodesEnum.Success
            };

            return ret;
        }
    }
}
