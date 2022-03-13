using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using finance_app.Types.Repositories.Account;
using finance_app.Types.Services.V1.Interfaces;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.Models.ResourceIdentifiers;
using AutoMapper;
using System;

namespace finance_app.Types.Services.V1
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountServiceDbo;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _context;
        

        public AccountService(IMapper mapper, IAccountRepository accountServiceDbo,
                             IAuthorizationService authorizationService,
                             IHttpContextAccessor context) {

            _accountServiceDbo = accountServiceDbo;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _context = context;
        }

        /// <inheritdoc cref="IAccountService.GetAccounts"/>
    public async Task<ApiResponse<ListResponse<AccountDto>>> GetAccounts(UserResourceIdentifier userId) {
            if (userId == null) { throw new ArgumentNullException(nameof(UserResourceIdentifier)); }
            var accounts = await _accountServiceDbo.GetAllByUserId(userId.Id);

            var accessibleAccounts = await FilterAccessibleAccounts(accounts);

            var ret = new ListResponse<AccountDto>(_mapper.Map<List<AccountDto>>(accessibleAccounts)) {
                ExcludedItems = accounts.Count() - accessibleAccounts.Count()
            };


            return new ApiResponse<ListResponse<AccountDto>>(ret, ApiResponseCodesEnum.Success, "Success");
        }


        /// <inheritdoc cref="IAccountService.GetAccount"/>
        public async Task<ApiResponse<AccountDto>> GetAccount(AccountResourceIdentifier accountId) {
            if (accountId == null) { throw new ArgumentNullException(nameof(AccountResourceIdentifier)); }
            var account = await _accountServiceDbo.GetAccountByAccountId(accountId.Id);

            if (!(await _authorizationService.AuthorizeAsync(_context.HttpContext.User, account, "CanAccessResourcePolicy" )).Succeeded) 
            {
                return new ApiResponse<AccountDto>(ApiResponseCodesEnum.Unauthorized, "Unauthorized");
            }

            return new ApiResponse<AccountDto>(_mapper.Map<AccountDto>(account));
        }

        /// <inheritdoc cref="IAccountService.GetChildren"/>
        public async Task<ApiResponse<ListResponse<AccountDto>>> GetChildren(AccountResourceIdentifier accountId) {
            // TODO: Consider fetching children of children in the future.
            return await GetChildren(accountId.Id);

        }
        
        /// <inheritdoc cref="IAccountService.GetPaginatedAccounts"/>
        public async Task<ApiResponse<ListResponse<AccountDto>>> GetPaginatedAccounts(UserResourceIdentifier userId, PaginationInfo pageInfo)
        {
            if (userId== null) { throw new ArgumentNullException(nameof(UserResourceIdentifier)); }
            if (!(pageInfo?.PageNumber != null) || pageInfo?.PageNumber <= 0) { return new ApiResponse<ListResponse<AccountDto>>(ApiResponseCodesEnum.BadRequest, "Invlaid Page Number."); }
            if (!(pageInfo?.ItemsPerPage!= null) || pageInfo?.ItemsPerPage <= 0) { return new ApiResponse<ListResponse<AccountDto>>(ApiResponseCodesEnum.BadRequest, "Invlaid Items Per Page.");; }

            uint offset = (uint)pageInfo.PageNumber - 1;
            
            var accounts = await _accountServiceDbo.GetPaginatedByUserId(userId.Id, (uint)pageInfo.ItemsPerPage, offset);
            var accessibleAccounts = await FilterAccessibleAccounts(accounts);

            var ret = new ListResponse<AccountDto>(_mapper.Map<List<AccountDto>>(accessibleAccounts)) {
                ExcludedItems = accounts.Count() - accessibleAccounts.Count()
            };

            return new ApiResponse<ListResponse<AccountDto>>(ret);
        }

        /// <inheritdoc cref="IAccountService.CreateAccount"/>
        public async Task<ApiResponse<AccountDto>> CreateAccount(Account account) {
            var existingAccount = await _accountServiceDbo.GetAccountByAccountName(account.User_Id, account.Name);
            if (existingAccount != null) {   
                var message = $"Error creating account. Account with name '{account.Name}' already exists.";
                return new ApiResponse<AccountDto>(_mapper.Map<AccountDto>(existingAccount), ApiResponseCodesEnum.DuplicateResource,message);
            };
            var newAccount = await _accountServiceDbo.CreateAccount(account);


            return new ApiResponse<AccountDto>(_mapper.Map<AccountDto>(newAccount));
        }


        /// <inheritdoc cref="IAccountService.UpdateAccount"/>
        public async Task<ApiResponse<AccountDto>> UpdateAccount(Account account) {

            var existingAccount = await _accountServiceDbo.GetAccountByAccountId(account.Id);
            if (existingAccount == null) {    
                var message = $"Error updating account. Account with id '{account.Id}' does not exist.";
                return new ApiResponse<AccountDto>(ApiResponseCodesEnum.ResourceNotFound, message);
            }

            if (existingAccount.Name != account.Name) {
                var duplicateName = await _accountServiceDbo.GetAccountByAccountName(existingAccount.User_Id, account.Name);
                if (duplicateName != null) {
                    var message = $"Error updating account. Account with name '{duplicateName.Name}' already Exists.";
                    return new ApiResponse<AccountDto>(_mapper.Map<AccountDto>(duplicateName), ApiResponseCodesEnum.DuplicateResource, message);
                }
            }

            if (existingAccount.Closed != account.Closed) {

                // If you are trying to close an account, all of it's children need to be closed
                if (account.Closed == true) {

                    var children = await GetChildren(account.Id);
                    if (children.Data.ExcludedItems > 0) {
                        var message = $"Error updating account. Account with id '{account.Id}' has cildren that you don't have access to.";
                        return new ApiResponse<AccountDto>(ApiResponseCodesEnum.Unauthorized, message);
                    }

                    // If any children accounts are open
                    if (children?.Data?.Items.Any(child => child.Closed == true) == true) {
                        var message = $"Error updating account. Cannot close an account when it has child accounts that are not closed.";
                        return new ApiResponse<AccountDto>(_mapper.Map<AccountDto>(existingAccount), ApiResponseCodesEnum.DuplicateResource, message);
                    }

                }

                // If you are opening an account, its parent must be open
                if (account.Closed == false && existingAccount.Parent_Account_Id != null) {
                    var parent = await _accountServiceDbo.GetAccountByAccountId((uint) existingAccount.Parent_Account_Id);
                    if (parent?.Closed == true) {
                        var message = $"Error updating account. Cannot open an account when its parent is closed.";
                        return new ApiResponse<AccountDto>(_mapper.Map<AccountDto>(existingAccount), ApiResponseCodesEnum.DuplicateResource, message);
                    }
                }

            }

            var updatedAccount = await _accountServiceDbo.UpdateAccount(account);

            return new ApiResponse<AccountDto>(_mapper.Map<AccountDto>(updatedAccount));
        }

        /// <inheritdoc cref="IAccountService.CloseAccount"/>
        public async Task<ApiResponse<ListResponse<AccountDto>>> CloseAccount(AccountResourceIdentifier accountId) {
            
            string message;
            var accountToClose = await _accountServiceDbo.GetAccountByAccountId(accountId.Id);
            if (accountToClose == null) {    
                message = $"Error closing account. Account with id '{accountId.Id}' could not be found.";
                return new ApiResponse<ListResponse<AccountDto>>(ApiResponseCodesEnum.ResourceNotFound, message);
            };

            if (accountToClose.Closed == true) {
                message = $"The Account with id {accountToClose.Id} is already closed.";
                return new ApiResponse<ListResponse<AccountDto>>(new ListResponse<AccountDto>(new List<AccountDto> {{_mapper.Map<AccountDto>(accountToClose)}}), ApiResponseCodesEnum.InternalError, message);
            }

            var children = await GetChildren(accountId);
            if (children.Data.ExcludedItems > 0) {
                message = $"Error closing account. Account with id '{accountId.Id}' has cildren that you don't have access to.";
                return new ApiResponse<ListResponse<AccountDto>>(ApiResponseCodesEnum.Unauthorized, message);
            }

            var accountsClosed = await _accountServiceDbo.CloseAccount(accountId.Id);
            if (!accountsClosed.Any()) {
                message = $"Error closing account. Account with id '{accountId.Id}' found, but not closed.";
                return new ApiResponse<ListResponse<AccountDto>>(ApiResponseCodesEnum.InternalError, message );
            }

            message = $"{accountsClosed.Count()} accounts closed.";
            return new ApiResponse<ListResponse<AccountDto>>(
                new ListResponse<AccountDto>(_mapper.Map<List<AccountDto>>(accountsClosed)),
                ApiResponseCodesEnum.Success,
                message);
 
        }

        private async Task<ApiResponse<ListResponse<AccountDto>>> GetChildren(uint accountId) {
            // TODO: Consider fetching children of children in the future.
            var accounts = await _accountServiceDbo.GetChildrenByAccountId(accountId);

            var accessibleAccounts = await FilterAccessibleAccounts(accounts);

            var ret = new ListResponse<AccountDto>(_mapper.Map<List<AccountDto>>(accessibleAccounts)) {
                ExcludedItems = accounts.Count() - accessibleAccounts.Count()
            };

            return new ApiResponse<ListResponse<AccountDto>>(ret);

        }

        private async Task<IEnumerable<Account>> FilterAccessibleAccounts(IEnumerable<Account> accounts) {
            return (
                await Task.WhenAll(accounts?.Select(async (account) => {
                    return new AccountsWithAccess {
                        Account = account,
                        HasAccess = (await _authorizationService.AuthorizeAsync(_context.HttpContext.User, account, "CanAccessResourcePolicy" )).Succeeded
                    };
                    })
                ))
                .Where(account => account.HasAccess == true)
                .Select(a => a.Account);
        }
    
    }
    public class AccountsWithAccess {
        public Account Account { get; set; }
        public bool HasAccess { get; set; }
    }
}
