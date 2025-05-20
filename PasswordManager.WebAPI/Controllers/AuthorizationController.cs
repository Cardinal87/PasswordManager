using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DataConnectors;
using System.Net.Http.Headers;
using System.Text;
using Models.AppConfiguration;
using Interfaces;

namespace PasswordManager.WebAPI.Controllers
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
            var uri = new Uri(url);
            var host = uri.Host;
            
            var dataList = await _dbclient.GetByPredicateAsync<WebSiteModel>(m => m.WebAddress == host);
            if (dataList.Count() > 0)
            {
                return Ok(dataList);
            }
            else return BadRequest(new {message = "data not found"});
            
        }


        [HttpPost("post")]
        public async Task<IActionResult> SaveData([FromQuery]string url, [FromQuery] string password, [FromQuery] string login, [FromQuery] string name)
        {
            var uri = new Uri(url);
            var host = uri.Host;
            var model = new WebSiteModel(name, login, password, host, false);
            _dbclient.Insert(model);
            await _dbclient.SaveChangesAsync();
            
            return Ok();
        }

        

    }
}
