using Microsoft.Extensions.Configuration;
using PasswordManager.Configuration;
using PasswordManager.Configuration.OptionExtensions;
using PasswordManager.Factories;
using PasswordManager.ViewModels.BaseClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels
{
    internal class StartUpViewModel : ViewModelBase
    {
        private IViewModelFactory _factory;
        private ViewModelBase? currentPage;

        public StartUpViewModel(IViewModelFactory factory, IWritableOptions<LoggingOptions> options)
        {
            _factory = factory;
            MenuViewModel = new MenuViewModel(options);
            MenuViewModel.PropertyChanged += StartApp;
            CurrentPage = MenuViewModel;
        }

        public MainViewModel? MainViewModel { get; set; }
        public MenuViewModel? MenuViewModel { get; set; }


        public ViewModelBase? CurrentPage
        {

            get { return currentPage; }
            set
            {
                currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        private async void StartApp(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PasswordChecked")
            {
                DecryptDatabase();
                var mainVm = await MainViewModel.CreateAsync(_factory);
                CurrentPage = mainVm;
            }
            else if (e.PropertyName == "PasswordCreated")
            {
                CreateDatabase();
                var mainVm = await MainViewModel.CreateAsync(_factory);
                CurrentPage = mainVm;
            }
        }

        private void DecryptDatabase()
        {

        }
        private void CreateDatabase()
        {

        }

    }
}
