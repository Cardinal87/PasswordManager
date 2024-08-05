using Microsoft.Identity.Client;

namespace PasswordManager.ViewModels;

internal partial class MainViewModel : ViewModelBase
{
    public MainViewModel() { }
    
    public MainViewModel(DataConnectors.IDataBaseConnector con)
    {
        WebSitesVm = new WebSitesViewModel(con);
    }
    public WebSitesViewModel WebSitesVm { get; }
    
}
