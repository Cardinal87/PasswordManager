using Avalonia.Input.Platform;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Helpers;
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
        public AppItemViewModel(Models.App app, RelayCommand delete, RelayCommand change, Action<ItemViewModelBase> ShowData, IClipBoardService clipboard) : base(app.Name, delete, change, ShowData)
        {
            model = app;
            Password = app.Password;

            clipBoard = clipboard;
            ShowDataCommand = new RelayCommand(() => ShowData(this));
        }

       
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
        public RelayCommand ShowDataCommand;

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
    }
}

