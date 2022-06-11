using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using finance_app.Types.Services.V1.Interfaces;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.DataContracts.V1.Requests.Accounts;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.Repositories.Account;
using finance_app.Types.Models.ResourceIdentifiers;
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
        [UserAuthorizationFilter]
        public async Task<IActionResult> GetAccounts([FromQuery]UserResourceIdentifier userId, [FromQuery]GetAccountsRequest request)
        {
            ApiResponse<ListResponse<AccountDto>> ret;
            if (request.PageInfo != null) {
                
                 ret = await  _accountService.GetPaginatedAccounts(userId, request.PageInfo);

            } else {
                 ret = await _accountService.GetAccounts(userId);
            }
            return StatusCode(_mapper.Map<int>(ret?.ResponseCode), ret);
        }

        /// <summary>
        /// Creates an account for a user.
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
        ///     "Equity"
        /// 
        /// 
        /// </remarks>
        /// <returns>The account that was created</returns>
        [HttpPost]
        [UserAuthorizationFilter]
        public async Task<IActionResult> CreateAccount([FromQuery]UserResourceIdentifier userId, [FromBody]CreateAccountRequest request)
        {
            var account = _mapper.Map<Account>(request);
            account.UserId = userId.Id;

            var ret = await _accountService.CreateAccount(account);

            return StatusCode(_mapper.Map<int>(ret?.ResponseCode), ret);
        }

        
        /// <summary>
        /// Closes an Account and all of it's children.
        /// </summary>
        /// <param name="accountId">An AccountResourceIdentifier</param>
        /// <returns>A list of accounts that were closed.</returns>
        [HttpDelete]
        [Route("/api/[controller]/{accountId}")]
        public async Task<IActionResult> DeleteAccount([FromQuery]AccountResourceIdentifier accountId)
        {
            var ret = await _accountService.CloseAccount(accountId);
            return StatusCode(_mapper.Map<int>(ret?.ResponseCode), ret);
        }

        /// <summary>
        /// Gets one Account
        /// </summary>
        /// <param name="accountId">An AccountResourceIdentifier</param>
        /// <returns>A list of accounts that were closed.</returns>
        [HttpGet]
        [Route("/api/[controller]/{accountId}")]
        public async Task<IActionResult> GetAccount([FromQuery]AccountResourceIdentifier accountId)
        {
            var ret = await _accountService.GetAccount(accountId);
            return StatusCode(_mapper.Map<int>(ret?.ResponseCode), ret);
        }

        /// <summary>
        /// Gets all children on an account
        /// </summary>
        /// <param name="accountId">An AccountResourceIdentifier</param>
        /// <returns>A list of accounts that were closed.</returns>
        [HttpGet]
        [Route("/api/[controller]/{accountId}/children")]
        public async Task<IActionResult> GetChildren([FromQuery]AccountResourceIdentifier accountId)
        {
            var ret = await _accountService.GetChildren(accountId);
            return StatusCode(_mapper.Map<int>(ret?.ResponseCode), ret);
        }

        
        /// <summary>
        /// Updates an Account with the account provided.
        /// </summary>
        /// <param name="accountId">An AccountResourceIdentifier</param>
        /// <param name="request">A PostAccountRequest</param>
        /// <returns>A list of accounts that were closed.</returns>
        [HttpPatch]
        [Route("/api/[controller]/{accountId}")]
        public async Task<IActionResult> PostAccount([FromQuery]AccountResourceIdentifier accountId, [FromBody]PostAccountRequest request)
        {
            var account = _mapper.Map<Account>(request);
            account.Id = accountId.Id;

            var ret = await _accountService.UpdateAccount(account);

            return StatusCode(_mapper.Map<int>(ret?.ResponseCode), ret);

        }

    }
}
