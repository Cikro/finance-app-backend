using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using finance_app.Types.EFModels;
using finance_app.Types.Interfaces;
using finance_app.Types;
using finance_app.Types.Responses;
using finance_app.Types.Responses.Dtos;

namespace finance_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        
        private readonly ILogger<AccountsController> _logger;
        private readonly IAccountService _accountService;

        public AccountsController(ILogger<AccountsController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<ApiResponse<ListResponse<AccountDto>>> Get([FromQuery] uint userId, [FromQuery] PaginationInfo pageInfo)
        {
            List<AccountDto> accounts;
            if (pageInfo.PageNumber != null && pageInfo.ItemsPerPage != null) {
                accounts = _accountService.GetPaginatedAccounts(userId, pageInfo);

            } else {
                accounts = _accountService.GetAccounts(userId);
            }

            var ret = new ApiResponse<ListResponse<AccountDto>>();

            ret.Data = new ListResponse<AccountDto>(accounts);
            ret.ResponseMessage = "Success";
            
            return ret;
        }
    }
}
