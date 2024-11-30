using Avalonia.Markup.Xaml.MarkupExtensions;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PasswordManager;
using PasswordManager.DataConnectors;
using PasswordManager.Factories;
using PasswordManager.Helpers;
using PasswordManager.Models;
using PasswordManager.ViewModels.AppViewModels;
using PasswordManager.ViewModels.BaseClasses;
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
    internal partial class WebSiteViewModel : ViewModelBase
    {

        public WebSiteViewModel(IDatabaseClient dbClient, IDialogService dialogService, IItemViewModelFactory itemFactory)
        {
            this.itemFactory = itemFactory;
            this.dbClient = dbClient;
            this.dialogService = dialogService;
            AddNewCommand = new RelayCommand(ShowAddNewDialog);
            AddToFavouriteCommand = new RelayCommand<WebSiteItemViewModel>(AddToFavourite);
            DeleteCommand = new RelayCommand<WebSiteItemViewModel>(Delete);
            ChangeCommand = new RelayCommand<WebSiteItemViewModel>(ShowChangeDialog);
            WebSites =  new ObservableCollection<WebSiteItemViewModel>();
            LoadViewModelsList();  
        }
        private IItemViewModelFactory itemFactory;
        private IDialogService dialogService;
        private IDatabaseClient dbClient;
        private WebSiteItemViewModel? currentItem;
        public RelayCommand AddNewCommand { get; }
        public RelayCommand<WebSiteItemViewModel> AddToFavouriteCommand { get; set; }
        public RelayCommand<WebSiteItemViewModel> DeleteCommand { get; set; }
        public RelayCommand<WebSiteItemViewModel> ChangeCommand { get; set; }

        public ObservableCollection<WebSiteItemViewModel> WebSites { get; private set; }
        
        public WebSiteItemViewModel? CurrentItem
        {

            get { return currentItem; }
            set
            {
                currentItem = value;
                OnPropertyChanged(new PropertyChangedExtendedEventArgs(nameof(CurrentItem), value));
            }
        }

        private void ShowChangeDialog(WebSiteItemViewModel? webSiteItem)
        {
            if (webSiteItem != null && webSiteItem.Model != null)
                ShowDialog(new WebSiteDialogViewModel(webSiteItem.Model));
        }
        
        private void ShowAddNewDialog() 
        { 
            var Dialog = new WebSiteDialogViewModel();
            ShowDialog(Dialog);
        }
        
        private void Delete(WebSiteItemViewModel? webSiteItem)
        {
            if (webSiteItem != null)
            {
                dbClient.Delete(webSiteItem.Model);
                WebSites.Remove(webSiteItem);
                if (WebSites.Count > 0) CurrentItem = WebSites[0];
            }
            dbClient.Save();
        }

        private void ShowDialog(WebSiteDialogViewModel? Dialog)
        {
            if (Dialog != null)
            {
                Dialog.dialogResultRequest += GetDialogResult;
                dialogService.OpenDialog(Dialog);
            }
        }


        private void GetDialogResult(object? sender, DialogResultEventArgs e)
        {
            if (e.DialogResult && sender is WebSiteDialogViewModel vm)
            {
                WebSiteModel model = vm.Model!;
                
                if (vm.IsNew)
                {
                    dbClient.Insert(model);
                    dbClient.Save();
                    WebSiteItemViewModel item = itemFactory.CreateWebSiteItem(model);
                    WebSites.Add(item);
                }
                else
                {
                    dbClient.Replace(model);
                    var a = WebSites.FirstOrDefault(x => x.Id == model.Id);
                    a?.UpdateModel(model);
                    dbClient.Save();
                }
                dialogService.CloseDialog(vm!);
            }
            
           
        }
        private void AddToFavourite(WebSiteItemViewModel? webSiteItem)
        {
            if (webSiteItem != null)
            {
                WebSiteModel? el = dbClient.GetById<WebSiteModel>(webSiteItem.Id);
                if (el != null)
                {
                    webSiteItem.IsFavourite = !webSiteItem.IsFavourite;
                    el.IsFavourite = !el.IsFavourite;
                    dbClient.Save();
                }

            }
        }
        private void LoadViewModelsList()
        {
            foreach (var a in dbClient.GetListOfType<WebSiteModel>())
            {
                var item = itemFactory.CreateWebSiteItem(a);
                WebSites.Add(item);
            }

        }
    }    
    
}
