using System;
using System.Collections.Generic;
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
            var offset = (int) pageInfo.PageNumber - 1;
            var journalEntries = await _dbContext.JournalEntries
                                    .Include(j => j.Transactions)
                                    .Where(j => j.UserId == userId.Id)
                                    .OrderByDescending(x => x.DateCreated)
                                    .Skip(offset * (int) pageInfo.ItemsPerPage)
                                    .Take((int) pageInfo.ItemsPerPage)
                                    .AsNoTracking()
                                    .ToListAsync();


            // TODO: figure out How I want to authorize these,
            //     - You need permission to see all accounts on the entry? else, error
            //     - You need permission to see all accounts on the entry? else, filter
            //     - You can only access journal entries made by your account? else error

            return new ApiResponse<ListResponse<JournalEntryDto>>(
                new ListResponse<JournalEntryDto> (_mapper.Map<List<JournalEntryDto>>(journalEntries))
            );
        }

        /// <inheritdoc cref="IJournalEntryService.Create"/>
        public async Task<ApiResponse<JournalEntryDto>> Create(JournalEntry journalEntry) {
            
            // TODO: Ensure Amount is correct
            //          - Use Getters and Setters on Journal Class
            //          - Map Transactions to Journal entry in mappers
            //          - Validate in Fluent Validation?
            
            // TODO: Figure out better return messaging object structure


            // Unique Account Ids of transactions being created
            var accountIds = journalEntry.Transactions
                .GroupBy(t => t.AccountId)
                .Select(grp => grp.Key).ToList();

            // Fetch Accounts that will be modified
            var accounts = _dbContext.Accounts
            .Select(a => new Account{
                Id = a.Id,
                Balance = a.Balance,
                UserId = a.UserId,
                Type = a.Type
            })
            .Where(a => accountIds.Contains((uint) a.Id))
            .ToList();


            // Authorize that user can modify the accounts
            if(!await _financeAppAuthorizationService.AuthorizeEnumerable(accounts, "CanAccessResourcePolicy")) {
                return new ApiResponse<JournalEntryDto>(ApiResponseCodesEnum.Unauthorized, "Error Creating Journal Entry. You are unauthorized to access some of the accounts that you are making transactions on.");
            }

            // Modify Account Balances
            foreach (var a in accounts) {
                foreach (var t in journalEntry.Transactions)
                {
                    if(t.AccountId == a.Id)
                    {
                        a.ApplyTransaction(_dbContext, t);
                    }
                }
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
                var message = $"Error correcting journal entry. Journal entry with id '{journalEntry.Id}' does not exist.";
                return new ApiResponse<JournalEntryDto>(ApiResponseCodesEnum.ResourceNotFound, message);
            }
            
            // TODO: Ensure Amount is correct
            // TODO: Ensure Date Created / Modified
            journalEntry.Transactions = journalEntry.Transactions.Concat(journalToCorrect.ReversedTransactions()).ToList();
            journalToCorrect.Corrected = true;
            _dbContext.Entry(journalToCorrect).Property(x => x.Corrected).IsModified = true;

            // Unique Account Ids of transactions being created
            var accountIds = journalEntry.Transactions
                .GroupBy(t => t.AccountId)
                .Select(grp => grp.Key).ToList();

            // Fetch Accounts that will be modified
            var accounts = _dbContext.Accounts
            .Select(a => new Account{
                Id = a.Id,
                Balance = a.Balance,
                UserId = a.UserId,
                Type = a.Type
            })
            .Where(a => accountIds.Contains((uint) a.Id))
            .ToList();
                                
            // Modify Account Balances
            foreach (var a in accounts) {
                foreach (var t in journalEntry.Transactions)
                {
                    if(t.AccountId == a.Id)
                    {
                        a.ApplyTransaction(_dbContext, t);
                    }
                }
            }

            _dbContext.JournalEntries.Add(journalEntry);
            await _dbContext.SaveChangesAsync();
            return new ApiResponse<JournalEntryDto>(_mapper.Map<JournalEntryDto>(journalEntry));
        }
    }
}