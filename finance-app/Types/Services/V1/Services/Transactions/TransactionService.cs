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
using finance_app.Types.DataContracts.V1.Responses.ErrorResponses;
using finance_app.Types.DataContracts.V1.Responses.ResourceMessages;
using finance_app.Types.DataContracts.V1.Responses.ReasonMessages;
using finance_app.Types.Services.V1.ResponseMessages;

namespace finance_app.Types.Services.V1.Transactions {
    public class TransactionService : ITransactionService {
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
            if (transaction == null) {
                var errorMessage = new ErrorResponseMessage(
                    new GettingActionMessage(transaction),
                    new ResourceWithPropertyMessage(transaction, "Id",  transaction.Id),
                    new NotFoundReason());
                return new ApiResponse<TransactionWithJournalDto>(ApiResponseCodesEnum.ResourceNotFound, errorMessage);
            }

            // Verify that the use can access the transaction
            var account = await _accountRepository.GetAccountByAccountId(transaction.AccountId);
            if (!(await _authorizationService.AuthorizeAsync(_context.HttpContext.User, account, "CanAccessResourcePolicy")).Succeeded) {
                var errorMessage = new ErrorResponseMessage(
                    new UpdatingActionMessage(transaction),
                    new ResourceWithPropertyMessage(transaction, "Id",  transaction.Id),
                    new UnauthorizedToAccessResourceReason(transaction));
                return new ApiResponse<TransactionWithJournalDto>(ApiResponseCodesEnum.Unauthorized, errorMessage);
            }

            var ret = _mapper.Map<TransactionWithJournalDto>(transaction);
            return new ApiResponse<TransactionWithJournalDto>(ret);


        }

        /// <inheritdoc cref="ITransactionService.GetRecentTransactions"/>
        public async Task<ApiResponse<ListResponse<TransactionWithJournalDto>>> GetRecentTransactions(AccountResourceIdentifier accountId, PaginationInfo pageInfo, bool includeJournals = false) {
            if (accountId == null) { throw new ArgumentNullException(nameof(AccountResourceIdentifier)); }

            var account = await _accountRepository.GetAccountByAccountId(accountId.Id);
            if (!(await _authorizationService.AuthorizeAsync(_context.HttpContext.User, account, "CanAccessResourcePolicy")).Succeeded) {
                var errorMessage = new ErrorResponseMessage(
                    new GettingActionMessage(typeof(Transaction)),
                    new ResourceWithPropertyMessage(account, "Id",  account.Id),
                    new UnauthorizedToAccessResourceReason(account));
                return new ApiResponse<ListResponse<TransactionWithJournalDto>>(ApiResponseCodesEnum.Unauthorized, errorMessage);
            }

            int offset = (int)pageInfo.PageNumber - 1;
            var transactions = includeJournals ?
                await _transactionRepository.GetRecentTransactionsWithJournalByAccountId(accountId.Id, (int)pageInfo.ItemsPerPage, offset) :
                await _transactionRepository.GetRecentTransactionsByAccountId(accountId.Id, (int)pageInfo.ItemsPerPage, offset);

            var ret = new ListResponse<TransactionWithJournalDto>(_mapper.Map<List<TransactionWithJournalDto>>(transactions));

            return new ApiResponse<ListResponse<TransactionWithJournalDto>>(ret);
        }

        /// <inheritdoc cref="ITransactionService.UpdateTransaction"/>   
        public async Task<ApiResponse<TransactionDto>> UpdateTransaction(Transaction transaction) {
            // Fetch Transaction to update
            var transactionToUpdate = await _transactionRepository.GetTransaction(transaction.Id);
            if (transactionToUpdate == null) {
                var errorMessage = new ErrorResponseMessage(
                    new UpdatingActionMessage(transaction),
                    new ResourceWithPropertyMessage(transaction, "Id",  transaction.Id),
                    new NotFoundReason());
                return new ApiResponse<TransactionDto>(ApiResponseCodesEnum.ResourceNotFound, errorMessage);
            }

            // Verify that the user can access the transaction
            var account = await _accountRepository.GetAccountByAccountId(transactionToUpdate.AccountId);
            if (!(await _authorizationService.AuthorizeAsync(_context.HttpContext.User, account, "CanAccessResourcePolicy")).Succeeded) {
                var errorMessage = new ErrorResponseMessage(
                    new UpdatingActionMessage(transaction),
                    new ResourceWithPropertyMessage(transaction, "Id",  transaction.Id),
                    new UnauthorizedToAccessResourceReason(account));
                return new ApiResponse<TransactionDto>(ApiResponseCodesEnum.Unauthorized, errorMessage);
            }

            // Currently can only update the notes on a transaction
            transactionToUpdate.Notes = transaction.Notes;

            var updatedTransaction = await _transactionRepository.UpdateTransaction(transactionToUpdate);

            return new ApiResponse<TransactionDto>(_mapper.Map<TransactionDto>(updatedTransaction));

        }
    }
}
