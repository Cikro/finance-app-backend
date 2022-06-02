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
    [Route("api/Accounts/{accountId}/[controller]")]
    public class TransactionsController : ControllerBase
    {
        
        private readonly ILogger<AccountsController> _logger;
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public TransactionsController(ILogger<AccountsController> logger, 
                                  ITransactionService transactionService,
                                  IMapper mapper)
        {
            _logger = logger;
            _transactionService = transactionService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a list of financial accounts.
        /// </summary>
        /// <param name="accountId">The id of the Account who's transactions you are fetching</param>
        /// <param name="request">A GetTransactionsRequest</param>
        /// <remarks> 
        /// Sample Request:
        /// 
        ///     GET /api/Accounts/{accountId}/Transactions
        ///     {
        ///         "pageNumber": 1,
        ///         "itemsPerPage": 5
        ///     }
        /// 
        /// 
        /// 
        /// </remarks>
        /// <returns>A List of Recent Transactions on the provided Account</returns>
        [HttpGet]
        [UserAuthorizationFilter]
        public async Task<IActionResult> GetTransactions([FromQuery]AccountResourceIdentifier accountId,  [FromQuery]GetTransactionsRequest request)
        {
            // TODO: Add proper code. (default page sizes, error checks, Fluent Validation if not)

            ApiResponse<ListResponse<TransactionDto>> ret;
            if (request.PageInfo != null) {
                
                 ret =  await _transactionService.GetRecentTransactions(accountId,
                                                                        request.PageInfo,
                                                                        request.IncludeJournals);

            } else {
                 ret = await _transactionService.GetRecentTransactions(accountId,
                                                                        request.PageInfo,
                                                                        request.IncludeJournals);
            }


            return StatusCode(_mapper.Map<int>(ret?.ResponseCode), ret);
        }

        // TODO: Add Update Endpoint

      

    }
}
