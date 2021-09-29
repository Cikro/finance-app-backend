using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using finance_app.Types.Services.V1.Interfaces;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.DataContracts.V1.Requests.Accounts;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.Repositories.Account;
using AutoMapper;

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
        private readonly IMapper _mapper;

        public AccountsController(ILogger<AccountsController> logger, 
                                  IAccountService accountService,
                                  IMapper mapper)
        {
            _logger = logger;
            _accountService = accountService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a list of financial accounts.
        /// </summary>
        /// <param name="userId">The id of the User who's accounts you are fetching</param>
        /// <param name="request">A GetAccountsRequest</param>
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
        public async Task<ApiResponse<ListResponse<AccountDto>>> Get([FromRoute(Name ="userId")] uint userId, [FromQuery]GetAccountsRequest request)
        {
            List<AccountDto> accounts;
            if (request.PageInfo != null) {
                
                accounts = await _accountService.GetPaginatedAccounts(userId, request.PageInfo);

            } else {
                accounts = await _accountService.GetAccounts(userId);
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

        /// <summary>
        /// Gets a list of financial accounts.
        /// </summary>
        /// <param name="userId">The id of the User you are creating an account for.</param>
        /// <param name="request">A CreateAccountRequest</param>
        /// <remarks> 
        /// Sample Request:
        /// 
        ///     POST /api/Users/{userId}/Accounts 
        ///     {
        ///         "Account": {
        ///             "Name": "Sample Account Name"
        ///             "Description": "A Sample Account for the Sample Request"
        ///             "Balance": 
        ///             "Type": "Asset""
        ///             "CurrencyCode": "Ca"
        ///             "ParentAccountId": null
        ///     }
        /// Valid Account Types:
        ///     "Asset"
        ///     "Liability"
        ///     "Expense"
        ///     "Equuity"
        /// 
        /// 
        /// </remarks>
        /// <returns>The account that was created</returns>
        [HttpPost]
        public async Task<ApiResponse<AccountDto>> CreateAccount([FromRoute(Name ="userId")] uint userId, [FromBody]CreateAccountRequest request)
        {
            var account = _mapper.Map<Account>(request);
            account.User_Id = userId;

            var newAccount = await _accountService.CreateAccount(account);
            var ret = new ApiResponse<AccountDto>
            {
                Data = newAccount,
                ResponseMessage = "Success",
                StatusCode = System.Net.HttpStatusCode.OK,
                ResponseCode = ApiResponseCodesEnum.Success
            };
            return ret;
        }
    }
}
