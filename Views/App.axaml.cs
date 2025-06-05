using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using ViewModels;
using Interfaces.PasswordGenerator;
using Views.Services;
using Interfaces;
using Services;
using Models;
using ViewModels.CardViewModels;
using ViewModels.AppViewModels;
using ViewModels.WebSiteViewModels;
using Serilog;

namespace Views;

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

        var services = new ServiceCollection();
        SetUpContainer(services);
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var provider = services.BuildServiceProvider();
            using (var scope = provider.CreateScope())
            {
                desktop.MainWindow = new MainWindow
                {
                    
                    DataContext = scope.ServiceProvider.GetRequiredService<StartUpViewModel>()
                    

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

    private void SetUpContainer(IServiceCollection services)
    {
        services.AddScoped<IClipboardService, ClipboardService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IPasswordGenerator, PasswordGenerator>();
        services.AddHttpClient();
        services.AddSingleton<IHttpDataConnector<AppModel>, HttpAppDataConnector>();
        services.AddSingleton<IHttpDataConnector<CardModel>, HttpCardDataConnector>();
        services.AddSingleton<IHttpDataConnector<WebSiteModel>, HttpWebSiteDataConnector>();
        services.AddSingleton<HttpDatabaseManager>();
        services.AddSingleton<ITokenHandlerService, TokenHandlerService>();
        services.AddScoped<CardViewModel>();
        services.AddScoped<AppViewModel>();
        services.AddScoped<MenuViewModel>();
        services.AddScoped<WebSiteViewModel>();
        services.AddScoped<MainViewModel>();
        services.AddScoped<StartUpViewModel>();
        services.AddLogging(builder => builder.AddSerilog());
    }
}
