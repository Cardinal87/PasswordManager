using Interfaces.PasswordGenerator;
using System.Security.Cryptography;

namespace Services;

public class PasswordGenerator : IPasswordGenerator
{
    private const string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
    private const string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Digits = "0123456789";
    private const string SpecialCharacters = "~!@#$%^&*()_-+={}[]|:;<>,.?/";
    public string GeneratePassword(PasswordGeneratorArgs args)
    {
        IEnumerable<char> validSymbols = new List<char>();
        if (args.ActivateLowcase)
            validSymbols = validSymbols.Concat(LowercaseLetters);
        if (args.ActivateUpcase)
            validSymbols = validSymbols.Concat(UppercaseLetters);
        if (args.ActivateDigits)
            validSymbols = validSymbols.Concat(Digits);
        if (args.ActivateSpecSymbs) 
            validSymbols = validSymbols.Concat(SpecialCharacters);

        string password = RandomNumberGenerator.GetString(validSymbols.ToArray(), args.Length);
        return password;
    }
}
