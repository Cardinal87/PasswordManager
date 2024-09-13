using Microsoft.Identity.Client;
using PasswordManager.ViewModels.WebSiteViewModels;
using PasswordManager.ViewModels.AllEntriesViewModels;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Helpers;

namespace PasswordManager.ViewModels;

internal partial class MainViewModel : ViewModelBase
{
    public MainViewModel() { }

    private ViewModelBase currentPage;
    public ViewModelBase CurrentPage {

        get { return currentPage; }
        set {
            currentPage = value;
            OnPropertyChanged("CurrentPage");
        }
    }
    public MainViewModel(DataConnectors.IDataBaseConnector con, IDialogService dialogService, IClipBoardService clipboard)
    {
        connector = con;
        this.dialogService = dialogService;
        this.clipboard = clipboard;
        AllEntriesVm = new AllEntriesViewModel();
        WebSitesVm = new WebSitesViewModel(connector, this.dialogService, this.clipboard);


        CurrentPage = AllEntriesVm;
    }
    private IClipBoardService clipboard;
    private IDialogService dialogService;
    private DataConnectors.IDataBaseConnector connector;

    public AllEntriesViewModel AllEntriesVm { get; }
    public WebSitesViewModel WebSitesVm { get; }

    [RelayCommand]
    public void SetCurrentPage (ViewModelBase vm)
    {
        CurrentPage = vm;
    }


}
