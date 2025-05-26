
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Extensions;
using ViewModels;
using Interfaces.PasswordGenerator;
using Views.Services;
using System.IO;
using Interfaces;
using System;
using Models.AppConfiguration;
using Services;
using Models;
using ViewModels.CardViewModels;
using ViewModels.AppViewModels;
using ViewModels.WebSiteViewModels;

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
                },
                Jwt = new
                {
                    Issuer = "localhost",
                    Audience = "extension"
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
            var provider = services.BuildServiceProvider();
            desktop.MainWindow = new MainWindow
            {
                DataContext = provider.GetRequiredService<StartUpViewModel>()
            };
            
            
           
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
        services.AddHttpClient();
        services.AddSingleton<IHttpDataConnector<AppModel>, HttpAppDataConnector>();
        services.AddSingleton<IHttpDataConnector<CardModel>, HttpCardDataConnector>();
        services.AddSingleton<IHttpDataConnector<WebSiteModel>, HttpWebSiteDataConnector>();
        services.AddSingleton<TokenHandlerService>();
        services.AddTransient<CardViewModel>();
        services.AddTransient<AppViewModel>();
        services.AddTransient<MenuViewModel>();
        services.AddTransient<WebSiteViewModel>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<StartUpViewModel>();
    }
}
