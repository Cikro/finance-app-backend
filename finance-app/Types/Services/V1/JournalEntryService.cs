using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.DataContracts.V1.Responses.ErrorResponses;
using finance_app.Types.DataContracts.V1.Responses.ReasonMessages;
using finance_app.Types.DataContracts.V1.Responses.ResourceMessages;
using finance_app.Types.DataContracts.V1.Responses.ResponseMessage;
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
        private readonly IAccountRepository _accountRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IFinanceAppAuthorizationService _financeAppAuthorizationService;

        private readonly IHttpContextAccessor _context;
        private readonly FinanceAppContext _dbContext;

        public JournalEntryService (IMapper mapper, 
                                    IAccountRepository accountRepository,
                                    IAuthorizationService authorizationService,
                                    IFinanceAppAuthorizationService financeAppAuthorizationService,
                                    IHttpContextAccessor context,
                                    FinanceAppContext dbContext) 
        {
            _mapper = mapper;
            _accountRepository = accountRepository;
            _authorizationService = authorizationService;
            _financeAppAuthorizationService = financeAppAuthorizationService;
            _context = context;
            _dbContext = dbContext;
        } 


        /// <inheritdoc cref="IJournalEntryService.Get"/>
        public async Task<ApiResponse<JournalEntryDto>> Get(JournalEntryResourceIdentifier journalEntryId) {
            var journalEntry = await _dbContext.JournalEntries
                        .Where(j => j.Id == journalEntryId.Id)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();

            if(journalEntry == null) {
                var errorMessage = new ErrorResponseMessage(
                    new GettingActionMessage(journalEntry),
                    new ResourceWithPropertyMessage(journalEntry, journalEntry.Id),
                    new NotFoundReason());
                return new ApiResponse<JournalEntryDto>(ApiResponseCodesEnum.ResourceNotFound, errorMessage);
            }
            
            if (!(await _authorizationService.AuthorizeAsync(_context.HttpContext.User,
                           journalEntry, "CanAccessResourcePolicy" )).Succeeded) {
                var errorMessage = new ErrorResponseMessage(
                    new GettingActionMessage(journalEntry),
                    new ResourceWithPropertyMessage(journalEntry, journalEntry.Id),
                    new UnauthorizedToAccessResourceReason(journalEntry));
                return new ApiResponse<JournalEntryDto>(ApiResponseCodesEnum.Unauthorized, errorMessage);
            }

            return new ApiResponse<JournalEntryDto>(_mapper.Map<JournalEntryDto>(journalEntry));
        }

        /// <inheritdoc cref="IJournalEntryService.GetRecent"/>
        public async Task<ApiResponse<ListResponse<JournalEntryDto>>> GetRecent(UserResourceIdentifier userId, PaginationInfo pageInfo) {
            
            pageInfo ??= new PaginationInfo{ PageNumber = 1, ItemsPerPage = 10};
            var offset = (int) pageInfo.PageNumber - 1;
            var journalEntries = await _dbContext.JournalEntries
                                    .Include(j => j.Transactions)
                                    .Where(j => j.UserId == userId.Id)
                                    .OrderByDescending(x => x.DateCreated)
                                    .Skip(offset * (int) pageInfo.ItemsPerPage)
                                    .Take((int) pageInfo.ItemsPerPage)
                                    .AsNoTracking()
                                    .ToListAsync();


            
            // Authorize Journal Entries
            // For now, you can only access journal entries made by your account.
            // Maybe a bit redundant since the DB query sort of does this already
            journalEntries = (await _financeAppAuthorizationService.Filter(journalEntries, "CanAccessResourcePolicy"))
                            .ToList();

            return new ApiResponse<ListResponse<JournalEntryDto>>(
                new ListResponse<JournalEntryDto> (_mapper.Map<List<JournalEntryDto>>(journalEntries))
            );
        }

        /// <inheritdoc cref="IJournalEntryService.Create"/>
        public async Task<ApiResponse<JournalEntryDto>> Create(JournalEntry journalEntry) {

            // Fetch Accounts that will be modified
            var accounts = _dbContext.Accounts
                .SelectAccountsForTransactions(journalEntry.Transactions);

            // Authorize that user can modify the accounts
            if(!await _financeAppAuthorizationService.Authorize(accounts, "CanAccessResourcePolicy")) {
                // FIXME: When creating a resource, does it have an id to display?
                var errorMessage = new ErrorResponseMessage(
                        new CreatingActionMessage(journalEntry),
                        new ResourceWithPropertyMessage(journalEntry, journalEntry.Id),
                        new UnauthorizedAccessToAccountsReason());
                return new ApiResponse<JournalEntryDto>(ApiResponseCodesEnum.Unauthorized, errorMessage);
            }                                
            
            var accountDict = accounts.ToDictionary(a => a.Id, a => a);
            // Modify Account Balances
            foreach (var t in journalEntry.Transactions)
            {
                if (!accountDict.ContainsKey(t.AccountId)) {
                    var errorMessage = new ErrorResponseMessage(
                        new CreatingActionMessage(journalEntry),
                        new ResourceWithPropertyMessage(t, t.AccountId),
                        new NotFoundReason());
                    return new ApiResponse<JournalEntryDto>(ApiResponseCodesEnum.ResourceNotFound, errorMessage);
                }
                accountDict[t.AccountId].ApplyTransaction(_dbContext, t);
            }

            // Save to Database
            await _dbContext.JournalEntries.AddAsync(journalEntry);
            await _dbContext.SaveChangesAsync();

            return new ApiResponse<JournalEntryDto>(_mapper.Map<JournalEntryDto>(journalEntry));
        }

        /// <inheritdoc cref="IJournalEntryService.Correct"/>
        public async Task<ApiResponse<JournalEntryDto>>Correct(JournalEntryResourceIdentifier toCorrectId,  JournalEntry journalEntry) {

            var journalToCorrect = await _dbContext.JournalEntries
                .Include(x => x.Transactions)
                .SingleOrDefaultAsync(x => x.Id == toCorrectId.Id);
            if(journalToCorrect == null) {
                var errorMessage = new ErrorResponseMessage(
                        new CorrectingActionMessage(journalEntry),
                        new ResourceWithPropertyMessage(journalEntry, journalEntry.Id),
                        new NotFoundReason());
                return new ApiResponse<JournalEntryDto>(ApiResponseCodesEnum.ResourceNotFound, errorMessage);
            }
            

            // TODO: Ensure Date Created / Modified
            journalEntry.Transactions = journalEntry.Transactions.Concat(journalToCorrect.ReversedTransactions()).ToList();
            journalToCorrect.Corrected = true;
            _dbContext.Entry(journalToCorrect).Property(x => x.Corrected).IsModified = true;

            // Fetch Accounts that will be modified
            var accounts = _dbContext.Accounts
                .SelectAccountsForTransactions(journalEntry.Transactions);
                                
            // Authorize that user can modify the accounts
            if(!await _financeAppAuthorizationService.Authorize(accounts, "CanAccessResourcePolicy")) {
                var errorMessage = new ErrorResponseMessage(
                        new CorrectingActionMessage(journalEntry),
                        new ResourceWithPropertyMessage(journalEntry, journalEntry.Id),
                        new UnauthorizedAccessToAccountsReason());
                return new ApiResponse<JournalEntryDto>(ApiResponseCodesEnum.Unauthorized, errorMessage);
            }

            var accountDict = accounts.ToDictionary(a => a.Id, a => a);
            // Modify Account Balances
            foreach (var t in journalEntry.Transactions)
            {
                if (!accountDict.ContainsKey(t.AccountId)) {
                    var errorMessage = new ErrorResponseMessage(
                        new CorrectingActionMessage(journalEntry),
                        new ResourceWithPropertyMessage(t, t.AccountId),
                        new NotFoundReason());
                    return new ApiResponse<JournalEntryDto>(ApiResponseCodesEnum.ResourceNotFound, errorMessage);
                }
                accountDict[t.AccountId].ApplyTransaction(_dbContext, t);
            }

            _dbContext.JournalEntries.Add(journalEntry);
            await _dbContext.SaveChangesAsync();
            return new ApiResponse<JournalEntryDto>(_mapper.Map<JournalEntryDto>(journalEntry));
        }
    }
}