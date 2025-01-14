using Avalonia.Input.Platform;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.DataConnectors;

using PasswordManager.Helpers;
using PasswordManager.Models;
using PasswordManager.ViewModels.BaseClasses;
using PasswordManager.ViewModels.Interfaces;
using PasswordManager.ViewModels.WebSiteViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.AppViewModels
{
    internal partial class AppViewModel : ViewModelBase
    {
        
        public static async Task<AppViewModel> CreateAsync(IServiceProvider provider)
        {
            var appVm = new AppViewModel(provider);
            await appVm.LoadViewModelListAsync();
            return appVm;
        }
        
        private AppViewModel(IServiceProvider provider)
        {
            this.provider = provider;
            contextFactory = provider.GetRequiredService<IDbContextFactory<DatabaseClient>>();
            dialogService = provider.GetRequiredService<IDialogService>();
            
            AddNewCommand = new RelayCommand(ShowAddNewDialog);
            AddToFavouriteCommand = new AsyncRelayCommand<AppItemViewModel>(AddToFavouriteAsync);
            DeleteCommand = new AsyncRelayCommand<AppItemViewModel>(DeleteAsync);
            ChangeCommand = new RelayCommand<AppItemViewModel>(ShowChangeDialog);
            
        }
        IServiceProvider provider;
        IDbContextFactory<DatabaseClient> contextFactory;
        IDialogService dialogService;
        private AppItemViewModel? currentItem;
        private string searchKey = "";
        public ObservableCollection<AppItemViewModel> Apps { get; set; } = new ObservableCollection<AppItemViewModel>();
        public IEnumerable<AppItemViewModel> FilteredCollection
        {
            get => Apps.Where(x => x.Name!.Contains(SearchKey, StringComparison.CurrentCultureIgnoreCase));
        }

        public bool IsEmptyCollection { get => !Apps.Any(); }
        public AsyncRelayCommand<AppItemViewModel> AddToFavouriteCommand { get; set; }
        public RelayCommand AddNewCommand { get; set; }
        public AsyncRelayCommand<AppItemViewModel> DeleteCommand { get; set; }
        public RelayCommand<AppItemViewModel> ChangeCommand { get; set; }
        
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

        public AppItemViewModel? CurrentItem
        {

            get { return currentItem; }
            set
            {
                currentItem = value;
                OnPropertyChanged(nameof(CurrentItem));
            }
        }

        
        private async Task DeleteAsync(AppItemViewModel? appItem)
        {
            using (var dbClient = await contextFactory.CreateDbContextAsync()) 
            {
                if (appItem != null)
                {
                    dbClient.Delete(appItem.Model);
                    Apps.Remove(appItem);
                    CurrentItem = Apps.Count != 0 ? Apps[0] : null;
                    OnPropertyChanged(nameof(FilteredCollection));
                    OnPropertyChanged(nameof(IsEmptyCollection));
                    await dbClient.SaveChangesAsync();
                }
            }
        }
        
        private void ShowChangeDialog(AppItemViewModel? appItem)
        {
            if (appItem != null && appItem.Model != null)
                ShowDialog(new AppDialogViewModel(appItem.Model, provider));
        }
        private void ShowAddNewDialog()
        {
            ShowDialog(new AppDialogViewModel(provider));
        }

        private void ShowDialog(AppDialogViewModel? dialogVM)
        {
            if (dialogVM != null)
            {
                dialogVM.dialogResultRequest += GetDialogResult;
                dialogService.OpenDialog(dialogVM);
            }
        }
        private async void GetDialogResult(object? sender, DialogResultEventArgs e)
        {
            using (var dbClient = contextFactory.CreateDbContext())
            {
                
                if (sender is AppDialogViewModel vm)
                {

                    if (e.DialogResult && vm.Model != null)
                    {
                        if (vm.IsNew)
                        {
                            dbClient.Insert(vm.Model);
                            await dbClient.SaveChangesAsync();
                            AppItemViewModel item = new AppItemViewModel(vm.Model, provider);
                            Apps.Add(item);
                            CurrentItem = item;
                        }
                        else
                        {
                            dbClient.Replace(vm.Model);
                            var a = Apps.FirstOrDefault(x => x.Id == vm.Model.Id);
                            a?.UpdateModel(vm.Model);
                            await dbClient.SaveChangesAsync();
                        }
                        OnPropertyChanged(nameof(FilteredCollection));
                        OnPropertyChanged(nameof(IsEmptyCollection));
                    }
                    dialogService.CloseDialog(vm);
                }
            }
            
        }
        private async Task AddToFavouriteAsync(AppItemViewModel? appItem)
        {
            if (appItem != null)
            {
                appItem.IsFavourite = !appItem.IsFavourite;
                using (var dbClient = await contextFactory.CreateDbContextAsync())
                {
                    AppModel? el = await dbClient.GetByIdAsync<AppModel>(appItem.Id);
                    if (el != null)
                    {
                        el.IsFavourite = !el.IsFavourite;
                        await dbClient.SaveChangesAsync();
                    }

                }
            }
        }
        private async Task LoadViewModelListAsync()
        {

            using (var dbClient = contextFactory.CreateDbContext())
            {
                await Task.Yield();
                var models = await dbClient.GetListOfTypeAsync<AppModel>();
                var viewmodels = new ObservableCollection<AppItemViewModel>();
                await foreach (var model in models.ToAsyncEnumerable())
                {
                    var item = new AppItemViewModel(model, provider);
                    viewmodels.Add(item);
                }
                if (viewmodels.Count > 0) CurrentItem = viewmodels[0];
                Apps = viewmodels;
            }
            
        }
        
    }
}
