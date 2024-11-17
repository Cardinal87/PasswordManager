using CommunityToolkit.Mvvm.Input;
using PasswordManager.DataConnectors;
using PasswordManager.Factories;
using PasswordManager.Helpers;
using PasswordManager.Models;
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

        private void Delete(WebSite model)
        {
            var a = Cards.FirstOrDefault(x => x.Id == model.Id);
            if (a != null)
            {
                dbClient.Delete(model);
                Cards.Remove(a);
            }
            dbClient.Save();
        }

        
        private void LoadViewModelsList()
        {
            foreach (var a in dbClient.GetListOfType<Card>())
            {
                
                //var item = itemFactory.CreateWebSiteItem(a, new RelayCommand(() => Delete(a)), new RelayCommand(() => ShowChangeDialog(a)), ShowDataOfItem);
                //Cards.Add(item);
            }

        }


    }
}
