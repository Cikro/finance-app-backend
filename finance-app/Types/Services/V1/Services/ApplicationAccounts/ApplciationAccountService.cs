using AutoMapper;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Requests.ApplicationAccounts;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.Repositories.ApplicationAccounts;
using finance_app.Types.Repositories.Authentication;
using finance_app.Types.Repositories.FinanceApp;
using finance_app.Types.Services.V1.Interfaces;
using finance_app.Types.Services.V1.ResponseMessages;
using finance_app.Types.Services.V1.ResponseMessages.ActionMessages;
using finance_app.Types.Services.V1.ResponseMessages.ReasonMessages;
using finance_app.Types.Services.V1.ResponseMessages.ResourcesMessages;
using finance_app.Types.Services.V1.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finance_app.Types.Services.V1 {
    public class ApplciationAccountService : IApplciationAccountService {


        private readonly AuthenticationContext _authenticationDbContext;
        private readonly FinanceAppContext _financeAppDbContext;
        private readonly IMapper _mapper;



        public ApplciationAccountService(
                    AuthenticationContext authenticationDbContext,
                    IPasswordService passwordService,
                    Microsoft.AspNetCore.Authentication.IAuthenticationService microsfotAuthService,
                    FinanceAppContext financeAppDbContext,
                    IMapper mapper) {

            _authenticationDbContext = authenticationDbContext;
            _financeAppDbContext = financeAppDbContext;
            _mapper = mapper;
        }


        public async Task<ApiResponse<AuthenticationUserDto>> CreateApplicationUser(CreateApplicationAccountRequest request) {

            var authUser = await _authenticationDbContext.Users
                .Where(x => x.Id == request.AuthorizationUserId)
                .FirstOrDefaultAsync();


            if (authUser == null) {
                var errorMessage = new ErrorResponseMessage(
                        new CreatingActionMessage(request),
                        new ResourceWithPropertyMessage(authUser, "Username", authUser.UserName),
                        new NotFoundReason());
                return new ApiResponse<AuthenticationUserDto>(ApiResponseCodesEnum.DuplicateResource, errorMessage);
            }

            var applicationUser = await _financeAppDbContext.ApplicationUsers
                .Where(x => x.Id == request.ApplicationAccountId)
                .FirstOrDefaultAsync();

           
            // If user does not exist, create Account For them, then create user
            // Else, Add user to existing account
            if (applicationUser == null) {

                var roles = await _financeAppDbContext.ApplicationRoles.ToListAsync();
                var appRoles = roles.Where(x => request.Roles.Contains((uint)x.Id))
                        .Select(y => new ApplicationUserRole {
                            ApplicationRole= y,
                            ApplicationRoleId= (uint) y.Id
                        }).ToList();

                // Create Application User
                ApplicationUser newUser = new ApplicationUser() 
                { 
                    ApplicationUserRoles = request.Roles.Select(x => new ApplicationUserRole { ApplicationRoleId = x }).ToList(),
                    DateCreated = DateTime.UtcNow,
                    AuthenticationUserId = (uint) authUser.Id,
                    
                };



                ApplicationAccount applicationAccount = new() {
                    DateCreated = DateTime.UtcNow,
                    ApplicationUsers = new List<ApplicationUser>{ newUser },
                };

                await _financeAppDbContext.ApplicationAccounts.AddAsync(applicationAccount);
            } 
            else 
            {
                applicationUser.ApplicationAccount.ApplicationUsers.Concat( new[] { applicationUser });
                _financeAppDbContext.Entry(applicationUser.ApplicationAccount).Property(x => x.ApplicationUsers).IsModified = true;
                _financeAppDbContext.ApplicationAccounts.Attach(applicationUser.ApplicationAccount);
            }


            await _financeAppDbContext.SaveChangesAsync();

            var ret = new ApiResponse<AuthenticationUserDto>(_mapper.Map<AuthenticationUserDto>(authUser));
            return ret;

        }

    }
}
