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
using finance_app.Types.DataContracts.V1.Responses.ResponseMessage;
using finance_app.Types.DataContracts.V1.Responses.ResponseMessage.Accounts;
using finance_app.Types.DataContracts.V1.Responses.ErrorResponses;
using finance_app.Types.DataContracts.V1.Responses.ResourceMessages;
using finance_app.Types.DataContracts.V1.Responses.ReasonMessages;
using finance_app.Types.DataContracts.V1.Responses.ReasonMessages.Accounts;

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
            
            var accounts = await _accountServiceDbo.GetChildrenByAccountId(accountId.Id);
            var accessibleAccounts = await FilterAccessibleAccounts(accounts);

            var ret = new ListResponse<AccountDto>(_mapper.Map<List<AccountDto>>(accessibleAccounts)) {
                ExcludedItems = accounts.Count() - accessibleAccounts.Count()
            };

            return new ApiResponse<ListResponse<AccountDto>>(ret);

        }
        
        /// <inheritdoc cref="IAccountService.GetPaginatedAccounts"/>
        public async Task<ApiResponse<ListResponse<AccountDto>>> GetPaginatedAccounts(UserResourceIdentifier userId, PaginationInfo pageInfo)
        {
            if (userId== null) { throw new ArgumentNullException(nameof(UserResourceIdentifier)); }
            if (!(pageInfo?.PageNumber != null) || pageInfo?.PageNumber <= 0) { return new ApiResponse<ListResponse<AccountDto>>(ApiResponseCodesEnum.BadRequest, "Invalid Page Number."); }
            if (!(pageInfo?.ItemsPerPage!= null) || pageInfo?.ItemsPerPage <= 0) { return new ApiResponse<ListResponse<AccountDto>>(ApiResponseCodesEnum.BadRequest, "Invalid Items Per Page.");; }

            uint offset = (uint)pageInfo.PageNumber - 1;
            
            var accounts = await _accountServiceDbo.GetPaginatedByUserId(userId.Id, (uint)pageInfo.ItemsPerPage, offset);
            var accessibleAccounts = await FilterAccessibleAccounts(accounts);

            var ret = new ListResponse<AccountDto>(_mapper.Map<List<AccountDto>>(accessibleAccounts)) {
                ExcludedItems = (accounts?.Count() - accessibleAccounts?.Count()) ?? 0
            };

            return new ApiResponse<ListResponse<AccountDto>>(ret);
        }

        /// <inheritdoc cref="IAccountService.CreateAccount"/>
        public async Task<ApiResponse2<AccountDto>> CreateAccount(Account account) {
            var existingAccount = await _accountServiceDbo.GetAccountByAccountName(account.UserId, account.Name);
            if (existingAccount != null) {
                var errorMessage = new ErrorResponseMessage(
                    new CreatingActionMessage(account),
                    new ResourceWithPropertyMessage(existingAccount, existingAccount.Name),
                    new PropertyAlreadyExistsReason());
                return new ApiResponse2<AccountDto>(_mapper.Map<AccountDto>(existingAccount), ApiResponseCodesEnum.DuplicateResource, errorMessage);
            }

            var newAccount = await _accountServiceDbo.CreateAccount(account);


            return new ApiResponse2<AccountDto>(_mapper.Map<AccountDto>(newAccount));
        }


        /// <inheritdoc cref="IAccountService.UpdateAccount"/>
        public async Task<ApiResponse2<AccountDto>> UpdateAccount(Account account) {

            var existingAccount = await _accountServiceDbo.GetAccountByAccountId(account.Id);
            if (existingAccount == null) {    
                var errorMessage = new ErrorResponseMessage(
                    new UpdatingActionMessage(account),
                    new ResourceWithPropertyMessage(existingAccount, existingAccount.Id),
                    new PropertyAlreadyExistsReason());
                return new ApiResponse2<AccountDto>(ApiResponseCodesEnum.ResourceNotFound, errorMessage);
            }

            if (existingAccount.Name != account.Name) {
                var duplicateName = await _accountServiceDbo.GetAccountByAccountName(existingAccount.UserId, account.Name);
                if (duplicateName != null) {
                    var errorMessage = new ErrorResponseMessage(
                        new UpdatingActionMessage(account),
                        new ResourceWithPropertyMessage(duplicateName, duplicateName.Name),
                        new PropertyAlreadyExistsReason());
                    return new ApiResponse2<AccountDto>(_mapper.Map<AccountDto>(duplicateName), ApiResponseCodesEnum.DuplicateResource, errorMessage);
                }
            }

            // If the 'Closed' Status is changing.
            if (existingAccount.Closed != account.Closed) {

                // If you are trying to close an account
                if (account.Closed == true) {

                    // You need access to all of it's children all of it's children need to be closed.
                    var children = await _accountServiceDbo.GetChildrenByAccountId(account.Id);

                    var accessibleChildren = await FilterAccessibleAccounts(children);

                    if (accessibleChildren.Count() != children.Count()) {
                        var errorMessage = new ErrorResponseMessage(
                            new UpdatingActionMessage(account),
                            new ResourceWithPropertyMessage(account, account.Id),
                            new UnauthorizedToAccessChildrenReason());
                        return new ApiResponse2<AccountDto>(ApiResponseCodesEnum.Unauthorized, errorMessage);
                    }

                    // And all of it's children need to be closed already
                    var openChildren = children.Where(child => child.Closed == false).ToList();
                    if (openChildren.Count > 0) {
                        var errorMessage = new ErrorResponseMessage(
                            new UpdatingActionMessage(account),
                            new ResourceWithPropertyMessage(account, account.Id),
                            new ChildAccountsNotClosedReason(openChildren));
                        return new ApiResponse2<AccountDto>(_mapper.Map<AccountDto>(existingAccount), ApiResponseCodesEnum.DuplicateResource, errorMessage);
                    }

                }

                // If you are opening an account, its parent must be open
                if (account.Closed == false && existingAccount.ParentAccountId != null) {
                    var parent = await _accountServiceDbo.GetAccountByAccountId((uint) existingAccount.ParentAccountId);
                    if (parent?.Closed == true) {
                        var errorMessage = new ErrorResponseMessage(
                            new UpdatingActionMessage(account),
                            new ResourceWithPropertyMessage(account, account.Id),
                            new ParentAccountIsClosedReason(parent));
                        return new ApiResponse2<AccountDto>(_mapper.Map<AccountDto>(existingAccount), ApiResponseCodesEnum.DuplicateResource, errorMessage);
                    }
                }

            }

            var updatedAccount = await _accountServiceDbo.UpdateAccount(account);

            return new ApiResponse2<AccountDto>(_mapper.Map<AccountDto>(updatedAccount));
        }

        /// <inheritdoc cref="IAccountService.CloseAccount"/>
        public async Task<ApiResponse2<ListResponse<AccountDto>>> CloseAccount(AccountResourceIdentifier accountId) 
        {
            var accountToClose = await _accountServiceDbo.GetAccountByAccountId(accountId.Id);
            if (accountToClose == null) {    
                var errorMessage = new ErrorResponseMessage(
                            new ClosingActionMessage(accountToClose),
                            new ResourceWithPropertyMessage(accountToClose, accountToClose.Id),
                            new NotFoundReason());
                return new ApiResponse2<ListResponse<AccountDto>>(ApiResponseCodesEnum.ResourceNotFound, errorMessage);
            };

            if (accountToClose.Closed == true) {
                var errorMessage = new ErrorResponseMessage(
                            new ClosingActionMessage(accountToClose),
                            new ResourceWithPropertyMessage(accountToClose, accountToClose.Id),
                            new AlreadyClosedReason());
                return new ApiResponse2<ListResponse<AccountDto>>(new ListResponse<AccountDto>(new List<AccountDto> {{_mapper.Map<AccountDto>(accountToClose)}}), ApiResponseCodesEnum.InternalError, errorMessage);
            }

            var children = await _accountServiceDbo.GetChildrenByAccountId(accountId.Id);
            var accessibleChildren = await FilterAccessibleAccounts(children);
            if (accessibleChildren.Count() != children.Count()) {
                var errorMessage = new ErrorResponseMessage(
                            new ClosingActionMessage(accountToClose),
                            new ResourceWithPropertyMessage(accountToClose, accountToClose.Id),
                            new UnauthorizedToAccessChildrenReason());
                return new ApiResponse2<ListResponse<AccountDto>>(ApiResponseCodesEnum.Unauthorized, errorMessage);
            }

            var accountsClosed = await _accountServiceDbo.CloseAccount(accountId.Id);
            if (!accountsClosed.Any()) {
                var errorMessage = new ErrorResponseMessage(
                            new ClosingActionMessage(accountToClose),
                            new ResourceWithPropertyMessage(accountToClose, accountToClose.Id),
                            new NotClosedReason());
                return new ApiResponse2<ListResponse<AccountDto>>(ApiResponseCodesEnum.InternalError, errorMessage );
            }

            return new ApiResponse2<ListResponse<AccountDto>>(
                new ListResponse<AccountDto>(_mapper.Map<List<AccountDto>>(accountsClosed)),
                ApiResponseCodesEnum.Success,
                new AccountsClosedResponseMessage(accountsClosed));
 
        }

        private async Task<IEnumerable<Account>> FilterAccessibleAccounts(IEnumerable<Account> accounts) {
            if (accounts == null) { return null; }
            return (
                await Task.WhenAll(accounts?.Select(async (account) => {
                    return new AccountsWithAccess {
                        Account = account,
                        HasAccess = (await _authorizationService.AuthorizeAsync(_context.HttpContext.User, account, "CanAccessResourcePolicy" )).Succeeded
                    };
                    })
                ))
                ?.Where(account => account.HasAccess == true)
                ?.Select(a => a.Account);
        }
    
    }
    public class AccountsWithAccess {
        public Account Account { get; set; }
        public bool HasAccess { get; set; }
    }
}
