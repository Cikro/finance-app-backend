using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using finance_app.Types.Services.V1.Interfaces;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.DataContracts.V1.Requests.Accounts;
using finance_app.Types.DataContracts.V1.Dtos;

using finance_app.Types.Models.ResourceIdentifiers;
using AutoMapper;
using finance_app.Types.DataContracts.V1.Requests.Transactions;
using finance_app.Types.Repositories.Transaction;

namespace finance_app.Controllers.V1
{
    [ApiController]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/Users/{accountId}/[controller]")]
    public class TransactionsController : ControllerBase
    {
        
        private readonly ILogger<AccountsController> _logger;
        private readonly ITransactionRepository _transactionService;
        private readonly IMapper _mapper;

        public TransactionsController(ILogger<AccountsController> logger, 
                                  ITransactionRepository transactionService,
                                  IMapper mapper)
        {
            _logger = logger;
            _transactionService = transactionService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a list of financial accounts.
        /// </summary>
        /// <param name="userId">The id of the User who's accounts you are fetching</param>
        /// <param name="accountId">The id of the Account who's transactions you are fetching</param>
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
        public async Task<IActionResult> GetTransactions([FromQuery]AccountResourceIdentifier accountId,  [FromQuery]GetTransactionsRequest request)
        {
            // ApiResponse<ListResponse<TransactionDto>> ret;
            // if (request.PageInfo != null) {
                
            //      ret = await  _accountService.GetPaginatedAccounts(userId, request.PageInfo);

            // } else {
            //      ret = await _accountService.GetAccounts(userId);
            // }

            var ret = await _transactionService.GetRecentTransactionsByAccountId(accountId.Id, (int) request.PageInfo.ItemsPerPage, (int)request.PageInfo.PageNumber-1);
            return Ok(ret);

            // return StatusCode(_mapper.Map<int>(ret?.ResponseCode), ret);
        }

      

    }
}
