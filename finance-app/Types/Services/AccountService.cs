using finance_app.Types.Interfaces;

namespace finance_app.Types.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountServiceDbo _accountServiceDbo;
        public AccountService(IAccountServiceDbo accountServiceDbo){
            _accountServiceDbo = accountServiceDbo;
        }

        public void GetAccounts(){

        }

        public void InsertAccounts(){

        }

        public void UpdateAccounts(){

        }
        public void DeleteAccounts(){

        }

    }
}
