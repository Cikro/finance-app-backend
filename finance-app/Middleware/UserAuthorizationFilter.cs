using System;
using System.Net;
using System.Threading.Tasks;
using finance_app.Types;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.Services.V1.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class UserAuthorizationFilter : IAsyncActionFilter {

    public async Task OnActionExecutionAsync(ActionExecutingContext context,
                                                ActionExecutionDelegate next) 
    {
        uint userId = 0;
        bool unauthorized = true;

        // No userId Provided. Continue.
        // TODO: Consider moving this whole class into an attribute to only run on routes 
        // that require Authorization.  Then, return error if there is no userId.
        if (context.RouteData.Values.TryGetValue("userId", out var userIdParam) 
            && userIdParam is string
            && uint.TryParse((string)userIdParam, out userId)) {

            IUserService userService = (IUserService)context.HttpContext
                     .RequestServices.GetService(typeof(IUserService));


            unauthorized = !await userService.CanAccessUser(1, userId);
        }
        
        if (!unauthorized) {
            await next();
        } else {
            var response = new ApiResponse<string>
            {
                ResponseCode = ApiResponseCodesEnum.BadRequest,
                StatusCode = HttpStatusCode.Unauthorized,
                ResponseMessage = $"Unauthorized: You are not authorized to access user {userId}",
                Data = $"You are not authorized to access user {userId}"
            };
            context.Result = new JsonResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        
    }
}