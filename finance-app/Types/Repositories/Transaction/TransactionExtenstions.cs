using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace finance_app.Types.Repositories.Transaction
{
    public static class TransactionExtensions {
            public static IQueryable<Transaction> SelectTransaction(this DbSet<Transaction> transactions) {
                return transactions.Select(t => new Transaction {
                        Id = t.Id,
                        DateCreated = t.DateCreated,
                        DateLastEdited = t.DateLastEdited,
                        AccountId = t.AccountId,
                        Amount = t.Amount,
                        Notes = t.Notes ,
                        JournalEntryId = t.JournalEntryId,
                        TransactionDate = t.TransactionDate,
                        Type = t.Type 
                    });
            }

            public static IQueryable<Transaction> SelectTransactionWithJournal(this DbSet<Transaction> transactions) {
                return transactions.Include(t => t.JournalEntry)
                        .Select(t => new Transaction {
                            Id = t.Id,
                            DateCreated = t.DateCreated,
                            DateLastEdited = t.DateLastEdited,
                            AccountId = t.AccountId,
                            Amount = t.Amount,
                            Notes = t.Notes ,
                            JournalEntryId = t.JournalEntryId,
                            TransactionDate = t.TransactionDate,
                            Type = t.Type,
                            JournalEntry = t.JournalEntry
                        });
            }
        }
}