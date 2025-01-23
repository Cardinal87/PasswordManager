using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Extension.WebAPI.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
namespace Extension.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private JwtOptions _options;
        public LoginController(IOptions<JwtOptions> options) 
        { 
            _options = options.Value;
        }
        
        [HttpGet("{name}")]
        public string GetToken(string name)
        {
            var claims = new List<Claim>() { new Claim(ClaimTypes.Name, name) };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

            var descriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddHours(4),
                Subject = new ClaimsIdentity(claims),
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}
