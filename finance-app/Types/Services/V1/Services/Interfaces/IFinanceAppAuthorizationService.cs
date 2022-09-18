using System.Collections.Generic;
using System.Threading.Tasks;

namespace finance_app.Types.Services.V1.Interfaces
{
    public interface IFinanceAppAuthorizationService
    {

        /// <summary>
        /// Checks if a user meets the the policy requirements to access a resource
        /// </summary>
        /// <param name="resource">The resource to authorize</param>
        /// <param name="policy">The policy to authorize against</param>
        /// <returns>True if authorized for the resource</returns>
        public Task<bool> Authorize(object resource, string policy);

        /// <summary>
        /// Checks if a user meets the the policy requirements to access every resource in an enumerable
        /// </summary>
        /// <param name="resources">An enumerable of resources to authorize</param>
        /// <param name="policy">The policy to authorize against</param>
        /// <returns>True if the user is authorized for ALL resources</returns>
        public Task<bool> Authorize(IEnumerable<object> resources, string policy);


        /// <summary>
        /// Filters an enumerable of resources and returns an enumerable without 
        /// unauthorized resources.
        /// </summary>
        /// <param name="resources">An enumerable of resources to authorize</param>
        /// <param name="policy">The policy to authorize against</param>
        /// <returns>A list containing only authorized resources</returns>
        public Task<IEnumerable<T>> Filter<T>(IEnumerable<T> resources, string policy);

    }
}