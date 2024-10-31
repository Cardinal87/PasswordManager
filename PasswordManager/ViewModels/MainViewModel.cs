
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


namespace PasswordManager.ViewModels;

internal partial class MainViewModel : ViewModelBase
{

    private ViewModelBase currentPage;
    public ViewModelBase CurrentPage {

        get { return currentPage; }
        [MemberNotNull(nameof(currentPage))]
        set {
            currentPage = value;
            OnPropertyChanged("CurrentPage");
        }
    }
    public MainViewModel(IViewModelFactory factory)
    {
        
        
        WebSitesVm = factory.CreateWebSiteVM();
        AppVm = factory.CreateAppVM();
        list.Add(WebSitesVm);
        list.Add(AppVm);
        items.AddRange(WebSitesVm.WebSites);
        items.AddRange(AppVm.Apps);


        AllEntriesVm = new AllEntriesViewModel(items);
        WebSitesVm.WebSites.CollectionChanged += AllEntriesVm.LoadViewModelList;
        AppVm.Apps.CollectionChanged += AllEntriesVm.LoadViewModelList;

        CurrentPage = AllEntriesVm;

        Subscribe();
    }
    
    private List<ObservableObject> list = new List<ObservableObject>();
    private List<ItemViewModelBase> items = new List<ItemViewModelBase>();
    public AllEntriesViewModel AllEntriesVm { get; }
    public AppViewModel AppVm { get; }
    public WebSiteViewModel WebSitesVm { get; }

    [RelayCommand]
    private void SetCurrentPage(ViewModelBase vm)
    {
        CurrentPage = vm;
    }
    private void Subscribe()
    {
        foreach (var item in list)
        {
            item.PropertyChanged += AllEntriesVm.SetItem;
        }
    }

}
