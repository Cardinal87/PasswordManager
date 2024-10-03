using CommunityToolkit.Mvvm.Input;
using PasswordManager.Models;
using PasswordManager.ViewModels.DialogInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.WebSiteViewModels
{
    internal partial class WebSiteDialogViewModel : ViewModelBase, IDialogViewModel, IDialogResultHelper
    {

        private bool dialogResult;
        public WebSiteDialogViewModel()
        {
            AddCommand = new RelayCommand(Add);
            CloseCommand = new RelayCommand(Close);
            IsFavourite = false;
        }

        public WebSiteDialogViewModel(WebSite item)
        {
            AddCommand = new RelayCommand(Add);
            CloseCommand = new RelayCommand(Close);
            Id = item.Id;
            Name = item.Name;
            WebAddress = item.WebAddress;
            Login = item.Login;
            Password = item.Password;
            IsFavourite = item.IsFavourite;
        }
       
        private string password = "";
        public string login = "";
        public string webAdress = ""; 
        public string name = "";
        public int? Id { get; private set; }
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
            }
        }
        public bool IsFavourite { get; private set; }

        RelayCommand AddCommand;
        RelayCommand CloseCommand;

        public event EventHandler<DialogResultEventArgs>? dialogResultRequest;

        public void Add()
        {
            dialogResult = true;
            dialogResultRequest?.Invoke(this, new DialogResultEventArgs(dialogResult));
        }

        public bool CanClose()
        {
            return true;
        }

        public void Close()
        {
            dialogResult = false;
            if (CanClose())
            {
                dialogResultRequest?.Invoke(this, new DialogResultEventArgs(dialogResult));
            }
        }
    }
}
