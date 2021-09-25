using System.Collections.Generic;
using System.Threading.Tasks;



namespace finance_app.Types.Repositories.Account
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Fetches All Accounts that belong to the associated user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<Account>> GetAllByUserId(uint userId);

        /// <summary>
        /// Fetches pages of Accounts that belong ot the associated user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageSize">The number of items for the page</param>
        /// <param name="offset">The page offset</param>
        /// <returns></returns>
        Task<List<Account>> GetPaginatedByUserId(uint userId, uint pageSize, uint offset);

        /// <summary>
        /// Adds a new account to the database.
        /// </summary>
        /// <param name="account">The account you want to add to the database</param>
        /// <returns></returns>
        Task<Account> CreateAccount(Account account);

        Account DeleteItem(int accountId);

        void UpdateItem(Account account);
    }
}
