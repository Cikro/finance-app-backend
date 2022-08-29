using System.Collections.Generic;
using System.Threading.Tasks;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Requests.Accounts;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.Models.ResourceIdentifiers;
using finance_app.Types.Repositories.Account;

namespace finance_app.Types.Services.V1.Interfaces
{
    public interface IAccountService
    {

        /// <summary>
        /// Gets a list of all accounts that a user has access to 
        /// </summary><param name="userId">  Identifier for the user who's accounts you are fetching</param>
        /// <returns> A list of AccountDtos</returns>
        Task<ApiResponse<ListResponse<AccountDto>>> GetAccounts(UserResourceIdentifier userId);

        /// <summary>
        /// Gets an Account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>An AccountDto</returns>
        Task<ApiResponse<AccountDto>> GetAccount(AccountResourceIdentifier accountId);

        /// <summary>
        /// Gets all Children on an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>A list of children on an account</returns>
        Task<ApiResponse<ListResponse<AccountDto>>> GetChildren(AccountResourceIdentifier accountId);


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
        /// <param name="account">A populated account object</param>
        /// <returns> An AccountDto of the created account</returns>
        Task<ApiResponse2<AccountDto>> CreateAccount(Account account);

        /// <summary>
        /// Updates an account with values from an existing account
        /// </summary>
        /// <param name="account">A populated account object</param>
        /// <returns> An AccountDto of the updated account</returns>
        Task<ApiResponse2<AccountDto>> UpdateAccount(Account account);

        /// <summary>
        /// Closes an account that has the provided Id.
        /// </summary>
        /// <param name="accountId">An Identifier for the account you are closing</param>
        /// <returns>AccountDtos of the closed accounts</returns>
        Task<ApiResponse2<ListResponse<AccountDto>>> CloseAccount(AccountResourceIdentifier accountId);
        
    }
}
