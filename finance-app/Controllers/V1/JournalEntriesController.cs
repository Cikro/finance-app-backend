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
using finance_app.Types.Repositories.Transactions;
using finance_app.Types.DataContracts.V1.Requests.JournalEntries;
using finance_app.Types.Repositories.JournalEntries;

namespace finance_app.Controllers.V1
{
    [ApiController]
    [Produces("application/json")]
    [ApiVersion("1.0")]
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
        /// <param name="request">A GetJournalEntriesRequest</param>
        /// <remarks> 
        /// Sample Request:
        ///     GET /api/user/{userId}/JournalEntries?pageNumber=1&amp;itemsPerPage=10
        /// </remarks>
        /// <returns>A List of Recent JournalEntries made by the user</returns>
        [HttpGet]
        [UserAuthorizationFilter]
        [Route("api/User/{userId}/[controller]")]
        public async Task<IActionResult> GetJournalEntries([FromQuery]UserResourceIdentifier userId,  [FromQuery]GetRecentJournalEntriesRequest request)
        {
            ApiResponse<ListResponse<JournalEntryDto>> ret;
            ret =  await _journalEntryService.GetRecent(userId, request.PageInfo);
            return StatusCode(_mapper.Map<int>(ret?.ResponseCode), ret);
        }

        /// <summary>
        /// Creates a journal Entry.
        /// </summary>
        /// <param name="userId">A UserResourceIdentifier</param>
        /// <param name="request">A CreateJournalEntryRequest</param>
        /// <remarks> 
        /// Sample Request:
        ///     POST /api/user/{userId}/JournalEntries
        /// </remarks>
        /// <returns>The Created Journal Entry</returns>
        [HttpPost]
        [Route("api/User/{userId}/[controller]")]
        [UserAuthorizationFilter]
        public async Task<IActionResult> CreateJournalEntry([FromQuery]UserResourceIdentifier userId, [FromBody]CreateJournalEntryRequest request)
        {
            var journalEntry = _mapper.Map<JournalEntry>(request);
            journalEntry.UserId = userId.Id;
            journalEntry.Transactions.Select(t => t.UserId = userId.Id).ToList();
            var ret = await _journalEntryService.Create(journalEntry);

            return StatusCode(_mapper.Map<int>(ret?.ResponseCode), ret);
        }

        /// <summary>
        /// Corrects a Journal Entry.
        /// </summary>
        /// <param name="userId">The user who is making the correction</param>
        /// <param name="journalEntryId">The id of the Journal Entry you want to correct</param>
        /// <param name="request">A CorrectJournalEntryRequest with the new correct transactions</param>
        /// <remarks> 
        /// Corrects a Journal Entry by reversing the transactions on the journal Entry to be corrected,
        /// and creates a new journal with those reversed transactions, and the new provided transactions.
        /// 
        /// Sample Request:
        /// 
        ///     PATCH /api/User/{userId}/[controller]/{journalId}
        ///     Body:
        ///     Transactions: {
        ///         {Amount: 10, AccountId: 10, Type: 1, Notes: "TestNotes"}
        ///         {Amount: 10, AccountId: 15, Type: 2, Notes: "TestNotes"}
        ///     }
        /// 
        /// </remarks>
        /// <returns>A Journal entry correcting the requested journal entry to correct </returns>
        [HttpPatch]
        [Route("/api/User/{userId}/[controller]/{journalEntryId}")]
        [UserAuthorizationFilter]
        public async Task<IActionResult> CorrectJournalEntry([FromQuery]UserResourceIdentifier userId, [FromQuery]JournalEntryResourceIdentifier journalEntryId, [FromBody]CorrectJournalEntryRequest request)
        {
            var journalEntry = _mapper.Map<JournalEntry>(request);
            journalEntry.UserId = userId.Id;
            journalEntry.Transactions.Select(t => t.UserId = userId.Id).ToList();
            var ret = await _journalEntryService.Correct(journalEntryId, journalEntry);

            return StatusCode(_mapper.Map<int>(ret?.ResponseCode), ret);
        }
      

    }
}
