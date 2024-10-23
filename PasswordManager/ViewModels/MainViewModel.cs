
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
    public MainViewModel(DataConnectors.IDataBaseClient dbClient, IDialogService dialogService, IClipBoardService clipboard)
    {
        this.dbClient = dbClient;
        this.dialogService = dialogService;
        this.clipboard = clipboard;

        
        WebSitesVm = new WebSitesViewModel(dbClient, dialogService, clipboard);
        AppVm = new AppViewModel(dbClient, dialogService, clipboard);
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
    private IClipBoardService clipboard;
    private IDialogService dialogService;
    private DataConnectors.IDataBaseClient dbClient;
    private List<ObservableObject> list = new List<ObservableObject>();
    private List<ItemViewModelBase> items = new List<ItemViewModelBase>();
    public AllEntriesViewModel AllEntriesVm { get; }
    public AppViewModel AppVm { get; }
    public WebSitesViewModel WebSitesVm { get; }

    [RelayCommand]
    public void SetCurrentPage(ViewModelBase vm)
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
