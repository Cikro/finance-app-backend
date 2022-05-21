using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finance_app.Types.Repositories.Transaction
{
    public class TransactionRepository : ITransactionRepository {

        private readonly FinanceAppContext _context;
        public TransactionRepository(FinanceAppContext context) 
        {
            _context = context;

        }

        public async Task<IEnumerable<Transaction>> GetRecentTransactionsByAccountId(uint accountId, int pageSize, int offset)
        {
            var transactions = _context.Transactions
                .Where(x => x.AccountId == accountId)
                .OrderByDescending(x => x.DateCreated);
            return transactions;
        }
    }
}