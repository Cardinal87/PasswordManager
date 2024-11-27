﻿using CommunityToolkit.Mvvm.Input;
using PasswordManager.Models;
using PasswordManager.ViewModels.BaseClasses;
using PasswordManager.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.WebSiteViewModels
{
    internal partial class WebSiteDialogViewModel : DialogViewModelBase, IDialogResultHelper
    {

        private bool dialogResult;
        public WebSiteDialogViewModel()
        {
            AddCommand = new RelayCommand(Add);
            CloseCommand = new RelayCommand(Close);
            IsFavourite = false;
            IsNew = true;
            
        }

        public WebSiteDialogViewModel(WebSite item)
        {
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
        
        public WebSite? Model { get; private set; }
        
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
        public bool IsNew { get; set; }

        RelayCommand AddCommand;
        RelayCommand CloseCommand;

        public event EventHandler<DialogResultEventArgs>? dialogResultRequest;

        protected void Add()
        {
            dialogResult = true;
            dialogResultRequest?.Invoke(this, new DialogResultEventArgs(dialogResult));
        }

        public override bool CanClose
        {
            get
            {
                return Password != string.Empty;
            }
        }


        protected override void Close()
        {
            dialogResult = true;
            if (CanClose)
            {
                Model = new WebSite(Name, Login, Password, WebAddress, IsFavourite);
                dialogResultRequest?.Invoke(this, new DialogResultEventArgs(dialogResult));
                
            }
        }

        
    }
}
