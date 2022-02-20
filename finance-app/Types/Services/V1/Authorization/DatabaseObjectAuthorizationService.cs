using System.Linq;
using System.Threading.Tasks;
using finance_app.Types.Repositories;
using Microsoft.AspNetCore.Authorization;


//https://docs.microsoft.com/en-us/aspnet/core/security/authorization/resourcebased?view=aspnetcore-3.1
namespace finance_app.Types.Services.V1.Authorization
{
    public class DatabaseObjectAuthorizationService : 
        AuthorizationHandler<SameUserIdRequirement, DatabaseObject>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                    SameUserIdRequirement requirement,
                                                    DatabaseObject resource)
        {
            var x = context.User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (!int.TryParse(context.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value, out var userId)) 
            {
                context.Fail();

            };

            if (userId == resource.Id)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
    public class SameUserIdRequirement : IAuthorizationRequirement { }
}