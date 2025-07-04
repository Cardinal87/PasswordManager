﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Interfaces;

namespace Services.Extensions;

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
