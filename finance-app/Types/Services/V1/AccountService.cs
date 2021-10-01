using System.Collections.Generic;
using System.Threading.Tasks;

using finance_app.Types.Repositories.Account;
using finance_app.Types.Services.V1.Interfaces;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.Models;

using AutoMapper;

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

        /// <summary>
        /// Gets a list of all accounts for a given userId
        /// </summary>
        /// <param name="userId">The user's Id</param>
        /// <returns> A list of AccountDtos</returns>
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


        public async Task<ApiResponse<AccountDto>> CreateAccount(Account account) {
            // TODO: Confirm default values of input (i.e. currency code)
            // TODO: Implement GetAccountByAccountName
            //        - restructure db folders
            // TODO: Implement GetAccountByAccountId
            if (await _accountServiceDbo.GetAccountByAccountName(account.User_Id, account.Name) != null) {    
                return new ApiResponse<AccountDto>
                {
                    Data = null,
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
        public void DeleteAccounts(){

        }
    }
}
