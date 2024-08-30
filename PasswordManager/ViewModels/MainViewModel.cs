using Microsoft.Identity.Client;
using PasswordManager.ViewModels.WebSiteViewModels;
using PasswordManager.ViewModels.AllEntriesViewModels;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;

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
    public MainViewModel(DataConnectors.IDataBaseConnector con)
    {
        AllEntriesVm = new AllEntriesViewModel();
        WebSitesVm = new WebSitesViewModel(con);


        CurrentPage = AllEntriesVm;
    }


    public AllEntriesViewModel AllEntriesVm { get; }
    public WebSitesViewModel WebSitesVm { get; }

    [RelayCommand]
    public void SetCurrentPage (ViewModelBase vm)
    {
        CurrentPage = vm;
    }


}
