using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.Repositories.Authentication;
using finance_app.Types.Services.V1.ResponseMessages.ActionMessages;
using finance_app.Types.Services.V1.ResponseMessages.ReasonMessages;
using finance_app.Types.Services.V1.ResponseMessages.ResourcesMessages;
using finance_app.Types.Services.V1.ResponseMessages;
using finance_app.Types.Services.V1.Services.Authentication;
using finance_app.Types.Services.V1.Services.Interfaces;
using repository = finance_app.Types.Repositories.Authentication;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using finance_app.Types.DataContracts.V1.Requests.Authentication;

namespace Authentication
{
    public class AuthenticationUser
    {

        private IPasswordService _passwordService = new PasswordService();

        public string UserName { get; set;}
        
        public string Email { get; set; }

        public uint AuthenticationUserId { get; set; }


        public bool Authenticate(string password)
        {
            return Authenticate(UserName, password);
        }

        public bool Authenticate(string username, string password)
        {
            using (var context = new repository.AuthenticationContext())
            {
                var existingUser = context.Users
                     .Include(x => x.AuthenticationUserInfo)
                     .Where(x => x.UserName == username)
                     .FirstOrDefault();

                if (existingUser == null)
                {
                    // TODO: Throw Exception here?
                    return false;
                }


                if (!VerifyPassword(existingUser, password))
                {

                    return false;
                }

                return true;
            }


        }

        public async Task<AuthenticationUser> CreateAsync(AuthenticationUser userToCreate, string password) 
        {
            using (var context = new repository.AuthenticationContext())
            {
                var authUser = await context.Users
                    .Where(x => x.UserName == userToCreate.UserName)
                    .FirstOrDefaultAsync();



                if (authUser != null)
                {
                    return null;
                }

                var salt = _passwordService.CreateSalt();

                repository.AuthenticationUser newUser = new()
                {
                    UserName = userToCreate.UserName,
                    DateCreated = DateTime.UtcNow,
                    PasswordSalt = salt,
                    PasswordHash = _passwordService.HashPassword(password, salt),
                    AuthenticationUserInfo = new AuthenticationUserInfo
                    {
                        Email = userToCreate.Email,
                    }
                };

                await context.Users.AddAsync(newUser);
                await context.SaveChangesAsync();

                
                return new AuthenticationUser
                {
                    AuthenticationUserId = (uint) newUser.Id,
                    Email= userToCreate.Email,
                    UserName= userToCreate.UserName
                };
            }
        }


        private bool VerifyPassword(repository.AuthenticationUser user ,string password)
        {
            return _passwordService.VerifyHash(password, user.PasswordSalt, user.PasswordHash);
        }

    }
}
