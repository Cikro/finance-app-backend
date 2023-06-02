using Authentication.AuthenticationUsers;
using finance_app.Types.Repositories.FinanceApp;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinanceApplicationUsers
{
    public class FinanceApplicationUser
    {
        public uint Id { get; set; }
        public List<uint> Roles { get; set; }
        public uint AuthenticationUserId { get; set; }

        FinanceAppContext _context ;
        public FinanceApplicationUser(FinanceAppContext context)
        {
            _context = context;

        }

        public async Task<FinanceApplicationUser> GetUser(uint authUserId)
        {
            
            
        var applicationUser = await _context.ApplicationUsers
            .Where(x => x.Id == authUserId)
            .FirstOrDefaultAsync();

        if (applicationUser == null)
        {
            return null;
        }

        return new FinanceApplicationUser(_context)
        {
            AuthenticationUserId = authUserId,
            Id = (uint)applicationUser.Id,
            Roles = applicationUser.ApplicationUserRoles.Select(x => (uint)x.Id).ToList(),
        };
            

        }

        public List<Claim> GetClaims(IAuthenticationUser authenticationUser)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                new Claim(ClaimTypes.Email, authenticationUser.Email),
                new Claim(ClaimTypes.Name, authenticationUser.UserName),
            };

            claims.AddRange(Roles.Select(x => new Claim(ClaimTypes.Role, x.ToString())));

            return claims;

        }

    }
}
