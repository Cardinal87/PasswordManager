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
        public AppItemViewModel(Models.App app, Action<Models.App> delete, Action<Models.App> change, IClipBoardService clipboard)
        {
            model = app;
            Name = app.Name;
            Password = app.Password;

            this.delete = delete;
            this.change = change;

            clipBoard = clipboard;
        }
        Action<Models.App> delete;
        Action<Models.App> change;

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
        [RelayCommand]
        public void GoToWebSite()
        {

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

