using System.Collections.Generic;
using System.Threading.Tasks;
using finance_app.Types.EFModels;



namespace finance_app.Types.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Checks if a user can access data from the provided userId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userIdToAccess"></param>
        /// <returns></returns>
        Task<bool> CanAccessUser(uint userId, uint userIdToAccess);

    }
}
