using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Services;
using Microsoft.Extensions.DependencyInjection;
using Interfaces;
using Models.DataConnectors;
using ViewModels.BaseClasses;
using Models.AppConfiguration;


namespace ViewModels
{
    public class StartUpViewModel : ViewModelBase
    {
        private ViewModelBase? currentPage;
        public StartUpViewModel(MainViewModel mainViewModel,
                                MenuViewModel menuViewModel)
        {
            MainViewModel = mainViewModel;
            MenuViewModel = menuViewModel;
            MenuViewModel.SetStartAction(StartApp);
            CurrentPage = MenuViewModel;
        }

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
    }
}
