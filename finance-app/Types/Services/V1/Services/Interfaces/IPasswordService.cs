using System;

namespace finance_app.Types.Services.V1.Services.Interfaces {
    public interface IPasswordService
    {
        public Byte[] HashPassword(string password, byte[] salt);
        bool VerifyHash(string password, byte[] salt, byte[] hash);

        Byte[] CreateSalt();
    }
}
