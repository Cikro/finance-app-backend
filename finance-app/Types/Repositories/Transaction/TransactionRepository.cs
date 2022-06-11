using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace finance_app.Types.Repositories.Transaction
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
        public async Task<Transaction> GetTransaction(uint transactionId) 
        {
            var transaction = await _context.Transactions
                .SelectTransaction()
                .FirstOrDefaultAsync(t => t.Id == transactionId);
            return transaction;
        }

        /// <inheritdoc cref="ITransactionRepository.GetRecentTransactionsByAccountId"/>
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

        /// <inheritdoc cref="ITransactionRepository.GetRecentTransactionsWithJournalByAccountId"/>
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

        
        /// <summary>
        /// Updates a transaction's updateable properties with 
        /// the provided transaction's properties.
        /// Decided to use a Stored Procedure here since EF Core doesn't play nice
        /// with SQL DB Update Triggers (used for Date Last Modified)
        /// </summary>
        /// <param name="transaction">The transaction to update</param>
        /// <returns>The updated Transaction</returns>
        public async Task<Transaction> UpdateTransaction(Transaction transaction) {
            Transaction updatedTransaction = null;

            var parameters = new object[]
            {
                new MySqlParameter("transactionId", transaction.Id),
                new MySqlParameter("notes", transaction.Notes),
            };
            
            var connection = _context.Database.GetDbConnection();

            try {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "UpdateTransaction";

                foreach (var p in parameters)
                {
                    command.Parameters.Add(p);
                }

                
                using (var reader = await command.ExecuteReaderAsync()) {
                    while (await reader.ReadAsync()) {
                        updatedTransaction = ReadTransaction(reader);
                    }
                }
                await connection.CloseAsync();

            } catch (Exception e) {
                if (connection?.State == ConnectionState.Open) {
                    await connection.CloseAsync();
                }
                throw e;
            }

            return updatedTransaction;
        }

        /// <summary>
        /// Reads a transaction from a DBreader
        /// </summary>
        /// <param name="reader">A DB reader with a transaction</param>
        /// <returns>A populated Transition</returns>
        private Transaction ReadTransaction(DbDataReader reader) {

            return new Transaction
            {
                Id = (uint)reader.GetInt32("id"),
                Notes  = reader.IsDBNull("notes") ? "" : reader.GetString("notes"),
                Amount = reader.IsDBNull("amount") ? 0 : reader.GetDecimal("amount"),
                Type = Enum.IsDefined(typeof(TransactionTypeEnum), reader.GetByte("type")) ? (TransactionTypeEnum) reader.GetByte("type") : TransactionTypeEnum.Unknown,
                Corrected = reader.IsDBNull("corrected") ? null : (bool?)reader.GetBoolean("corrected"),
                ServerGenerated = reader.IsDBNull("server_generated") ? null : (bool?)reader.GetBoolean("server_generated"),
                JournalEntryId =  (uint)reader.GetInt32("journal_entry_id"),
                TransactionDate = reader.GetDateTime("transaction_date"),
                DateCreated = reader.GetDateTime("date_created"),
                DateLastEdited = reader.GetDateTime("date_last_edited")
            };
        }
    }
}