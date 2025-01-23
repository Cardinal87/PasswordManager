using Extension.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;
using Models.DataConnectors;
using System.Net.Http.Headers;
using System.Text;
using ViewModels.Services;
using ViewModels.Services.AppConfiguration;

namespace Extension.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : Controller
    {
        private DatabaseClient _dbclient;
        private IWritableOptions<AppAuthorizationOptions> _options;
        public AuthorizationController(DatabaseClient dbclient, IWritableOptions<AppAuthorizationOptions> options)
        {
            _dbclient = dbclient;
            _options = options;
        }

        
        [HttpGet("get")]
        public async Task<IActionResult> GetData([FromQuery] string url)
        {
            var inst = DbConnectionStringSingleton.GetInstance();
            var uri = new Uri(url);
            var host = uri.Host;
            _dbclient.Database.EnsureCreated();
            var dataList = await _dbclient.GetByPredicate<WebSiteModel>(m => m.WebAddress == host);
            if (dataList.Count() > 0)
            {
                return Ok(dataList);
            }
            else return BadRequest(new {message = "data not found"});
            
        }


        [HttpPost("postdata")]
        public async Task<IActionResult> SaveData([FromQuery]string url, [FromQuery] string password, [FromQuery] string login, [FromQuery] string name)
        {
            var uri = new Uri(url);
            var host = uri.Host;
            var model = new WebSiteModel(name, login, password, host, false);
            _dbclient.Insert(model);
            await _dbclient.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("setpassword")]
        public IActionResult SetPassword()
        {

            var context = HttpContext;
            var responce = context.Request;
            string? auth = responce.Headers.Authorization;
            if (auth != null && auth.StartsWith("Basic"))
            {
                if (AuthenticationHeaderValue.TryParse(auth,out var encodedPassword) && encodedPassword.Parameter != null)
                {
                    string password = Encoding.UTF8.GetString(Convert.FromBase64String(encodedPassword.Parameter));
                    string salt = _options.Value.Salt;
                    var key = DatabaseEncoding.GetEcryptionKey(password, salt);
                    DbConnectionStringSingleton.SetPassword(key);
                    return Ok();
                }
                else return BadRequest(new { Message = "Authorization header is not correct" });

            }
            else return BadRequest(new { Message = "Authorization header is empty or not basic" });

            
        }

    }
}
