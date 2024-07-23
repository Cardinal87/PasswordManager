using Microsoft.Identity.Client;

namespace PasswordManager.ViewModels;

internal partial class MainViewModel : ViewModelBase
{
    public MainViewModel(DataConnectors.IDataBaseConnector con)
    {
        PasMan = new PasswordManagerViewModel(con);
    }
    public PasswordManagerViewModel PasMan { get; }
    
}
