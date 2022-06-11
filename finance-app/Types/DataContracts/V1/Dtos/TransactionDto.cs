using System;
using finance_app.Types.DataContracts.V1.Dtos.Enums;
using finance_app.Types.Repositories.JournalEntry;

namespace finance_app.Types.DataContracts.V1.Dtos 
{
    public class TransactionDto : BaseDto 
    {

        /// <summary>
        /// The Id the transaction belongs to.
        /// </summary>
        public uint AccountId { get; set; }
        
        /// <summary>
        /// The type of transaction
        /// </summary>
        public EnumDto<TransactionTypeDtoEnum> Type { get; set; }
        
        /// <summary>
        /// The amount of the Transaction
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// The date of transaction. 
        /// NOTE: Different than when the transaction was added to the system
        /// </summary>
        public DateTime? TransactionDate { get; set; }

        /// <summary>
        /// Notes about the transaction
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Did the user correct the transaction?
        /// </summary>
        public bool Corrected { get; set; }

        /// <summary>
        /// Did the server create this?
        /// </summary>
        public bool ServerGenerated { get; set; }

        /// <summary>
        /// The Id of the journal entry that the transaction belongs to.
        /// </summary>
        /// <value></value>
        public uint JournalEntryId { get; set; }

        /// <summary>
        /// The Journal Entry
        /// </summary>
        public JournalEntry JournalEntry { get; set; }
    }
}