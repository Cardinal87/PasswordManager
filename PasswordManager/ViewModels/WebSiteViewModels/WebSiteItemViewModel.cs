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
using PasswordManager.ViewModels.BaseClasses;
using System.Diagnostics;

namespace PasswordManager.ViewModels.WebSiteViewModels
{
    internal partial class WebSiteItemViewModel : ItemViewModelBase
    {
        
        public WebSiteModel Model { get; private set; }
        private IClipboardService clipBoardService;
        
        public WebSiteItemViewModel(WebSiteModel model, IClipboardService clipBoardService) 
        {
            
            UpdateModel(model);
            this.clipBoardService = clipBoardService;

            OpenWebSiteCommand = new RelayCommand(OpenWebSite);
            CopyToClipboardCommand = new RelayCommand<string>(CopyToClipboard);
            
            
        }
        public RelayCommand OpenWebSiteCommand { get; set; }
        public RelayCommand<string> CopyToClipboardCommand { get; }
        

        
        private string login;
        private string password;
        private string webAddress;
        
        
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


        private void CopyToClipboard(string? text)
        {
            if (text != null)
                clipBoardService.SaveToClipBoard(text);
        }

        private void OpenWebSite()
        {
            if (!String.IsNullOrEmpty(WebAddress))
            {
                string url = "http://" + WebAddress + "/";
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
        }


        [MemberNotNull(nameof(webAddress))]
        [MemberNotNull(nameof(login))]
        [MemberNotNull(nameof(password))]
        [MemberNotNull(nameof(Model))]
        public void UpdateModel(WebSiteModel model)
        {
            
            Model = model;
            Id = model.Id;
            Name = model.Name;
            Password = model.Password;
            Login = model.Login;
            WebAddress = model.WebAddress;
            IsFavourite = model.IsFavourite;
        }
        
    }
}
