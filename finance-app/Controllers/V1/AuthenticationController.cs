using Authentication;
using AutoMapper;
using finance_app.Types.DataContracts.V1.Requests.Authentication;
using finance_app.Types.Repositories.FinanceApp;
using FinanceApplicationUsers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;
using finAuth = Authentication;

namespace finance_app.Controllers.V1 {
    [ApiController]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAuthenticationUserService _userAuthencationService;
        private readonly FinanceAppContext _context;

        public AuthenticationController(ILogger<AuthenticationController> logger,
                                    FinanceAppContext context,
                                    IAuthenticationService authenticationService,
                                    IAuthenticationUserService userAuthencationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _userAuthencationService = userAuthencationService;
            _context = context;
        }

        /// <summary>
        /// Logs a user into the system
        /// </summary>
        /// <param name="loginRequest">A Login request</param>
        /// <remarks> 
        /// </remarks>
        /// <returns>A Logged in user</returns>
        [HttpPost]
        [Route("api/Login")]
        [UserAuthorizationFilter]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {

            var authUser = await _userAuthencationService.GetAuthencationUser(loginRequest.Username);


            if (authUser.Authenticate(loginRequest.Password)) 
            {
                FinanceApplicationUser appUser = await new FinanceApplicationUser(_context).GetUser(authUser.AuthenticationUserId);
                if (appUser == null) {
                    return StatusCode(500);
                }


                var claimsIdentity = new ClaimsIdentity(appUser.GetClaims(authUser), CookieAuthenticationDefaults.AuthenticationScheme);

                await _authenticationService.SignInAsync(HttpContext, null, new ClaimsPrincipal(claimsIdentity), null);

            }


            // TODO: Get Roles, Set Claims



            return StatusCode(200);
        }

        /// <summary>
        /// Creates a user
        /// </summary>
        /// <param name="createUserRequest">A Create User request</param>
        /// <remarks> 
        /// </remarks>
        /// <returns>A Logged in user</returns>
        [HttpPost]
        [Route("api/Create")]
        [UserAuthorizationFilter]
        public async Task<IActionResult> Create(CreateAuthenticationUserRequest createUserRequest) 
        {

            var user = await _userAuthencationService.NewAuthencationUser(createUserRequest.Username);
            await user.Create(createUserRequest.Password);
            return StatusCode(200);

        }



    }
}
