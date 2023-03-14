using AutoMapper;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Requests.Authentication;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.Repositories.Authentication;
using finance_app.Types.Services.V1.Interfaces;
using finance_app.Types.Services.V1.ResponseMessages;
using finance_app.Types.Services.V1.ResponseMessages.ActionMessages;
using finance_app.Types.Services.V1.ResponseMessages.ReasonMessages;
using finance_app.Types.Services.V1.ResponseMessages.ResourcesMessages;
using finance_app.Types.Services.V1.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace finance_app.Types.Services.V1
{
    public class  AuthenticationService: IAuthenticationService {


        private readonly AuthenticationContext _authenticationDbContext;
        private readonly IPasswordService _passwordService;
        private readonly Microsoft.AspNetCore.Authentication.IAuthenticationService _microsfotAuthService;
        //private readonly FinanceAppContext _financeAppDbContext;
        private readonly IMapper _mapper;


        public AuthenticationService(
                    AuthenticationContext authenticationDbContext,
                    IPasswordService passwordService,
                    Microsoft.AspNetCore.Authentication.IAuthenticationService microsfotAuthService,
                    IMapper mapper) {

            _authenticationDbContext = authenticationDbContext;
            _passwordService = passwordService;
            _microsfotAuthService = microsfotAuthService;
            //_financeAppDbContext = financeAppDbContext;
            _mapper = mapper;
        }

        public async Task<ApiResponse<AuthenticationUserDto>> Login(LoginRequest loginRequest, HttpContext httpContext) {
            var authenticationUser = _authenticationDbContext.Users
                .Include(x => x.AuthenticationUserInfo)
                .Where(x => x.UserName == loginRequest.Username)
                .FirstOrDefault();

            if (authenticationUser == null) {

                // TODO: Fix Response to avoid givving information to a user;
                var errorMessage= new ErrorResponseMessage(
                        new GettingActionMessage(loginRequest.Username),
                        new ResourceWithPropertyMessage(authenticationUser, "Username", loginRequest.Username),
                        new UnauthorizedToAccessResourceReason());
                return new ApiResponse<AuthenticationUserDto>(ApiResponseCodesEnum.ResourceNotFound, errorMessage);
            }


            if (!authenticationUser.VerifyPassword(_passwordService, loginRequest.Password)) {
              
                // TODO: Fix Response to avoid givving information to a user;
                var errorMessage = new ErrorResponseMessage(
                        new LogginInActionMessage(loginRequest.Username),
                        new ResourceWithPropertyMessage(authenticationUser, "Username", loginRequest.Username),
                        new UnauthorizedToAccessResourceReason());
                return new ApiResponse<AuthenticationUserDto>(ApiResponseCodesEnum.Unauthorized, errorMessage);
            }
            var ret = new ApiResponse<AuthenticationUserDto>(_mapper.Map<AuthenticationUserDto>(authenticationUser));
            return ret;

            // TODO: Get Roles, Set Claims

            //var appUser = _financeAppDbContext.ApplicationUsers
            //    .Include(x => x.ApplicationUserRoles)
            //    .Where(x => x.AuthenticationUserId == authenticationUser.Id)
            //    .FirstOrDefault();

            //if (appUser == null) {
            //    // TODO: Fix Response to avoid givving information to a user;
            //    var errorMessage = new ErrorResponseMessage(
            //            new LogginInActionMessage(loginRequest.Username),
            //            new ResourceWithPropertyMessage(authenticationUser, "Username", loginRequest.Username),
            //            new UnauthorizedToAccessResourceReason());
            //    return new ApiResponse<AuthenticationUserDto>(ApiResponseCodesEnum.Unauthorized, errorMessage);
            //}


            //var claimsIdentity = new ClaimsIdentity(appUser.GetClaims(authenticationUser), CookieAuthenticationDefaults.AuthenticationScheme);

            //await _microsfotAuthService.SignInAsync(httpContext, null, new ClaimsPrincipal(claimsIdentity), null);

            //// Create Cooke to tell SPA that it is logged in
            //_cookieManager.Set("LoggedIn", true.ToString(), new CookieOptions {
            //    Secure = true,
            //    HttpOnly = false,
            //    SameSite = SameSiteMode.None
            //});

            //var ret = new ApiResponse<AuthenticationUserDto>(_mapper.Map<AuthenticationUserDto>(authenticationUser));
            //return ret;

        }
        public async Task<ApiResponse<AuthenticationUserDto>> CreateAuthUser(CreateAuthenticationUserRequest createUserRequest) {

            var authUser = await _authenticationDbContext.Users
                .Where(x => x.UserName == createUserRequest.Username)
                .FirstOrDefaultAsync();

                

            if (authUser != null) {
                var errorMessage = new ErrorResponseMessage(
                        new CreatingActionMessage(createUserRequest.Username),
                        new ResourceWithPropertyMessage(authUser, "Username", createUserRequest.Username),
                        new PropertyAlreadyExistsReason());
                return new ApiResponse<AuthenticationUserDto>(ApiResponseCodesEnum.DuplicateResource, errorMessage);
            }

            var salt = _passwordService.CreateSalt();

            AuthenticationUser newUser = new() {
                UserName = createUserRequest.Username,
                DateCreated = DateTime.UtcNow,
                PasswordSalt = salt,
                PasswordHash = _passwordService.HashPassword(createUserRequest.Password, salt),
                AuthenticationUserInfo = new AuthenticationUserInfo {
                    Email = createUserRequest.Email,
                }
            };

            await _authenticationDbContext.Users.AddAsync(newUser);
            await _authenticationDbContext.SaveChangesAsync();

            var ret = new ApiResponse<AuthenticationUserDto>(_mapper.Map<AuthenticationUserDto>(authUser));
            return ret;

        }

    }
}
