using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using finance_app.Types.Repositories.FinanceApp;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace finance_app.Types.Repositories.JournalEntries {
    public class JournalEntryRepository : IJournalEntryRepository {
        
        private readonly FinanceAppContext _context;

        public JournalEntryRepository(FinanceAppContext context) {
            _context = context;
        }



        /// <inheritdoc cref="IJournalEntryRepository.GetJournalEntry"/>
        public async Task<JournalEntry> GetJournalEntry(uint journalEntryId) {
            return await _context.JournalEntries
                        .Where(j => j.Id == journalEntryId)
                        .FirstOrDefaultAsync();
        }


        /// <inheritdoc cref="IJournalEntryRepository.GetRecentJournalEntriesByUserId"/>
        public async Task<IEnumerable<JournalEntry>> GetRecentJournalEntriesByUserId(uint userId, int pageSize, int offset) {
            var journalEntries =  await _context.JournalEntries
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.DateCreated)
                .Skip(offset * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
            return journalEntries;
        }

        /// <inheritdoc cref="IJournalEntryRepository.CreateJournalEntry"/>
        public async Task<JournalEntry> CreateJournalEntry(JournalEntry journalEntry) {
            journalEntry.DateCreated = DateTime.Now;
            journalEntry.Transactions.Select(t => t.DateCreated = DateTime.Now);

           _context.JournalEntries.Add(journalEntry);
           await _context.SaveChangesAsync();
           return journalEntry;

        }

        /// <inheritdoc cref="IJournalEntryRepository.UpdateJournalEntry"/>
        public async Task<JournalEntry> UpdateJournalEntry(JournalEntry journalEntry) {
            journalEntry.DateLastEdited = DateTime.Now;
            journalEntry.Transactions.Select(t => t.DateCreated = DateTime.Now);

           _context.JournalEntries.Add(journalEntry);
           await _context.SaveChangesAsync();
           return journalEntry;
        }
    }
}