using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using finance_app.Types.Repositories.Accounts;
using finance_app.Types.Services.V1.Interfaces;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.Models.ResourceIdentifiers;
using AutoMapper;
using System;
using finance_app.Types.Services.V1.ResponseMessages;
using finance_app.Types.Services.V1.Services.Accounts.ResponseMessages.Reasons;
using finance_app.Types.Services.V1.Services.Accounts.ResponseMessages;
using finance_app.Types.Services.V1.ResponseMessages.ActionMessages;
using finance_app.Types.Services.V1.ResponseMessages.ResourcesMessages;
using finance_app.Types.Services.V1.ResponseMessages.ReasonMessages;
using Microsoft.EntityFrameworkCore;
using finance_app.Types.Services.V1.Authorization;
using finance_app.Types.Repositories.FinanceApp;

namespace finance_app.Types.Services.V1.Accounts {
    public class AccountService : IAccountService {
        private readonly IAccountRepository _accountRepository;
        private readonly  FinanceAppContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly IFinanceAppAuthorizationService _financeAppAuthorizationService;
        private readonly IHttpContextAccessor _context;


        public AccountService(IMapper mapper, IAccountRepository accountRepository,
                             IAuthorizationService authorizationService,
                             IFinanceAppAuthorizationService financeAppAuthorizationService,
                             FinanceAppContext dbContext,
                             IHttpContextAccessor context) {

            _accountRepository = accountRepository;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _financeAppAuthorizationService = financeAppAuthorizationService;
            _dbContext = dbContext;
            _context = context;
        }

        /// <inheritdoc cref="IAccountService.GetAccounts"/>
        public async Task<ApiResponse<ListResponse<AccountDto>>> GetAccounts(UserResourceIdentifier userId) {
            
            var accounts = await _dbContext.Accounts
                                .Where(a => a.UserId == userId.Id)
                                .AsNoTracking()
                                .ToListAsync();

            var accessibleAccounts = (await _financeAppAuthorizationService
                                        .Filter(accounts, AuthorizationPolicies.CanAccessResource))
                                        .ToList();

            var ret = new ListResponse<AccountDto>(_mapper.Map<List<AccountDto>>(accessibleAccounts)) {
                ExcludedItems = accounts.Count() - accessibleAccounts.Count()
            };


            return new ApiResponse<ListResponse<AccountDto>>(ret);
        }


        /// <inheritdoc cref="IAccountService.GetAccount"/>
        public async Task<ApiResponse<AccountDto>> GetAccount(AccountResourceIdentifier accountId) {
            
            var account = await _dbContext.Accounts
                                    .Where(a => a.Id == accountId.Id)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync();

            if (await _financeAppAuthorizationService.Authorize(account, AuthorizationPolicies.CanAccessResource)) {
                var errorMessage = new ErrorResponseMessage(
                    new GettingActionMessage(account),
                    new ResourceWithPropertyMessage(account, "Id",  account.Id),
                    new UnauthorizedToAccessResourceReason());
                return new ApiResponse<AccountDto>(ApiResponseCodesEnum.Unauthorized, errorMessage);
            }

            return new ApiResponse<AccountDto>(_mapper.Map<AccountDto>(account));
        }

        /// <inheritdoc cref="IAccountService.GetChildren"/>
        public async Task<ApiResponse<ListResponse<AccountDto>>> GetChildren(AccountResourceIdentifier accountId) {

            var accounts = await _dbContext.Accounts
                                    .Where(a => a.ParentAccountId == accountId.Id)
                                    .AsNoTracking()
                                    .ToListAsync();

            var accessibleAccounts = (await _financeAppAuthorizationService
                                        .Filter(accounts, AuthorizationPolicies.CanAccessResource))
                                        .ToList();

            var ret = new ListResponse<AccountDto>(_mapper.Map<List<AccountDto>>(accessibleAccounts)) {
                ExcludedItems = accounts.Count() - accessibleAccounts.Count()
            };

            return new ApiResponse<ListResponse<AccountDto>>(ret);

        }

        /// <inheritdoc cref="IAccountService.GetPaginatedAccounts"/>
        public async Task<ApiResponse<ListResponse<AccountDto>>> GetPaginatedAccounts(UserResourceIdentifier userId, PaginationInfo pageInfo) {
            if (userId == null) { throw new ArgumentNullException(nameof(UserResourceIdentifier)); }

            var offset = (int)pageInfo.PageNumber - 1;

            var accounts = await _dbContext.Accounts
                                    .Where(a => a.UserId == userId.Id)
                                    .OrderByDescending(x => x.Name)
                                    .Skip(offset * (int)pageInfo.ItemsPerPage)
                                    .Take((int)pageInfo.ItemsPerPage)
                                    .AsNoTracking()
                                    .ToListAsync();

            var accessibleAccounts = (await _financeAppAuthorizationService
                                        .Filter(accounts, AuthorizationPolicies.CanAccessResource))
                                        .ToList();

            var ret = new ListResponse<AccountDto>(_mapper.Map<List<AccountDto>>(accessibleAccounts)) {
                ExcludedItems = accounts?.Count() - accessibleAccounts?.Count() ?? 0
            };

            return new ApiResponse<ListResponse<AccountDto>>(ret);
        }

        /// <inheritdoc cref="IAccountService.CreateAccount"/>
        public async Task<ApiResponse<AccountDto>> CreateAccount(Account account) {
            var existingAccount = await _dbContext.Accounts
                                    .Where(a => a.Name == account.Name)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync();

            if (existingAccount != null) {
                var errorMessage = new ErrorResponseMessage(
                    new CreatingActionMessage(account),
                    new ResourceWithPropertyMessage(existingAccount, "Name",  existingAccount.Name),
                    new PropertyAlreadyExistsReason());
                return new ApiResponse<AccountDto>(_mapper.Map<AccountDto>(existingAccount), ApiResponseCodesEnum.DuplicateResource, errorMessage);
            }

            await _dbContext.Accounts.AddAsync(account);
            await _dbContext.SaveChangesAsync();

            return new ApiResponse<AccountDto>(_mapper.Map<AccountDto>(account));
        }


        /// <inheritdoc cref="IAccountService.UpdateAccount"/>
        public async Task<ApiResponse<AccountDto>> UpdateAccount(Account account) {

            var accountToUpdate = await _dbContext.Accounts
                                    .Where(a => a.Id == account.Id)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync();
            if (accountToUpdate == null) {
                var errorMessage = new ErrorResponseMessage(
                    new UpdatingActionMessage(account),
                    new ResourceWithPropertyMessage(accountToUpdate, "Id",  accountToUpdate.Id),
                    new PropertyAlreadyExistsReason());
                return new ApiResponse<AccountDto>(ApiResponseCodesEnum.ResourceNotFound, errorMessage);
            }

            if (accountToUpdate.Description != account.Description) {
                accountToUpdate.Description = account.Description;
                _dbContext.Entry(accountToUpdate).Property(x => x.Description).IsModified = true;

            }

            #region Updating Name 
            // if you are changing the account's name,
            // verify that no other account has the same name
            if (accountToUpdate.Name != account.Name) {
                _dbContext.Entry(accountToUpdate).Property(x => x.Name).IsModified = true;
                var duplicateName = await _dbContext.Accounts
                                    .Where(a => a.Name == account.Name)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync();
                if (duplicateName != null) {
                    var errorMessage = new ErrorResponseMessage(
                        new UpdatingActionMessage(account),
                        new ResourceWithPropertyMessage(duplicateName, "Name",  duplicateName.Name),
                        new PropertyAlreadyExistsReason());
                    return new ApiResponse<AccountDto>(_mapper.Map<AccountDto>(duplicateName), ApiResponseCodesEnum.DuplicateResource, errorMessage);
                }
                _dbContext.Entry(accountToUpdate).Property(x => x.Name).IsModified = true;
                accountToUpdate.Name = account.Name;
            }
            #endregion Updating Name

            #region Updating Closed Status
            // If the 'Closed' Status is changing.
            if (accountToUpdate.Closed != account.Closed) {
                // And you are trying to close an account
                if (account.Closed == true) {

                    // You need access to all of it's children all of it's children need to be closed.
                    var children = await _dbContext.Accounts
                                    .Where(a => a.ParentAccountId == account.Id)
                                    .AsNoTracking()
                                    .ToListAsync();

                    var accessibleChildren = (await _financeAppAuthorizationService
                                                .Filter(children, AuthorizationPolicies.CanAccessResource))
                                                .ToList();

                    // Cannot Close. 
                    // There are some children that you cannot access. 
                    if (accessibleChildren.Count() != children.Count()) {
                        var errorMessage = new ErrorResponseMessage(
                            new UpdatingActionMessage(account),
                            new ResourceWithPropertyMessage(account, "Id",  account.Id),
                            new UnauthorizedToAccessChildrenReason());
                        return new ApiResponse<AccountDto>(ApiResponseCodesEnum.Unauthorized, errorMessage);
                    }

                    // If Any Children are open, you cannot close the account.
                    var openChildren = children.Where(child => child.Closed == false).ToList();
                    if (openChildren.Count > 0) {
                        var errorMessage = new ErrorResponseMessage(
                            new UpdatingActionMessage(account),
                            new ResourceWithPropertyMessage(account, "Id",  account.Id),
                            new ChildAccountsNotClosedReason(openChildren));
                        return new ApiResponse<AccountDto>(_mapper.Map<AccountDto>(accountToUpdate), ApiResponseCodesEnum.DuplicateResource, errorMessage);
                    }

                }

                // If you are opening an account, its parent must be open
                if (account.Closed == false && accountToUpdate.ParentAccountId != null) {
                    var parent = await _dbContext.Accounts
                                    .Where(a => a.ParentAccountId == account.Id)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync();
                    if (parent?.Closed == true) {
                        var errorMessage = new ErrorResponseMessage(
                            new UpdatingActionMessage(account),
                            new ResourceWithPropertyMessage(account, "Id",  account.Id),
                            new ParentAccountIsClosedReason(parent));
                        return new ApiResponse<AccountDto>(_mapper.Map<AccountDto>(accountToUpdate), ApiResponseCodesEnum.DuplicateResource, errorMessage);
                    }
                }
                _dbContext.Entry(accountToUpdate).Property(x => x.Closed).IsModified = true;
                accountToUpdate.Closed = account.Closed;
            }
            #endregion Updating Closed Status


            _dbContext.Accounts.Attach(accountToUpdate);
            await _dbContext.SaveChangesAsync();

            return new ApiResponse<AccountDto>(_mapper.Map<AccountDto>(accountToUpdate));
        }

        /// <inheritdoc cref="IAccountService.CloseAccount"/>
        public async Task<ApiResponse<ListResponse<AccountDto>>> CloseAccount(AccountResourceIdentifier accountId) {
            var accountToClose = await _dbContext.Accounts
                                    .Where(a => a.Id == accountId.Id)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync();

            if (accountToClose == null) {
                var errorMessage = new ErrorResponseMessage(
                            new ClosingActionMessage(accountToClose),
                            new ResourceWithPropertyMessage(accountToClose, "Id",  accountToClose.Id),
                            new NotFoundReason());
                return new ApiResponse<ListResponse<AccountDto>>(ApiResponseCodesEnum.ResourceNotFound, errorMessage);
            };

            if (accountToClose.Closed == true) {
                var errorMessage = new ErrorResponseMessage(
                            new ClosingActionMessage(accountToClose),
                            new ResourceWithPropertyMessage(accountToClose, "Id",  accountToClose.Id),
                            new AlreadyClosedReason());
                return new ApiResponse<ListResponse<AccountDto>>(new ListResponse<AccountDto>(new List<AccountDto> { { _mapper.Map<AccountDto>(accountToClose) } }), ApiResponseCodesEnum.InternalError, errorMessage);
            }

            // Ensure the user has access to all of the account's children
            var children = await _dbContext.Accounts
                                    .Where(a => a.ParentAccountId == accountToClose.Id)
                                    .AsNoTracking()
                                    .ToListAsync();

            var accessibleChildren = (await _financeAppAuthorizationService
                                        .Filter(children, AuthorizationPolicies.CanAccessResource))
                                        .ToList();

            // Cannot Close account, you don't have access to it's children                      
            if (accessibleChildren.Count() != children.Count()) {
                var errorMessage = new ErrorResponseMessage(
                            new ClosingActionMessage(accountToClose),
                            new ResourceWithPropertyMessage(accountToClose, "Id",  accountToClose.Id),
                            new UnauthorizedToAccessChildrenReason());
                return new ApiResponse<ListResponse<AccountDto>>(ApiResponseCodesEnum.Unauthorized, errorMessage);
            }

            // Lets use a stored proc for this since it will be easier to close the account 
            // and all of it's children
            var accountsClosed = await _accountRepository.CloseAccount(accountId.Id);
            if (!accountsClosed.Any()) {
                var errorMessage = new ErrorResponseMessage(
                            new ClosingActionMessage(accountToClose),
                            new ResourceWithPropertyMessage(accountToClose, "Id",  accountToClose.Id),
                            new NotClosedReason());
                return new ApiResponse<ListResponse<AccountDto>>(ApiResponseCodesEnum.InternalError, errorMessage);
            }

            return new ApiResponse<ListResponse<AccountDto>>(
                new ListResponse<AccountDto>(_mapper.Map<List<AccountDto>>(accountsClosed)),
                ApiResponseCodesEnum.Success,
                new AccountsClosedResponseMessage(accountsClosed));

        }

    }
}
