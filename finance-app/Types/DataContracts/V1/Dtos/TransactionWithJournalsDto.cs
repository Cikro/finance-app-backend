using System;
using finance_app.Types.DataContracts.V1.Dtos.Enums;
using finance_app.Types.Repositories.JournalEntry;

namespace finance_app.Types.DataContracts.V1.Dtos 
{
    /// <summary>
    /// A Transaction that contains a Journal Entry.
    /// I could not figure out how to avoid populating the Transaction's 
    /// `Journal Entry` property when selecting using EFCore. This means that 
    ///  there are circular references that end up causing JSON serialization issues.
    ///  System.Text.json JsonSerializerOptions.ReferenceHandler.Preserve wasn't fixing the issue,
    ///  Nor was Newtonsoft's JSON.
    /// .AddJsonOptions(o => {
    ///                   o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    ///                   o.JsonSerializerOptions.WriteIndented = true;
    ///             }); 
    /// </summary>
    public class TransactionWithJournalDto : TransactionDto
    {

        /// <summary>
        /// The Journal Entry for the transaction.
        /// </summary>
        public JournalEntry JournalEntry { get; set; }
    }
}