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
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PasswordManager.ViewModels.WebSiteViewModels
{
    internal partial class WebSiteItemViewModel : ViewModelBase 
    {
        private WebSite model;
        private IClipBoardService clipBoardService;
        
        public WebSiteItemViewModel(WebSite model, IClipBoardService clipBoardService, Action<WebSite> delete, Action<WebSite> change)
        {
            this.model = model;
            
            Name = model.Name;
            Login = model.Login;
            Password = model.Password;
            IsFavourite = model.IsFavourite;
            WebAddress = model.WebAddress;
            this.clipBoardService = clipBoardService; 
            this.change = change;
            this.delete = delete;
        }
        Action<WebSite> delete;
        Action<WebSite> change;

        private string name;
        private string login;
        private string password;
        private string webAddress;
        private bool isFavourite;

        public string Name
        {
            get
            {
                return name; 
            }
            [MemberNotNull(nameof(name))]
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
            [MemberNotNull(nameof(login))]
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
            [MemberNotNull(nameof(password))]
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        
        public bool IsFavourite 
        {
            get
            {
                return isFavourite;
            }
            [MemberNotNull(nameof(isFavourite))]
            set
            {
                isFavourite = value;
                OnPropertyChanged(nameof(IsFavourite));
            }
        }
        
        public string WebAddress 
        {
            get
            {
                return webAddress;
            }
            [MemberNotNull(nameof(webAddress))]
            set
            {
                webAddress = value;
                OnPropertyChanged(nameof(WebAddress));
            }
        }


       

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
            if (IsFavourite) IsFavourite = false;
            else IsFavourite = true;
        }
        [RelayCommand]
        public void Change()
        {
            change.Invoke(model);
        }
        [RelayCommand]
        public void Delete()
        {
            delete.Invoke(model);
        }


    }
}
