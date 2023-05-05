using PasswordHash.Interfaces;
using System.Security.Cryptography;

namespace PasswordHash
{
    public class PasswordGenerator : IPasswordGenerator
    {
        private const int Iterate = 1000;
        private const int StartIndex = 0;

        public string GeneratePasswordHashUsingSalt(string password, byte[] salt)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterate);
            var hash = pbkdf2.GetBytes(salt.Length);
            var hashBytes = new byte[salt.Length + hash.Length];
            Buffer.BlockCopy(salt, StartIndex, hashBytes, StartIndex, salt.Length);
            Buffer.BlockCopy(hash, StartIndex, hashBytes, salt.Length, hash.Length);

            return Convert.ToBase64String(hashBytes);
        }
    }
}
