using System.Collections.Generic;
using System.Threading.Tasks;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Requests.Accounts;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.Models.ResourceIdentifiers;
using finance_app.Types.Repositories.Account;
using finance_app.Types.Repositories.Transaction;

namespace finance_app.Types.Services.V1.Interfaces
{
    public interface ITransactionService
    {

        /// <summary>
        /// Gets a Transaction
        /// </summary>
        /// /// <param name="transactionId">The Id of the Transaction you want to get</param>
        /// <param name="includeJournals">An object represent information for paging</param>
        /// <returns> A list of TransactionDtos</returns>
        Task<ApiResponse<TransactionDto>> GetTransaction(TransactionResourceIdentifier transactionId, bool includeJournals = false);

        /// <summary>
        /// Gets a list of recent transactions 
        /// </summary>
        /// <param name="accountId">Identifier for the account you are getting transactions for</param>
        /// <param name="pageInfo">An object represent information for paging</param>
        /// <param name="includeJournals">An object represent information for paging</param>
        /// <returns> A list of TransactionDtos</returns>
        Task<ApiResponse<ListResponse<TransactionDto>>> GetRecentTransactions(AccountResourceIdentifier accountId, PaginationInfo pageInfo, bool includeJournals);

        /// <summary>
        /// Updates an transaction with values from an existing transaction.
        /// Only valid properties will be updated.
        /// Current Valid Properties:
        ///     Notes
        /// </summary>
        /// <param name="transaction">A populated transaction object</param>
        /// <returns>The updated Transaction</returns>
        Task<ApiResponse<TransactionDto>> UpdateTransaction(Transaction transaction);
        
    }
}
