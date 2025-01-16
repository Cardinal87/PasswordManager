using CommunityToolkit.Mvvm.Input;
using PasswordManager.ViewModels;
using PasswordManager.Models;
using PasswordManager.ViewModels.AppViewModels;
using PasswordManager.ViewModels.BaseClasses;
using PasswordManager.ViewModels.CardViewModels;
using PasswordManager.ViewModels.WebSiteViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.AllEntriesViewModels
{
    internal class AllEntriesViewModel : ViewModelBase
    {
        
        public AllEntriesViewModel(AppViewModel appVm, CardViewModel cardVm, WebSiteViewModel webVm) 
        {
            AppViewModel = appVm;
            WebSiteViewModel = webVm;
            CardViewModel = cardVm;

            ChangeCommand = new RelayCommand<ItemViewModelBase>(ShowChangeDialog);
            DeleteCommand = new AsyncRelayCommand<ItemViewModelBase>(DeleteAsync);
            AddToFavouriteCommand = new AsyncRelayCommand<ItemViewModelBase>(AddToFavouriteAsync);
            

            AppViewModel.Apps.CollectionChanged += UpdateItems;
            WebSiteViewModel.WebSites.CollectionChanged += UpdateItems;
            CardViewModel.Cards.CollectionChanged += UpdateItems;


            Items = [.. WebSiteViewModel.WebSites, .. AppViewModel.Apps, .. CardViewModel.Cards];
            if (Items.Count > 0) CurrentItem = Items[0];

        }
        
        public RelayCommand<ItemViewModelBase> ChangeCommand { get;private set; }
        public AsyncRelayCommand<ItemViewModelBase> DeleteCommand { get; private set; }
        public AsyncRelayCommand<ItemViewModelBase> AddToFavouriteCommand { get; private set; }

        public AppViewModel AppViewModel { get; set; }
        public CardViewModel CardViewModel { get; set; }
        public WebSiteViewModel WebSiteViewModel { get; set; }

        public ObservableCollection<ItemViewModelBase> Items { get; private set; }

        private string searchKey = "";
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
        public IEnumerable<ItemViewModelBase> FilteredCollection 
        {
            get => Items.Where(x => x.Name!.Contains(SearchKey, StringComparison.CurrentCultureIgnoreCase));
        }
        private ItemViewModelBase? currentItem;
        public ItemViewModelBase? CurrentItem
        {

            get { return currentItem; }
            set
            {
                currentItem = value;
                OnPropertyChanged(nameof(CurrentItem));
            }
        }

        private async Task DeleteAsync(ItemViewModelBase? item)
        {
            if (item != null)
            {
                if (item.GetType() == typeof(CardItemViewModel))
                    await CardViewModel.DeleteCommand.ExecuteAsync((CardItemViewModel)item);
                else if (item.GetType() == typeof(WebSiteItemViewModel))
                    await WebSiteViewModel.DeleteCommand.ExecuteAsync((WebSiteItemViewModel)item);
                else if (item.GetType() == typeof(AppItemViewModel))
                    await AppViewModel.DeleteCommand.ExecuteAsync((AppItemViewModel)item);
                OnPropertyChanged(nameof(FilteredCollection));
            }
        }

        private async Task AddToFavouriteAsync(ItemViewModelBase? item)
        {
            if (item != null)
            {
                if (item.GetType() == typeof(CardItemViewModel))
                    await CardViewModel.AddToFavouriteCommand.ExecuteAsync((CardItemViewModel)item);
                else if (item.GetType() == typeof(WebSiteItemViewModel))
                    await WebSiteViewModel.AddToFavouriteCommand.ExecuteAsync((WebSiteItemViewModel)item);
                else if (item.GetType() == typeof(AppItemViewModel))
                    await AppViewModel.AddToFavouriteCommand.ExecuteAsync((AppItemViewModel)item);
                OnPropertyChanged(nameof(FilteredCollection));
            }
        }

        private void ShowChangeDialog(ItemViewModelBase? item)
        {
            if (item != null)
            {
                if (item.GetType() == typeof(CardItemViewModel))
                    CardViewModel.ChangeCommand.Execute((CardItemViewModel)item);
                else if (item.GetType() == typeof(WebSiteItemViewModel))
                    WebSiteViewModel.ChangeCommand.Execute((WebSiteItemViewModel)item);
                else if (item.GetType() == typeof(AppItemViewModel))
                    AppViewModel.ChangeCommand.Execute((AppItemViewModel)item);
            }
        }

        
        public void UpdateItems(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems![0] is ItemViewModelBase nw)
            {
                Items.Add(nw);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems![0] is ItemViewModelBase old)
            {
                var el = Items.FirstOrDefault(x => x.Id == old.Id);
                if (el != null) 
                Items.Remove(el);
            }
            OnPropertyChanged(nameof(FilteredCollection));
        }
        

    }
}
