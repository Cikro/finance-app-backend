using System.Collections.Generic;
using System.Threading.Tasks;
using finance_app.Types.EFModels;

namespace finance_app.Types.Interfaces
{
    public interface IAccountService
    {
        IEnumerable<Account> GetAccounts(uint userId);
        IEnumerable<Account> GetPaginatedAccounts(uint userId, int itemsPerPage, int pageNumber);

        void InsertAccounts();

        void UpdateAccounts();
        void DeleteAccounts();
    }
}
