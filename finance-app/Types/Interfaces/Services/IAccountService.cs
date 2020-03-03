using finance_app.Types.EFModels;

namespace finance_app.Types.Interfaces
{
    public interface IAccountService
    {
        void GetAccounts();

        void InsertAccounts();

        void UpdateAccounts();
        void DeleteAccounts();
    }
}
