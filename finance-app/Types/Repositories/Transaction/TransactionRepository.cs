using System.Collections.Generic;
using System.Threading.Tasks;

namespace finance_app.Types.Repositories.Transaction
{
    public class TransactionRepository : ITransactionRepository {
        public TransactionRepository() {

        }

        public async Task<IEnumerable<Transaction>> GetRecentTransactionsByAccountId(uint accountId, uint pageSize, uint offset)
        {
            return new List<Transaction>();
        }
    }
}