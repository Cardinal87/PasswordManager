using Avalonia.Input.Platform;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Helpers;
using PasswordManager.Models;
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
        public AppItemViewModel(Models.App app, RelayCommand delete, RelayCommand change, Action<ItemViewModelBase> ShowDataOfItem, IClipboardService clipboard) : base(app.Id, app.Name, delete, change, ShowDataOfItem)
        {
            model = app;
            UpdateModel(app);
            clipBoard = clipboard;
            ShowDataCommand = new RelayCommand(() => ShowDataOfItem(this));
            CopyToClipboardCommand = new RelayCommand<string>(CopyToClipBoard);
            AddToFavouriteCommand = new RelayCommand(AddToFavourite);
        }
        public RelayCommand ShowDataCommand { get; }
        public RelayCommand<string> CopyToClipboardCommand { get; }
        public RelayCommand AddToFavouriteCommand { get; }
        IClipboardService clipBoard;


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
        

        private void CopyToClipBoard(string? text)
        {
            if (text != null)
                clipBoard.SaveToClipBoard(text);
        }
        private void AddToFavourite()
        {
            if (IsFavourite) IsFavourite = false;
            else IsFavourite = true;
        }
        
        [MemberNotNull(nameof(password))]
        public void UpdateModel(Models.App model)
        {
            this.model = model;
            Id = model.Id;
            Name = model.Name;
            Password = model.Password;
            IsFavourite = model.IsFavourite;
        }
    }
}

