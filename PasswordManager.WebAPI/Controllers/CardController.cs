using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DataConnectors;

namespace PasswordManager.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class CardController : Controller
    {
        CardContext _client;

        public CardController(CardContext client)
        {
            _client = client;
        }


        [HttpGet("cards")]
        public IActionResult List()
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
        [HttpDelete("cards/{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                var item = _client.GetById(id);
                if (item == null)
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

        [HttpPut("cards/{id}")]
        public IActionResult Put([FromBody] CardModel model, [FromRoute] int id)
        {
            try
            {
                var item = _client.Update(model, id);
                _client.SaveChanges();
                return Ok(item);

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

        [HttpPost("cards")]
        public IActionResult Post([FromBody] CardModel model)
        {
            try
            {
                model.Id = 0;
                _client.Insert(model);
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
