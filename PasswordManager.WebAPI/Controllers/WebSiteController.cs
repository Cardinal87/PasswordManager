using Microsoft.AspNetCore.Authorization;
using Models;
using Microsoft.AspNetCore.Mvc;
using Models.DataConnectors;

namespace PasswordManager.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api")]
public class WebSiteController : Controller
{
    private WebSiteContext _client;
    
    public WebSiteController(WebSiteContext client)
    {
        _client = client;
    }

    [HttpGet("websites")]
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


    [HttpDelete("websites/{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        try
        {
            var item = _client.GetById(id);
            if (item != null)
            {
                _client.Delete(item);
                _client.SaveChanges();
                return Ok(item);
            }
            else
            {
                return BadRequest(new { error = $"entity with {id} was not found" });
            }
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpPut("websites/{id}")]
    public IActionResult Put([FromBody] WebSiteModel model, [FromRoute] int id)
    {
        try
        {
            var result = _client.Update(model, id);
            _client.SaveChanges();
            return Ok(result);
            
        }
        catch(KeyNotFoundException)
        {
            return BadRequest(new { error = $"entity with {id} was not found" });
        }
        catch
        {
            return StatusCode(500);
        }
    }

    [HttpPost("websites")]
    public IActionResult Post([FromBody] WebSiteModel model)
    {
        try
        {
            if (model != null)
            {
                model.Id = 0;
                _client.Insert(model);
                _client.SaveChanges();
                return Ok(model);
            }
            else
            {
                return BadRequest(new { error = "no necessary data in body" });
            }
        }
        catch
        {
            return StatusCode(500);
        }
    }
}
