
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Views.Configuration.OptionExtensions;
using ViewModels;
using Interfaces.PasswordGenerator;
using Views.Services;
using Views;
using System.IO;
using Interfaces;
using System.Threading.Tasks;
using System;
using ViewModels.AppConfiguration;


namespace Views;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        
    }
    private string _configPath = "";
    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);



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
                }
            };
            var json = JsonConvert.SerializeObject(model, Formatting.Indented);
            File.WriteAllText(condfigPath, json);
        }
        _configPath = condfigPath;

        var conf = new ConfigurationBuilder()
            .AddJsonFile(condfigPath)
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
        services.AddWritebleOptions<AppAuthorizationOptions>(config.GetSection(AppAuthorizationOptions.Section), _configPath);
        services.AddTransient<StartUpViewModel>(prov =>
        {
            return new StartUpViewModel(prov.GetRequiredService<IWritableOptions<AppAuthorizationOptions>>(),
                services);
        });
    }
}
