using System.Threading.Tasks;

namespace finance_app.Types.Services.V1.Interfaces
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
