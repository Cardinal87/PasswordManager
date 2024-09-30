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
        

        public WebSitesViewModel(DataConnectors.IDataBaseClient dataBaseClient, IDialogService dialogService, IClipBoardService clipboard)
        {
            dbClient = dataBaseClient;
            this.dialogService = dialogService;
            this.clipboard = clipboard;
            ShowDialogCommand = new RelayCommand(ShowDialog);
            DeleteCommand = new RelayCommand<WebSite>(Delete);
            ChangeCommand = new RelayCommand<WebSite>(Change);
            
            WebSites =  new ObservableCollection<WebSiteItemViewModel>();
            LoadViewModelsList();  


        }
        private IClipBoardService clipboard;
        private IDialogService dialogService;
        private DataConnectors.IDataBaseClient dbClient;
        public WebSiteDialogViewModel Dialog { get; private set; }
        public ObservableCollection<WebSiteItemViewModel> WebSites { get; private set; }

        public RelayCommand<WebSite> DeleteCommand;
        public RelayCommand ShowDialogCommand;
        public RelayCommand<WebSite> ChangeCommand;
        
        
        private void Change(WebSite model)
        {

        }
        
        private void Delete(WebSite model)
        {
            dbClient.Delete(model);
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
                WebSite model = new WebSite(0, vm.Name!, vm.Login, vm.Password!,vm.WebAddress! ,vm.IsFavourite);
                // update WebSites list

            }
            dialogService.CloseDialog(Dialog);
        }

        private void LoadViewModelsList()
        {
            foreach (var a in dbClient.Load<WebSite>())
            {
                WebSiteItemViewModel item = new WebSiteItemViewModel(a, clipboard, DeleteCommand, ChangeCommand);
                WebSites.Add(item);
            }
        }

        


    }    
    
}
