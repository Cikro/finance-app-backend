using System.Collections.Generic;
using System.Threading.Tasks;

using finance_app.Types.Repositories.Account;
using finance_app.Types.Services.V1.Interfaces;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.Models;

using AutoMapper;
using System.Linq.Expressions;
using System.Linq;
using finance_app.Types.Models.ResourceIdentifiers;
using Microsoft.AspNetCore.Authorization;

namespace finance_app.Types.Services.V1
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountServiceDbo;
        private readonly IMapper _mapper;
        private readonly IAuthorizationHandler _authorizationService;
        private readonly IUserAuthorizationService _userAuthorizationService;
        

        public AccountService(IMapper mapper, IAccountRepository accountServiceDbo,
                             IAuthorizationHandler authorizationService, IUserAuthorizationService userAuthorizationService){

            _accountServiceDbo = accountServiceDbo;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _userAuthorizationService = userAuthorizationService;
        }

        /// <inheritdoc cref="IAccountService.GetAccounts"/>
    public async Task<ApiResponse<ListResponse<AccountDto>>> GetAccounts(UserResourceIdentifier userId){
            var accounts = await _accountServiceDbo.GetAllByUserId(userId.Id);

            var accessibleAccounts = accounts.Where(account => _userAuthorizationService.CanAccessResource(account, 1));

            var ret = new ListResponse<AccountDto>(_mapper.Map<List<AccountDto>>(accessibleAccounts)) {
                ExcludedItems = accounts.Count() - accessibleAccounts.Count()
            };


            return new ApiResponse<ListResponse<AccountDto>>
            {
                Data = ret,
                ResponseMessage = "Success",
                StatusCode = System.Net.HttpStatusCode.OK,
                ResponseCode = ApiResponseCodesEnum.Success
            };
        }


        /// <inheritdoc cref="IAccountService.GetAccount"/>
        public async Task<ApiResponse<AccountDto>> GetAccount(AccountResourceIdentifier accountId) {
            var account = await _accountServiceDbo.GetAccountByAccountId(accountId.Id);

            if (!_userAuthorizationService.CanAccessResource(account, 1)) 
            {
                return new ApiResponse<AccountDto>
                {
                    Data = null,
                    ResponseMessage = "Unauthorizaed",
                    StatusCode = System.Net.HttpStatusCode.Unauthorized,
                    ResponseCode = ApiResponseCodesEnum.Forbidden
                };
            }

            return new ApiResponse<AccountDto>
            {
                Data = _mapper.Map<AccountDto>(account),
                ResponseMessage = "Success",
                StatusCode = System.Net.HttpStatusCode.OK,
                ResponseCode = ApiResponseCodesEnum.Success
            };
        }

        /// <inheritdoc cref="IAccountService.GetChildren"/>
        public async Task<ApiResponse<ListResponse<AccountDto>>> GetChildren(AccountResourceIdentifier accountId) {
            // TODO: Consider fetching children of children in the future.
            var accounts = await _accountServiceDbo.GetChildrenByAccountId(accountId.Id);

            var accessibleAccounts = accounts.Where(account => _userAuthorizationService.CanAccessResource(account, 1));

            var ret = new ListResponse<AccountDto>(_mapper.Map<List<AccountDto>>(accessibleAccounts)) {
                ExcludedItems = accounts.Count() - accessibleAccounts.Count()
            };

            return new ApiResponse<ListResponse<AccountDto>>
            {
                Data = ret,
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
            accounts = accounts.Where(account => _userAuthorizationService.CanAccessResource(account, 1));

            var accessibleAccounts = accounts.Where(account => _userAuthorizationService.CanAccessResource(account, 1));

            var ret = new ListResponse<AccountDto>(_mapper.Map<List<AccountDto>>(accessibleAccounts)) {
                ExcludedItems = accounts.Count() - accessibleAccounts.Count()
            };

            return new ApiResponse<ListResponse<AccountDto>>
            {
                Data = ret,
                ResponseMessage = "Success",
                StatusCode = System.Net.HttpStatusCode.OK,
                ResponseCode = ApiResponseCodesEnum.Success
            };
        }

        /// <inheritdoc cref="IAccountService.CreateAccount"/>
        public async Task<ApiResponse<AccountDto>> CreateAccount(Account account) {
            var existingAccount = await _accountServiceDbo.GetAccountByAccountName(account.User_Id, account.Name);
            if (existingAccount != null) {    
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


        /// <inheritdoc cref="IAccountService.UpdateAccount"/>
        public async Task<ApiResponse<AccountDto>> UpdateAccount(Account account) {
            var existingAccount = await _accountServiceDbo.GetAccountByAccountId(account.Id);
            if (existingAccount == null) {    
                return new ApiResponse<AccountDto>
                {
                    Data = null,
                    ResponseMessage = $"Error updating account. Account with id '{account.Id}' does not exist.",
                    StatusCode = System.Net.HttpStatusCode.Conflict,
                    ResponseCode = ApiResponseCodesEnum.ResourceNotFound
                };
            }

            if (existingAccount.Name != account.Name) {
                var duplicateName = await _accountServiceDbo.GetAccountByAccountName(existingAccount.User_Id, account.Name);
                if (duplicateName != null) {
                    return new ApiResponse<AccountDto>
                    {
                        Data = _mapper.Map<AccountDto>(duplicateName),
                        ResponseMessage = $"Error updating account. Account name id '{duplicateName.Name}' already Exists.",
                        StatusCode = System.Net.HttpStatusCode.Conflict,
                        ResponseCode = ApiResponseCodesEnum.DuplicateResource
                    };
                }
            }

            if (existingAccount.Closed != account.Closed) {

                // If you are trying to close an account, all of it's children need to be closed
                if (account.Closed == true) {
                    var children = await _accountServiceDbo.GetChildrenByAccountId((uint) existingAccount.Id);

                    // If any children accounts are open
                    if (children.Any(child => child.Closed != true)) {
                        return new ApiResponse<AccountDto>
                        {
                            Data = _mapper.Map<AccountDto>(existingAccount),
                            ResponseMessage = $"Error updating account. cannot close an account when it has child accounts that are not closed.",
                            StatusCode = System.Net.HttpStatusCode.Conflict,
                            ResponseCode = ApiResponseCodesEnum.DuplicateResource
                        };

                    }

                }

                // If you are opening an account, its parent must be open
                if (account.Closed == false
                    && existingAccount.Parent_Account_Id != null) {

                    var parent = await _accountServiceDbo.GetAccountByAccountId((uint) existingAccount.Parent_Account_Id);
                    if (parent?.Closed == true) {
                        return new ApiResponse<AccountDto>
                        {
                            Data = _mapper.Map<AccountDto>(existingAccount),
                            ResponseMessage = $"Error updating account. Cannot open an account when its parent is closed.",
                            StatusCode = System.Net.HttpStatusCode.Conflict,
                            ResponseCode = ApiResponseCodesEnum.DuplicateResource
                        };
                    }
                        
                }

            }

            var updatedAccount = await _accountServiceDbo.UpdateAccount(account);


            return new ApiResponse<AccountDto>
            {
                Data = _mapper.Map<AccountDto>(updatedAccount),
                ResponseMessage = "Success",
                StatusCode = System.Net.HttpStatusCode.OK,
                ResponseCode = ApiResponseCodesEnum.Success
            };


        }

        /// <inheritdoc cref="IAccountService.CloseAccount"/>
        public async Task<ApiResponse<ListResponse<AccountDto>>> CloseAccount(AccountResourceIdentifier accountId) {
            var accountToClose = await _accountServiceDbo.GetAccountByAccountId(accountId.Id);
            if (accountToClose == null) {    
                return new ApiResponse<ListResponse<AccountDto>>
                {
                    Data = null,
                    ResponseMessage = $"Error closing account. Account with id '{accountId.Id}' could not be found.",
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    ResponseCode = ApiResponseCodesEnum.ResourceNotFound
                };
            };

            var children = await GetChildren(accountId);
            if (children.Data.ExcludedItems > 0) {
                return new ApiResponse<ListResponse<AccountDto>>
                {
                    Data = null,
                    ResponseMessage = $"Error closing account. Account with id '{accountId.Id}' has cildren that you don't have access to.",
                    StatusCode = System.Net.HttpStatusCode.Forbidden,
                    ResponseCode = ApiResponseCodesEnum.Forbidden
                };
            }
            

            if (accountToClose.Closed == true) {
                return new ApiResponse<ListResponse<AccountDto>>
                {
                    Data = new ListResponse<AccountDto>(new List<AccountDto> {
                         _mapper.Map<AccountDto>(accountToClose) 
                    }),
                    ResponseMessage = $"The Account with id {accountToClose.Id} is already closed.",
                    StatusCode = System.Net.HttpStatusCode.Conflict,
                    ResponseCode = ApiResponseCodesEnum.InternalError
                };
            }

            var accountsClosed = await _accountServiceDbo.CloseAccount(accountId.Id);
            if (!accountsClosed.Any()) {
                return new ApiResponse<ListResponse<AccountDto>>
                {
                    Data = null,
                    ResponseMessage = $"Error closing account. Account with id '{accountId.Id}' found, but not closed.",
                    StatusCode = System.Net.HttpStatusCode.Conflict,
                    ResponseCode = ApiResponseCodesEnum.InternalError
                };
            }

            return new ApiResponse<ListResponse<AccountDto>>
            {
                Data = new ListResponse<AccountDto>(_mapper.Map<List<AccountDto>>(accountsClosed)),
                ResponseMessage = $"{accountsClosed.Count()} accounts closed.",
                StatusCode = System.Net.HttpStatusCode.OK,
                ResponseCode = ApiResponseCodesEnum.Success
            };
 
        }

        
    }
}
