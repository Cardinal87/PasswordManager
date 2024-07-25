using Microsoft.Identity.Client;

namespace PasswordManager.ViewModels;

internal partial class MainViewModel : ViewModelBase
{
    public MainViewModel(DataConnectors.IDataBaseConnector con)
    {
        PasMan = new WebSitesViewModel(con);
    }
    public WebSitesViewModel PasMan { get; }
    
}
