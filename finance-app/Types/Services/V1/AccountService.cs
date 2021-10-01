using System.Collections.Generic;
using System.Threading.Tasks;

using finance_app.Types.Repositories.Account;
using finance_app.Types.Services.V1.Interfaces;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.Models;

using AutoMapper;
using System.Linq.Expressions;

namespace finance_app.Types.Services.V1
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountServiceDbo;
        private readonly IMapper _mapper;
        

        public AccountService(IMapper mapper, IAccountRepository accountServiceDbo){

            _accountServiceDbo = accountServiceDbo;
            _mapper = mapper;
            
        }

        /// <inheritdoc cref="IAccountService.GetAccounts"/>
        public async Task<ApiResponse<ListResponse<AccountDto>>> GetAccounts(UserResourceIdentifier userId){
            var accounts = await _accountServiceDbo.GetAllByUserId(userId.Id);

            return new ApiResponse<ListResponse<AccountDto>>
            {
                Data = new ListResponse<AccountDto>(_mapper.Map<List<AccountDto>>(accounts)),
                ResponseMessage = "Success",
                StatusCode = System.Net.HttpStatusCode.OK,
                ResponseCode = ApiResponseCodesEnum.Success
            };
        }
        
        /// <inheritdoc cref="IAccountService.GetPaginatedAccounts"/>
        public async Task<ApiResponse<ListResponse<AccountDto>>> GetPaginatedAccounts(UserResourceIdentifier userId, PaginationInfo pageInfo)
        {
            if (pageInfo.PageNumber <= 0) { return null; }
            if (pageInfo.ItemsPerPage < 0) { return null; }

            uint offset = (uint)pageInfo.PageNumber - 1;
            
            var accounts = await _accountServiceDbo.GetPaginatedByUserId(userId.Id, (uint)pageInfo.ItemsPerPage, offset);

            return new ApiResponse<ListResponse<AccountDto>>
            {
                Data = new ListResponse<AccountDto>(_mapper.Map<List<AccountDto>>(accounts)),
                ResponseMessage = "Success",
                StatusCode = System.Net.HttpStatusCode.OK,
                ResponseCode = ApiResponseCodesEnum.Success
            };
        }

        /// <inheritdoc cref="IAccountService.CreateAccount"/>
        public async Task<ApiResponse<AccountDto>> CreateAccount(Account account) {
            // TODO: Confirm default values of input (i.e. currency code)
            var existingAccount = await _accountServiceDbo.GetAccountByAccountName(account.User_Id, account.Name);
            if ( existingAccount != null) {    
                return new ApiResponse<AccountDto>
                {
                    Data = _mapper.Map<AccountDto>(existingAccount),
                    ResponseMessage = $"Error creating account. Account with name '{account.Name}' already exists.",
                    StatusCode = System.Net.HttpStatusCode.Conflict,
                    ResponseCode = ApiResponseCodesEnum.DuplicateResource
                };
            };
            var newAccount = await _accountServiceDbo.CreateAccount(account);


            return new ApiResponse<AccountDto>
            {
                Data = _mapper.Map<AccountDto>(newAccount),
                ResponseMessage = "Success",
                StatusCode = System.Net.HttpStatusCode.OK,
                ResponseCode = ApiResponseCodesEnum.Success
            };
        }

        public void UpdateAccounts(){

        }

        /// <inheritdoc cref="IAccountService.CloseAccount"/>
        public async Task<ApiResponse<AccountDto>> CloseAccount(AccountResourceIdentifier accountId) {

            var accountToClose = await _accountServiceDbo.GetAccountByAccountId(accountId.Id);
            if (accountToClose == null) {    
                return new ApiResponse<AccountDto>
                {
                    Data = null,
                    ResponseMessage = $"Error closing account. Account with id '{accountId.Id}' could not be found.",
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    ResponseCode = ApiResponseCodesEnum.ResourceNotFound
                };
            };

            if (accountToClose.Closed == true) {
                return new ApiResponse<AccountDto>
                {
                    Data = _mapper.Map<AccountDto>(accountToClose),
                    ResponseMessage = $"The Account with id {accountToClose.Id} is already closed.",
                    StatusCode = System.Net.HttpStatusCode.Conflict,
                    ResponseCode = ApiResponseCodesEnum.InternalError
                };
            }

            await _accountServiceDbo.CloseAccount(accountId.Id);
            accountToClose.Closed = true;
            return new ApiResponse<AccountDto>
            {
                Data = _mapper.Map<AccountDto>(accountToClose),
                ResponseMessage = $"The Account with id {accountToClose.Id} is already closed.",
                StatusCode = System.Net.HttpStatusCode.Conflict,
                ResponseCode = ApiResponseCodesEnum.InternalError
            };
 
        }
    }
}
