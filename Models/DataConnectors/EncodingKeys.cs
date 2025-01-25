using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Models.DataConnectors
{
    public static class EncodingKeys
    {
        public async static Task<string> GetEcryptionKey(string password, string salt)
        {
            int iterCount = 100000;
            int keySize = 32;
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt), iterCount, HashAlgorithmName.SHA256))
            {

                var hash = await Task.Run(() => pbkdf2.GetBytes(keySize));
                var hashString = BitConverter.ToString(hash).ToLower().Replace("-", "");
                return hashString;
            }
        }

        public async static Task<string> GetHash(string password, string salt)
        {
            int iterCount = 300000;
            int keySize = 32;
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt), iterCount, HashAlgorithmName.SHA256))
            {

                var hash = await Task.Run(() => pbkdf2.GetBytes(keySize));
                var hashString = BitConverter.ToString(hash).ToLower().Replace("-", "");
                return hashString;
            }
        }

    }
}
