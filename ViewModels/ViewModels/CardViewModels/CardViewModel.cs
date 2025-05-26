using CommunityToolkit.Mvvm.Input;
using Models;
using ViewModels.BaseClasses;
using Interfaces;
using System.Collections.ObjectModel;

namespace ViewModels.CardViewModels
{
    public class CardViewModel : ViewModelBase
    {
        
        
        public CardViewModel(IHttpDataConnector<CardModel> dataConnector,
                              IDialogService dialogService,
                              IClipboardService clipboardService) 
        {
            _dialogService = dialogService;
            _dataConnector = dataConnector;
            _clipboardService = clipboardService;
            AddToFavouriteCommand = new AsyncRelayCommand<CardItemViewModel>(AddToFavouriteAsync);
            DeleteCommand = new AsyncRelayCommand<CardItemViewModel>(DeleteAsync);
            ChangeCommand = new RelayCommand<CardItemViewModel>(ShowChangeDialog);
            AddNewCommand = new RelayCommand(ShowAddNewDialog);
            Cards = new ObservableCollection<CardItemViewModel>();
            
        }
        private string searchKey = "";
        IHttpDataConnector<CardModel> _dataConnector;
        IDialogService _dialogService;
        IClipboardService _clipboardService;
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
            
            if (cardItem != null)
            {
                await _dataConnector.Delete(cardItem.Id);
                Cards.Remove(cardItem);
                if (Cards.Count > 0) CurrentItem = Cards[0];
                OnPropertyChanged(nameof(FilteredCollection));
                OnPropertyChanged(nameof(IsEmptyCollection));
            }
                
            
        }

        private void ShowDialog(CardDialogViewModel? Dialog)
        {
            if (Dialog != null)
            {
                Dialog.dialogResultRequest += GetDialogResult;
                _dialogService.OpenDialog(Dialog);
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
            if (sender is CardDialogViewModel vm)
            {
                if (e.DialogResult && vm.Model != null)
                {
                    CardModel model = vm.Model;

                    if (vm.IsNew)
                    {
                        int id = await _dataConnector.Post(model);
                        model.Id = id;
                        CardItemViewModel item = new CardItemViewModel(model, _clipboardService);
                        Cards.Add(item);
                        CurrentItem = null;
                        CurrentItem = item;
                    }
                    else
                    {
                        await _dataConnector.Put(model, model.Id);
                        var a = Cards.FirstOrDefault(x => x.Id == model.Id);
                        a?.UpdateModel(model);
                    }
                    OnPropertyChanged(nameof(FilteredCollection));
                    OnPropertyChanged(nameof(IsEmptyCollection));
                }
                _dialogService.CloseDialog(vm!);
            }
                
        }  
        

        private async Task AddToFavouriteAsync(CardItemViewModel? cardItem)
        {
            
            if (cardItem != null)
            {
                var el = cardItem.Model;   
                cardItem.IsFavourite = !cardItem.IsFavourite;
                el.IsFavourite = !el.IsFavourite;
                await _dataConnector.Put(el, el.Id);
                

            }
            
        }
        public async Task LoadViewModelsListAsync()
        {
            var models = await _dataConnector.GetList();
            var viewmodels = new ObservableCollection<CardItemViewModel>();
            foreach (var model in models)
            {
                var item = new CardItemViewModel(model, _clipboardService);
                viewmodels.Add(item);
            }
            if (viewmodels.Count > 0) CurrentItem = viewmodels[0];
            Cards = viewmodels;
            

        }


    }
}
