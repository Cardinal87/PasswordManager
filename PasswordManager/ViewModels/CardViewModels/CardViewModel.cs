using CommunityToolkit.Mvvm.Input;
using PasswordManager.DataConnectors;
using PasswordManager.Factories;
using PasswordManager.Helpers;
using PasswordManager.Models;
using PasswordManager.ViewModels.AppViewModels;
using PasswordManager.ViewModels.BaseClasses;
using PasswordManager.ViewModels.Interfaces;
using PasswordManager.ViewModels.WebSiteViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.CardViewModels
{
    internal class CardViewModel : ViewModelBase
    {
        
        public static async Task<CardViewModel> CreateAsync(IContextFactory contextFactory, IDialogService dialogService, IItemViewModelFactory itemFactory)
        {
            var cardVm = new CardViewModel(contextFactory,dialogService, itemFactory);
            await cardVm.LoadViewModelsListAsync();
            return cardVm;
        }
        
        
        private CardViewModel(IContextFactory contextFactory, IDialogService dialogService, IItemViewModelFactory itemFactory) 
        { 
            this.contextFactory = contextFactory;
            this.dialogService = dialogService;
            this.itemFactory = itemFactory;
            AddToFavouriteCommand = new AsyncRelayCommand<CardItemViewModel>(AddToFavouriteAsync);
            DeleteCommand = new AsyncRelayCommand<CardItemViewModel>(DeleteAsync);
            ChangeCommand = new RelayCommand<CardItemViewModel>(ShowChangeDialog);
            AddNewCommand = new RelayCommand(ShowAddNewDialog);
            Cards = new ObservableCollection<CardItemViewModel>();
            
        }
        private IContextFactory contextFactory;
        private IDialogService dialogService;
        private IItemViewModelFactory itemFactory;
        public CardItemViewModel? currentItem;
        
        public CardItemViewModel? CurrentItem
        {

            get { return currentItem; }
            set
            {
                currentItem = value;
                OnPropertyChanged(new PropertyChangedExtendedEventArgs(nameof(CurrentItem), value));
            }
        }

        public ObservableCollection<CardItemViewModel> Cards { get; set; }
        public AsyncRelayCommand<CardItemViewModel> AddToFavouriteCommand { get; set; }
        public AsyncRelayCommand<CardItemViewModel> DeleteCommand { get; set; }
        public RelayCommand<CardItemViewModel> ChangeCommand { get; set; }
        public RelayCommand AddNewCommand { get; set; }

        
        private async Task DeleteAsync(CardItemViewModel? cardItem)
        {
            using (IDatabaseClient dbClient = contextFactory.CreateContext())
            {
                if (cardItem != null)
                {
                    dbClient.Delete(cardItem.Model);
                    Cards.Remove(cardItem);
                    if (Cards.Count > 0) CurrentItem = Cards[0];
                }
                await dbClient.SaveChangesAsync();
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
            using (IDatabaseClient dbClient = contextFactory.CreateContext())
            {
                if (e.DialogResult && sender is CardDialogViewModel vm)
                {

                    CardModel model = vm.Model!;

                    if (vm.IsNew)
                    {
                        dbClient.Insert(model);
                        await dbClient.SaveChangesAsync();
                        CardItemViewModel item = itemFactory.CreateCardItem(model);
                        Cards.Add(item);
                    }
                    else
                    {
                        dbClient.Replace(model);
                        var a = Cards.FirstOrDefault(x => x.Id == model.Id);
                        a?.UpdateModel(model);
                        await dbClient.SaveChangesAsync();
                    }
                    dialogService.CloseDialog(vm!);
                }
            }
            
            
        }

        private async Task AddToFavouriteAsync(CardItemViewModel? cardItem)
        {
            using (IDatabaseClient dbClient = contextFactory.CreateContext())
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
            using (IDatabaseClient dbClient = contextFactory.CreateContext())
            {
                var models = await dbClient.GetListOfTypeAsync<CardModel>();
                var viewmodels = new ObservableCollection<CardItemViewModel>();
                await foreach (var model in models.ToAsyncEnumerable())
                {
                    var item = itemFactory.CreateCardItem(model);
                    viewmodels.Add(item);
                }
                if (viewmodels.Count > 0) CurrentItem = viewmodels[0];
                Cards = viewmodels;
            }

        }


    }
}
