using Autofac;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.DataConnectors;
using PasswordManager.Factories;
using PasswordManager.Helpers;
using PasswordManager.ViewModels;
using PasswordManager.ViewModels.AppViewModels;
using PasswordManager.ViewModels.WebSiteViewModels;
using PasswordManager.Views;
using PasswordManager.Views.WebSiteViews;

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

        var builder = new ContainerBuilder();
        SetUpContainer(builder);
        var container = builder.Build();
        
        
        IDatabaseClient dataBaseClient = new DataBaseClient();
        IClipboardService clipboard = new ClipboardService();
        IDialogService dialogService = new DialogService();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            using (var a = container.BeginLifetimeScope())
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = container.Resolve<MainViewModel>()
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
    private void SetUpContainer(ContainerBuilder builder)
    {
        builder.RegisterType<ClipboardService>().As<IClipboardService>();
        builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
        builder.RegisterType<DataBaseClient>().As<IDatabaseClient>().InstancePerDependency();
        builder.RegisterType<MainViewModel>().AsSelf().SingleInstance();
        builder.RegisterType<ViewModelFactory>().As<IViewModelFactory>().SingleInstance();
        builder.RegisterType<ItemViewModelFactory>().As<IItemViewModelFactory>().SingleInstance();
        
    }
}
