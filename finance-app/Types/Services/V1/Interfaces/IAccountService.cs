using System.Collections.Generic;
using System.Threading.Tasks;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Requests.Accounts;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.Models;
using finance_app.Types.Repositories.Account;

namespace finance_app.Types.Services.V1.Interfaces
{
    public interface IAccountService
    {

        /// <summary>
        /// Gets a list of all accounts that a user has access to 
        /// </summary>
        /// <param name="userId">An Identifier for the user who's accounts you are fetching</param>
        /// <returns> A list of AccountDtos</returns>
        Task<ApiResponse<ListResponse<AccountDto>>> GetAccounts(UserResourceIdentifier userId);

        /// <summary>
        /// Gets a list of paginated accounts that a user has access to 
        /// </summary>
        /// <param name="userId">An Identifier for the user who's accounts you are fetching</param>
        /// <param name="pageInfo">An object represent information for paging</param>
        /// <returns> A list of AccountDtos</returns>
        Task<ApiResponse<ListResponse<AccountDto>>> GetPaginatedAccounts(UserResourceIdentifier userId, PaginationInfo pageInfo);

        /// <summary>
        /// Creates an account in the database
        /// </summary>
        /// <param name="account">A popualted account object</param>
        /// <returns> A list of AccountDtos</returns>
        Task<ApiResponse<AccountDto>> CreateAccount(Account account);
        void UpdateAccounts();

        /// <summary>
        /// Closes an account that has the provided Id.
        /// </summary>
        /// <param name="accountId">An Identifier for the account you are closing</param>
        /// <returns> A list of AccountDtos</returns>
        Task<ApiResponse<ListResponse<AccountDto>>> CloseAccount(AccountResourceIdentifier accountId);
        
    }
}
