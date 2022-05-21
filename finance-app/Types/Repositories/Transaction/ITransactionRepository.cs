using System.Collections.Generic;
using System.Threading.Tasks;

namespace finance_app.Types.Repositories.Transaction
{
    public interface ITransactionRepository 
    {
        public Task<IEnumerable<Transaction>> GetRecentTransactionsByAccountId(uint accountId, int pageSize, int offset);
    }
}