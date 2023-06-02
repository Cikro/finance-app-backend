using Authentication.AuthenticationUsers;

namespace Authentication.UnitTests
{
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    [TestFixture]
    public class AuthenticationUserTests
    {
        private AuthenticationUser authenticationUser;

        [OneTimeSetUp]
        public static void OneTimeSetup()
        {
            
        }

        [OneTimeTearDown]
        public static void OnTimeCleanUp()
        {
        }

        [SetUp]
        public void Setup()
        {
            // TODO: Stop using the actual database when testing
            authenticationUser = new();
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

            var authenticated = authenticationUser.Authenticate("NotExisting Username", "password");
            Assert.IsFalse(authenticated);
        }

        [Test]
        public void Authenticate_Returns_False_With_IncorrectPassword()
        {
            var authenticated = authenticationUser.Authenticate("Test Username", "IncorrectPassword");
            Assert.IsFalse(authenticated);
        }

        [Test]
        public void Authenticate_Returns_True_With_Matching_Username_And_Password_Combonation()
        {
            var authenticated = authenticationUser.Authenticate("Test Username", "MatchingPassword");
            Assert.IsTrue(authenticated);
        }

        #endregion Authenticate
    }
}