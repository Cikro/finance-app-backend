using finance_app.Types.Repositories.ApplicationAccounts;
using finance_app.Types.Repositories.FinanceApp;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.EntityFrameworkCore;

namespace FinanceApplicationUsers
{
    public class FinanceApplicationAccount
    {
        public uint Id { get; set; }
        public List<FinanceApplicationUser> Users { get; set; }

        private readonly FinanceAppContext _context;

        public FinanceApplicationAccount(FinanceAppContext context) 
        { 
            _context = context;
        }


        public async Task<FinanceApplicationUser> AddUser(FinanceApplicationUser authUser, uint applicationAccountId, List<uint> newUserRoles)
        {
            
            var applicationUser = await _context.ApplicationUsers
                .Where(x => x.Id == applicationAccountId)
                .FirstOrDefaultAsync();


            // If user does not exist, create Account For them, then create user
            // Else, Add user to existing account
            if (applicationUser == null)
            {

                var roles = await _context.ApplicationRoles.ToListAsync();
                var appRoles = roles.Where(x => newUserRoles.Contains((uint)x.Id))
                        .Select(y => new ApplicationUserRole
                        {
                            ApplicationRole = y,
                            ApplicationRoleId = (uint)y.Id
                        }).ToList();

                // Create Application User
                ApplicationUser newUser = new ApplicationUser()
                {
                    ApplicationUserRoles = newUserRoles.Select(x => new ApplicationUserRole { ApplicationRoleId = x }).ToList(),
                    DateCreated = DateTime.UtcNow,
                    AuthenticationUserId = (uint)authUser.AuthenticationUserId,

                };


                ApplicationAccount applicationAccount = new()
                {
                    DateCreated = DateTime.UtcNow,
                    ApplicationUsers = new List<ApplicationUser> { newUser },
                };

                await _context.ApplicationAccounts.AddAsync(applicationAccount);
            }
            else
            {
                applicationUser.ApplicationAccount.ApplicationUsers.Concat(new[] { applicationUser });
                _context.Entry(applicationUser.ApplicationAccount).Property(x => x.ApplicationUsers).IsModified = true;
                _context.ApplicationAccounts.Attach(applicationUser.ApplicationAccount);
            }


            await _context.SaveChangesAsync();

            return new FinanceApplicationUser(_context)
            {
                Roles = newUserRoles.ToList(),
                Id = (uint) applicationUser.Id,
                AuthenticationUserId = authUser.AuthenticationUserId

            };
            
        }
    }
}
