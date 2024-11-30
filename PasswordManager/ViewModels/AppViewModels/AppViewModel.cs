using Avalonia.Input.Platform;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Update.Internal;
using PasswordManager.DataConnectors;
using PasswordManager.Factories;
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
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.AppViewModels
{
    internal partial class AppViewModel : ViewModelBase
    {
        public AppViewModel(IDatabaseClient dbClient, IDialogService dialogService, IItemViewModelFactory itemFactory)
        {
            this.itemFactory = itemFactory;
            this.dbClient = dbClient;
            this.dialogService = dialogService;
            AddNewCommand = new RelayCommand(ShowAddNewDialog);
            AddToFavouriteCommand = new RelayCommand<AppItemViewModel>(AddToFavourite);
            DeleteCommand = new RelayCommand<AppItemViewModel>(Delete);
            ChangeCommand = new RelayCommand<AppItemViewModel>(ShowChangeDialog);
            
            LoadViewModelList();
            if (Apps.Count != 0) CurrentItem = Apps[0];
            Apps.CollectionChanged += CollectionChanged;
            OnPropertyChanged(nameof(FilteredCollection));

        }
        IItemViewModelFactory itemFactory;
        IDatabaseClient dbClient;
        IDialogService dialogService;
        private AppItemViewModel? currentItem;
        private string searchKey = "";
        public ObservableCollection<AppItemViewModel> Apps { get; set; } = new ObservableCollection<AppItemViewModel>();
        public IEnumerable<AppItemViewModel> FilteredCollection
        {
            get => Apps.Where(x => x.Name!.Contains(SearchKey, StringComparison.CurrentCultureIgnoreCase));
        }
        public RelayCommand<AppItemViewModel> AddToFavouriteCommand { get; set; }
        public RelayCommand AddNewCommand { get; set; }
        public RelayCommand<AppItemViewModel> DeleteCommand { get; set; }
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
                OnPropertyChanged(new PropertyChangedExtendedEventArgs(nameof(CurrentItem), value));
            }
        }

        
        private void Delete(AppItemViewModel? appItem)
        {
            if (appItem != null)
            {
                dbClient.Delete(appItem.Model);
                Apps.Remove(appItem);
                if (Apps.Count > 0) CurrentItem = Apps[0];
            }
            dbClient.Save();

        }
        
        private void ShowChangeDialog(AppItemViewModel? appItem)
        {
            if (appItem != null && appItem.Model != null)
                ShowDialog(new AppDialogViewModel(appItem.Model));
        }
        private void ShowAddNewDialog()
        {
            ShowDialog(new AppDialogViewModel());
        }

        private void ShowDialog(AppDialogViewModel? dialogVM)
        {
            if (dialogVM != null)
            {
                dialogVM.dialogResultRequest += GetDialogResult;
                dialogService.OpenDialog(dialogVM);
            }
        }
        private void GetDialogResult(object? sender, DialogResultEventArgs e)
        {
            if (sender is AppDialogViewModel vm)
            {
                
                if (vm.IsNew && e.DialogResult)
                {
                    dbClient.Insert(vm.Model);
                    dbClient.Save();
                    AppItemViewModel item = itemFactory.CreateAppItem(vm.Model);
                    Apps.Add(item);
                    

                }
                else if (e.DialogResult)
                {
                    dbClient.Replace(vm.Model);
                    dbClient.Save();
                    var a = Apps.FirstOrDefault(x => x.Id == vm.Model.Id);
                    a?.UpdateModel(vm.Model);
                }
                dialogService.CloseDialog(vm);
            }
            
        }
        private void AddToFavourite(AppItemViewModel? appItem)
        {
            if (appItem != null)
            {
                AppModel? el = dbClient.GetById<AppModel>(appItem.Id);
                if (el != null)
                {
                    appItem.IsFavourite = !appItem.IsFavourite;
                    el.IsFavourite = !el.IsFavourite;
                    dbClient.Save();
                }
                
            }
        }
        private void LoadViewModelList()
        {
            foreach (var model in dbClient.GetListOfType<AppModel>())
            {
                var item = itemFactory.CreateAppItem(model);
                Apps.Add(item);
            }
        }
        private void CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(FilteredCollection));
        }
    }
}
