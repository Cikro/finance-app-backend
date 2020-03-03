using System.Collections.Generic;
using System.Web.Mvc;
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

        public async Task<IEnumerable<Account>> GetAccounts(uint userId){
            return await _accountServiceDbo.GetAllByUserId(userId);

        }

        public void InsertAccounts(){

        }

        public void UpdateAccounts(){

        }
        public void DeleteAccounts(){

        }

    }
}
