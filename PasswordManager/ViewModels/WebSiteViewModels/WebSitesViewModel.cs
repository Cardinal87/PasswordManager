using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PasswordManager;
using PasswordManager.Helpers;
using PasswordManager.Models;
using PasswordManager.ViewModels.DialogInterfaces;
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
            //foreach (var a in con.Load())d
            //{
            //    WebSitesItemViewModel item = new WebSitesItemViewModel(a);
            //    item.DeleteCommand = new RelayCommand<int>(Delete);
            //    websites.Add(item);
            //}
            //WebSites = websites;
            
            
        }
        private IDialogService dialogService;
        private DataConnectors.IDataBaseConnector connector;
        public WebSiteDialogViewModel Dialog { get; private set; }
        public ObservableCollection<WebSitesItemViewModel> WebSites { get; private set; }

        public RelayCommand DeleteCommand;
        public RelayCommand ShowDialogCommand;
        public RelayCommand ChangeCommand;
        
        
        private void Delete(int id)
        {
            connector.Delete(id);
            // update WebSites list
        }

        public void ShowDialog() => dialogService.OpenDialog(Dialog);
        
        public void GetDialogResult(object sender, DialogResultEventArgs e)
        {
            if (e.DialogResult)
            {
                WebSiteDialogViewModel? vm = sender as WebSiteDialogViewModel;
                if (vm == null) return;
                WebSite model = new WebSite(0, vm.Name!, vm.Login, vm.Password!);
                // update WebSites list

            }
        }

        


    }    
    
}
