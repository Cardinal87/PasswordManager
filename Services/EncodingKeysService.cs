using System.Security.Cryptography;
using System.Text;
using Scrypt;

namespace Services;

public static class EncodingKeysService
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

    public async static Task<string> GetHash(string password)
    {
        var scrypt = new ScryptEncoder();
        var hash = await Task.Run(() => scrypt.Encode(password));
        return hash;
        
    }

    public static bool CompareHash(string password, string hash)
    {
        var scrypt = new ScryptEncoder();
        bool b = scrypt.Compare(password, hash);
        return b;
    }

}
