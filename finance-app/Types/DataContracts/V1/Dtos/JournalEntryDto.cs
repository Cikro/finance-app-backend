using System;
using System.Collections.Generic;
using System.Security;
using finance_app.Types.DataContracts.V1.Dtos.Enums;

namespace finance_app.Types.DataContracts.V1.Dtos
{
    public class JournalEntryDto : BaseDto
    {
        /// <summary>
        /// The user that created the Journal Entry
        /// </summary>
        public uint UserId { get; set; }

        /// <summary>
        /// The amount of the Journal Entry
        /// (The Sum of the Debits OR the Sum of the Credits. Dr == Cr)
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Has the Journal Entry been corrected?
        /// </summary>
        public bool Corrected { get; set; }

        /// <summary>
        /// Was the Journal Entry been server generated?
        /// </summary>
        public bool ServerGenerated { get; set; }

        /// <summary>
        /// The Transactions the make up the Journal Entry
        /// </summary>
        /// <value></value>
        public List<TransactionDto> Transaction { get; set; }
    }
}
