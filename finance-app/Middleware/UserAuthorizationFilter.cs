using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.Models.ResourceIdentifiers;
using finance_app.Types.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class UserAuthorizationFilter : Attribute, IAsyncActionFilter  {

    public async Task OnActionExecutionAsync(ActionExecutingContext context,
                                                ActionExecutionDelegate next) 
    {

        var authorizationService = (IAuthorizationService)context.HttpContext
                    .RequestServices.GetService(typeof(IAuthorizationService));
        var userResourceId = (UserResourceIdentifier)context.ActionArguments.Values
                .FirstOrDefault(arg => arg is UserResourceIdentifier);
        

        var unauthorized = true;

        unauthorized = userResourceId != null && !(await authorizationService.AuthorizeAsync(
                                                        context.HttpContext.User,
                                                        new DatabaseObject { Id = userResourceId.Id },
                                                        "CanAccessResourcePolicy")).Succeeded;
        
        if (!unauthorized) {
            await next();
        } else {
            var response = new ApiResponse<string>
            {
                ResponseCode = ApiResponseCodesEnum.BadRequest,
                StatusCode = HttpStatusCode.Unauthorized,
                ResponseMessage = $"Unauthorized: You are not authorized to access user with Id {userResourceId?.Id}",
                Data = $"You are not authorized to access user with Id {userResourceId?.Id}"
            };
            context.Result = new JsonResult(response)
            {
                StatusCode = (int) response.StatusCode
            };
        }

        
    }
}