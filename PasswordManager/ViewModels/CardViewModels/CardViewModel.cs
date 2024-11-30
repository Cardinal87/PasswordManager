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
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.CardViewModels
{
    internal class CardViewModel : ViewModelBase
    {
        public CardViewModel(IDatabaseClient dbClient, IDialogService dialogService, IItemViewModelFactory itemFactory) 
        { 
            this.dbClient = dbClient;
            this.dialogService = dialogService;
            this.itemFactory = itemFactory;
            AddToFavouriteCommand = new RelayCommand<CardItemViewModel>(AddToFavourite);
            DeleteCommand = new RelayCommand<CardItemViewModel>(Delete);
            ChangeCommand = new RelayCommand<CardItemViewModel>(ShowChangeDialog);
            AddNewCommand = new RelayCommand(ShowAddNewDialog);

            Cards = new ObservableCollection<CardItemViewModel>();
            LoadViewModelsList();
        }
        private IDatabaseClient dbClient;
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
        public RelayCommand<CardItemViewModel> AddToFavouriteCommand { get; set; }
        public RelayCommand<CardItemViewModel> DeleteCommand { get; set; }
        public RelayCommand<CardItemViewModel> ChangeCommand { get; set; }
        public RelayCommand AddNewCommand { get; set; }

        
        private void Delete(CardItemViewModel? cardItem)
        {
            if (cardItem != null)
            {
                dbClient.Delete(cardItem.Model);
                Cards.Remove(cardItem);
                if (Cards.Count > 0) CurrentItem = Cards[0];
            }
            dbClient.Save();
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

        private void GetDialogResult(object? sender, DialogResultEventArgs e)
        {
            if (e.DialogResult && sender is CardDialogViewModel vm)
            {
                
                CardModel model = vm.Model!;

                if (vm.IsNew)
                {
                    dbClient.Insert(model);
                    dbClient.Save();
                    CardItemViewModel item = itemFactory.CreateCardItem(model);
                    Cards.Add(item);
                }
                else
                {
                    dbClient.Replace(model);
                    var a = Cards.FirstOrDefault(x => x.Id == model.Id);
                    a?.UpdateModel(model);
                    dbClient.Save();
                }
                dialogService.CloseDialog(vm!);
            }
            
            
        }

        private void AddToFavourite(CardItemViewModel? cardItem)
        {
            if (cardItem != null)
            {
                CardModel? el = dbClient.GetById<CardModel>(cardItem.Id);
                if (el != null)
                {
                    cardItem.IsFavourite = !cardItem.IsFavourite;
                    el.IsFavourite = !el.IsFavourite;
                    dbClient.Save();
                }

            }
        }
        private void LoadViewModelsList()
        {
            foreach (var a in dbClient.GetListOfType<CardModel>())
            {

                var item = itemFactory.CreateCardItem(a);
                Cards.Add(item);
            }

        }


    }
}
