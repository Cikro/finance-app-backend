using Authentication.AuthenticationUsers;

namespace Authentication
{
    public interface IAuthenticationUserService
    {
        public Task<IAuthenticationUser> GetAuthencationUser(string username);
        public Task<IAuthenticationUser> NewAuthencationUser(string username);

    }
}
