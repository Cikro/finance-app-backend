using System.Collections.Generic;
using System.Threading.Tasks;

namespace finance_app.Types.Repositories.Transaction
{
    public interface ITransactionRepository 
    {
        //TODO: Add comments
        public Task<Transaction> GetTransaction(uint transactionId);
        public Task<IEnumerable<Transaction>> GetRecentTransactionsByAccountId(uint accountId,  int pageSize, int offset);
        public Task<IEnumerable<Transaction>> GetRecentTransactionsWithJournalByAccountId(uint accountId,  int pageSize, int offset);
        public Task<Transaction> UpdateTransaction(Transaction transaction);
    }
}