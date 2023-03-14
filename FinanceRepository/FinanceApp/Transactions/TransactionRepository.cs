using finance_app.Types.Repositories.FinanceApp;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace finance_app.Types.Repositories.Transactions
{
    public class TransactionRepository : ITransactionRepository {

        private readonly FinanceAppContext _context;

        /// <summary>
        /// An object for accessing Persisted Transactions from 
        /// a MariaDb Data Store
        /// </summary>
        /// <param name="context"></param>
        public TransactionRepository(FinanceAppContext context) 
        {
            _context = context;

        }

        /// <inheritdoc cref="ITransactionRepository.GetTransaction"/>
        public async Task<Transaction> GetTransaction(uint? transactionId) 
        {
            var transaction = await _context.Transactions
                .SelectTransaction()
                .FirstOrDefaultAsync(t => t.Id == transactionId);
            return transaction;
        }

        /// <inheritdoc cref="ITransactionRepository.GetTransactionWithJournal"/>
        public async Task<Transaction> GetTransactionWithJournal(uint? transactionId) 
        {
            var transaction = await _context.Transactions
                .SelectTransactionWithJournal()
                .FirstOrDefaultAsync(t => t.Id == transactionId);
            return transaction;
        }

        /// <inheritdoc cref="ITransactionRepository.GetRecentTransactionsByAccountId"/>
        public async Task<IEnumerable<Transaction>> GetRecentTransactionsByAccountId(uint? accountId, int pageSize, int offset)
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

        /// <inheritdoc cref="ITransactionRepository.GetRecentTransactionsWithJournalByAccountId"/>
        public async Task<IEnumerable<Transaction>> GetRecentTransactionsWithJournalByAccountId(uint? accountId, int pageSize, int offset) {
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

        /// <summary>
        /// Creates a new transaction
        /// </summary>
        /// <param name="transaction">The transaction to create</param>
        /// <returns>The updated Transaction</returns>
        public async Task<Transaction> CreateTransaction(Transaction transaction) {
            transaction.DateCreated = DateTime.Now;
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        /// <summary>
        /// Updates a transaction's updateable properties with 
        /// the provided transaction's properties.
        /// </summary>
        /// <param name="transaction">The transaction to update</param>
        /// <returns>The updated Transaction</returns>
        public async Task<Transaction> UpdateTransaction(Transaction transaction) {
            
            transaction.DateLastEdited = DateTime.Now;
            _context.Transactions.Attach(transaction);
            _context.Entry(transaction).Property(x => x.Notes).IsModified = true;
            _context.Entry(transaction).Property(x => x.DateLastEdited).IsModified = true;
            await _context.SaveChangesAsync();

            return await GetTransactionWithJournal(transaction.Id);
            
        }
    }
}