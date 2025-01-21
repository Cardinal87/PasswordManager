
using Extension.WebAPI.Services;
using Microsoft.EntityFrameworkCore;
using Models.DataConnectors;
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

            builder.Configuration.AddJsonFile("config.json").Build();
            ConfigureServices(builder.Services, builder.Configuration);
            
            var app = builder.Build();
            app.MapControllers();
            app.Run();
        }
        public static void ConfigureServices(IServiceCollection services, IConfiguration conf)
        {
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddWritebleOptions<AuthorizationOptions>(conf.GetSection(AuthorizationOptions.Section), "config.json");
            services.AddDbContext<DatabaseClient>((prov,opt) =>
            {
                var factory = Models.DataConnectors.DbConnectionStringFactory.GetInstance();
                opt.UseSqlite(factory.ConnectionString);
            });
        }
       
    }
}
