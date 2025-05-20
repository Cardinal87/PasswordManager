
using PasswordManager.WebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.DataConnectors;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Hosting.WindowsServices;
using Models.AppConfiguration;
using Services.Extensions;
namespace PasswordManager.WebAPI;


public class Program
{
    private static string _configPath = "";

    public static void Main(string[] args)
    {
        try
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseWindowsService(opt =>
            {
                opt.ServiceName = "ExtensionAPI";
            });
            var roaminPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var directory = Path.Combine(roaminPath, "PasswordManager");
            if (WindowsServiceHelpers.IsWindowsService())
            {
                string path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
                var jObj = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(path)) ?? throw new FileNotFoundException($"appsettings.json by path {path} was not found");
                var section = jObj["Config"]!;
                directory = section["ConfigPath"]!.ToString();

            }
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var configPath = Path.Combine(directory, "config.json");
            if (args.Contains("--save"))
            {
                string path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
                var jObj = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(path)) ?? throw new FileNotFoundException($"appsettings.json by path {path} was not found");
                var section = jObj["Config"]!;
                section["ConfigPath"] = directory;
                jObj["Config"] = JObject.Parse(JsonConvert.SerializeObject(section));
                string json = JsonConvert.SerializeObject(jObj, Formatting.Indented);
                File.WriteAllText(path, json);
                return;
            }
            if (!File.Exists(configPath))
            {
                var model = new
                {
                    Authorization = new
                    {
                        Hash = "",
                        ConnectionString = Path.Combine(directory, "passwordmanager.db"),
                        Salt = ""
                    },
                    Jwt = new
                    {
                        Issuer = "localhost",
                        Audience = "extension"
                    }
                };
                var json = JsonConvert.SerializeObject(model, Formatting.Indented);
                File.WriteAllText(configPath, json);
            }
            _configPath = configPath;

            builder.Configuration.AddJsonFile(_configPath)
                .Build();
            ConfigureServices(builder.Services, builder.Configuration);


            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Listen(IPAddress.Loopback, 5167);
            });

            var app = builder.Build();
            app.UseCors("MainPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    public static void ConfigureServices(IServiceCollection services, IConfiguration conf)
    {
        var jwt = conf.GetRequiredSection(JwtOptions.Section);
        var keyService = new JwtKeyService();
        
        
        services.AddAuthorization();
        services.AddControllers();
        services.AddHttpContextAccessor();
        services.AddSingleton<JwtKeyService>(keyService);
        services.AddHostedService<JwtKeyRotationService>();
        services.AddWritebleOptions<AppAuthorizationOptions>(conf.GetSection(AppAuthorizationOptions.Section), _configPath);
        services.Configure<JwtOptions>(conf.GetSection(JwtOptions.Section));
        services.AddCors(pol =>
        {
            pol.AddPolicy("MainPolicy", opt =>
            {
                opt.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = jwt["issuer"],
                ValidAudience = jwt["audience"],
                IssuerSigningKey = new SymmetricSecurityKey(keyService.GetJwtKey()),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true
            };
        });
        services.AddDbContext<DatabaseClient>((prov,opt) =>
        {
            var connStr = DbConnectionStringSingleton.GetInstance();
            opt.UseSqlite(connStr.ConnectionString);
        });
    }
   
    
}
