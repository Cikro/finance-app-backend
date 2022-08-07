using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using finance_app.Types.Configurations;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

public class LocalAuthenticationFilter : Attribute, IAsyncActionFilter  
{

    public async Task OnActionExecutionAsync(ActionExecutingContext context,
                                                ActionExecutionDelegate next) 
    {
        var user = ((IOptions<LocalUserOptions>) context.HttpContext
                    .RequestServices.GetService(typeof(IOptions<LocalUserOptions>))).Value;
        
         context.HttpContext.User.Identities.FirstOrDefault().AddClaim(new Claim("UserId", user.UserId.ToString()));

   
        await next();

    }
}