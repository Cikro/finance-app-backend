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
        /// Gets a list of recent transactions 
        /// </summary>
        /// <param name="accountId">Identifier for the account you are getting transactions for</param>
        /// <param name="pageInfo">An object represent information for paging</param>
        /// <param name="includeJournals">An object represent information for paging</param>
        /// <returns> A list of TransactionDtos</returns>
        Task<ApiResponse<ListResponse<TransactionDto>>> GetRecentTransactions(AccountResourceIdentifier accountId, PaginationInfo pageInfo, bool includeJournals);

        /// <summary>
        /// Updates an account with values from an existing account
        /// </summary>
        /// <param name="transaction">A populated transaction object</param>
        /// <returns> A Transaction of the updated transaction</returns>
        Task<ApiResponse<TransactionDto>> UpdateTransaction(Transaction transaction);
        
    }
}
