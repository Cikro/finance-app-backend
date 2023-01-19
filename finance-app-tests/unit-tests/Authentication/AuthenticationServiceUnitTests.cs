
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using finance_app.Controllers.V1;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Requests.Authentication;
using finance_app.Types.DataContracts.V1.Responses;
using finance_app.Types.Repositories.Authentication;
using finance_app.Types.Services.V1.Authentication;
using finance_app.Types.Services.V1.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinanceApp.Unit_Tests.Authentication
{
    [TestClass]
    public class AuthenticationServiceUnitTests {

        private Mock<IMapper> _mapper;
        private Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private Mock<IPasswordService> _mockPasswordService;
        private AuthenticationContext _fakeAuthenticationContext;
        private HttpContext _fakeHttpContext;
        

        private AuthenticationService _authenticationService;

        

        [TestInitialize]
        public void Setup() {
            _mapper = new Mock<IMapper>(MockBehavior.Strict);
            _mockPasswordService = new Mock<IPasswordService>(MockBehavior.Strict);
            var options = new DbContextOptionsBuilder<AuthenticationContext>()
                .UseInMemoryDatabase(databaseName: "AuthenticationDatabase")
                .Options;
            using (var context = new AuthenticationContext(options))
            {
                context.Users.Add(new AuthenticationUser {  Id = 7, UserName = "UnitTestUser1", PasswordHash = new byte[] { 0x00, 0x01, 0x02 }, PasswordSalt = new byte[] { 0x02, 0x01, 0x00 } });
                context.Users.Add(new AuthenticationUser { Id = 8, UserName = "UnitTestUser2", PasswordHash = new byte[] { 0x03, 0x04, 0x05 }, PasswordSalt = new byte[] { 0x05, 0x04, 0x03 } });
                context.Users.Add(new AuthenticationUser { Id = 9, UserName = "UnitTestUser3", PasswordHash = new byte[] { 0x06, 0x07, 0x08 }, PasswordSalt = new byte[] { 0x08, 0x07, 0x06 } });
                context.SaveChanges();
            }

            _fakeAuthenticationContext = new AuthenticationContext(options);
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            

            _fakeHttpContext = new DefaultHttpContext();
            _mockHttpContextAccessor.SetupGet(x => x.HttpContext).Returns(_fakeHttpContext);

            _authenticationService = new AuthenticationService(_mapper.Object,
                                                               _fakeAuthenticationContext,
                                                               _mockPasswordService.Object);
        }

        [TestCleanup]
        public void Cleanup() { 
            _fakeAuthenticationContext.Dispose();
        }

        // What tests do we want
        #region Login
        [TestMethod]
        [TestCategory("Login")]
        public async Task AuthenticationService_LoginSuccessful_Expect_LogginedInUser()
        {
            // Arrange
            var loginRequest = new LoginRequest()
            {
                Username = "UnitTestUser1",
                Password = "testPassword"
            };
            var hashedPassword = new Byte[] { 0x00, 0x01, 0x02 };
            _mapper.Setup(x => x.Map<ApiResponse<AuthenticationUserDto>>(It.IsAny<AuthenticationUser>()))
                .Returns(new ApiResponse<AuthenticationUserDto>(new AuthenticationUserDto
                {
                    Id = 9,
                    Username = loginRequest.Username
                }));
            _mockPasswordService.Setup(x =>
                x.VerifyHash(
                    loginRequest.Password,
                    new byte[] { 0x02, 0x01, 0x00 },
                    new byte[] { 0x00, 0x01, 0x02 }))
                .Returns(true);


            // Act
            var result = await _authenticationService.Login(loginRequest);


            // Assert
            Assert.AreEqual(loginRequest.Username, result.Data.Username);
        }

        [TestMethod]
        [TestCategory("Login")]
        public async Task AuthenticationService_UserDoesNotExist_Expect_Null()
        {
            // Arrange
            var loginRequest = new LoginRequest()
            {
                Username = "User that does not exist",
                Password = "testPassword"
            };
            _mapper.Setup(x => x.Map<ApiResponse<AuthenticationUserDto>>(It.IsAny<AuthenticationUser>()))
                .Returns(new ApiResponse<AuthenticationUserDto>(new AuthenticationUserDto
                {
                    Id = 9,
                    Username = loginRequest.Username
                }));


            // Act
            var result = await _authenticationService.Login(loginRequest);


            // Assert
            _mapper.Verify(x => x.Map<ApiResponse<AuthenticationUserDto>>(It.IsAny<AuthenticationUser>()), Times.Never);
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("Login")]
        public async Task AuthenticationService_InvliadPassword_ExpectNull()
        {
            // Arrange
            var loginRequest = new LoginRequest()
            {
                Username = "UnitTestUser1",
                Password = "testPassword"
            };

            _mapper.Setup(x => x.Map<ApiResponse<AuthenticationUserDto>>(It.IsAny<AuthenticationUser>()))
                .Returns(new ApiResponse<AuthenticationUserDto>(new AuthenticationUserDto
                {
                    Id = 9,
                    Username = loginRequest.Username
                }));
            _mockPasswordService.Setup(x =>
                x.VerifyHash(
                    loginRequest.Password,
                    It.IsAny<byte[]>(),
                    It.IsAny<byte[]>()))
                .Returns(false);



            // Act
            var result = await _authenticationService.Login(loginRequest);


            // Assert
            _mapper.Verify(x => x.Map<ApiResponse<AuthenticationUserDto>>(It.IsAny<AuthenticationUser>()), Times.Never);
            Assert.IsNull(result);
        }


        #endregion Login


    }
}