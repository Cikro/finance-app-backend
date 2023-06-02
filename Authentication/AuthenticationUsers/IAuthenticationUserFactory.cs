using finance_app.Types.Repositories.Authentication;
using finance_app.Types.Services.V1.Services.Authentication;
using finance_app.Types.Services.V1.Services.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication.AuthenticationUsers
{
    public interface IAuthenticationUserFactory
    {
        public IAuthenticationUser CreateUser(byte[] passwordHash, byte[] passwordSalt);
        public IAuthenticationUser CreateUser(string username);
    }
}