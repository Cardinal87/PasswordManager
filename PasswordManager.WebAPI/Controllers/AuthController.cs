using Microsoft.AspNetCore.Mvc;
using PasswordManager.WebAPI.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Models.DataConnectors;
using System.Net.Http.Headers;
using Models.AppConfiguration;
using Services;
using Interfaces;

namespace PasswordManager.WebAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : Controller
{
    private JwtOptions _jwtOptions;
    private IWritableOptions<AppAuthorizationOptions> _appOptions;
    private JwtKeyService _jwtKeyService;
    public AuthController(IOptions<JwtOptions> jwtOptions, 
        IWritableOptions<AppAuthorizationOptions> appOptions,
        JwtKeyService keyService) 
    { 
        _jwtOptions = jwtOptions.Value;
        _appOptions = appOptions;
        _jwtKeyService = keyService;
    }
    
    [HttpPost("tokens")]
    public IActionResult CreateToken()
    {
        
        var context = HttpContext;
        var request = context.Request;
        string? auth = request.Headers.Authorization;
        (bool isCorrect, string message) = TryParseBasicAuth(auth, out string password);
        if (!isCorrect)
        {
            return BadRequest(new { error = message });
        }
        if (String.IsNullOrEmpty(_appOptions.Value.Hash) || !System.IO.File.Exists(_appOptions.Value.ConnectionString))
        {
            return BadRequest(new { error = "data base was not created yet" });
        }
        else
        {
            var iscorrect = EncodingKeysService.CompareHash(password, _appOptions.Value.Hash);
            if (!iscorrect) return BadRequest(new { Message = "Password is not correct" });
        }
        var key = EncodingKeysService.GetEcryptionKey(password, _appOptions.Value.Salt);
        DbConnectionStringSingleton.SetCreditals(key, _appOptions.Value.ConnectionString);
        var token = InitializeToken();
        return Ok(new { token });
            
    }


    private string InitializeToken()
    {
        var key = new SymmetricSecurityKey(_jwtKeyService.GetJwtKey());
        var descriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddHours(4),
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(descriptor);
        return tokenHandler.WriteToken(token);

    }

    private (bool, string) TryParseBasicAuth(string? header, out string password)
    {
        password = "";
        if (header != null && header.StartsWith("Basic"))
        {
            if (AuthenticationHeaderValue.TryParse(header, out var encodedPassword) && encodedPassword.Parameter != null)
            {
                password = Encoding.UTF8.GetString(Convert.FromBase64String(encodedPassword.Parameter));
                return (true, "");
            }
            else return (false, "Authorization header is not correct");
        }
        else return (false, "Authorization header is empty or not basic");
    }


}
