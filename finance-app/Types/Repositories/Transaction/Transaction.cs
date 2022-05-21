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
        [Column("account_id")]
        public uint AccountId { get; set; }
        
        /// <summary>
        /// The type of transaction
        /// </summary> 
        [Required]
        public TransactionTypeEnum Type { get; set; }
        
        /// <summary>
        /// The amount of the Transation
        /// </summary>
        [Required]
        public decimal Amount { get; set; }

        /// <summary>
        /// The date the transaction occurred 
        /// </summary>
        [Required]
        [Column("transaction_date")]
        public DateTime? TransactionDate { get; set; }

        /// <summary>
        /// Notes about the transaction
        /// </summary>
        public string   Notes { get; set; }

        /// <summary>
        /// The Id of the journal entry that the transaction belongs to.
        /// </summary>
        [Column("journal_entry")]
        public uint JournalEntry { get; set; }

    }

}