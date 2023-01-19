using AutoMapper;
using finance_app.Types.Repositories.Authentication;
using finance_app.Types.Repositories.FinanceApp;
using finance_app.Types.Services.V1.ResponseMessages;
using finance_app.Types.Services.V1.ResponseMessages.ActionMessages;
using finance_app.Types.Services.V1.ResponseMessages.ReasonMessages;
using finance_app.Types.Services.V1.ResponseMessages.ResourcesMessages;
using finance_app.Types.Services.V1.Services.Interfaces;
using CookieManager;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace finance_app.Types.Requests.Authenticaiton {


    public class LoginRequest : Request {

        /// <summary>
        /// The user who is logging in
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The user's password
        /// </summary>
        public string Password { get; set; }

        public HttpContext _httpContext;


      
        private readonly AuthenticationContext _authenticationDbContext;
        private readonly IPasswordService _passwordService;
        private readonly Microsoft.AspNetCore.Authentication.IAuthenticationService _microsfotAuthService;
        private readonly FinanceAppContext _financeAppDbContext;

        private readonly CookieManager.ICookieManager _cookieManager;


        public LoginRequest(
                    AuthenticationContext authenticationDbContext,
                    IPasswordService passwordService,
                    Microsoft.AspNetCore.Authentication.IAuthenticationService microsfotAuthService,
                    FinanceAppContext financeAppDbContext,
                    CookieManager.ICookieManager cookieManager) {

            _authenticationDbContext = authenticationDbContext;
            _passwordService = passwordService;
            _microsfotAuthService = microsfotAuthService;
            _financeAppDbContext = financeAppDbContext;
            _cookieManager = cookieManager;
        }

        public override T GetResponse<T>(IMapper mapper) {
            return mapper.Map<T>(this);
        }

        public void SetUpRequest(HttpContext httpContext) {
            _httpContext = httpContext;
        }

        public async override Task ProcessRequest() {
            if (_httpContext == null) {
                Status = DataContracts.V1.Responses.ApiResponseCodesEnum.InternalError;
                ResponseMessage = new ErrorResponseMessage(
                        new LogginInActionMessage(Username),
                        new NullReasonMessage("HttpContext"));
                return;
            }

            var authenticationUser = _authenticationDbContext.Users
                .Where(x => x.UserName == Username)
                .FirstOrDefault();

            if (authenticationUser == null) {
                Status = DataContracts.V1.Responses.ApiResponseCodesEnum.ResourceNotFound;
                // TODO: Fix Response to avoid givving information to a user;
                ResponseMessage = new ErrorResponseMessage(
                        new GettingActionMessage(Username),
                        new ResourceWithPropertyMessage(authenticationUser, "Username", Username),
                        new UnauthorizedToAccessResourceReason());
                return;
            }

            
            if (!authenticationUser.VerifyPassword(_passwordService, Password)) {
                Status = DataContracts.V1.Responses.ApiResponseCodesEnum.Unauthorized;
                // TODO: Fix Response to avoid givving information to a user;
                ResponseMessage = new ErrorResponseMessage(
                        new LogginInActionMessage(Username),
                        new ResourceWithPropertyMessage(authenticationUser, "Username", Username),
                        new UnauthorizedToAccessResourceReason());
                return;
            }

            // TODO: Get Roles, Set Claims

            var appUser = _financeAppDbContext.ApplicationUsers
                .Where(x => x.AuthenticationUserId == authenticationUser.Id)
                .FirstOrDefault();

            if (appUser == null) {
                Status = DataContracts.V1.Responses.ApiResponseCodesEnum.Unauthorized;
                // TODO: Fix Response to avoid givving information to a user;
                ResponseMessage = new ErrorResponseMessage(
                        new LogginInActionMessage(Username),
                        new ResourceWithPropertyMessage(authenticationUser, "Username", Username),
                        new UnauthorizedToAccessResourceReason());
                return;
            }

            var claimsIdentity = new ClaimsIdentity(appUser.GetClaims(authenticationUser), CookieAuthenticationDefaults.AuthenticationScheme);

            await _microsfotAuthService.SignInAsync(_httpContext, null, new ClaimsPrincipal(claimsIdentity), null);

            // Create Cooke to tell SPA that it is logged in
            _cookieManager.Set("LoggedIn", true.ToString(), new CookieOptions {
                Secure = true,
                HttpOnly = false,
                SameSite = SameSiteMode.None
            });


        }

    }
}
