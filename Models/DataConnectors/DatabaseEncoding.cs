using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Models.DataConnectors
{
    public static class DatabaseEncoding
    {
        public static string GetEcryptionKey(string password, string salt)
        {
            int iterCount = 100000;
            int keySize = 32;
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt), iterCount, HashAlgorithmName.SHA256))
            {

                var hash = pbkdf2.GetBytes(keySize);
                var hashString = BitConverter.ToString(hash).ToLower().Replace("-", "");
                return hashString;
            }
        }



    }
}
