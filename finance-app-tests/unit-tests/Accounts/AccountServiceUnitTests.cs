using AutoMapper;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.Models.ResourceIdentifiers;
using finance_app.Types.Repositories.Account;
using finance_app.Types.Services.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace unit_tests.Accounts
{
    [TestClass]
    public class AccountServiceUnitTests
    {
        private Mock<IMapper> _mapper;
        private Mock<IAccountRepository> _accountServiceDbo;
        private Mock<IAuthorizationService> _authorizationService;
        private Mock<IHttpContextAccessor> _httpContextAccessor;


        private AccountService accountService;

        #region Setup

        [TestInitialize]
        public void Setup()
        {

            _mapper = new Mock<IMapper>();
            _accountServiceDbo = new Mock<IAccountRepository>();
            _authorizationService = new Mock<IAuthorizationService>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();

            var context = new DefaultHttpContext();
            _httpContextAccessor.Setup(x =>x.HttpContext).Returns(context);


            accountService = new AccountService(_mapper.Object, _accountServiceDbo.Object, _authorizationService.Object, _httpContextAccessor.Object);
        }

        [TestCleanup]
        public void TearDown()
        {

            _mapper.Verify();
            _accountServiceDbo.Verify();
            _authorizationService.Verify();
            _httpContextAccessor.Verify();

            accountService = new AccountService(_mapper.Object, _accountServiceDbo.Object, _authorizationService.Object, _httpContextAccessor.Object);
        }

        #endregion Setup

        #region GetAccount
        [TestMethod]
        [TestProperty ("GetAccount","")]
        [ExpectedException(typeof(Exception))]
        public async Task GetAccount_RepositoryThrows_Expect_ThrowsException()
        {
            // Arrange
            var accountId = new AccountResourceIdentifier(77);
            _accountServiceDbo.Setup(x => x.GetAccountByAccountId(accountId.Id))
                .ThrowsAsync(new Exception("GetAccountById Threw an Exception"))
                .Verifiable();
            
            // Act
            var response = await accountService.GetAccount(accountId);

        }

        [TestMethod]
        [TestProperty ("GetAccount","")]
        public async Task GetAccount_UnauthourizedToAccess_Expect_ReturnsUnauthorizd()
        {
            // Arrange
            var accountId = new AccountResourceIdentifier(77);
            var policy = "CanAccessResourcePolicy";

            var account = new Account() {
                Id = 1337,
                User_Id = 15, 
                Name = "TestAccount",
                Balance = 1337,
                Closed = false,
                Currency_Code = "CAD",
                Description = "A test account",
                Type = AccountTypeEnum.Asset,
            };

            _accountServiceDbo.Setup(x => x.GetAccountByAccountId(accountId.Id))
                .ReturnsAsync(account)
                .Verifiable();
            _authorizationService.Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), account, policy))
                .ReturnsAsync(AuthorizationResult.Failed());
            
            // Act
            var response = await accountService.GetAccount(accountId);

            // Assert
            Assert.IsInstanceOfType(response, typeof(ApiResponse<AccountDto>));
            Assert.AreEqual(ApiResponseCodesEnum.Unauthorized, response.ResponseCode);
        }

        [TestMethod]
        [TestProperty ("GetAccount","")]
        public async Task GetAccount_Success_Expect_SuccessWithAccount()
        {
            // Arrange
            var accountId = new AccountResourceIdentifier(77);
            var policy = "CanAccessResourcePolicy";

            var account = new Account();
            var accountDto = new AccountDto();

            _accountServiceDbo.Setup(x => x.GetAccountByAccountId(accountId.Id))
                .ReturnsAsync(account)
                .Verifiable();
            _authorizationService.Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), account, policy))
                .ReturnsAsync(AuthorizationResult.Success());
            _mapper.Setup(x => x.Map<AccountDto>(account))
                .Returns(accountDto);
            
            // Act
            var response = await accountService.GetAccount(accountId);

            // Assert
            Assert.IsInstanceOfType(response, typeof(ApiResponse<AccountDto>));
            Assert.AreEqual(ApiResponseCodesEnum.Success, response.ResponseCode);
            Assert.AreEqual(accountDto, response.Data);
        }

        [TestMethod]
        [TestProperty ("GetAccount","")]
        public async Task GetAccount_RepositoryReturnsNull_Expect_SuccessWithNullAccount()
        {
            // Arrange
            var accountId = new AccountResourceIdentifier(77);
            var policy = "CanAccessResourcePolicy";

            var account = new Account();
            var accountDto = new AccountDto();

            _accountServiceDbo.Setup(x => x.GetAccountByAccountId(accountId.Id))
                .ReturnsAsync((Account) null)
                .Verifiable();
            _authorizationService.Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), null, policy))
                .ReturnsAsync(AuthorizationResult.Success());
            _mapper.Setup(x => x.Map<AccountDto>(account))
                .Returns(accountDto);
            
            // Act
            var response = await accountService.GetAccount(accountId);

            // Assert
            Assert.IsInstanceOfType(response, typeof(ApiResponse<AccountDto>));
            Assert.AreEqual(ApiResponseCodesEnum.Success, response.ResponseCode);
            Assert.IsNull(response.Data);
        }

        [TestMethod]
        [TestProperty ("GetAccount","")]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetAccount_NullUserResourceIdentifier_Expect_ThrowsException()
        {
            // Arrange
            AccountResourceIdentifier accountId = null;

            // Act
            var response = await accountService.GetAccount(accountId);
        }

        #endregion

        #region GetAccounts
        [TestMethod]
        [TestProperty ("GetAccounts","")]
        public async Task GetAccounts_RepositoryThrows_Expect_ThrowsException()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("GetAccounts","")]
        public async Task GetAccounts_UnauthourizedToAccess_Expect_FilteredAccounts()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("GetAccounts","")]
        public async Task GetAccounts_Success_Expect_SuccessWithAccounts()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("GetAccounts","")]
        public async Task GetAccounts_RepositoryReturnsNull_Expect_SuccessWithNullAccounts()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("GetAccounts","")]
        public async Task GetAccounts_Expect_CallsRepositoryWithExpectedData()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("GetAccounts","")]
        public async Task GetAccounts_NullUserResourceIdentifier_Expect_CallsRepositoryWithNull()
        {
            Assert.Fail();
        }
        #endregion

        #region GetPaginatedAccounts

        [TestMethod]
        [TestProperty ("GetPaginatedAccounts","")]
        public async Task GetPaginatedAccounts_InvalidPageNumber_Expect_BadRequest()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("GetPaginatedAccounts","")]
        public async Task GetPaginatedAccounts_InvalidItemsPerPage_Expect_Success_With_DefaultItemsPerPage()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("GetPaginatedAccounts","")]
        public async Task GetPaginatedAccounts_RepositoryThrows_Expect_ThrowsException()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("GetPaginatedAccounts","")]
        public async Task GetPaginatedAccounts_UnauthourizedToAccessAccounts_Expect_FilteredAccounts()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("GetPaginatedAccounts","")]
        public async Task GetPaginatedAccounts_Success_Expect_SuccessWithAccounts()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("GetPaginatedAccounts","")]
        public async Task GetPaginatedAccounts_RepositoryReturnsNull_Expect_SuccessWithNullAccounts()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("GetPaginatedAccounts","")]
        public async Task GetPaginatedAccounts_Expect_CallsRepositoryWithExpectedData()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("GetPaginatedAccounts","")]
        public async Task GetPaginatedAccounts_NullUserResourceIdentifier_Expect_CallsRepositoryWithNull()
        {
            Assert.Fail();
        }
        #endregion

        #region GetChildren
        [TestMethod]
        [TestProperty ("GetChildren","")]
        public async Task GetChildren_RepositoryThrows_Expect_ThrowsException()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("GetChildren","")]
        public async Task GetChildren_UnauthourizedToAccessParent_Expect_Unauthorized()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("GetChildren","")]
        public async Task GetChildren_UnauthourizedToAccessSomeChildren_Expect_FilteredAccounts()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("GetChildren","")]
        public async Task GetChildren_Success_Expect_SuccessWithAccounts()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("GetChildren","")]
        public async Task GetChildren_NoChildren_Expect_SuccessWithNullAccounts()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("GetChildren","")]
        public async Task GetChildren_Expect_CallsRepositoryWithExpectedData()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("GetChildren","")]
        public async Task GetChildren_NullUserResourceIdentifier_Expect_CallsRepositoryWithNull()
        {
            Assert.Fail();
        }
        #endregion

        #region CreateAccount

        [TestMethod]
        [TestProperty ("CreateAccount","")]
        public async Task CreateAccount_AccountAlreadyExists_Expect_DuplicateResourceWithMessage()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("CreateAccount","")]
        public async Task CreateAccount_AccountAlreadyExists_Expect_DuplicateAccount()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("CreateAccount","")]
        public async Task CreateAccount_AccountAlreadyExists_Expect_NoDuplicateAccountIfUserDoesntHaveAccess()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("CreateAccount","")]
        public async Task CreateAccount_GetAccountThrows_Expect_ThrowsException()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("CreateAccount","")]
        public async Task CreateAccount_CreateAccountThrows_Expect_ThrowsException()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("CreateAccount","")]
        public async Task CreateAccount_Success_Expect_SuccessCreatedAccount()
        {
            Assert.Fail();
        }


        [TestMethod]
        [TestProperty ("CreateAccount","")]
        public async Task CreateAccount_Expect_CallsRepositoryWithExpectedData()
        {
            Assert.Fail();
        }

        #endregion

        #region UpdateAccount

        [TestMethod]
        [TestProperty ("UpdateAccount","")]
        public async Task UpdateAccount_AccountAlreadyExists_Expect_DuplicateResourceWithMessage()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("UpdateAccount","")]
        public async Task UpdateAccount_AccountAlreadyExists_Expect_DuplicateAccount()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("UpdateAccount","")]

        public async Task UpdateAccount_AccountAlreadyExists_Expect_NoDuplicateAccountIfUserDoesntHaveAccess()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("UpdateAccount","")]
        public async Task UpdateAccount_GetAccountThrows_Expect_ThrowsException()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("UpdateAccount","")]
        public async Task UpdateAccount_CreateAccountThrows_Expect_ThrowsException()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("UpdateAccount","")]
        public async Task UpdateAccount_AccountNameIsChanging_Expect_ChecksForDuplicateName()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("UpdateAccount","")]
        public async Task UpdateAccount_AccountNameIsNotChanging_Expect_DoesNotCheckForDuplicateName()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("UpdateAccount","")]
        public async Task UpdateAccount_CheckForDuplicateNameThrows_Expect_ThrowsException()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("UpdateAccount","")]
        public async Task UpdateAccount_ClosingAccount_GetChildrenThrows_Expect_ThrowsException()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("UpdateAccount","")]
        public async Task UpdateAccount_ClosingAccount_DontHaveAccessToAllAccounts_Expect_Unauthorized()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("UpdateAccount","")]
        public async Task UpdateAccount_ClosingAccount_SomeChildAccountsAreOpen_Expect_DuplicateResource()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("UpdateAccount","")]
        public async Task UpdateAccount_OpeningAccount_Expect_GetsParent()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("UpdateAccount","")]
        public async Task UpdateAccount_OpeningAccountDoesNotHaveparent_Expect_DoesNotGetGetsParent()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("UpdateAccount","")]
        public async Task UpdateAccount_ParentClosed_Expect_DuplicateResource()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("UpdateAccount","")]
        public async Task UpdateAccount_ValidParameters_Expect_Success()
        {
            Assert.Fail();
        }

        #endregion

        #region CloseAccount

        [TestMethod]
        [TestProperty ("CloseAccount","")]
        public async Task CloseAccount_AccountDoesNotExist_Expect_ResourceNotFoundWithMessage()
        {
            Assert.Fail();
        }


        [TestMethod]
        [TestProperty ("CloseAccount","")]
        public async Task CloseAccount_AccountAlreadyExists_Expect_NoDuplicateAccountIfUserDoesntHaveAccess()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("CloseAccount","")]
        public async Task CloseAccount_GetAccountThrows_Expect_ThrowsException()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("CloseAccount","")]
        public async Task CloseAccount_AccountIsClosed_Expect_InternalErrorWithMessage()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("CloseAccount","")]
        public async Task CloseAccount_ClosingAccount_GetChildrenThrows_Expect_ThrowsException()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("CloseAccount","")]
        public async Task CloseAccount_ClosingAccount_DontHaveAccessToAllAccounts_Expect_Unauthorized()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("CloseAccount","")]
        public async Task CloseAccount_CreateAccountThrows_Expect_ThrowsException()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("CloseAccount","")]
        public async Task CloseAccount_AccountNotClosed_Expect_InternalErrorWithMessage()
        {
            Assert.Fail();
        }

        [TestMethod]
        [TestProperty ("CloseAccount","")]
        public async Task CloseAccount_Success_Expect_AccountsClosedWithMessage()
        {
            Assert.Fail();
        }

        #endregion
    }


}
