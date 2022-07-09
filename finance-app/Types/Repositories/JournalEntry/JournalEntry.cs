using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using finance_app.Types.Repositories.Transaction;

namespace finance_app.Types.Repositories.JournalEntry
{
    [Table("journal_entries")]
    public class JournalEntry : DatabaseObject
    {
        /// <summary>
        /// The user that create the journal entry.
        /// </summary>
        [Required]
        [Column("user_id")]
        public uint UserId { get; set; }
        
        /// <summary>
        /// The amount of all the debits and credits
        /// </summary>
        [Required]
        public decimal Amount { get; set; }

        /// <summary>
        /// If the journal entry has been corrected or not
        /// </summary>
        [Required]
        public bool Corrected { get; set; }

        /// <summary>
        /// If the journal entry was created automatically
        /// </summary>
        [Required]
        [Column("server_generated")]
        public bool ServerGenerated { get; set; }

        public IEnumerable<Transaction.Transaction> Transactions { get; set; }

        public IEnumerable<Transaction.Transaction> ReversedTransactions() {
            return Transactions.Select(t => {
                t.Type = ReverseTransactionType(t.Type);
                return t;
            });
        }

        public static TransactionTypeEnum ReverseTransactionType(TransactionTypeEnum t) => t switch
        {
            TransactionTypeEnum.Debit    =>  TransactionTypeEnum.Credit,
            TransactionTypeEnum.Credit    =>  TransactionTypeEnum.Debit,
            _ => throw new ArgumentOutOfRangeException(nameof(t), $"Error Reversing Journal. Not expected Transaction Type Enum : {t}"),
        };

    }
}