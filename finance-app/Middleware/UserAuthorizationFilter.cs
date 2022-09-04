using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.DataContracts.V1.Responses.ErrorResponses;
using finance_app.Types.DataContracts.V1.Responses.ReasonMessages;
using finance_app.Types.DataContracts.V1.Responses.ResourceMessages;
using finance_app.Types.DataContracts.V1.Responses.ResponseMessage;
using finance_app.Types.Models.ResourceIdentifiers;
using finance_app.Types.Repositories;
using finance_app.Types.Repositories.Account;
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
                                                        new Account { UserId = userResourceId.Id },
                                                        "CanAccessResourcePolicy")).Succeeded;
        
        if (!unauthorized) {
            await next();
        } else {
            var errorMessage = new UnauthorizedToAccessRouteByUserId(context?.HttpContext?.User, userResourceId);
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