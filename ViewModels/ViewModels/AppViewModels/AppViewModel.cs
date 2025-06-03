using CommunityToolkit.Mvvm.Input;
using Interfaces;
using Models;
using ViewModels.BaseClasses;
using System.Collections.ObjectModel;
using Interfaces.PasswordGenerator;
using Microsoft.Extensions.Logging;


namespace ViewModels.AppViewModels
{
    public partial class AppViewModel : ViewModelBase
    {
        
        public AppViewModel(IHttpDataConnector<AppModel> dataConnector,
                            IDialogService dialogService,
                            IClipboardService clipboardService,
                            IPasswordGenerator passwordGenerator,
                            ILogger<AppViewModel> logger)
        {
            _dataConnector = dataConnector;
            _dialogService = dialogService;
            _clipboardService = clipboardService;
            _passwordGenerator = passwordGenerator;
            _logger = logger;
            AddNewCommand = new RelayCommand(ShowAddNewDialog);
            AddToFavouriteCommand = new AsyncRelayCommand<AppItemViewModel>(AddToFavouriteAsync);
            DeleteCommand = new AsyncRelayCommand<AppItemViewModel>(DeleteAsync);
            ChangeCommand = new RelayCommand<AppItemViewModel>(ShowChangeDialog);
            
        }
        IHttpDataConnector<AppModel> _dataConnector;
        IDialogService _dialogService;
        IClipboardService _clipboardService;
        IPasswordGenerator _passwordGenerator;
        ILogger<AppViewModel> _logger;
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
            try
            {
                if (appItem != null)
                {
                    await _dataConnector.Delete(appItem.Id);
                    Apps.Remove(appItem);
                    CurrentItem = Apps.Count != 0 ? Apps[0] : null;
                    OnPropertyChanged(nameof(FilteredCollection));
                    OnPropertyChanged(nameof(IsEmptyCollection));
                }
            }
            catch(UnauthorizedAccessException)
            {
                _logger.LogInformation("Token expired, app blocked");
            }
}
        
        private void ShowChangeDialog(AppItemViewModel? appItem)
        {
            if (appItem != null && appItem.Model != null)
                ShowDialog(new AppDialogViewModel(appItem.Model, _dialogService, _passwordGenerator));
        }
        private void ShowAddNewDialog()
        {
            ShowDialog(new AppDialogViewModel(_dialogService, _passwordGenerator));
        }

        private void ShowDialog(AppDialogViewModel? dialogVM)
        {
            if (dialogVM != null)
            {
                dialogVM.dialogResultRequest += GetDialogResult;
                _dialogService.OpenDialog(dialogVM);
            }
        }
        private async void GetDialogResult(object? sender, DialogResultEventArgs e)
        {
            try
            {
                if (sender is AppDialogViewModel vm)
                {

                    if (e.DialogResult && vm.Model != null)
                    {
                        if (vm.IsNew)
                        {
                            int id = await _dataConnector.Post(vm.Model);
                            if (id != -1)
                            {
                                vm.Model.Id = id;
                                AppItemViewModel item = new AppItemViewModel(vm.Model, _clipboardService);
                                Apps.Add(item);
                                CurrentItem = null;
                                CurrentItem = item;
                            }
                        }
                        else
                        {
                            await _dataConnector.Put(vm.Model, vm.Model.Id);
                            var a = Apps.FirstOrDefault(x => x.Id == vm.Model.Id);
                            a?.UpdateModel(vm.Model);
                        }
                        OnPropertyChanged(nameof(FilteredCollection));
                        OnPropertyChanged(nameof(IsEmptyCollection));
                    }
                    _dialogService.CloseDialog(vm);
                }
            }
            catch(UnauthorizedAccessException)
            {
                _logger.LogInformation("Token expired, app blocked");
            }
            
            
        }
        private async Task AddToFavouriteAsync(AppItemViewModel? appItem)
        {
            try
            {
                if (appItem != null)
                {
                    var model = appItem.Model;
                    appItem.IsFavourite = !appItem.IsFavourite;
                    model.IsFavourite = !model.IsFavourite;
                    await _dataConnector.Put(model, appItem.Id);
                }
            }
            catch (UnauthorizedAccessException)
            {
                _logger.LogInformation("Token expired, app blocked");
            }
        }
        public async Task LoadViewModelListAsync()
        {
            try { 
                var models = await _dataConnector.GetList();
                var viewmodels = new ObservableCollection<AppItemViewModel>();
                foreach (var model in models)
                {
                    var item = new AppItemViewModel(model, _clipboardService);
                    viewmodels.Add(item);
                }
                if (viewmodels.Count > 0) CurrentItem = viewmodels[0];
                Apps = viewmodels;
            }
            catch (UnauthorizedAccessException)
            {
                _logger.LogInformation("JWT token expired, app blocked");
            }

        }
        
    }
}
