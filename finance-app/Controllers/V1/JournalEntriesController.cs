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
using finance_app.Types.DataContracts.V1.Requests.JournalEntries;
using finance_app.Types.Repositories.JournalEntry;

namespace finance_app.Controllers.V1
{
    [ApiController]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/[controller]/")]
    public class JournalEntriesController : ControllerBase
    {
        
        private readonly ILogger<AccountsController> _logger;
        private readonly IJournalEntryService _journalEntryService;
        private readonly IMapper _mapper;

        public JournalEntriesController(ILogger<AccountsController> logger, 
                                  IJournalEntryService transactionService,
                                  IMapper mapper)
        {
            _logger = logger;
            _journalEntryService = transactionService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets recent journal Entries.
        /// </summary>
        /// <param name="userId">The id of the User who's journal entries you are fetching</param>
        /// <param name="request">A GetTransactionsRequest</param>
        /// <remarks> 
        /// Sample Request:
        /// 
        ///     GET /api/Accounts/{userId}/JournalEntries
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
        [Route("api/User/{userId}/[controller]")]
        public async Task<IActionResult> GetJournalEntries([FromQuery]UserResourceIdentifier userId,  [FromQuery]GetJournalEntriesRequest request)
        {
            ApiResponse<ListResponse<JournalEntryDto>> ret;
            ret =  await _journalEntryService.GetRecent(userId, request.PageInfo);
            return StatusCode(_mapper.Map<int>(ret?.ResponseCode), ret);
        }

        /// <summary>
        /// Creates a journal Entry.
        /// </summary>
        /// <param name="request">A CreateJournalEntryRequest</param>
        /// <remarks> 
        /// Sample Request:
        /// 
        ///     GET /api/Accounts/{accountId}/Transactions/{transactionId}
        ///     {
        ///         "pageNumber": 1,
        ///         "itemsPerPage": 5
        ///     }
        /// 
        /// 
        /// 
        /// </remarks>
        /// <returns>The Updated Transaction</returns>
        [HttpPost]
        [Route("/api/[controller]/")]
        [UserAuthorizationFilter]
        public async Task<IActionResult> CreateJournalEntry( [FromQuery]CreateJournalEntryRequest request)
        {
            var journalEntry = _mapper.Map<JournalEntry>(request);
            var ret = await _journalEntryService.Create(journalEntry);

            return StatusCode(_mapper.Map<int>(ret?.ResponseCode), ret);
        }

        /// <summary>
        /// Updates a Transaction.
        /// </summary>
        /// <param name="journalId">The id of the Journal Entry you want to correct</param>
        /// <param name="request">A GetTransactionsRequest</param>
        /// <remarks> 
        /// Sample Request:
        /// 
        ///     GET /api/Accounts/{accountId}/Transactions/{transactionId}
        ///     {
        ///         "pageNumber": 1,
        ///         "itemsPerPage": 5
        ///     }
        /// 
        /// 
        /// 
        /// </remarks>
        /// <returns>The Updated Transaction</returns>
        [HttpPatch]
        [Route("/api/[controller]/{journalEntryId}")]
        [UserAuthorizationFilter]
        public async Task<IActionResult> CorrectJournalEntry([FromQuery]JournalEntryResourceIdentifier journalId, [FromQuery]CorrectJournalEntryRequest request)
        {
            var journalEntry = _mapper.Map<JournalEntry>(request);
            journalEntry.Id = journalId.Id;
            var ret = await _journalEntryService.Correct(journalEntry);

            return StatusCode(_mapper.Map<int>(ret?.ResponseCode), ret);
        }
      

    }
}
