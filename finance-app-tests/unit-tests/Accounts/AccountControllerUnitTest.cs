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

namespace unit_tests.accounts
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
                Currency_Code = "Cad",
                Description = accountRequest.Description,
                Name = accountRequest.Name,
                Parent_Account_Id = null,
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
        public void GetAccounts_PageInfoNotNull_Expect_GetPaginatedAccounts()
        {
            // Arrange
            var request = new GetAccountsRequest {
                PageInfo = new PaginationInfo {
                    ItemsPerPage = 5,
                    PageNumber = 5
                }
            };

            // Act
            var response = controller.GetAccounts(TEST_USER_ID, request);
            // Assert
            _accountService.Verify(x => x.GetPaginatedAccounts(
                                            It.IsAny<UserResourceIdentifier>(),
                                            It.IsAny<PaginationInfo>()), Times.Once);
        }

        [TestMethod]
        public void GetAccounts_PageInfoNull_Expect_GetPaginatedAccounts()
        {
            // Arrange
            var request = new GetAccountsRequest
            {
                PageInfo = null
            };

            // Act
            var response = controller.GetAccounts(TEST_USER_ID, request);
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
        public void PostAccounts_Expect_CallsCreateAccount()
        {
            // Arrange
            var request = GetTestCreateAccountRequest();

            var expectedAccount = GetTestAccountToCreate();
            expectedAccount.User_Id = TEST_USER_ID.Id;


            // Act
            var response = controller.CreateAccount(TEST_USER_ID, request);

            // Assert
            _accountService.Verify(x => x.CreateAccount(It.IsAny<Account>()), Times.Once);
        }

        [TestMethod]
        public void PostAccounts_Expect_CallsCreateAccountWithTheExpectedAccount()
        {
            // Arrange
            var request = GetTestCreateAccountRequest();
            var expectedAccount = GetTestAccountToCreate();
            expectedAccount.User_Id = TEST_USER_ID.Id;

            Account calledWith = null;

            _accountService.Setup(x => x.CreateAccount(It.IsAny<Account>()))
                .Callback<Account>(account => calledWith = account );


            // Act
            var response = controller.CreateAccount(TEST_USER_ID, request);

            // Assert
            Assert.AreEqual(expectedAccount.Id, calledWith.Id);
            Assert.AreEqual(expectedAccount.User_Id, calledWith.User_Id);
            Assert.AreEqual(expectedAccount.Name, calledWith.Name);
            Assert.AreEqual(expectedAccount.Description, calledWith.Description);
            Assert.AreEqual(expectedAccount.Currency_Code, calledWith.Currency_Code);
            Assert.AreEqual(expectedAccount.Type, calledWith.Type);
            Assert.AreEqual(expectedAccount.Parent_Account_Id, calledWith.Parent_Account_Id);
        }
        #endregion PostAccounts

        #region DeleteAccount
        
        [TestMethod]
        public void DeleteAccount_Expect_CallsDeleteAccount()
        {
            // Arrange
            // Act
            var response = controller.DeleteAccount(TEST_ACCOUNT_ID);

            // Assert
            _accountService.Verify(x => x.CloseAccount(It.IsAny<AccountResourceIdentifier>()), Times.Once);
            
        }
        #endregion DeleteAccount

        #region GetAccount
        [TestMethod]
        public void GetAccount_Expect_CallsDeleteAccount()
        {
            // Arrange
            // Act
            var response = controller.GetAccount(TEST_ACCOUNT_ID);

            // Assert
            _accountService.Verify(x => x.GetAccount(It.IsAny<AccountResourceIdentifier>()), Times.Once);

        }
        #endregion GetAccount

        #region GetChildren
        [TestMethod]
        public void GetChildren_Expect_CallsDeleteAccount()
        {
            // Arrange
            // Act
            var response = controller.GetChildren(TEST_ACCOUNT_ID);

            // Assert
            _accountService.Verify(x => x.GetChildren(It.IsAny<AccountResourceIdentifier>()), Times.Once);

        }
        #endregion GetChildren

        #region PostAccount
        [TestMethod]
        public void PostAccount_ExpectCallsUpdateAccount()
        {
            // Arrange
            var request = GetTestPostAccountRequest();
            var expectedAccount = GetTestAccountToPost();
            expectedAccount.Id = TEST_ACCOUNT_ID.Id;

            Account calledWith = null;

            _accountService.Setup(x => x.CreateAccount(It.IsAny<Account>()))
                .Callback<Account>(account => calledWith = account);
            // Act
            var response = controller.PostAccount(TEST_ACCOUNT_ID, request);

            // Assert
            _accountService.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
        }

        [TestMethod]
        public void PostAccount_ExpectCallsUpdateAccountWithTheExpectedAccount()
        {
            // Arrange
            var request = GetTestPostAccountRequest();
            var expectedAccount = GetTestAccountToPost();
            expectedAccount.Id = TEST_ACCOUNT_ID.Id;

            Account calledWith = null;

            _accountService.Setup(x => x.CreateAccount(It.IsAny<Account>()))
                .Callback<Account>(account => calledWith = account);

            // Act
            var response = controller.PostAccount(TEST_ACCOUNT_ID, request);

            // Assert
            Assert.AreEqual(expectedAccount.Id, calledWith.Id);
            Assert.AreEqual(expectedAccount.User_Id, calledWith.User_Id);
            Assert.AreEqual(expectedAccount.Name, calledWith.Name);
            Assert.AreEqual(expectedAccount.Description, calledWith.Description);
            Assert.AreEqual(expectedAccount.Currency_Code, calledWith.Currency_Code);
            Assert.AreEqual(expectedAccount.Type, calledWith.Type);
            Assert.AreEqual(expectedAccount.Parent_Account_Id, calledWith.Parent_Account_Id);
        }
        #endregion PostAccount

    }
}
