using System.Collections.Generic;
using finance_app.Types.Responses.Dtos;

namespace finance_app.Types.Interfaces
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
