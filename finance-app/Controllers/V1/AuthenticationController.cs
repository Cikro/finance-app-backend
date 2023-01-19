using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using finance_app.Types.Services.V1.Interfaces;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.DataContracts.V1.Dtos;
using AutoMapper;
using finance_app.Types.DataContracts.V1.Requests.Authentication;

namespace finance_app.Controllers.V1
{
    [ApiController]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IMapper _mapper;

        public AuthenticationController(ILogger<AuthenticationController> logger,
                                  IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Logs a user into the system
        /// </summary>
        /// <param name="request">A Login request</param>
        /// <remarks> 
        /// </remarks>
        /// <returns>A Logged in user</returns>
        [HttpPost]
        [Route("api/Login")]
        [UserAuthorizationFilter]
        public async Task<IActionResult> Login([FromBody] Types.Requests.Authenticaiton.LoginRequest request)
        {
            request.SetUpRequest(HttpContext);
            await request.ProcessRequest();
            return StatusCode(_mapper.Map<int>(request.GetResponse<AuthenticationUserDto>(_mapper)));
        }

        /// <summary>
        /// Creates a user
        /// </summary>
        /// <param name="request">A Login request</param>
        /// <remarks> 
        /// </remarks>
        /// <returns>A Logged in user</returns>
        [HttpPost]
        [Route("api/Create")]
        [UserAuthorizationFilter]
        public async Task<IActionResult> Create([FromBody] Types.Requests.Authenticaiton.CreateAuthenticationUserRequest request) {

            await request.ProcessRequest();
            return  StatusCode(_mapper.Map<int>(request.GetResponse<AuthenticationUserDto>(_mapper)));

        }



    }
}
