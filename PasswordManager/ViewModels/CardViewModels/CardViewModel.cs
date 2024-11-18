using CommunityToolkit.Mvvm.Input;
using PasswordManager.DataConnectors;
using PasswordManager.Factories;
using PasswordManager.Helpers;
using PasswordManager.Models;
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

            Cards = new ObservableCollection<CardItemViewModel>();
            LoadViewModelsList();
        }
        private IDatabaseClient dbClient;
        private IDialogService dialogService;
        private IItemViewModelFactory itemFactory;
        public CardItemViewModel? currentItem;
        public CardDialogViewModel? Dialog { get; private set; }
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
        


        private void ShowDataOfItem(CardItemViewModel vm)
        {
            CurrentItem = vm;
        }

        private void Delete(Card model)
        {
            var a = Cards.FirstOrDefault(x => x.Id == model.Id);
            if (a != null)
            {
                dbClient.Delete(model);
                Cards.Remove(a);
            }
            dbClient.Save();
        }

        private void ShowDialog()
        {
            if (Dialog != null)
            {
                Dialog.dialogResultRequest += GetDialogResult;
                dialogService.OpenDialog(Dialog!);
            }
        }
        private void ShowChangeDialog(Card model)
        {
            Dialog = new CardDialogViewModel(model);
            ShowDialog();
        }

        private void ShowAddNewDialog()
        {
            Dialog = new CardDialogViewModel();
            ShowDialog();
        }

        private void GetDialogResult(object? sender, DialogResultEventArgs e)
        {
            if (e.DialogResult && sender != null)
            {
                CardDialogViewModel vm = (CardDialogViewModel)sender;
                Card model = vm.Model!;

                if (vm.IsNew)
                {
                    dbClient.Insert(model);
                    dbClient.Save();
                    CardItemViewModel item = itemFactory.CreateCardItem(model, new RelayCommand(() => Delete(model)), new RelayCommand(() => ShowChangeDialog(model)), ShowDataOfItem);
                    Cards.Add(item);
                }
                else
                {
                    dbClient.Replace(model);
                    var a = Cards.FirstOrDefault(x => x.Id == model.Id);
                    a?.UpdateModel(model);
                    dbClient.Save();
                }

            }
            dialogService.CloseDialog(Dialog!);
            Dialog = null;
        }

        private void LoadViewModelsList()
        {
            foreach (var a in dbClient.GetListOfType<Card>())
            {

                var item = itemFactory.CreateCardItem(a, new RelayCommand(() => Delete(a)), new RelayCommand(() => ShowChangeDialog(a)), ShowDataOfItem);
                Cards.Add(item);
            }

        }


    }
}
