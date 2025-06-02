
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Models;
using ViewModels.BaseClasses;
using Interfaces;
using System.Collections.ObjectModel;
using Interfaces.PasswordGenerator;


namespace ViewModels.WebSiteViewModels
{
    public partial class WebSiteViewModel : ViewModelBase
    {

        
        public WebSiteViewModel(IHttpDataConnector<WebSiteModel> dataConnector,
                                IDialogService dialogService,
                                IClipboardService clipboardService,
                                IPasswordGenerator passwordGenerator)
        {
            _dataConnector = dataConnector;
            _dialogService = dialogService;
            _clipboardService = clipboardService;
            _passwordGenerator = passwordGenerator;

            AddNewCommand = new RelayCommand(ShowAddNewDialog);
            AddToFavouriteCommand = new AsyncRelayCommand<WebSiteItemViewModel>(AddToFavouriteAsync);
            DeleteCommand = new AsyncRelayCommand<WebSiteItemViewModel>(DeleteAsync);
            ChangeCommand = new RelayCommand<WebSiteItemViewModel>(ShowChangeDialog);
            WebSites =  new ObservableCollection<WebSiteItemViewModel>();
              
        }
        private string searchKey = "";
        IHttpDataConnector<WebSiteModel> _dataConnector;
        IDialogService _dialogService;
        IClipboardService _clipboardService;
        IPasswordGenerator _passwordGenerator;
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
        public bool IsEmptyCollection { get => !WebSites.Any(); }
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
                OnPropertyChanged(nameof(CurrentItem));
            }
        }

        private void ShowChangeDialog(WebSiteItemViewModel? webSiteItem)
        {
            if (webSiteItem != null && webSiteItem.Model != null)
                ShowDialog(new WebSiteDialogViewModel(webSiteItem.Model, _dialogService, _passwordGenerator));
        }
        
        private void ShowAddNewDialog() 
        { 
            var Dialog = new WebSiteDialogViewModel(_dialogService, _passwordGenerator);
            ShowDialog(Dialog);
        }
        
        private async Task DeleteAsync(WebSiteItemViewModel? webSiteItem)
        {
            try
            {
                if (webSiteItem != null)
                {
                    await _dataConnector.Delete(webSiteItem.Id);
                    WebSites.Remove(webSiteItem);
                    if (WebSites.Count > 0) CurrentItem = WebSites[0];
                    OnPropertyChanged(nameof(FilteredCollection));
                    OnPropertyChanged(nameof(IsEmptyCollection));
                }
            }
            catch (UnauthorizedAccessException)
            {

            }

        }

        private void ShowDialog(WebSiteDialogViewModel? Dialog)
        {
            if (Dialog != null)
            {
                Dialog.dialogResultRequest += GetDialogResult;
                _dialogService.OpenDialog(Dialog);
            }
        }


        private async void GetDialogResult(object? sender, DialogResultEventArgs e)
        {
            try
            {
                if (sender is WebSiteDialogViewModel vm)
                {
                    if (e.DialogResult && vm.Model != null)
                    {
                        WebSiteModel model = vm.Model;

                        if (vm.IsNew)
                        {
                            int id = await _dataConnector.Post(model);
                            if (id != -1)
                            {
                                model.Id = id;
                                WebSiteItemViewModel item = new WebSiteItemViewModel(model, _clipboardService);
                                WebSites.Add(item);
                                CurrentItem = null;
                                CurrentItem = item;
                            }
                        }
                        else
                        {
                            await _dataConnector.Put(model, model.Id);
                            var a = WebSites.FirstOrDefault(x => x.Id == model.Id);
                            a?.UpdateModel(model);
                        }
                        OnPropertyChanged(nameof(FilteredCollection));
                        OnPropertyChanged(nameof(IsEmptyCollection));
                    }
                    _dialogService.CloseDialog(vm!);
                }
            }
            catch (UnauthorizedAccessException)
            {

            }


        }

        
        private async Task AddToFavouriteAsync(WebSiteItemViewModel? webSiteItem)
        {
            try
            {
                if (webSiteItem != null)
                {
                    var el = webSiteItem.Model;
                    webSiteItem.IsFavourite = !webSiteItem.IsFavourite;
                    el.IsFavourite = !el.IsFavourite;
                    await _dataConnector.Put(el, el.Id);
                }
            }
            catch (UnauthorizedAccessException)
            {

            }
        }
        public async Task LoadViewModelsListAsync()
        {
            try
            {
                var models = await _dataConnector.GetList();
                var viewmodels = new ObservableCollection<WebSiteItemViewModel>();
                foreach (var model in models)
                {
                    var item = new WebSiteItemViewModel(model, _clipboardService);
                    viewmodels.Add(item);
                }
                if (viewmodels.Count > 0) CurrentItem = viewmodels[0];
                WebSites = viewmodels;
            }
            catch (UnauthorizedAccessException)
            {

            }
        }
    }    
    
}
