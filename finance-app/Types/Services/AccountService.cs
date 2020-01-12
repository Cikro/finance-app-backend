using finance_app.Types.EFModels;
using finance_app.Types.Interfaces;

namespace finance_app.Types.Services
{
    public class AccountService : ICrudService<Account>
    {
        public AccountService(){}

        public Account CreateItems(Account account) {
            return new Account();
            
        }

        public Account GetItems(Account account) {
            // Add options parameter
            // 
            return new Account();

        }

        public Account DeleteItems(int accountId) {
            return new Account();

        }

        public Account UpdateItems(Account account) {
            return new Account();

        }

    }
}
