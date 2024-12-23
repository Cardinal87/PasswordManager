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

        public static async Task<WebSiteViewModel> CreateAsync(IContextFactory contextFactory, IDialogService dialogService, IItemViewModelFactory itemFactory)
        {
            var webVm = new WebSiteViewModel(contextFactory, dialogService, itemFactory);
            await webVm.LoadViewModelsListAsync();
            return webVm;
        }
        
        
        private WebSiteViewModel(IContextFactory contextFactory, IDialogService dialogService, IItemViewModelFactory itemFactory)
        {
            this.itemFactory = itemFactory;
            this.contextFactory = contextFactory;
            this.dialogService = dialogService;
            AddNewCommand = new RelayCommand(ShowAddNewDialog);
            AddToFavouriteCommand = new AsyncRelayCommand<WebSiteItemViewModel>(AddToFavouriteAsync);
            DeleteCommand = new AsyncRelayCommand<WebSiteItemViewModel>(DeleteAsync);
            ChangeCommand = new RelayCommand<WebSiteItemViewModel>(ShowChangeDialog);
            WebSites =  new ObservableCollection<WebSiteItemViewModel>();
              
        }
        private string searchKey = "";
        private IItemViewModelFactory itemFactory;
        private IDialogService dialogService;
        private IContextFactory contextFactory;
        private WebSiteItemViewModel? currentItem;
        public RelayCommand AddNewCommand { get; }
        public AsyncRelayCommand<WebSiteItemViewModel> AddToFavouriteCommand { get; set; }
        public AsyncRelayCommand<WebSiteItemViewModel> DeleteCommand { get; set; }
        public RelayCommand<WebSiteItemViewModel> ChangeCommand { get; set; }

        public ObservableCollection<WebSiteItemViewModel> WebSites { get; private set; }

        public IEnumerable<WebSiteItemViewModel> FilteredCollection
        {
            get => WebSites.Where(x => x.Name!.Contains(SearchKey, StringComparison.CurrentCultureIgnoreCase));
        }

        public string SearchKey
        {
            get => searchKey;
            set
            {
                if (value != searchKey)
                {
                    searchKey = value;
                    OnPropertyChanged(nameof(FilteredCollection));
                    OnPropertyChanged(nameof(SearchKey));
                }
            }
        }



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
        
        private async Task DeleteAsync(WebSiteItemViewModel? webSiteItem)
        {
            using (IDatabaseClient dbClient = contextFactory.CreateContext())
            {    
                if (webSiteItem != null)
                {
                    dbClient.Delete(webSiteItem.Model);
                    WebSites.Remove(webSiteItem);
                    if (WebSites.Count > 0) CurrentItem = WebSites[0];
                }
                OnPropertyChanged(nameof(FilteredCollection));
                await dbClient.SaveChangesAsync();
            }
        }

        private void ShowDialog(WebSiteDialogViewModel? Dialog)
        {
            if (Dialog != null)
            {
                Dialog.dialogResultRequest += GetDialogResult;
                dialogService.OpenDialog(Dialog);
            }
        }


        private async void GetDialogResult(object? sender, DialogResultEventArgs e)
        {
            using (IDatabaseClient dbClient = contextFactory.CreateContext())
            { 
                if (e.DialogResult && sender is WebSiteDialogViewModel vm)
                {
                    WebSiteModel model = vm.Model!;

                    if (vm.IsNew)
                    {
                        dbClient.Insert(model);
                        await dbClient.SaveChangesAsync();
                        WebSiteItemViewModel item = itemFactory.CreateWebSiteItem(model);
                        WebSites.Add(item);
                    }
                    else
                    {
                        dbClient.Replace(model);
                        var a = WebSites.FirstOrDefault(x => x.Id == model.Id);
                        a?.UpdateModel(model);
                        await dbClient.SaveChangesAsync();
                    }
                    OnPropertyChanged(nameof(FilteredCollection));
                    dialogService.CloseDialog(vm!);
                }
            }
            
           
        }
        private async Task AddToFavouriteAsync(WebSiteItemViewModel? webSiteItem)
        {
            using (IDatabaseClient dbClient = contextFactory.CreateContext())
            {   
                if (webSiteItem != null)
                {
                    WebSiteModel? el = await dbClient.GetByIdAsync<WebSiteModel>(webSiteItem.Id);
                    if (el != null)
                    {
                        webSiteItem.IsFavourite = !webSiteItem.IsFavourite;
                        el.IsFavourite = !el.IsFavourite;
                        await dbClient.SaveChangesAsync();
                    }

                }
            }
        }
        private async Task LoadViewModelsListAsync()
        {

            using (IDatabaseClient dbClient = contextFactory.CreateContext())
            {
                var models = await dbClient.GetListOfTypeAsync<WebSiteModel>();
                var viewmodels = new ObservableCollection<WebSiteItemViewModel>();
                await foreach (var model in models.ToAsyncEnumerable())
                {
                    var item = itemFactory.CreateWebSiteItem(model);
                    viewmodels.Add(item);
                }
                if (viewmodels.Count > 0) CurrentItem = viewmodels[0];
                WebSites = viewmodels;
            }

        }
    }    
    
}
