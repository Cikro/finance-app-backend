using System.Collections.Generic;
using System.Threading.Tasks;
using finance_app.Types.EFModels;



namespace finance_app.Types.Interfaces
{
    public interface IAccountServiceDbo
    {
        /// <summary>
        /// Fetches All Accounts that belong to the associated user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<Account> GetAllByUserId(uint userId) ;

        /// <summary>
        /// Fetches pages of Accounts that belong ot the associated user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageSize">The number of items for the page</param>
        /// <param name="offset">The page offset</param>
        /// <returns></returns>
        IEnumerable<Account> GetPaginatedByUserId(uint userId, uint pageSize, uint offset);


        Task CreateItem(Account account);

        Account DeleteItem(int accountId);

        void UpdateItem(Account account);
    }
}
