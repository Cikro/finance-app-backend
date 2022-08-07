using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using finance_app.Types.Repositories.Account;
using finance_app.Types.Services.V1.Interfaces;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.Models.ResourceIdentifiers;
using AutoMapper;
using System;
using finance_app.Types.Repositories.Transaction;

namespace finance_app.Types.Services.V1
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _context;
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapper">A Mapper dependency</param>
        /// <param name="transactionRepository">A transaction repository</param>
        /// <param name="accountRepository">An account repository</param>
        /// <param name="authorizationService">An Authorization Service</param>
        /// <param name="context">An IHttpContext Accessor</param>
        public TransactionService(IMapper mapper, 
                             ITransactionRepository transactionRepository,
                             IAccountRepository accountRepository,
                             IAuthorizationService authorizationService,
                             IHttpContextAccessor context) {

            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _context = context;
        }

        /// <inheritdoc cref="ITransactionService.GetTransaction"/>
        public async Task<ApiResponse<TransactionWithJournalDto>> GetTransaction(TransactionResourceIdentifier transactionId, bool includeJournals = false) {
            // Fetch Transaction to update
            var transaction = await _transactionRepository.GetTransaction(transactionId.Id);
            if (transaction == null) 
            {
                var message = $"Error getting transaction. Transaction with id '{transaction.Id}' does not exist.";
                return new ApiResponse<TransactionWithJournalDto>(ApiResponseCodesEnum.ResourceNotFound, message);
            }

            // Verify that the use can access the transaction
            var account = await _accountRepository.GetAccountByAccountId(transaction.AccountId);
            if (!(await _authorizationService.AuthorizeAsync(_context.HttpContext.User, account, "CanAccessResourcePolicy" )).Succeeded) 
            {
                return new ApiResponse<TransactionWithJournalDto>(ApiResponseCodesEnum.Unauthorized, "Unauthorized");
            }

            var ret = _mapper.Map<TransactionWithJournalDto>(transaction);
            return new ApiResponse<TransactionWithJournalDto>(ret);


        }

        /// <inheritdoc cref="ITransactionService.GetRecentTransactions"/>
        public async Task<ApiResponse<ListResponse<TransactionWithJournalDto>>> GetRecentTransactions(AccountResourceIdentifier accountId, PaginationInfo pageInfo, bool includeJournals = false) {
            if (accountId == null) { throw new ArgumentNullException(nameof(AccountResourceIdentifier)); }
            if (!(pageInfo?.PageNumber != null) || pageInfo?.PageNumber <= 0) { return new ApiResponse<ListResponse<TransactionWithJournalDto>>(ApiResponseCodesEnum.BadRequest, "Invalid Page Number."); }
            if (!(pageInfo?.ItemsPerPage!= null) || pageInfo?.ItemsPerPage <= 0) { return new ApiResponse<ListResponse<TransactionWithJournalDto>>(ApiResponseCodesEnum.BadRequest, "Invalid Items Per Page.");; }

            var account = await _accountRepository.GetAccountByAccountId(accountId.Id);
            if (!(await _authorizationService.AuthorizeAsync(_context.HttpContext.User, account, "CanAccessResourcePolicy" )).Succeeded) 
            {
                return new ApiResponse<ListResponse<TransactionWithJournalDto>>(ApiResponseCodesEnum.Unauthorized, "Unauthorized");
            }

            int offset = (int)pageInfo.PageNumber - 1;
            var transactions = includeJournals ?
                await _transactionRepository.GetRecentTransactionsWithJournalByAccountId(accountId.Id, (int)pageInfo.ItemsPerPage, (int)offset) :
                await _transactionRepository.GetRecentTransactionsByAccountId(accountId.Id, (int)pageInfo.ItemsPerPage, (int)offset);

            var ret = new ListResponse<TransactionWithJournalDto>(_mapper.Map<List<TransactionWithJournalDto>>(transactions));

            return new ApiResponse<ListResponse<TransactionWithJournalDto>>(ret);
        }

        /// <inheritdoc cref="ITransactionService.UpdateTransaction"/>   
        public async Task<ApiResponse<TransactionDto>> UpdateTransaction(Transaction transaction) {
            // Fetch Transaction to update
            var transactionToUpdate = await _transactionRepository.GetTransaction(transaction.Id);
            if (transactionToUpdate == null) 
            {
                var message = $"Error updating transaction. Transaction with id '{transaction.Id}' does not exist.";
                return new ApiResponse<TransactionDto>(ApiResponseCodesEnum.ResourceNotFound, message);
            }

            // Verify that the user can access the transaction
            var account = await _accountRepository.GetAccountByAccountId(transactionToUpdate.AccountId);
            if (!(await _authorizationService.AuthorizeAsync(_context.HttpContext.User, account, "CanAccessResourcePolicy" )).Succeeded) 
            {
                return new ApiResponse<TransactionDto>(ApiResponseCodesEnum.Unauthorized, "Unauthorized");
            }

            // Currently can only update the notes on a transaction
            transactionToUpdate.Notes = transaction.Notes;

            var updatedTransaction = await _transactionRepository.UpdateTransaction(transactionToUpdate);

            return new ApiResponse<TransactionDto>( _mapper.Map<TransactionDto>(updatedTransaction));

        }
    }
}
