using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels
{
    internal class WebSitesViewModel : ViewModelBase
    {
        public WebSitesViewModel(DataConnectors.IDataBaseConnector con)
        {
            connector = con;
            WebSites = new ObservableCollection<WebSite>(connector.Load());
        }
        private DataConnectors.IDataBaseConnector connector;
        public ObservableCollection<WebSite> WebSites { get; private set; } 

        
    }
}
