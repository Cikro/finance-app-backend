using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.Models.ResourceIdentifiers;
using finance_app.Types.Repositories;
using finance_app.Types.Repositories.Accounts;
using finance_app.Types.Services.V1.Authorization;
using finance_app.Types.Services.V1.Interfaces;
using finance_app.Types.Services.V1.ResponseMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

/// <summary>
/// A filter attribute to apply to controller routes. Authorizes that the User can 
/// call the route.
/// </summary>
public class UserAuthorizationFilter : Attribute, IAsyncActionFilter  {

    public async Task OnActionExecutionAsync(ActionExecutingContext context,
                                                ActionExecutionDelegate next) 
    {

        // Get Authorization Service
        var authorizationService = (IFinanceAppAuthorizationService)context.HttpContext
                    .RequestServices.GetService(typeof(IFinanceAppAuthorizationService));
                    
        // Get UserResourceId From Route
        var userResourceId = (UserResourceIdentifier)context.ActionArguments.Values
                .FirstOrDefault(arg => arg is UserResourceIdentifier);
        

        // If there is no userId in the route, the user is authorized to access it.
        var authorized = userResourceId == null;

        // Check If User is authorized
        if (!authorized) 
        {
            authorized = await authorizationService.Authorize(
                                    new Account { UserId = userResourceId.Id },
                                    AuthorizationPolicies.CanAccessResource);
        }
        
        // If Authorized, Success. Otherwise, error
        if (authorized) {
            await next();
        } else {
            
            var errorMessage = new UnauthorizedToAccessRouteByUserIdMessage(context?.HttpContext?.User, userResourceId);
            var response = new ApiResponse<string>(ApiResponseCodesEnum.Unauthorized, errorMessage);

            var mapper = (IMapper)context.HttpContext
                    .RequestServices.GetService(typeof(IMapper));

            context.Result = new JsonResult(response)
            {
                StatusCode = mapper.Map<int>(response.ResponseCode)
            };
        }

        
    }
}