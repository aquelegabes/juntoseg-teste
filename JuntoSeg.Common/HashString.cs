using System;
using System.Security.Cryptography;

namespace JuntoSeg.Common
{
    public static class HashString
    {
        public static string CreateHashString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(
                    message: "Value cannot be null.",
                    paramName: nameof(value)
                );

            byte[] salt = default;
            using var provider = new RNGCryptoServiceProvider();
            provider.GetBytes(salt = new byte[16]);
            using var pbkdf2 = new Rfc2898DeriveBytes(value, salt, 1000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyHashString(string hashedString, string notHashedString)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedString);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            using var pbkdf2 = new Rfc2898DeriveBytes(notHashedString, salt, 1000);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;

            return true;
        }
    }
}
