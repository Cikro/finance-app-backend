using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using finance_app.Types.Services.V1.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;

namespace finance_app.Types.Services.V1.Authorization
{
    public class FinanceAppAuthorizationService : IFinanceAppAuthorizationService {

        private readonly IHttpContextAccessor _context;
        private readonly IAuthorizationService _authorizationService;

        public FinanceAppAuthorizationService(IHttpContextAccessor context,
                                              IAuthorizationService authorizationService) {
            _context = context;
            _authorizationService = authorizationService;
        }

        /// <inheritdoc cref="IFinanceAppAuthorizationService.Authorize"/>
        public async Task<bool> Authorize(object resource, string policy) {

            var authorized = (await _authorizationService.AuthorizeAsync(_context.HttpContext.User,
                           resource, policy)).Succeeded;
            return authorized;
        }

        /// <inheritdoc cref="IFinanceAppAuthorizationService.AuthorizeEnumerable"/>
        public async Task<bool> AuthorizeEnumerable(IEnumerable<object> resources, string policy) {

            // I don't really like how this looks. I'm awaiting each call one by one, 
            // rather than kicking all of the async calls off at once. 
            // I can't figure out how to do that while cancelling the 
            // calls if one were to return back false. (Task.WhenAll works for the waiting, butI don't think i can cancel the task since I can't
            // pass a cancellation token into the AuthorizeAsync Method, Nor can i use Linq.IEnumerable.Any() with an async lambda function)
            bool allAuthorized = true;
            foreach(var resource in resources) 
            {
                var authorized = (await _authorizationService.AuthorizeAsync(_context.HttpContext.User, resource,
                         policy )).Succeeded;
                if(!authorized) 
                {
                    allAuthorized = false;
                    continue;
                }
            }

            return allAuthorized;
        }

        /// <inheritdoc cref="IFinanceAppAuthorizationService.FilterEnumerable"/>
        public async Task<IEnumerable<T>> FilterEnumerable<T>(IEnumerable<T> resources, string policy) {
            if (resources?.Count() == 0) { return resources; }

            var authorizedList = (await Task.WhenAll(resources?.Select(async (r) => {
                    return new ResourceWithAccess<T> {
                        Resource = r,
                        HasAccess = (await _authorizationService.AuthorizeAsync(_context.HttpContext.User, r, policy )).Succeeded
                    };
                    })
                ))
                ?.Where(r => r.HasAccess == true)
                ?.Select(r => r.Resource);
            return authorizedList;
        }

        private class ResourceWithAccess<T> {
            public T Resource;
            public bool HasAccess;
        }
    }
}