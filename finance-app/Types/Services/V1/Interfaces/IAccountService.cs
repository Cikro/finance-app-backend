using System.Collections.Generic;
using System.Threading.Tasks;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.Models;
using finance_app.Types.Repositories.Account;

namespace finance_app.Types.Services.V1.Interfaces
{
    public interface IAccountService
    {
        Task<ApiResponse<ListResponse<AccountDto>>> GetAccounts(UserResourceIdentifier userId);
        Task<ApiResponse<ListResponse<AccountDto>>> GetPaginatedAccounts(UserResourceIdentifier userId, PaginationInfo pageInfo);
        Task<ApiResponse<AccountDto>> CreateAccount(Account account);
        void UpdateAccounts();
        void DeleteAccounts();
    }
}
