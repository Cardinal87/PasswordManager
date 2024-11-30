using Avalonia.Input.Platform;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Helpers;
using PasswordManager.Models;
using PasswordManager.ViewModels.BaseClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.AppViewModels
{
    internal partial class AppItemViewModel : ItemViewModelBase
    {
        public AppItemViewModel(AppModel app,IClipboardService clipboard)
        {
            
            UpdateModel(app);
            clipBoard = clipboard;
            CopyToClipboardCommand = new RelayCommand<string>(CopyToClipBoard);
            
        }
        
        public RelayCommand<string> CopyToClipboardCommand { get; }
        IClipboardService clipBoard;


        public AppModel Model { get; private set; }
        string password;
        bool isFavourite;
        
        
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
            set
            {
                isFavourite = value;
                OnPropertyChanged(nameof(IsFavourite));
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

