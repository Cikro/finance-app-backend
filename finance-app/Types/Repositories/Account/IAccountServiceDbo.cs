using System.Collections.Generic;
using System.Threading.Tasks;
using finance_app.Types.Models;

namespace finance_app.Types.Repositories.Account
{
    public interface IAccountRepository
    {

        /// <summary>
        /// Fetches an account by the accountId
        /// </summary>
        /// <param name="accountId">The Id of the account</param>
        /// <returns></returns>
        Task<Account> GetAccountByAccountId(uint accountId);


        /// <summary>
        /// Fetches an account by name from the given user.
        /// </summary>
        /// <param name="userId">the user that owns the account</param>
        /// <param name="accountName">The name of the account</param>
        /// <returns></returns>
        Task<Account> GetAccountByAccountName(uint userId, string accountName);


        /// <summary>
        /// Fetches all Accounts that the user has access to 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<Account>> GetAllByUserId(uint userId);

        /// <summary>
        /// Fetches pages of Accounts that the user has access to 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageSize">The number of items for the page</param>
        /// <param name="offset">The page offset</param>
        /// <returns>A list of accounts the user has access to</returns>
        Task<IEnumerable<Account>> GetPaginatedByUserId(uint userId, uint pageSize, uint offset);

        /// <summary>
        /// Fetches a list of all children on a an account
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>A list of an account's children</returns>
        Task<IEnumerable<Account>> GetChildrenByAccountId(uint accountId);
        

        /// <summary>
        /// Adds a new account to the database.
        /// </summary>
        /// <param name="account">The account you want to add to the database</param>
        /// <returns>The created Account</returns>
        Task<Account> CreateAccount(Account account);

        /// <summary>
        /// Closes an account, and all of it's children.
        /// </summary>
        /// <param name="accountId">The Account you want to close</param>
        /// <returns>A list of the closed accounts</returns>
        Task<IEnumerable<Account>> CloseAccount(uint accountId);

        /// <summary>
        /// Updates an account with all values provided by the account
        /// </summary>
        /// <param name="account">The Account and its updated values</param>
        /// <returns>The updated account</returns>
        Task<Account> UpdateAccount(Account account);
    }
}
