using Autofac;
using Autofac.Builder;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace PasswordManager.Configuration.OptionExtensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddWritebleOptions<T>(this IServiceCollection services,
            IConfigurationSection confSection,
            string file) where T : class, new()
        {
            services.Configure<T>(confSection);
            services.AddTransient<IWritableOptions<T>>(provider =>
            {
                var options = provider.GetRequiredService<IOptions<T>>();
                return new WritableOptions<T>(options, file, confSection.Path);
            });

        }
    }
}
