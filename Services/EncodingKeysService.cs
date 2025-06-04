using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Scrypt;

namespace Services;

public static class EncodingKeysService
{
    private const string passwordPattern = @"^(?=.*?[A-Z].*?[A-Z])(?=.*?[a-z].*?[a-z])(?=.*?\d.*?\d)(?=.*?[@$!%*?&].*?[@$!%*?&]).{10,50}$";

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

    public static string GetHash(string password)
    {
        var scrypt = new ScryptEncoder();
        var hash = scrypt.Encode(password);
        return hash;
        
    }

    public static bool CompareHash(string password, string hash)
    {
        var scrypt = new ScryptEncoder();
        bool b = scrypt.Compare(password, hash);
        return b;
    }

    public static bool IsStrongPassword(string password)
    {
        return Regex.IsMatch(password, passwordPattern);
    }

    public static string GenerateSalt()
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] salt = new byte[32];
            rng.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }
    }
}
