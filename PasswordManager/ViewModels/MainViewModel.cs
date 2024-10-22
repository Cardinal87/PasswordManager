
using PasswordManager.ViewModels.WebSiteViewModels;
using PasswordManager.ViewModels.AllEntriesViewModels;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Helpers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

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
        WebSitesVm = new WebSitesViewModel(dbClient, this.dialogService, this.clipboard);
        itemsCollections.Add(WebSitesVm.WebSites.Select(x => (ItemViewModelBase)x).ToList());
        AllEntriesVm = new AllEntriesViewModel(LoadViewModelList);
        CurrentPage = AllEntriesVm;

        WebSitesVm.OnDataBaseChanged += AllEntriesVm.LoadViewModelList;
    }
    private IClipBoardService clipboard;
    private IDialogService dialogService;
    private DataConnectors.IDataBaseClient dbClient;
    private List<List<ItemViewModelBase>> itemsCollections = new List<List<ItemViewModelBase>>();

    public AllEntriesViewModel AllEntriesVm { get; }
    public WebSitesViewModel WebSitesVm { get; }

    [RelayCommand]
    public void SetCurrentPage(ViewModelBase vm)
    {
        CurrentPage = vm;
    }
    private ObservableCollection<ItemViewModelBase> LoadViewModelList()
    {
        ObservableCollection<ItemViewModelBase> items = new ObservableCollection<ItemViewModelBase>();
        foreach(var collection in itemsCollections)
        {
            foreach(var item in collection)
            {
                items.Add(item);
            }
        }  
        return items;
    
    }

}
