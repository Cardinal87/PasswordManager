using Avalonia.Markup.Xaml.MarkupExtensions;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PasswordManager;
using PasswordManager.Helpers;
using PasswordManager.Models;
using PasswordManager.ViewModels.Interfaces;
using PasswordManager.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
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
            
            WebSites =  new ObservableCollection<WebSiteItemViewModel>();
            LoadViewModelsList();  
        }
        private IClipBoardService clipboard;
        private IDialogService dialogService;
        private DataConnectors.IDataBaseClient dbClient;
        private ItemViewModelBase? currentItem;

        
        
        public WebSiteDialogViewModel? Dialog { get; private set; }
        public ObservableCollection<WebSiteItemViewModel> WebSites { get; private set; }
        
        public ItemViewModelBase? CurrentItem
        {

            get { return currentItem; }
            set
            {
                currentItem = value;
                OnPropertyChanged(new PropertyChangedEtendedEventArgs(nameof(CurrentItem), value));
            }
        }

        private void Change(WebSite model)
        {
            Dialog = new WebSiteDialogViewModel(model);
            ShowDialog();
        }
        [RelayCommand]
        public void AddNew() 
        { 
            Dialog = new WebSiteDialogViewModel();
            ShowDialog();
        }
        private void ShowData(ItemViewModelBase vm)
        {
            CurrentItem = vm;
            
        }
        private void Delete(WebSite model)
        {
            dbClient.Delete(model);
            LoadViewModelsList();
            

        }

        public void ShowDialog()
        {
            if (Dialog != null)
            {
                Dialog.dialogResultRequest += GetDialogResult;
                dialogService.OpenDialog(Dialog!);
            }
        }
        public void GetDialogResult(object? sender, DialogResultEventArgs e)
        {
            if (e.DialogResult && sender != null)
            {
                WebSiteDialogViewModel vm = (WebSiteDialogViewModel)sender;
                WebSite model = new WebSite(vm.Name, vm.Login, vm.Password,vm.WebAddress,vm.IsFavourite);
                if (vm.IsNew) dbClient.Save(model);
                else dbClient.UpdateList(model);
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
                WebSiteItemViewModel item = new WebSiteItemViewModel(a, clipboard,new RelayCommand(() => Delete(a)), new RelayCommand(() => Change(a)), ShowData);
                WebSites.Add(item);
            }
            
        }
    }    
    
}
