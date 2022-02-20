using System.Threading.Tasks;
using finance_app.Types.Repositories;

namespace finance_app.Types.Services.V1.Interfaces
{
    public interface IUserAuthorizationService
    {
        /// <summary>
        /// Checks if a user can access data from the provided userId
        /// </summary>
        /// <param name="resourceId">The Id of the Resource to Access</param>
        /// <param name="userId">The Id of the user accessing the resource</param>
        /// <returns></returns>
        bool CanAccessResource(uint resourceId, uint userId);

        /// <summary>
        /// Checks if a user can access data from the provided userId
        /// </summary>
        /// <param name="resource">The resource you are trying to authorize</param>
        /// <param name="userId">The Id of the user accessing the resource</param>
        /// <returns></returns>
        bool CanAccessResource(DatabaseObject resource, uint userId);

    }
}
