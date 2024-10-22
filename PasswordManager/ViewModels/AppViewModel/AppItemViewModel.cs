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
    internal partial class AppItemViewModel : ItemViewModelBase
    {
        public AppItemViewModel(Models.App app, Action<Models.App> delete, Action<Models.App> change, IClipBoardService clipboard) : base(app.Name, new RelayCommand(() => delete.Invoke(app)))
        {
            model = app;
            Password = app.Password;

            DeleteCommand = new RelayCommand(() => delete.Invoke(app));
            this.change = change;
            
            clipBoard = clipboard;
        }
       
        Action<Models.App> change;
        new RelayCommand DeleteCommand;

        IClipBoardService clipBoard;
        Models.App model;
        
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
        
    }
}

