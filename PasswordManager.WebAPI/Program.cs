
using PasswordManager.WebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.DataConnectors;
using Newtonsoft.Json;
using System.Net;
using Models.AppConfiguration;
using Services.Extensions;
using Serilog;
using PasswordManager.WebAPI.DTO;
namespace PasswordManager.WebAPI;



public class Program
{
    private static string _configPath = "";
    

    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration().
            WriteTo.File($"{System.AppContext.BaseDirectory}/api-logs/log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7).
            MinimumLevel.Information().CreateLogger();

        try
        {
            
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseWindowsService(opt =>
            {
                opt.ServiceName = "ExtensionAPI";
            });
            var basePath = System.AppContext.BaseDirectory;
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            var configPath = Path.Combine(basePath, "config.json");
            if (!File.Exists(configPath))
            {
                var model = new
                {
                    Authorization = new
                    {
                        Hash = "",
                        ConnectionString = Path.Combine(basePath, "passwordmanager.db"),
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

            builder.Host.UseSerilog();
            builder.Configuration.AddJsonFile(_configPath)
                .Build();
            ConfigureServices(builder.Services, builder.Configuration);


            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Listen(IPAddress.Loopback, 5167);
            });
            
            var app = builder.Build();
            app.UseSerilogRequestLogging();
            app.UseCors("MainPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
        catch(Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
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
        services.AddDbContext<WebSiteContext>((opt) =>
        {
            var connStr = DbConnectionStringSingleton.GetInstance();
            opt.UseSqlite(connStr.ConnectionString);
        });
        services.AddDbContext<Models.DataConnectors.AppContext>((opt) =>
        {
            var connStr = DbConnectionStringSingleton.GetInstance();
            opt.UseSqlite(connStr.ConnectionString);
        });
        services.AddDbContext<CardContext>((opt) =>
        {
            var connStr = DbConnectionStringSingleton.GetInstance();
            opt.UseSqlite(connStr.ConnectionString);
        });
    }
   
    
}
