using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using finance_app.Types.Services.V1.Interfaces;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.DataContracts.V1.Dtos;
using AutoMapper;
using finance_app.Types.DataContracts.V1.Requests.Authentication;
using finance_app.Types.DataContracts.V1.Requests.ApplicationAccounts;
using Authentication;

namespace finance_app.Controllers.V1
{
    [ApiController]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class ApplicationAccountsController : ControllerBase
    {
        
        private readonly ILogger<ApplicationAccountsController> _logger;
        
        public ApplicationAccountsController(ILogger<ApplicationAccountsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Creates an Application User or adds them to an Application Account
        /// </summary>
        /// <param name="request">A CreateApplicationAccountRequest</param>
        /// <remarks> 
        /// </remarks>
        /// <returns>the created user</returns>
        [HttpPost]
        [Route("CreateApplicationAccount")]
        [UserAuthorizationFilter]
        public async Task<IActionResult> CreateApplicationUser([FromBody] CreateApplicationAccountRequest request)
        {

            return StatusCode(500);
        }

    }
}
