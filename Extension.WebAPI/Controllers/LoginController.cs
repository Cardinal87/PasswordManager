using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Extension.WebAPI.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Models.DataConnectors;
using System.Net.Http.Headers;
using System.Xml.Linq;
using ViewModels.Services.AppConfiguration;
namespace Extension.WebAPI.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private JwtOptions _jwtOptions;
        private AppAuthorizationOptions _appOptions;
        private JwtKeyService _jwtKeyService;
        public LoginController(IOptions<JwtOptions> jwtOptions, IOptions<AppAuthorizationOptions> appOptions, JwtKeyService keyService) 
        { 
            _jwtOptions = jwtOptions.Value;
            _appOptions = appOptions.Value;
            _jwtKeyService = keyService;
        }
        
        [HttpGet("{name}")]
        public async Task<IActionResult> GetToken(string name)
        {
            
            var context = HttpContext;
            var responce = context.Request;
            string? auth = responce.Headers.Authorization;
            if (auth != null && auth.StartsWith("Basic"))
            {
                if (AuthenticationHeaderValue.TryParse(auth, out var encodedPassword) && encodedPassword.Parameter != null)
                {
                    string password = Encoding.UTF8.GetString(Convert.FromBase64String(encodedPassword.Parameter));
                    string salt = _appOptions.Salt;

                    var iscorrect = EncodingKeys.CompareHash(password, _appOptions.Hash);
                    if (!iscorrect) return BadRequest(new { Message = "Password is not correct" });
                    var key = await EncodingKeys.GetEcryptionKey(password, salt);
                    DbConnectionStringSingleton.SetCreditals(key, _appOptions.ConnectionString);
                    var token = CreateToken(name);
                    return Ok(new { token });
                }
                else return BadRequest(new { Message = "Authorization header is not correct" });

            }
            else return BadRequest(new { Message = "Authorization header is empty or not basic" });


        }

        private string CreateToken(string name)
        {
            var claims = new List<Claim>() { new Claim(ClaimTypes.Name, name) };
            var key = new SymmetricSecurityKey(_jwtKeyService.GetJwtKey());
            var descriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddHours(4),
                Subject = new ClaimsIdentity(claims),
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}
