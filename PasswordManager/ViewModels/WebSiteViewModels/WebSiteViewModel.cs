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
        
        
        

        public WebSitesViewModel(DataConnectors.IDataBaseClient dataBaseClient, IDialogService dialogService, IClipBoardService clipboard)
        {
            dbClient = dataBaseClient;
            this.dialogService = dialogService;
            this.clipboard = clipboard;
            
            DeleteCommand = new RelayCommand<WebSite>(Delete);
            ChangeCommand = new RelayCommand<WebSite>(Change);
            
            WebSites =  new ObservableCollection<WebSiteItemViewModel>();
            LoadViewModelsList();  

            


        }
        private IClipBoardService clipboard;
        private IDialogService dialogService;
        private DataConnectors.IDataBaseClient dbClient;
        public WebSiteDialogViewModel? Dialog { get; private set; }
        public ObservableCollection<WebSiteItemViewModel> WebSites { get; private set; }

        public RelayCommand<WebSite> DeleteCommand;
        public RelayCommand<WebSite> ChangeCommand;


        public void Change(WebSite? model)
        {
            Dialog = new WebSiteDialogViewModel(model!);
            ShowDialog();
        }
        public void AddNew() 
        { 
            Dialog = new WebSiteDialogViewModel();
            ShowDialog();
        }

        private void Delete(WebSite? model)
        {
            dbClient.Delete(model!);
            
            LoadViewModelsList();
        }

        public void ShowDialog()
        {
            Dialog!.dialogResultRequest += GetDialogResult;
            dialogService.OpenDialog(Dialog);
        }
        public void GetDialogResult(object? sender, DialogResultEventArgs e)
        {
            if (e.DialogResult)
            {
                WebSiteDialogViewModel? vm = sender as WebSiteDialogViewModel;
                if (vm == null) return;
                WebSite model = new WebSite(vm.Name!, vm.Login, vm.Password!,vm.WebAddress! ,vm.IsFavourite);
                WebSite model1 = new WebSite("vm.Name!", "vm.Login"," vm.Password!", "vm.WebAddress!",true);
                dbClient.Save(model1);
                LoadViewModelsList();

            }
            dialogService.CloseDialog(Dialog!);
            Dialog = null;
        }

        private void LoadViewModelsList()
        {
            foreach (var a in dbClient.Load<WebSite>())
            {
                WebSites.Clear();
                WebSiteItemViewModel item = new WebSiteItemViewModel(a, clipboard, DeleteCommand, ChangeCommand);
                WebSites.Add(item);
            }
        }

        


    }    
    
}
