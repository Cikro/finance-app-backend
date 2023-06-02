namespace Authentication.AuthenticationUsers
{
    public interface IAuthenticationUser
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public uint AuthenticationUserId { get; set; }

        public bool Authenticate(string password);
        public Task Create(string password);
    }
}
