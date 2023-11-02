using Authentication.AuthenticationUsers;
using finance_app.Types.Repositories.Authentication;
using finance_app.Types.Services.V1.Services.Interfaces;
using Moq;
using repository = finance_app.Types.Repositories.Authentication;

namespace Authentication.UnitTests
{
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    [TestFixture]
    public class AuthenticationUserTests
    {

        private Mock<IPasswordService> _mockPaasswordService;
        private AuthenticationContext authenticationContext;

        private AuthenticationUsers.AuthenticationUser authenticationUser;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            authenticationContext = new();
            _mockPaasswordService = new();
        }

        [OneTimeTearDown]
        public static void OnTimeCleanUp()
        {
        }

        [SetUp]
        public void Setup()
        {
            // TODO: Stop using the actual database when testing
            //authenticationUser = new();
        }

        [TearDown]
        public void CleanUp()
        {
            authenticationUser = null;
        }

        #region Authenticate
        [Test]
        public void Authenticate_Returns_False_With_Not_Existing_User()
        {
            byte[] hash = Enumerable.Repeat((byte)0x20, 5).ToArray();
            byte[] salt = Enumerable.Repeat((byte)0x20, 5).ToArray();

            authenticationUser = new(_mockPaasswordService.Object, authenticationContext, new repository.AuthenticationUser
            {
                Id = 1,
                AuthenticationUserInfo = new()
                {
                    Email = "test@test.ca",
                },
                UserName = "testUsername",
                PasswordHash = hash,
                PasswordSalt = salt
            });

            _mockPaasswordService.Setup(x => x.VerifyHash("password", salt, hash)).Returns(false);
            var authenticated = authenticationUser.Authenticate("password");
            Assert.IsFalse(authenticated);
        }

        [Test]
        public void Authenticate_Returns_False_With_IncorrectPassword()
        {
            byte[] hash = Enumerable.Repeat((byte)0x20, 5).ToArray();
            byte[] salt = Enumerable.Repeat((byte)0x20, 5).ToArray();

            authenticationUser = new(_mockPaasswordService.Object, authenticationContext, new repository.AuthenticationUser
            {
                Id = 1,
                AuthenticationUserInfo = new()
                {
                    Email = "test@test.ca",
                },
                UserName = "testUsername",
                PasswordHash = hash,
                PasswordSalt = salt
            });

            var authenticated = authenticationUser.Authenticate("password");
            Assert.IsFalse(authenticated);
        }

        [Test]
        public void Authenticate_Returns_True_With_Matching_Username_And_Password_Combonation()
        {
            

            byte[] hash = Enumerable.Repeat((byte)0x20, 5).ToArray();
            byte[] salt = Enumerable.Repeat((byte)0x20, 5).ToArray();

            authenticationUser = new(_mockPaasswordService.Object, authenticationContext, new repository.AuthenticationUser
            {
                Id = 1,
                AuthenticationUserInfo = new()
                {
                    Email = "test@test.ca",
                },
                UserName = "testUsername",
                PasswordHash = hash,
                PasswordSalt = salt
            });

            _mockPaasswordService.Setup(x => x.VerifyHash("MatchingPassword", salt, hash)).Returns(true);
            var authenticated = authenticationUser.Authenticate("MatchingPassword");

            Assert.IsTrue(authenticated);
        }

        #endregion Authenticate
    } 
}