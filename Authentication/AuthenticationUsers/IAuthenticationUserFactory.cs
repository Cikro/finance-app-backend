namespace Authentication.AuthenticationUsers
{
    public interface IAuthenticationUserFactory
    {
        public IAuthenticationUser CreateUser(byte[] passwordHash, byte[] passwordSalt);
        public IAuthenticationUser CreateUser(string username);
    }
}