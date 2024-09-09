using CommunityToolkit.Mvvm.Input;
using PasswordManager;
using PasswordManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.WebSiteViewModels
{
    internal partial class WebSitesViewModel : ViewModelBase
    {
        
        public WebSitesViewModel() { }
        

        public WebSitesViewModel(DataConnectors.IDataBaseConnector con)
        {
            connector = con;
            ObservableCollection<WebSitesItemViewModel> websites = new ObservableCollection<WebSitesItemViewModel>();
            //foreach (var a in con.Load())
            //{
            //    WebSitesItemViewModel item = new WebSitesItemViewModel(a);
            //    item.DeleteCommand = new RelayCommand<int>(Delete);
            //    websites.Add(item);
            //}
            //WebSites = websites;
            
            
        }
        public WebSiteFormViewModel Form { get; private set; }
        private DataConnectors.IDataBaseConnector connector;
        public ObservableCollection<WebSitesItemViewModel> WebSites { get; private set; }
        private void Delete(int id)
        {
            WebSites.Remove(WebSites.First(x => x.Id == id));
            connector.Delete(id);
        }
        
        public void ShowDialog()
        {
            

        }


    }    
    
}
