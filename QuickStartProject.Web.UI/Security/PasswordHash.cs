using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Logfox.Web.UI.Security
{
    internal static class PasswordHash
    {
        private const int SaltBytes = 64;
        private const int HashBytes = 64;
        private const int Pbkdf2Iterations = 4709;
        
        public static HashObj CreateHash(string password)
        {
            var csprng = new RNGCryptoServiceProvider();
            var saltByte = new byte[SaltBytes];
            csprng.GetBytes(saltByte);
            
            var salt = Convert.ToBase64String(saltByte);
            var hash = PBKDF2(password, saltByte, Pbkdf2Iterations, HashBytes);
            return new HashObj { Hash = Convert.ToBase64String(hash), Salt = salt };
        }

        public static bool ValidatePassword(string password, string hash, string salt)
        {
            var saltByte = Convert.FromBase64String(salt);
            var hashByte = Convert.FromBase64String(hash);

            var testHash = PBKDF2(password, saltByte, Pbkdf2Iterations, hashByte.Length);
            return SlowEquals(hashByte, testHash);
        }

        private static bool SlowEquals(IList<byte> a, IList<byte> b)
        {
            var diff = (uint)a.Count ^ (uint)b.Count;
            for (var i = 0; i < a.Count && i < b.Count; i++)
            { diff |= (uint)(a[i] ^ b[i]); }
            return diff == 0;
        }

        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt) { IterationCount = iterations };
            return pbkdf2.GetBytes(outputBytes);
        }
    }
}