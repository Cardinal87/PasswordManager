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
    private DatabaseClient _client;
    
    public WebSiteController(DatabaseClient client)
    {
        _client = client;
    }

    [HttpGet("websites")]
    public IActionResult Get()
    {
        try
        {
            var list = _client.GetListOfType<WebSiteModel>();
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
            var item = _client.GetById<WebSiteModel>(id);
            if (item != null)
            {
                _client.Delete<WebSiteModel>(item);
                _client.SaveChanges();
                return Ok(item);
            }
            else
            {
                return BadRequest(new { error = "no model with such id" });
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
            var item = _client.GetById<WebSiteModel>(id);
            model.Id = id;
            if (item != null)
            {
                _client.Replace<WebSiteModel>(model);
                _client.SaveChanges();
                return Ok(model);
            }
            else
            {
                return BadRequest(new { error = "no model with such id" });
            }
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
                _client.Insert<WebSiteModel>(model);
                _client.SaveChanges();
                return Ok(model);
            }
            else
            {
                return BadRequest(new { error = "data not found" });
            }
        }
        catch
        {
            return StatusCode(500);
        }
    }
}
