using System.Collections.Generic;
using finance_app.Types.DataContracts.V1.Dtos;

namespace finance_app.Types.Services.V1.Interfaces
{
    public interface IAccountService
    {
        List<AccountDto> GetAccounts(uint userId);
        List<AccountDto> GetPaginatedAccounts(uint userId, PaginationInfo pageInfo);

        void InsertAccount();

        void UpdateAccounts();
        void DeleteAccounts();
    }
}
