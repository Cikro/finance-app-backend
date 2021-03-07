using System.Collections.Generic;
using System.Threading.Tasks;
using finance_app.Types.EFModels;



namespace finance_app.Types.Interfaces
{
    public interface IAccountServiceDbo
    {
        IEnumerable<Account> GetAllByUserId(uint userId) ;
        
        Task CreateItem(Account account);

        Account DeleteItem(int accountId);

        void UpdateItem(Account account);
    }
}
