using Autofac;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Configuration;
using PasswordManager.Configuration.OptionExtensions;
using PasswordManager.Factories;
using PasswordManager.Helpers;
using PasswordManager.ViewModels;
using PasswordManager.Views;
using System.IO;
using System.Text.Json;


namespace PasswordManager;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (!File.Exists("appsettings.json"))
        {
            var model = new 
            { 
                logging = new
                {
                    hash = ""
                }
            };
            var json = JsonSerializer.Serialize(model);
            File.WriteAllText("appsettings.json", json);
        }

        var conf = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection();
        SetUpContainer(services, conf);
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            using (var a = services.BuildServiceProvider())
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new StartUpViewModel(a.GetRequiredService<IViewModelFactory>(),
                    a.GetRequiredService<IWritableOptions<LoggingOptions>>())
                };
            }
            
        }
        //else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        //{
        //    singleViewPlatform.MainView = new MainView
        //    {
        //        DataContext = new MainViewModel(dbConnector)
        //    };
        //}

        base.OnFrameworkInitializationCompleted();
    }

    private void SetUpContainer(IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IClipboardService, ClipboardService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IContextFactory, ContextFactory>();
        services.AddSingleton<IViewModelFactory, ViewModelFactory>();
        services.AddSingleton<IItemViewModelFactory, ItemViewModelFactory>();
        services.AddWritebleOptions<LoggingOptions>(config.GetSection(LoggingOptions.Section), "appsettings.json");
    }
}
