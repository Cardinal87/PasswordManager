using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace PasswordManager.WebAPI.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api")]
    public class AppController : Controller
    {
        private Models.DataConnectors.AppContext _client;
        
        public AppController(Models.DataConnectors.AppContext client)
        {
            _client = client;
        }

        [HttpGet("apps")]
        public IActionResult Get()
        {
            try
            {
                var list = _client.List();
                return Ok(list);
            }
            catch
            {
                return StatusCode(500);

            }
        }

        [HttpDelete("apps/{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                var item = _client.GetById(id);
                if(item == null)
                {
                    return BadRequest(new { error = $"entity with {id} was not found" });
                }
                _client.Delete(item);
                _client.SaveChanges();
                return Ok(item);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPut("apps/{id}")]
        public IActionResult Put([FromBody] AppModel model, [FromRoute] int id)
        {
            try
            {
                var result = _client.Update(model, id);
                _client.SaveChanges();
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return BadRequest(new { error = $"entity with {id} was not found" });

            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost("apps")]
        public IActionResult Post([FromBody] AppModel model)
        {
            try
            {
                model.Id = 0;
                _client.Add(model);
                _client.SaveChanges();
                return Ok(model);
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
