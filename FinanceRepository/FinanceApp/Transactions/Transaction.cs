using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using finance_app.Types.Repositories.JournalEntries;

namespace finance_app.Types.Repositories.Transactions
{
    [Table("transactions")]
    public class Transaction : DatabaseObject
    {
        /// <summary>
        /// The Id the transaction belongs to.
        /// </summary>
        [Required]
        [Column("account_id")]
        public uint? AccountId { get; set; }

        [Required]
        [Column("user_id")]
        public uint? UserId {get ;set; }
        
        /// <summary>
        /// The type of transaction
        /// </summary> 
        [Required]
        public TransactionTypeEnum Type { get; set; }
        
        /// <summary>
        /// The amount of the Transaction
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
        public string Notes { get; set; }

        // TODO: Remove Corrected and Server Generated. Use Journal entry as source of truth?
        /// <summary>
        /// Did the user correct the transaction?
        /// </summary>
        public bool? Corrected { get; set; }

        /// <summary>
        /// Did the server create this?
        /// </summary>
        [Column("server_generated")]
        public bool? ServerGenerated { get; set; }

        /// <summary>
        /// The Id of the journal entry that the transaction belongs to.
        /// </summary>
        [Column("journal_entry_id")]
        public uint JournalEntryId { get; set; }

        /// <summary>
        /// The Journal Entry
        /// </summary>
        public JournalEntry JournalEntry { get; set; }
        
    }

}