
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


namespace PasswordManager.ViewModels;

internal partial class MainViewModel : ViewModelBase
{

    private ViewModelBase? currentPage;
    public ViewModelBase? CurrentPage {

        get { return currentPage; }
        [MemberNotNull(nameof(currentPage))]
        set {
            currentPage = value;
            OnPropertyChanged("CurrentPage");
        }
    }
    public MainViewModel(IViewModelFactory factory)
    {
        SetCurrentPageCommand = new RelayCommand<ViewModelBase>(SetCurrentPage);

        WebSiteVm = factory.CreateWebSiteVM();
        AppVm = factory.CreateAppVM();
        CardVm = factory.CreateCardVM();

        items.AddRange(WebSiteVm.WebSites);
        items.AddRange(AppVm.Apps);
        items.AddRange(CardVm.Cards);

        AllEntriesVm = factory.CreateAllEntriesVM(items);

        CurrentPage = AllEntriesVm;
        Subscribe();
    }
    public RelayCommand<ViewModelBase> SetCurrentPageCommand { get; set; }
    private List<ItemViewModelBase> items = new List<ItemViewModelBase>();
    public AllEntriesViewModel AllEntriesVm { get; }
    public AppViewModel AppVm { get; }
    public WebSiteViewModel WebSiteVm { get; }
    public CardViewModel CardVm { get; }
    
    private void SetCurrentPage(ViewModelBase? vm)
    {
        CurrentPage = vm;
    }

    private void Subscribe()
    {
        WebSiteVm.PropertyChanged += AllEntriesVm.SetItem;
        AppVm.PropertyChanged += AllEntriesVm.SetItem;
        CardVm.PropertyChanged += AllEntriesVm.SetItem;

        WebSiteVm.WebSites.CollectionChanged += AllEntriesVm.UpdateViewModelList;
        AppVm.Apps.CollectionChanged += AllEntriesVm.UpdateViewModelList;
        CardVm.Cards.CollectionChanged += AllEntriesVm.UpdateViewModelList;


    }

}
