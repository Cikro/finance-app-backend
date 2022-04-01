using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace finance_app.Types.Repositories.Transaction
{
    [Table("transactions")]
    public class Transaction : DatabaseObject
    {
        /// <summary>
        /// The Id the transaction belongs to.
        /// </summary>
        [Required]
        public uint AccountId { get; set; }
        
        /// <summary>
        /// The type of transation
        /// </summary> 
        [Required]
        public TransactionTypeEnum Type { get; set; }
        
        /// <summary>
        /// The amount of the Transation
        /// </summary>
        [Required]
        public decimal Amount { get; set; }

        /// <summary>
        /// Notes about the transaction
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// The Id of the journal entery that the transation belongs to.
        /// </summary>
        public uint JournalEntryId { get; set; }

    }
    public enum TransactionTypeEnum : byte
    {
        Unknown = 0,
        Debit = 1,
        Transaction = 2
    }
}