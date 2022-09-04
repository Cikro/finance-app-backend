using System.Linq;
using System.Security.Claims;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.Models.ResourceIdentifiers;

namespace finance_app.Types.Services.V1.ResponseMessages {
    public class UnauthorizedToAccessRouteByUserId : IResponseMessage {
        private readonly string _message;
        /// <summary>
        /// A message for when a user tries to access a user who's id they do not have access to.
        /// </summary>
        /// <param name="user">The User trying to access the rout</param>
        /// <param name="attemptedUser">The user who's data you are trying to access</param>
        public UnauthorizedToAccessRouteByUserId(ClaimsPrincipal user, UserResourceIdentifier attemptedUser) {
            var userIdFromClaims = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            _message = $"As userId {userIdFromClaims},  you are not authorized to access data belonging to user with Id {attemptedUser?.Id}";
        }

        public string GetMessage() {
            return _message;
        }
    }
}