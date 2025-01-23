
using Extension.WebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.DataConnectors;
using System.Text;
using ViewModels.Services;
using ViewModels.Services.AppConfiguration;
namespace Extension.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Configuration.AddJsonFile("config.json")
                .AddUserSecrets<Program>().Build();
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
            
            
            services.AddAuthorization();
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddWritebleOptions<AppAuthorizationOptions>(conf.GetSection(AppAuthorizationOptions.Section), "config.json");
            services.Configure<JwtOptions>(conf.GetSection(JwtOptions.Section));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                var key = Encoding.UTF8.GetBytes(jwt["key"] ?? throw new Exception("jwt key is null or empty"));
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwt["issuer"],
                    ValidAudience = jwt["audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true
                };
            });
            services.AddDbContext<DatabaseClient>((prov,opt) =>
            {
                var settings = prov.GetRequiredService<IOptions<AppAuthorizationOptions>>();
                var factory = DbConnectionStringSingleton.SetInstance(settings.Value.ConnectionString);
                opt.UseSqlite(factory.ConnectionString);
            });
        }
       
    }
}
