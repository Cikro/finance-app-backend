using finance_app.Types.EFModels;

namespace finance_app.Types.Interfaces
{
    interface IAccountService
    {
        Account CreateAccount();
        Account GetAccount(int id);
        Account DeleteAccount();
        Account UpdateAccount();
    }
}
