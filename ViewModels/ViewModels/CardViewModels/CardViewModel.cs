using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models.DataConnectors;

using ViewModels;
using Models;
using ViewModels.AppViewModels;
using ViewModels.BaseClasses;
using ViewModels.Interfaces;
using ViewModels.Services;
using ViewModels.WebSiteViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.CardViewModels
{
    public class CardViewModel : ViewModelBase
    {
        
        public static async Task<CardViewModel> CreateAsync(IServiceProvider provider)
        {
            var cardVm = new CardViewModel(provider);
            await cardVm.LoadViewModelsListAsync();
            return cardVm;
        }
        
        
        private CardViewModel(IServiceProvider provider) 
        {
            this.provider = provider;
            contextFactory = provider.GetRequiredService<IDbContextFactory<DatabaseClient>>();
            dialogService = provider.GetRequiredService<IDialogService>();
            AddToFavouriteCommand = new AsyncRelayCommand<CardItemViewModel>(AddToFavouriteAsync);
            DeleteCommand = new AsyncRelayCommand<CardItemViewModel>(DeleteAsync);
            ChangeCommand = new RelayCommand<CardItemViewModel>(ShowChangeDialog);
            AddNewCommand = new RelayCommand(ShowAddNewDialog);
            Cards = new ObservableCollection<CardItemViewModel>();
            
        }
        private string searchKey = "";
        private IDbContextFactory<DatabaseClient> contextFactory;
        private IDialogService dialogService;
        IServiceProvider provider;
        public CardItemViewModel? currentItem;
        
        public CardItemViewModel? CurrentItem
        {

            get { return currentItem; }
            set
            {
                currentItem = value;
                OnPropertyChanged(nameof(CurrentItem));
            }
        }

        public ObservableCollection<CardItemViewModel> Cards { get; set; }
        public AsyncRelayCommand<CardItemViewModel> AddToFavouriteCommand { get; set; }
        public AsyncRelayCommand<CardItemViewModel> DeleteCommand { get; set; }
        public RelayCommand<CardItemViewModel> ChangeCommand { get; set; }
        public RelayCommand AddNewCommand { get; set; }



        public IEnumerable<CardItemViewModel> FilteredCollection
        {
            get => Cards.Where(x => x.Name!.Contains(SearchKey, StringComparison.CurrentCultureIgnoreCase));
        }
        public bool IsEmptyCollection {  get => !Cards.Any(); }

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

        private async Task DeleteAsync(CardItemViewModel? cardItem)
        {
            using (var dbClient = await contextFactory.CreateDbContextAsync())
            {
                if (cardItem != null)
                {
                    dbClient.Delete(cardItem.Model);
                    Cards.Remove(cardItem);
                    if (Cards.Count > 0) CurrentItem = Cards[0];
                    OnPropertyChanged(nameof(FilteredCollection));
                    OnPropertyChanged(nameof(IsEmptyCollection));
                    await dbClient.SaveChangesAsync();
                }
                
            }
        }

        private void ShowDialog(CardDialogViewModel? Dialog)
        {
            if (Dialog != null)
            {
                Dialog.dialogResultRequest += GetDialogResult;
                dialogService.OpenDialog(Dialog);
            }
        }
        private void ShowChangeDialog(CardItemViewModel? cardItem)
        {
            if (cardItem != null && cardItem.Model != null)
                ShowDialog(new CardDialogViewModel(cardItem.Model));
        }

        private void ShowAddNewDialog()
        {
            var Dialog = new CardDialogViewModel();
            ShowDialog(Dialog);
        }

        private async void GetDialogResult(object? sender, DialogResultEventArgs e)
        {
            using (var dbClient = await contextFactory.CreateDbContextAsync())
            {
                if (sender is CardDialogViewModel vm)
                {

                    if (e.DialogResult && vm.Model != null)
                    {
                        CardModel model = vm.Model;

                        if (vm.IsNew)
                        {
                            dbClient.Insert(model);
                            await dbClient.SaveChangesAsync();
                            CardItemViewModel item = new CardItemViewModel(model, provider);
                            Cards.Add(item);
                            CurrentItem = null;
                            CurrentItem = item;
                        }
                        else
                        {
                            dbClient.Replace(model);
                            var a = Cards.FirstOrDefault(x => x.Id == model.Id);
                            a?.UpdateModel(model);
                            await dbClient.SaveChangesAsync();
                        }
                        OnPropertyChanged(nameof(FilteredCollection));
                        OnPropertyChanged(nameof(IsEmptyCollection));
                    }
                    dialogService.CloseDialog(vm!);
                }
                
            }
            
            
        }

        private async Task AddToFavouriteAsync(CardItemViewModel? cardItem)
        {
            using (var dbClient = await contextFactory.CreateDbContextAsync())
            {
                if (cardItem != null)
                {
                    
                    CardModel? el = await dbClient.GetByIdAsync<CardModel>(cardItem.Id);
                    if (el != null)
                    {
                        cardItem.IsFavourite = !cardItem.IsFavourite;
                        el.IsFavourite = !el.IsFavourite;
                        await dbClient.SaveChangesAsync();
                    }

                }
            }
        }
        private async Task LoadViewModelsListAsync()
        {
            using (var dbClient = await contextFactory.CreateDbContextAsync())
            {
                
                var models = await dbClient.GetListOfTypeAsync<CardModel>();
                var viewmodels = new ObservableCollection<CardItemViewModel>();
                await foreach (var model in models.ToAsyncEnumerable())
                {
                    var item = new CardItemViewModel(model, provider);
                    viewmodels.Add(item);
                }
                if (viewmodels.Count > 0) CurrentItem = viewmodels[0];
                Cards = viewmodels;
            }

        }


    }
}
