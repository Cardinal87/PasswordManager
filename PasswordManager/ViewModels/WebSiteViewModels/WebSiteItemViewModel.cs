using Avalonia;
using Avalonia.Controls;
using Avalonia.Input.Platform;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PasswordManager.Helpers;
using PasswordManager.Models;

namespace PasswordManager.ViewModels.WebSiteViewModels
{
    internal partial class WebSiteItemViewModel : ViewModelBase 
    {
        private WebSite model;
        private IClipBoardService clipBoardService;
        public WebSiteItemViewModel(WebSite model, IClipBoardService clipBoardService, RelayCommand<WebSite> deleteCommand, RelayCommand<WebSite> changeCommand)
        {
            this.model = model;
            Id = model.Id;
            Name = model.Name;
            Login = model.Login;
            Password = model.Password;
            WebAddress = model.WebAddress;
            this.clipBoardService = clipBoardService; 
            DeleteCommand = deleteCommand;
            ChangeCommand = changeCommand;
        }
        private string name;
        private string login;
        private string password;
        private string webAddress;
        private bool favourite;
        
        public int Id { get; private set; }
        public string Name
        {
            get
            {
                return name; 
            }
            set 
            { 
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Login {
            get
            {
                return login;
            }
            set
            {
                login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        public string Password 
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public bool Favourite 
        {
            get
            {
                return favourite;
            }
            set
            {
                favourite = value;
                OnPropertyChanged(nameof(Favourite));
            }
        }
        public string WebAddress 
        {
            get
            {
                return webAddress;
            }
            set
            {
                webAddress = value;
                OnPropertyChanged(nameof(WebAddress));
            }
        }

        
        public RelayCommand<WebSite> DeleteCommand;
        public RelayCommand<WebSite> ChangeCommand;

        [RelayCommand]
        public void CopyToClipboard(string text)
        {
            clipBoardService.SaveToClipBoard(text);
        }
        

        [RelayCommand]
        public void GoToWebSite()
        {

        }

        [RelayCommand]
        public void AddToFavourite()
        {
            if (Favourite) Favourite = false;
            else Favourite = true;
        }
            
            
        
    }
}
