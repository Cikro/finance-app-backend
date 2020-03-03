using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using finance_app.Types.EFModels;
using finance_app.Types.Interfaces;
using finance_app.Types.EFContexts;

namespace finance_app.Types.Services
{
    public class AccountServiceDbo: IAccountServiceDbo
    {
        private readonly AccountContext _context;
        public AccountServiceDbo(AccountContext context){
            _context = context;
        }

        private IQueryable<Account> GetQueryable() {
            return _context.Accounts.AsQueryable();

        }

        public async Task<IEnumerable<Account>> GetAllByUserId(uint userId) {
            return await GetQueryable().Where(i => i.Id == userId).ToListAsync();
        }

        public async Task CreateItem(Account account) {
            await _context.AddAsync(account);
        }

        public Account DeleteItem(int accountId) {
            _context.Remove(accountId);
            return new Account();

        }

        public void UpdateItem(Account account) {
            _context.Update(account);
        }

    }
}
