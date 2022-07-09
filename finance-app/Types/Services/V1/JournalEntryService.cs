using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.Models.ResourceIdentifiers;
using finance_app.Types.Repositories;
using finance_app.Types.Repositories.Account;
using finance_app.Types.Repositories.JournalEntry;
using finance_app.Types.Services.V1.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace finance_app.Types.Services.V1
{
    public class JournalEntryService : IJournalEntryService {

        private readonly IMapper _mapper;
        private readonly IJournalEntryRepository _journalEntryRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _context;
        private readonly FinanceAppContext _dbContext;

        public JournalEntryService (IMapper mapper, 
                                    IJournalEntryRepository journalEntryRepository,
                                    IAccountRepository accountRepository,
                                    IAuthorizationService authorizationService,
                                    IHttpContextAccessor context,
                                    FinanceAppContext dbContext) 
        {
            _mapper = mapper;
            _journalEntryRepository = journalEntryRepository;
            _accountRepository = accountRepository;
            _authorizationService = authorizationService;
            _context = context;
            _dbContext = dbContext;
        } 


        /// <inheritdoc cref="IJournalEntryService.Get"/>
        public async Task<ApiResponse<JournalEntryDto>> Get(JournalEntryResourceIdentifier journalEntryId) {
            var journalEntry = await _dbContext.JournalEntries
                        .Where(j => j.Id == journalEntryId.Id)
                        .FirstOrDefaultAsync();

            if(journalEntry == null) {
                var message = $"Error getting journal entry. Journal entry with id '{journalEntry.Id}' does not exist.";
                return new ApiResponse<JournalEntryDto>(ApiResponseCodesEnum.ResourceNotFound, message);
            }
            
            if (!(await _authorizationService.AuthorizeAsync(_context.HttpContext.User,
                           journalEntry, "CanAccessResourcePolicy" )).Succeeded) {
                return new ApiResponse<JournalEntryDto>(ApiResponseCodesEnum.Unauthorized, "Unauthorized");
            }

            return new ApiResponse<JournalEntryDto>(_mapper.Map<JournalEntryDto>(journalEntry));
        }

        /// <inheritdoc cref="IJournalEntryService.GetRecent"/>
        public async Task<ApiResponse<ListResponse<JournalEntryDto>>> GetRecent(UserResourceIdentifier userId, PaginationInfo pageInfo) {
            
            pageInfo ??= new PaginationInfo{ PageNumber = 1, ItemsPerPage = 10};
            var journalEntries = await _dbContext.JournalEntries
                    .Include("Transactions")
                    .Where(j => j.UserId == userId.Id)
                    .OrderByDescending(x => x.DateCreated)
                    .Skip(((int) pageInfo.PageNumber) - 1 * (int) pageInfo.ItemsPerPage)
                    .Take((int) pageInfo.ItemsPerPage)
                    .AsNoTracking()
                    .ToListAsync();
            
            if (!(await _authorizationService.AuthorizeAsync(_context.HttpContext.User,
                           journalEntries, "CanAccessResourcePolicy" )).Succeeded) {
                return new ApiResponse<ListResponse<JournalEntryDto>>(ApiResponseCodesEnum.Unauthorized, "Unauthorized");
            }

            return new ApiResponse<ListResponse<JournalEntryDto>>(_mapper.Map<ListResponse<JournalEntryDto>>(journalEntries));
        }

        /// <inheritdoc cref="IJournalEntryService.Create"/>
        public async Task<ApiResponse<JournalEntryDto>> Create(JournalEntry journalEntry) {
            _dbContext.JournalEntries.Add(journalEntry);
            
            // TODO: Ensure Amount is correct
            // TODO: Ensure Date Created / Modified
            // TODO: journal Entry Mappers
            // TODO: journal Entry Authorization
            // TODO: Move Authorization into own service
            // TODO: Figure out better return messaging object structure
            

            await _dbContext.SaveChangesAsync();
            return new ApiResponse<JournalEntryDto>(_mapper.Map<JournalEntryDto>(journalEntry));
        }

        /// <inheritdoc cref="IJournalEntryService.Correct"/>
        public async Task<ApiResponse<JournalEntryDto>>Correct(JournalEntry journalEntry) {

            var journalToCorrect = await _dbContext.JournalEntries.SingleOrDefaultAsync(x => x.Id == journalEntry.Id);
            if(journalToCorrect == null) {
                var message = $"Error correcting journal entry. Journal entry with id '{journalEntry.Id}' does not exist.";
                return new ApiResponse<JournalEntryDto>(ApiResponseCodesEnum.ResourceNotFound, message);
            }
            
            // TODO: Ensure Amount is correct
            // TODO: Ensure Date Created / Modified
            journalEntry.Transactions.Concat(journalToCorrect.ReversedTransactions());
            journalToCorrect.Corrected = true;

            
            _dbContext.JournalEntries.Add(journalEntry);
            await _dbContext.SaveChangesAsync();
            return new ApiResponse<JournalEntryDto>(_mapper.Map<JournalEntryDto>(journalEntry));
        }
    }
}