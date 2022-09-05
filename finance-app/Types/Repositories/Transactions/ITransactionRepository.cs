using System.Collections.Generic;
using System.Threading.Tasks;

namespace finance_app.Types.Repositories.Transactions
{
    public interface ITransactionRepository 
    {
        /// <summary>
        /// Fetches one Transaction by TransactionId from the database
        /// </summary>
        /// <param name="transactionId">The Id of the transaction you are fetching.</param>
        /// <returns>The transaction you wanted to fetch</returns>
        public Task<Transaction> GetTransaction(uint? transactionId);

        /// <summary>
        /// Fetches one Transaction with JournalEntries Populated by TransactionId from the database
        /// </summary>
        /// <param name="transactionId">The Id of the transaction you are fetching.</param>
        /// <returns>The transaction you wanted to fetch</returns>
        public Task<Transaction> GetTransactionWithJournal(uint? transactionId) ;

        /// <summary>
        /// Fetches recent Transactions that occurred on given Account.
        /// </summary>
        /// <param name="accountId">The ID of the Account you want transactions on.</param>
        /// <param name="pageSize">The number of Items per page you want transactions on.</param>
        /// <param name="offset">The page offset.</param>
        /// <returns>A List of recent Transactions on the Account</returns>
        public Task<IEnumerable<Transaction>> GetRecentTransactionsByAccountId(uint? accountId,  int pageSize, int offset);

        /// <summary>
        /// Fetches recent Transactions (with Journal Entries populated) that occurred on given Account.
        /// </summary>
        /// <param name="accountId">The ID of the Account you want transactions on.</param>
        /// <param name="pageSize">The number of Items per page you want transactions on.</param>
        /// <param name="offset">The page offset.</param>
        /// <returns>A List of recent Transactions on the Account</returns>
        public Task<IEnumerable<Transaction>> GetRecentTransactionsWithJournalByAccountId(uint? accountId,  int pageSize, int offset);

        /// <summary>
        /// Updates a given transaction with the properties on the Transaction
        /// </summary>
        /// <param name="transaction">The Transactions you want to save to the DB.</param>
        /// <returns></returns>
        public Task<Transaction> UpdateTransaction(Transaction transaction);

    }
}