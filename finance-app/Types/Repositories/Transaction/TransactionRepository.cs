using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
            var transactions =  await _context.Transactions
                .SelectTransaction()
                .Where(x => x.AccountId == accountId)
                .OrderByDescending(x => x.DateCreated)
                .Skip(offset * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
            return transactions;
        }

        public async Task<IEnumerable<Transaction>> GetRecentTransactionsWithJournalByAccountId(uint accountId, int pageSize, int offset) {
            var transactions = await _context.Transactions
                .SelectTransactionWithJournal()
                .Where(x => x.AccountId == accountId)
                .OrderByDescending(x => x.DateCreated)
                .Skip(offset * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
            return transactions;
        }
    }
}