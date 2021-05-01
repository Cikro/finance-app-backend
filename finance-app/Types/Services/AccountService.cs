using System.Collections.Generic;
using System.Threading.Tasks;
using finance_app.Types.Interfaces;
using finance_app.Types.EFModels;

namespace finance_app.Types.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountServiceDbo _accountServiceDbo;
        public AccountService(IAccountServiceDbo accountServiceDbo){
            _accountServiceDbo = accountServiceDbo;
        }

        public IEnumerable<Account> GetAccounts(uint userId){
            return _accountServiceDbo.GetAllByUserId(userId);
        }
        public IEnumerable<Account> GetPaginatedAccounts(uint userId, int itemsPerPage, int pageNumber)
        {
            if (pageNumber <= 0)
            {
                return null;
            }
            if (itemsPerPage < 0)
            {
                return null;
            }
            uint offset = (uint) pageNumber - 1;
            
            return _accountServiceDbo.GetPaginatedByUserId(userId, (uint) itemsPerPage, offset);
        }

        public void InsertAccounts(){

        }

        public void UpdateAccounts(){

        }
        public void DeleteAccounts(){

        }

    }
}
