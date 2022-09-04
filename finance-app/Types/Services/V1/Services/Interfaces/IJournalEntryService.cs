using System.Threading.Tasks;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.Models.ResourceIdentifiers;
using finance_app.Types.Repositories.JournalEntry;

namespace finance_app.Types.Services.V1.Interfaces 
{

    public interface IJournalEntryService 
    {
        /// <summary>
        /// Gets a Journal Entry.
        /// </summary><param name="journalEntryId">  Identifier for the Journal Entry</param>
        /// <returns>A JournalEntryDto</returns>
        Task<ApiResponse<JournalEntryDto>> Get(JournalEntryResourceIdentifier journalEntryId);

        /// <summary>
        /// Gets Recent Transactions
        /// </summary>
        /// <param name="userId">The user you want to fetch journal entries for.</param>
        /// <param name="pageInfo">The Page you want to fetch.</param>
        /// <returns>A List of recent Journal Entries</returns>
        Task<ApiResponse<ListResponse<JournalEntryDto>>> GetRecent(UserResourceIdentifier userId, PaginationInfo pageInfo);


        /// <summary>
        /// Creates a new Journal Entry
        /// </summary>
        /// <param name="journalEntry">A populated Journal Entry</param>
        /// <returns> A JournalEntryDto of the created Journal Entry</returns>
        Task<ApiResponse<JournalEntryDto>> Create(JournalEntry journalEntry);

        /// <summary>
        /// Corrects a Journal Entry by marking it as corrected, and creating a new Journal entry 
        /// with the new provided transactions, and transactions to undo the original journal 
        /// </summary>
        /// <param name="toCorrectId">
        /// the Id of the Journal Entry to correct
        /// </param>
        /// <param name="journalEntry">
        /// A Journal Entry with the id of the Journal you want corrected, 
        /// and the Transactions of the correct entry
        /// </param>
        /// <returns> A JournalEntryDto of the corrected Journal Entry</returns>
        Task<ApiResponse<JournalEntryDto>> Correct(JournalEntryResourceIdentifier toCorrectId, JournalEntry journalEntry);

    }
    

}