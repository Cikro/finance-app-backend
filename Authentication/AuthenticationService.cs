using Authentication.AuthenticationUsers;
using Microsoft.EntityFrameworkCore;

using repositories = finance_app.Types.Repositories.Authentication;

namespace Authentication
{
    public class AuthenticationUserService : IAuthenticationUserService
    {

        private readonly IAuthenticationUserFactory _authenticationUserFactory;
        private readonly repositories.AuthenticationContext _context;

        public AuthenticationUserService(IAuthenticationUserFactory authenticationUserFactory, repositories.AuthenticationContext _authenticationContext)
        {
            _authenticationUserFactory = authenticationUserFactory;
            _context = _authenticationContext;
        }

        public async Task<IAuthenticationUser> GetAuthencationUser(string username)
        {
            var user = await _context.Users
                    .Include(x => x.AuthenticationUserInfo)
                    .Where(x => x.UserName == username)
                    .FirstOrDefaultAsync();

            if (user == null) { return null;  }

            return _authenticationUserFactory.CreateUser(user.PasswordHash, user.PasswordSalt);

        }

        public async Task<IAuthenticationUser> NewAuthencationUser(string username)
        {
            return _authenticationUserFactory.CreateUser(username);

        }

    }
}
