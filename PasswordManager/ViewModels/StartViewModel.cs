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
    internal class StartViewModel : ViewModelBase
    {
        private IViewModelFactory _factory;
        private ViewModelBase? currentPage;

        public StartViewModel(IViewModelFactory factory)
        {
            _factory = factory;
            MenuViewModel = new MenuViewModel();
            MenuViewModel.PropertyChanged += ChangeCurrentItem;
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

        private async void ChangeCurrentItem(object? sender, PropertyChangedEventArgs e)
        {
            var mainVm = await MainViewModel.CreateAsync(_factory);
            CurrentPage = mainVm;
        }
    }
}
