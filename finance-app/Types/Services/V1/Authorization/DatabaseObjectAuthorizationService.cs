using System.Linq;
using System.Threading.Tasks;
using finance_app.Types.Repositories;
using Microsoft.AspNetCore.Authorization;


//https://docs.microsoft.com/en-us/aspnet/core/security/authorization/resourcebased?view=aspnetcore-3.1
namespace finance_app.Types.Services.V1.Authorization
{
    public class DatabaseObjectAuthorizationHandler : 
        AuthorizationHandler<UserOwnsResource, IUserIdResource>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                    UserOwnsResource requirement,
                                                   IUserIdResource resource)
        {
            if (!int.TryParse(context.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value, out var userId)) 
            {
                context.Fail();

            };

            if (userId == resource.User_Id)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
    public class UserOwnsResource : IAuthorizationRequirement { }
}