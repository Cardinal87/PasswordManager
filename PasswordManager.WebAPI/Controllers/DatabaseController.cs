using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Models.AppConfiguration;
using Models.DataConnectors;
using Services;
using System.Text.Json;

namespace PasswordManager.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private IWritableOptions<AppAuthorizationOptions> _appOptions;

        public DatabaseController(IWritableOptions<AppAuthorizationOptions> appOptions)
        {
            _appOptions = appOptions;
        }

        [HttpPost()]
        public IActionResult CreateDatabase([FromBody] JsonElement json)
        {

            string? password = json.GetProperty("password").GetString();
            if (String.IsNullOrEmpty(password))
            {
                return BadRequest(new { error = "Password is null or empty" });
            }
            if (!EncodingKeysService.IsStrongPassword(password))
            {
                return BadRequest(new { error = "Invalid password requirements" });
            }
            if (System.IO.File.Exists(_appOptions.Value.ConnectionString))
            {
                return BadRequest(new { message = "Database already created" });
            }
            string hash = EncodingKeysService.GetHash(password);
            string salt = EncodingKeysService.GenerateSalt();
            var key = EncodingKeysService.GetEcryptionKey(password, salt);
            string connStr = new SqliteConnectionStringBuilder
            {
                DataSource = _appOptions.Value.ConnectionString,
                Password = key
            }.ToString();
            var options = new DbContextOptionsBuilder<DatabaseClient>();
            options.UseSqlite(connStr);
            using (var dbClient = new DatabaseClient(options.Options))
            {
                dbClient.Database.EnsureCreated();
            }
            _appOptions.Update(opt => {
                opt.Hash = hash;
                opt.Salt = salt;
            });
            return Created();
        }
        [HttpDelete()]
        public IActionResult DeleteDatabase()
        {
            if (System.IO.File.Exists(_appOptions.Value.ConnectionString))
            {
                System.IO.File.Delete(_appOptions.Value.ConnectionString);
            }
            _appOptions.Update(opt =>
            {
                opt.Hash = "";
                opt.Salt = "";
            });
            return Ok(new {message = "Database successfully deleted"});

        }

        [HttpGet()]
        public IActionResult IsDatabaseCreated()
        {
            if (System.IO.File.Exists(_appOptions.Value.ConnectionString))
            {
                return Ok(new { message = "Database already created" });
            }
            return NoContent();
        }
    }
}
