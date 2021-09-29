using System.Collections.Generic;
using System.Threading.Tasks;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.Repositories.Account;

namespace finance_app.Types.Services.V1.Interfaces
{
    public interface IAccountService
    {
        Task<List<AccountDto>> GetAccounts(uint userId);
        Task<List<AccountDto>> GetPaginatedAccounts(uint userId, PaginationInfo pageInfo);
        Task<AccountDto> CreateAccount(Account account);
        void UpdateAccounts();
        void DeleteAccounts();
    }
}
