﻿
using ViewModels.WebSiteViewModels;
using ViewModels.AllEntriesViewModels;
using ViewModels.AppViewModels;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;


using ViewModels.CardViewModels;
using ViewModels.BaseClasses;
using System.Threading.Tasks;
using System;


namespace ViewModels;

public partial class MainViewModel : ViewModelBase
{

    private ViewModelBase? currentPage;
    

    public ViewModelBase? CurrentPage {

        get { return currentPage; }
        set {
            currentPage = value;
            OnPropertyChanged(nameof(CurrentPage));
        }
    }

    public async static Task<MainViewModel> CreateAsync(IServiceProvider provider)
    {
        var main = new MainViewModel(provider);
        await main.InizializeViewModelsAsync();
        main.CurrentPage = main.AllEntriesVm;
        return main;
    }

    private MainViewModel(IServiceProvider provider)
    {
        _provider = provider;
        SetCurrentPageCommand = new RelayCommand<ViewModelBase>(SetCurrentPage);
        
        
    }
    IServiceProvider _provider;
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
        
        var appTask = AppViewModel.CreateAsync(_provider);
        var cardTask = CardViewModel.CreateAsync(_provider);
        var webTask = WebSiteViewModel.CreateAsync(_provider);
        
        AppVm = await appTask;
        CardVm = await cardTask;
        WebSiteVm = await webTask;

        AllEntriesVm = new AllEntriesViewModel(AppVm, CardVm, WebSiteVm);
    }

}
