using Avalonia.Input.Platform;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.AppViewModel
{
    internal partial class AppItemViewModel : ViewModelBase
    {
        public AppItemViewModel(Models.App app, RelayCommand<Models.App> Delete, RelayCommand<Models.App> Change, IClipBoardService clipboard)
        {
            model = app;
            Name = app.Name;
            Password = app.Password;

            DeleteCommand = Delete;
            ChangeCommand = Change;

            clipBoard = clipboard;
        }
        RelayCommand<Models.App> DeleteCommand;
        RelayCommand<Models.App> ChangeCommand;

        IClipBoardService clipBoard;
        Models.App model;
        string name;
        string password;
        bool isFavourite;
        
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
        [RelayCommand]
        public void CopyToClipBoard(string text)
        {
            clipBoard.SaveToClipBoard(text);
        }

        [RelayCommand]
        public void AddToFavourite()
        {
            if (IsFavourite) IsFavourite = false;
            else IsFavourite = true;
        }
    }
}

