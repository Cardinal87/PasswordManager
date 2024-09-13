using Avalonia.Markup.Xaml.MarkupExtensions;
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
        

        public WebSitesViewModel(DataConnectors.IDataBaseConnector con, IDialogService dialogService, IClipBoardService clipboard)
        {
            connector = con;
            this.dialogService = dialogService;
            this.clipboard = clipboard;
            ShowDialogCommand = new RelayCommand(ShowDialog);
            DeleteCommand = new RelayCommand<int>(Delete);
            ChangeCommand = new RelayCommand<int>(Change);
            
            WebSites =  new ObservableCollection<WebSitesItemViewModel>();
            LoadViewModelsList();  


        }
        private IClipBoardService clipboard;
        private IDialogService dialogService;
        private DataConnectors.IDataBaseConnector connector;
        public WebSiteDialogViewModel Dialog { get; private set; }
        public ObservableCollection<WebSitesItemViewModel> WebSites { get; private set; }

        public RelayCommand<int> DeleteCommand;
        public RelayCommand ShowDialogCommand;
        public RelayCommand<int> ChangeCommand;
        
        
        private void Change(int id)
        {

        }
        
        private void Delete(int id)
        {
            connector.Delete(id);
            // update WebSites list
        }

        public void ShowDialog()
        {
            Dialog = new WebSiteDialogViewModel();
            dialogService.OpenDialog(Dialog);
        }
        public void GetDialogResult(object sender, DialogResultEventArgs e)
        {
            if (e.DialogResult)
            {
                WebSiteDialogViewModel? vm = sender as WebSiteDialogViewModel;
                if (vm == null) return;
                WebSite model = new WebSite(0, vm.Name!, vm.Login, vm.Password!);
                // update WebSites list

            }
            dialogService.CloseDialog(Dialog);
        }

        private void LoadViewModelsList()
        {
            foreach (var a in connector.Load())
            {
                WebSitesItemViewModel item = new WebSitesItemViewModel(a, clipboard, DeleteCommand, ChangeCommand);
                WebSites.Add(item);
            }
        }

        


    }    
    
}
