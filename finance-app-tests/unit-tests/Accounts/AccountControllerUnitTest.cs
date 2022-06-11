using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Extensions.Logging;
using AutoMapper;
using finance_app.Controllers.V1;
using finance_app.Types.Services.V1.Interfaces;
using finance_app.Types.DataContracts.V1.Requests.Accounts;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Dtos.Enums;
using finance_app.Types.Repositories.Account;
using finance_app.Types.Models.ResourceIdentifiers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using finance_app.Types.DataContracts.V1.Responses;
using System.Collections.Generic;

namespace unit_tests.Accounts
{
    [TestClass]
    public class AccountControllerUnitTest
    {
        private Mock<ILogger<AccountsController>> _logger; 
        private Mock<IMapper> _mapper; 
        private Mock<IAccountService> _accountService;

        private static readonly UserResourceIdentifier TEST_USER_ID = new()
        {
            Id = 1
        };

        private static readonly AccountResourceIdentifier TEST_ACCOUNT_ID = new()
        {
            Id = 1
        };

        #region requests

        private static CreateAccountRequest GetTestCreateAccountRequest()
        {

            return new CreateAccountRequest
            {
                CurrencyCode = "CAD",
                Description = "test description",
                Name = "test name",
                ParentAccountId = 0,
                Type = new EnumDto<AccountTypeDtoEnum>(AccountTypeDtoEnum.Expense)
            };
        }

        private static Account GetTestAccountToCreate()
        {
            var accountRequest = GetTestCreateAccountRequest();
            return new Account
            {
                Type = AccountTypeEnum.Expense,
                Balance = 0,
                Closed = false,
                CurrencyCode = "Cad",
                Description = accountRequest.Description,
                Name = accountRequest.Name,
                ParentAccountId = null,
            };
        }

        private static PostAccountRequest GetTestPostAccountRequest()
        {
            return new PostAccountRequest
            {
                Closed = false,
                Description = "Test Description",
                Name = "Test Name"
            };
        }

        private static Account GetTestAccountToPost()
        {
            var accountRequest = GetTestPostAccountRequest();
            return new Account
            {
                Closed = accountRequest.Closed,
                Description = accountRequest.Description,
                Name = accountRequest.Name
            };
        }

        #endregion

        private AccountsController controller;

        [TestInitialize]
        public void Setup() {

            _logger = new Mock<ILogger<AccountsController>>();
            _mapper = new Mock<IMapper>();
            _accountService = new Mock<IAccountService>();

            _mapper.Setup(x => x.Map<Account>(It.IsAny<CreateAccountRequest>()))
                .Returns(GetTestAccountToCreate());

            controller = new AccountsController(_logger.Object, _accountService.Object, _mapper.Object);

        }

        #region GetAccounts

        [TestMethod]
        public async Task GetAccounts_PageInfoNotNull_Expect_GetPaginatedAccounts()
        {
            // Arrange
            var request = new GetAccountsRequest {
                PageInfo = new PaginationInfo {
                    ItemsPerPage = 5,
                    PageNumber = 5
                }
            };
            var accountsList = new ListResponse<AccountDto>(
                new List<AccountDto>
                {
                    { new AccountDto() }
                });

            _mapper.Setup(x => x.Map<int>(It.IsAny<ApiResponseCodesEnum>()))
                .Returns(200);
            _accountService.Setup(x => x.GetPaginatedAccounts(It.IsAny<UserResourceIdentifier>(), It.IsAny<PaginationInfo>()))
                .ReturnsAsync(new ApiResponse<ListResponse<AccountDto>>(accountsList));

            // Act
            var response = await controller.GetAccounts(TEST_USER_ID, request);
            // Assert
            _accountService.Verify(x => x.GetPaginatedAccounts(
                                            It.IsAny<UserResourceIdentifier>(),
                                            It.IsAny<PaginationInfo>()), Times.Once);
        }

        [TestMethod]
        public async Task GetAccounts_PageInfoNull_Expect_GetPaginatedAccounts()
        {
            // Arrange
            var request = new GetAccountsRequest
            {
                PageInfo = null
            };

            var accountsList = new ListResponse<AccountDto>(
                new List<AccountDto>
                {
                    { new AccountDto() }
                });

            _mapper.Setup(x => x.Map<int>(It.IsAny<ApiResponseCodesEnum>()))
                .Returns(200);
            _accountService.Setup(x => x.GetPaginatedAccounts(It.IsAny<UserResourceIdentifier>(), It.IsAny<PaginationInfo>()))
                .ReturnsAsync(new ApiResponse<ListResponse<AccountDto>>(accountsList));

            // Act
            var response = await controller.GetAccounts(TEST_USER_ID, request);
            // Assert
            _accountService.Verify(x => x.GetPaginatedAccounts(
                                            It.IsAny<UserResourceIdentifier>(),
                                            It.IsAny<PaginationInfo>()), Times.Never);
            _accountService.Verify(x => x.GetAccounts(
                                            It.IsAny<UserResourceIdentifier>()), Times.Once);
        }

        #endregion GetAccounts

        #region PostAccounts
        [TestMethod]
        public async Task PostAccounts_Expect_CallsCreateAccount()
        {
            // Arrange
            var request = GetTestCreateAccountRequest();

            var expectedAccount = GetTestAccountToCreate();
            expectedAccount.UserId = TEST_USER_ID.Id;


            // Act
            var response = await controller.CreateAccount(TEST_USER_ID, request);

            // Assert
            _accountService.Verify(x => x.CreateAccount(It.IsAny<Account>()), Times.Once);
        }

        [TestMethod]
        public async Task PostAccounts_Expect_CallsCreateAccountWithTheExpectedAccount()
        {
            // Arrange
            var request = GetTestCreateAccountRequest();
            var expectedAccount = GetTestAccountToCreate();
            expectedAccount.UserId = TEST_USER_ID.Id;

            Account calledWith = null;

            _accountService.Setup(x => x.CreateAccount(It.IsAny<Account>()))
                .Callback<Account>(account => calledWith = account );


            // Act
            var response = await controller.CreateAccount(TEST_USER_ID, request);

            // Assert
            Assert.AreEqual(expectedAccount.Id, calledWith.Id);
            Assert.AreEqual(expectedAccount.UserId, calledWith.UserId);
            Assert.AreEqual(expectedAccount.Name, calledWith.Name);
            Assert.AreEqual(expectedAccount.Description, calledWith.Description);
            Assert.AreEqual(expectedAccount.CurrencyCode, calledWith.CurrencyCode);
            Assert.AreEqual(expectedAccount.Type, calledWith.Type);
            Assert.AreEqual(expectedAccount.ParentAccountId, calledWith.ParentAccountId);
        }
        #endregion PostAccounts

        #region DeleteAccount
        
        [TestMethod]
        public async Task DeleteAccount_Expect_CallsDeleteAccount()
        {
            // Arrange
            var accountsList = new ListResponse<AccountDto> (
                new List<AccountDto>
                {
                    { new AccountDto() }
                });

            _mapper.Setup(x => x.Map<int>(It.IsAny<ApiResponseCodesEnum>()))
                .Returns(200);
            _accountService.Setup(x => x.CloseAccount(It.IsAny<AccountResourceIdentifier>()))
                .ReturnsAsync(new ApiResponse<ListResponse<AccountDto>>(accountsList));
            // Act
            var response = await controller.DeleteAccount(TEST_ACCOUNT_ID);

            // Assert
            _accountService.Verify(x => x.CloseAccount(It.IsAny<AccountResourceIdentifier>()), Times.Once);
            
        }
        #endregion DeleteAccount

        #region GetAccount
        [TestMethod]
        public async Task GetAccount_Expect_CallsGetAccount()
        {
            // Arrange
            var account = new AccountDto();;

            _mapper.Setup(x => x.Map<int>(It.IsAny<ApiResponseCodesEnum>()))
                .Returns(200);
            _accountService.Setup(x => x.GetAccount(It.IsAny<AccountResourceIdentifier>()))
                .ReturnsAsync(new ApiResponse<AccountDto>(account));
            // Act
            var response = await controller.GetAccount(TEST_ACCOUNT_ID);

            // Assert
            _accountService.Verify(x => x.GetAccount(It.IsAny<AccountResourceIdentifier>()), Times.Once);

        }
        #endregion GetAccount

        #region GetChildren
        [TestMethod]
        public async Task GetChildren_Expect_CallsDeleteAccount()
        {
            // Arrange
            // Act
            var response = await controller.GetChildren(TEST_ACCOUNT_ID);

            // Assert
            _accountService.Verify(x => x.GetChildren(It.IsAny<AccountResourceIdentifier>()), Times.Once);

        }
        #endregion GetChildren

        #region PostAccount
        [TestMethod]
        public async Task PostAccount_ExpectCallsUpdateAccount()
        {
            // Arrange
            var request = GetTestPostAccountRequest();
            var mappedAccount = new Account()
            {
                Id = TEST_ACCOUNT_ID.Id,
                Name = request.Name,
                Description = request.Description,
                Closed = request.Closed
                
            };
            var expectedAccount = GetTestAccountToPost();
            expectedAccount.Id = TEST_ACCOUNT_ID.Id;

            Account calledWith = null;

            _mapper.Setup(x => x.Map<Account>(It.IsAny<PostAccountRequest>()))
                .Returns(mappedAccount);
            _accountService.Setup(x => x.UpdateAccount(It.IsAny<Account>()))
                .Callback<Account>(account => calledWith = account);
            // Act
            var response = await controller.PostAccount(TEST_ACCOUNT_ID, request);

            // Assert
            _accountService.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
        }

        [TestMethod]
        public async Task PostAccount_ExpectCallsUpdateAccountWithTheExpectedAccount()
        {
            // Arrange
            var request = GetTestPostAccountRequest();
            var mappedAccount = GetTestAccountToPost();
            mappedAccount.Id = TEST_ACCOUNT_ID.Id;

            _mapper.Setup(x => x.Map<Account>(It.IsAny<PostAccountRequest>()))
                .Returns(mappedAccount);
            _accountService.Setup(x => x.UpdateAccount(mappedAccount))
                .Verifiable();

            // Act
            var response = await controller.PostAccount(TEST_ACCOUNT_ID, request);

            _accountService.Verify(x => x.UpdateAccount(mappedAccount), Times.Once);

            // Assert
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }
        #endregion PostAccount

    }
}
