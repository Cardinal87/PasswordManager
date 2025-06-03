using Interfaces;
using ViewModels.BaseClasses;

namespace ViewModels
{
    public class StartUpViewModel : ViewModelBase
    {
        private ViewModelBase? currentPage;
        public StartUpViewModel(MainViewModel mainViewModel,
                                MenuViewModel menuViewModel, 
                                IDialogService dialogService,
                                ITokenHandlerService tokenService)
        {
            tokenService.TokenExpired += HandleExpirate;
            MainViewModel = mainViewModel;
            MenuViewModel = menuViewModel;
            MenuViewModel.SetStartAction(StartApp);
            CurrentPage = MenuViewModel;
            _dialogService = dialogService;

        }
        private IDialogService _dialogService;

        public MainViewModel MainViewModel { get; set; }
        public MenuViewModel MenuViewModel { get; set; }


        public ViewModelBase? CurrentPage
        {

            get { return currentPage; }
            set
            {
                currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        private async Task StartApp(string password)
        {
            await MainViewModel.InizializeViewModelsAsync();
            CurrentPage = MainViewModel;
        }

        public void HandleExpirate()
        {
            _dialogService.CloseAllDialogs();
            MainViewModel.ResetData();
            CurrentPage = MenuViewModel;
        }
    }
}
