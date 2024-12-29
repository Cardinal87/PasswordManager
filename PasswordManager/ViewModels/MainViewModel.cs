
using PasswordManager.ViewModels.WebSiteViewModels;
using PasswordManager.ViewModels.AllEntriesViewModels;
using PasswordManager.ViewModels.AppViewModels;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Helpers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Net;
using PasswordManager.DataConnectors;
using PasswordManager.Factories;
using PasswordManager.ViewModels.CardViewModels;
using PasswordManager.ViewModels.BaseClasses;
using System.Threading.Tasks;


namespace PasswordManager.ViewModels;

internal partial class MainViewModel : ViewModelBase
{

    private ViewModelBase? currentPage;
    private IViewModelFactory _factory;

    public ViewModelBase? CurrentPage {

        get { return currentPage; }
        set {
            currentPage = value;
            OnPropertyChanged(nameof(CurrentPage));
        }
    }

    public async static Task<MainViewModel> CreateAsync(IViewModelFactory factory)
    {
        var main = new MainViewModel(factory);
        await main.InizializeViewModelsAsync();
        main.CurrentPage = main.AllEntriesVm;
        return main;
    }

    private MainViewModel(IViewModelFactory factory)
    {
        _factory = factory;
        SetCurrentPageCommand = new RelayCommand<ViewModelBase>(SetCurrentPage);
        
        
    }
    public RelayCommand<ViewModelBase> SetCurrentPageCommand { get; set; }
    private List<ItemViewModelBase> items = new();
    public AllEntriesViewModel? AllEntriesVm { get; private set; }
    public AppViewModel? AppVm { get; private set; }
    public WebSiteViewModel? WebSiteVm { get; private set; }
    public CardViewModel? CardVm { get; private set; }
    
    private void SetCurrentPage(ViewModelBase? vm)
    {
        CurrentPage = vm;
    }

    
    private async Task InizializeViewModelsAsync()
    {
        var appTask =  _factory.CreateAppVMAsync();
        var cardTask = _factory.CreateCardVMAsync();
        var webTask = _factory.CreateWebSiteVMAsync();
        
        AppVm = await appTask;
        CardVm = await cardTask;
        WebSiteVm = await webTask;

        AllEntriesVm = _factory.CreateAllEntriesVM(WebSiteVm, CardVm, AppVm);
    }

}
