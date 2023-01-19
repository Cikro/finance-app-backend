using finance_app.Types.Services.V1.Services.Interfaces;
using Konscious.Security.Cryptography;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace finance_app.Types.Services.V1.Services.Authentication {
    public class PasswordService : IPasswordService
    {
        public Byte[] HashPassword(string password, byte[] salt) {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 8; // Four Cores
            argon2.Iterations = 4;
            argon2.MemorySize = 1024 * 1024; // 1 GB
            return argon2.GetBytes(16);

        }
        public bool VerifyHash(string password, byte[] salt, byte[] hash) {
            var newHash = HashPassword(password, salt);
            return hash.SequenceEqual(newHash);
        }

        public Byte[] CreateSalt() {
            var buffer = new byte[16];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buffer);
            return buffer;

        }
    }
}
