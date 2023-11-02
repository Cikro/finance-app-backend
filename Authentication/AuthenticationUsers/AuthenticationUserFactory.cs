using finance_app.Types.Repositories.Authentication;
using finance_app.Types.Services.V1.Services.Interfaces;
using repository = finance_app.Types.Repositories.Authentication;

namespace Authentication.AuthenticationUsers
{
    public class AuthenticationUserFactory : IAuthenticationUserFactory
    {
        private readonly IPasswordService _passwordService;
        private readonly AuthenticationContext _context;

        public AuthenticationUserFactory(IPasswordService passwordService, AuthenticationContext context)
        {
            _passwordService = passwordService;
            _context = context;
        }

        public IAuthenticationUser CreateUser(byte[] passwordHash, byte[] passwordSalt)
        {
            return new AuthenticationUser(_passwordService, _context, passwordHash, passwordSalt);
        }

        
        public IAuthenticationUser CreateUser(string username)
        {
            var user = new AuthenticationUser(_passwordService, _context);
            return user;
        }

    }
}