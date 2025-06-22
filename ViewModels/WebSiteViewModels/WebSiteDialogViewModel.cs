using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using ViewModels;
using Models;

using ViewModels.BaseClasses;


using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Interfaces.PasswordGenerator;
using Interfaces.Dialogs;

namespace ViewModels.WebSiteViewModels
{
    public partial class WebSiteDialogViewModel : DialogViewModelBase
    {
        private const string namePattern = @"[a-zA-Z0-9._%+-]+|^$";
        private const string loginPattern = @"^((\+?\\d{1,4}?[-.\s]?\(?\d{1,3}?\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,9})|([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})|([a-zA-Z0-9._]{4,}))$";
        private const string webAdressPattern = @"^((?!-)[A-Za-z0-9-]{1,40}(?<!-)\.)+[A-Za-z]{2,10}$";
        private const string passwordPattern = @"^[a-zA-Z0-9~!@#$%^*()_+={}[]|:,.?/-]{1,30}$";



        private bool dialogResult;
        public WebSiteDialogViewModel(IDialogService dialogService, IPasswordGenerator passwordGenerator)
        {
            _dialogService = dialogService;
            _passwordGenerator = passwordGenerator;
            GeneratePasswordCommand = new RelayCommand(ShowPasswordGenerator);
            AddCommand = new RelayCommand(Add);
            CloseCommand = new RelayCommand(Close);
            IsFavourite = false;
            IsNew = true;
            Id = 0;
        }

        public WebSiteDialogViewModel(WebSiteModel item,
            IDialogService dialogService,
            IPasswordGenerator passwordGenerator)
        {

            _dialogService = dialogService;
            _passwordGenerator = passwordGenerator;
            GeneratePasswordCommand = new RelayCommand(ShowPasswordGenerator);
            AddCommand = new RelayCommand(Add);
            CloseCommand = new RelayCommand(Close);
            Model = item;
            Id = item.Id;
            Name = item.Name!;
            WebAddress = item.WebAddress;
            Login = item.Login;
            Password = item.Password;
            IsFavourite = item.IsFavourite;
            IsNew = false;

        }
        IDialogService _dialogService;
        IPasswordGenerator _passwordGenerator;
        public WebSiteModel? Model { get; private set; }
        
        private string password = "";
        public string login = "";
        public string webAdress = ""; 
        public string name = "";
        public int Id { get; private set; }
        public string Name 
        {
            get
            {
                return name!;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(CanClose));
                OnPropertyChanged(nameof(IsValidName));
            }
        }
        public string WebAddress 
        {
            get
            {
                return webAdress!;
            }
            set
            {
                webAdress = value;
                OnPropertyChanged(nameof(WebAddress));
                OnPropertyChanged(nameof(CanClose));
                OnPropertyChanged(nameof(IsValidWebAdress));
            }
        }
        
        public string Login
        {
            get
            {
                return login!;
            }
            set
            {
                login = value;
                OnPropertyChanged(nameof(Login));
                OnPropertyChanged(nameof(CanClose));
                OnPropertyChanged(nameof(IsValidLogin));
            }
        }

        public string Password {
            get
            {
                return password!;
            }
            set 
            {
                password = value;
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(CanClose));
                OnPropertyChanged(nameof(IsValidPassword));
            }
        }
        public bool IsFavourite { get; private set; }
        public bool IsNew { get; set; }
        public RelayCommand GeneratePasswordCommand { get; private set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }


        private void ShowPasswordGenerator()
        {
            var vm = new PasswordGeneratorViewModel(_passwordGenerator);
            vm.dialogResultRequest += GetDialogResult;
            _dialogService.OpenDialog(vm);
        }

        protected void Add()
        {
            isChecked = true;
            if (CanClose)
            {
                if (Name == "") Name = "NewWebSite";
                dialogResult = true;
                Model = new WebSiteModel(Name, Login, Password, WebAddress, IsFavourite);
                Model.Id = Id;

                RequestClose(new DialogResultEventArgs(dialogResult));
            }
            else
            {
                OnPropertyChanged(nameof(CanClose));
                OnPropertyChanged(nameof(IsValidName));
                OnPropertyChanged(nameof(IsValidLogin));
                OnPropertyChanged(nameof(IsValidPassword));
                OnPropertyChanged(nameof(IsValidWebAdress));
                
            }
        }
        private void GetDialogResult(object? sender, DialogResultEventArgs e)
        {
            if (sender is PasswordGeneratorViewModel vm && e.DialogResult)
            {
                Password = vm.Password;
                _dialogService.CloseDialog(vm);
            }
        }

        public override bool CanClose
        {
            get
            {
                if (!isChecked) return true;
                return IsValidPassword &&
                       IsValidLogin &&
                       IsValidWebAdress &&
                       IsValidName;
            }
        }

        public bool IsValidName
        {
            get
            {
                if (!isChecked) return true;
                return Regex.IsMatch(Name, namePattern) && Name.Length >= 0
                    && Name.Length <= 100;
            }
        }
        public bool IsValidWebAdress
        {
            get
            {
                if (!isChecked) return true;
                return Regex.IsMatch(WebAddress, webAdressPattern);
            }
        }
        public bool IsValidLogin
        {
            get
            {
                if (!isChecked) return true;
                return Regex.IsMatch(Login, loginPattern) && Login.Length > 0
                    && Login.Length <= 50;
            }
        }

        public bool IsValidPassword
        {
            get
            {
                if (!isChecked) return true;
                return Regex.IsMatch(Password, passwordPattern);
            }
        }
        private bool isChecked = false;
        public override void Close()
        {
            dialogResult = false;
            RequestClose(new DialogResultEventArgs(dialogResult));
        }

        
    }
}
