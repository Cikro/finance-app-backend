using finance_app.Types.Repositories.Authentication;
using finance_app.Types.Services.V1.Services.Authentication;
using finance_app.Types.Services.V1.Services.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
            return new AuthenticationUser(_passwordService, _context, username);
        }
    }
}