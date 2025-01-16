
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.ViewModels;
using PasswordManager.Models;
using PasswordManager.ViewModels.BaseClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using PasswordManager.ViewModels.Services;
using PasswordManager.Models.DataConnectors;
using PasswordManager.Models.Models;

namespace PasswordManager.ViewModels.AppViewModels
{
    public partial class AppItemViewModel : ItemViewModelBase
    {
        public AppItemViewModel(AppModel app, IServiceProvider provider)
        {
            
            UpdateModel(app);
            clipBoard = provider.GetRequiredService<IClipboardService>();
            CopyToClipboardCommand = new RelayCommand<string>(CopyToClipBoard);
            
        }
        
        public RelayCommand<string> CopyToClipboardCommand { get; }
        IClipboardService clipBoard;


        public AppModel Model { get; private set; }
        string password;
        
        
        
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
        

        private void CopyToClipBoard(string? text)
        {
            if (text != null)
                clipBoard.SaveToClipBoard(text);
        }
        
        
        [MemberNotNull(nameof(password))]
        [MemberNotNull(nameof(Model))]
        public void UpdateModel(AppModel model)
        {
            Model = model;
            Id = model.Id;
            Name = model.Name;
            Password = model.Password;
            IsFavourite = model.IsFavourite;
        }
    }
}

