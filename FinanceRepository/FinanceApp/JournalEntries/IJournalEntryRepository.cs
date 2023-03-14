using System.Collections.Generic;
using System.Threading.Tasks;

namespace finance_app.Types.Repositories.JournalEntries
{
    public interface IJournalEntryRepository 
    {
        /// <summary>
        /// Fetches one JournalEntry by JournalEntryId from the database
        /// </summary>
        /// <param name="journalEntryId">The Id of the journalEntry you are fetching.</param>
        /// <returns>The transaction you wanted to fetch</returns>
        public Task<JournalEntry> GetJournalEntry(uint journalEntryId);

        /// <summary>
        /// Fetches recent Transactions that occurred on given Account.
        /// </summary>
        /// <param name="userId">The ID of the user you want journalEntries on.</param>
        /// <param name="pageSize">The number of Items per page you want journalEntries on.</param>
        /// <param name="offset">The page offset.</param>
        /// <returns>A list of recent journalEntries that the user has made</returns>
        public Task<IEnumerable<JournalEntry>> GetRecentJournalEntriesByUserId(uint userId,  int pageSize, int offset);


        /// <summary>
        /// Updates a given journalEntry with the properties on the journalEntry
        /// </summary>
        /// <param name="journalEntry">The journalEntry you want to save to the DB.</param>
        /// <returns>The created journal entry</returns>
        public Task<JournalEntry> CreateJournalEntry(JournalEntry journalEntry);

        /// <summary>
        /// Updates a given journalEntry with the properties on the journalEntry
        /// </summary>
        /// <param name="journalEntry">The journalEntry you want to save to the DB.</param>
        /// <returns>The updated journal entry</returns>
        public Task<JournalEntry> UpdateJournalEntry(JournalEntry journalEntry);
    }
}