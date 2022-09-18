using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using finance_app.Types.Repositories.JournalEntries;
using finance_app.Types.Repositories.Transactions;

namespace finance_app.Types.Repositories.Accounts
{
    public static class AccountExtensions
    {
        /// <summary>
        /// Selects Accounts with information required to apply transactions
        /// </summary>
        /// <param name="accounts">The Accounts DbSet</param>
        /// <param name="accountIds">A list of AccountIds to fetch</param>
        /// <returns>A Query to fetch the accounts</returns>
        public static IQueryable<Account> SelectAccountsForTransactions(this DbSet<Account> accounts, List<uint> accountIds) {
            // Fetch Accounts that will be modified
            return accounts.Select(a =>
                new Account{
                    Id = a.Id,
                    Balance = a.Balance,
                    UserId = a.UserId,
                    Type = a.Type,
                    Closed = a.Closed
                })
                .Where(a => accountIds.Contains((uint) a.Id));
        }


        /// <summary>
        /// Selects Accounts with information to apply transactions to.
        /// </summary>
        /// <param name="accounts">The Accounts DbSet</param>
        /// <param name="transactions">A list of Transactions with accounts</param>
        /// <returns>A Query to fetch the accounts</returns>
        public static IQueryable<Account> SelectAccountsForTransactions(this DbSet<Account> accounts, IEnumerable<Transaction> transactions) {
            // Unique Account Ids of transactions being created
            var accountIds = transactions
                .GroupBy(t => (uint) t.AccountId)
                .Select(grp => grp.Key).ToList();

            // Fetch Accounts that will be modified
            return accounts.SelectAccountsForTransactions(accountIds);
        }

        /// <summary>
        /// Selects Accounts with information to apply transactions to.
        /// </summary>
        /// <param name="accounts">The Accounts DbSet</param>
        /// <param name="journalEntry">A Journal Entry with Transactions</param>
        /// <returns>A Query to fetch the accounts</returns>
        public static IQueryable<Account> SelectAccountsForTransactions(this DbSet<Account> accounts, JournalEntry journalEntry) {
            return accounts.SelectAccountsForTransactions(journalEntry.Transactions);
        }
    }
}