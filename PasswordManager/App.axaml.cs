
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Configuration.OptionExtensions;

using PasswordManager.ViewModels;
using PasswordManager.ViewModels;
using PasswordManager.Views;
using System.IO;

using System.Threading.Tasks;
using PasswordManager.Configuration.AppConfiguration;
using PasswordManager.Services;

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
                    hash = "",
                    connectionString = "passwordmanager.db"
                }
            };
            var json = JsonConvert.SerializeObject(model, Formatting.Indented);
            File.WriteAllText("appsettings.json", json);
        }

        
        var conf = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection();
        SetUpContainer(services, conf);
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            using (var provider = services.BuildServiceProvider())
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = provider.GetRequiredService<StartUpViewModel>()
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
        services.AddSingleton<IPasswordGenerator, PasswordGenerator>();
        services.AddWritebleOptions<LoggingOptions>(config.GetSection(LoggingOptions.Section), "appsettings.json");
        services.AddTransient<StartUpViewModel>(prov =>
        {
            return new StartUpViewModel(prov.GetRequiredService<IWritableOptions<LoggingOptions>>(),
                services);
        });
    }
}
