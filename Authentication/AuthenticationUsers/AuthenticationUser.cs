using finance_app.Types.Repositories.Authentication;
using finance_app.Types.Services.V1.Services.Authentication;
using finance_app.Types.Services.V1.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using repository = finance_app.Types.Repositories.Authentication;

namespace Authentication.AuthenticationUsers
{
    public class AuthenticationUser : IAuthenticationUser
    {

        private readonly IPasswordService _passwordService = new PasswordService();
        private AuthenticationContext _context;

        private byte[] PasswordHash { get; set; }
        private byte[] PasswordSalt { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public uint AuthenticationUserId { get; set; }


        public AuthenticationUser(IPasswordService passwordService, AuthenticationContext context, string username)
        {
            _passwordService = passwordService ?? throw new ArgumentNullException(nameof(passwordService));
            _context = context;
            UserName = username;
        }

        public AuthenticationUser(IPasswordService passwordService, AuthenticationContext context, byte[] passwordHash, byte[] passwordSalt, uint AuthencationUserId)
        {
            _passwordService = passwordService ?? throw new ArgumentNullException(nameof(passwordService));
            _context = context;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }


        public bool Authenticate(string password)
        {

            if (!VerifyPassword(password))
            {

                return false;
            }

            return true;


        }

        public async Task Create(string password)
        {
            var salt = _passwordService.CreateSalt();
            var passwordHash = _passwordService.HashPassword(password, salt);
            await _context.Users.AddAsync(new repository.AuthenticationUser
            {
                PasswordHash = passwordHash,
                UserName = UserName,
                PasswordSalt = PasswordSalt,
                AuthenticationUserInfo = new()
                {
                    Email = Email
                }
            });
        }


        private bool VerifyPassword(string password)
        {
            return _passwordService.VerifyHash(password, PasswordSalt, PasswordHash);
        }
    }
}
