﻿using Avalonia;
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

namespace PasswordManager.ViewModels.WebSiteViewModels
{
    internal partial class WebSitesItemViewModel : ViewModelBase 
    {
        private WebSite model;
        private IClipBoardService clipBoardService;
        public WebSitesItemViewModel(WebSite model, IClipBoardService clipBoardService, RelayCommand<int> deleteCommand, RelayCommand<int> changeCommand)
        {
            this.model = model;
            Id = model.Id;
            Name = model.Name;
            Login = model.Login;
            Password = model.Password;
            this.clipBoardService = clipBoardService; 
            DeleteCommand = deleteCommand;
            ChangeCommand = changeCommand;
        }
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string? Login { get; private set; }
        public string Password { get; private set; }
        public bool Favourite { get; private set; }

        
        public RelayCommand<int> DeleteCommand;
        public RelayCommand<int> ChangeCommand;

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
