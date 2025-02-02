
using Extension.WebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.DataConnectors;
using Newtonsoft.Json;
using NuGet.Configuration;
using System.Security.Cryptography;
using System.Text;
using ViewModels.Services;
using ViewModels.Services.AppConfiguration;
namespace Extension.WebAPI
{
    public class Program
    {
        private static string _configPath = "";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var roaminPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var directory = Path.Combine(roaminPath, "PasswordManager");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var condfigPath = Path.Combine(directory, "config.json");
            if (!File.Exists(condfigPath))
            {
                var model = new
                {
                    Authorization = new
                    {
                        Hash = "",
                        ConnectionString = Path.Combine(directory, "passwordmanager.db"),
                        Salt = ""
                    },

                };
                var json = JsonConvert.SerializeObject(model, Formatting.Indented);
                File.WriteAllText(condfigPath, json);
            }
            _configPath = condfigPath;
            
            builder.Configuration.AddJsonFile(_configPath)
                .AddUserSecrets<Program>()
                .Build();
            ConfigureServices(builder.Services, builder.Configuration);
            
            var app = builder.Build();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
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
                var settings = prov.GetRequiredService<IOptions<AppAuthorizationOptions>>();
                var factory = DbConnectionStringSingleton.GetInstance();
                opt.UseSqlite(factory.ConnectionString);
            });
        }
       
        
    }
}
