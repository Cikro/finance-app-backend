    using System;
    using finance_app.Types.DataContracts.V1.Dtos;
    using finance_app.Types.DataContracts.V1.Dtos.Enums;
    
    public class TransactionForJournalEntryRequests : BaseRequestObject
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

    }