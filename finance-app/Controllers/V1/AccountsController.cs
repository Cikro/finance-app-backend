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
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/Users/{userId}/[controller]")]
    public class AccountsController : ControllerBase
    {
        
        private readonly ILogger<AccountsController> _logger;
        private readonly IAccountService _accountService;

        public AccountsController(ILogger<AccountsController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        /// <summary>
        /// Gets a list of financial accounts.
        /// </summary>
        /// <param name="request"></param>
        /// <remarks> 
        /// Sample Request:
        /// 
        ///     GET /api/Users/{userId}/Accounts 
        ///     {
        ///         "pageNumber": 1,
        ///         "itemsPerPage": 5
        ///     }
        /// 
        /// 
        /// 
        /// </remarks>
        /// <returns>A List of accounts, and the number of items in the list</returns>
        [HttpGet]
        public async Task<ApiResponse<ListResponse<AccountDto>>> Get([FromQuery]GetAccountsRequests request)
        {
            List<AccountDto> accounts;
            if (request.PageInfo != null) {
                
                accounts = await _accountService.GetPaginatedAccounts(request.UserId, request.PageInfo);

            } else {
                accounts = await _accountService.GetAccounts(request.UserId);
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
