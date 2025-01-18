
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using ViewModels;
using Models;
using ViewModels.BaseClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ViewModels.Services;
using Models.DataConnectors;

namespace ViewModels.AppViewModels
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

