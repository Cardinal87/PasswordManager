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
    public IActionResult Get([FromQuery] string? url)
    {
        try
        {
            if (!String.IsNullOrEmpty(url))
            {
                var uri = new Uri(url);
                var host = uri.Host;

                var dataList = _client.GetByDomain(host);
                if (dataList.Count() == 0)
                    return BadRequest(new { message = "data for this domain was not found" });

                return Ok(dataList);
            }

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
