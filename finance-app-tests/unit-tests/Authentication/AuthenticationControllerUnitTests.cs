
using System.Threading.Tasks;
using AutoMapper;
using finance_app.Controllers.V1;
using finance_app.Types.Services.V1.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FinanceApp.Unit_Tests.Authentication
{
    [TestClass]
    public class AuthenticationControllerUnitTests {

        private Mock<ILogger<AuthenticationController>> _logger;
        private Mock<IAuthenticationService> _authenticationService;
        private Mock<IMapper> _mapper;

        private AuthenticationController _authenticationController;

        
        public AuthenticationControllerUnitTests() {}

        [TestInitialize]
        public void TestInitialize()
        {
            _logger = new Mock<ILogger<AuthenticationController>>();
            _mapper = new Mock<IMapper>();
            _authenticationService = new Mock<IAuthenticationService>();
            _authenticationController = new AuthenticationController(_logger.Object, _authenticationService.Object, _mapper.Object);

        }

        // What tests do we want
        #region LoginIn
        [TestMethod]
        [TestCategory("Login")]
        public async Task AuthenticationController_LoginSuccessful_Expect_SetsAuthenticationCookie() { Assert.Fail("Test not Implemented"); }
        public async Task AuthenticationController_LoginSuccessful_Expect_SetsIsLoggedInCookieForSPAs() { Assert.Fail("Test not Implemented"); }
        public async Task AuthenticationController_Login_Maps_INPUT_To_ENTITY() { Assert.Fail("Test not Implemented"); }
        //public async Task AuthenticationController_Login() { Assert.Fail("Test not Implemented"); }
        //public async Task AuthenticationController_Login() { Assert.Fail("Test not Implemented"); }
        //public async Task AuthenticationController_Login() { Assert.Fail("Test not Implemented"); }
        //public async Task AuthenticationController_Login() { Assert.Fail("Test not Implemented"); }
        //public async Task AuthenticationController_Login() { Assert.Fail("Test not Implemented"); }
        //public async Task AuthenticationController_Login() { Assert.Fail("Test not Implemented"); }
        //public async Task AuthenticationController_Login() { Assert.Fail("Test not Implemented"); }
        #endregion LoginIn


    }
}