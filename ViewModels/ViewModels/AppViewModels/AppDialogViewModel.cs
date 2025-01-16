using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.ViewModels.Services;
using PasswordManager.Models.DataConnectors;
using PasswordManager.Models.Models;
using PasswordManager.ViewModels.BaseClasses;
using PasswordManager.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace PasswordManager.ViewModels.AppViewModels
{
    internal class AppDialogViewModel : DialogViewModelBase, IDialogResultHelper
    {
        private const string namePattern = @"[a-zA-Z0-9._%+-]+|^$";
        private const string passwordPattern = @"^[a-zA-Z0-9~!@#$%^*()_+={}[]|:,.?/-]{1,30}$";
        public AppDialogViewModel(IServiceProvider provider) 
        {
            this.provider = provider;
            dialogService = provider.GetRequiredService<IDialogService>();
            ShowPasswordGeneratorCommand = new RelayCommand(ShowPasswordGenerator);
            AddCommand = new RelayCommand(Add);
            CloseCommand = new RelayCommand(Close);
            id = 0;
            IsNew = true;
            
        }
        public AppDialogViewModel(AppModel model, IServiceProvider provider)
        {
            this.provider = provider;
            dialogService = provider.GetRequiredService<IDialogService>();
            Model = model;
            Name = model.Name!;
            Password = model.Password;
            id = model.Id;
            ShowPasswordGeneratorCommand = new RelayCommand(ShowPasswordGenerator);
            AddCommand = new RelayCommand(Add);
            CloseCommand = new RelayCommand(Close);
            IsFavourite = model.IsFavourite;
            IsNew = false;
        }
        
        private bool dialogResult;
        private string name = "";
        private string password = "";
        IServiceProvider provider;
        IDialogService dialogService;
        public event EventHandler<DialogResultEventArgs>? dialogResultRequest;
        public RelayCommand ShowPasswordGeneratorCommand { get; set; }
        public RelayCommand AddCommand { get; private set; }
        public RelayCommand CloseCommand { get; private set; }
        public AppModel? Model { get; set; }
        private int id;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(CanClose));
                OnPropertyChanged(nameof(IsValidName));
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(CanClose));
                OnPropertyChanged(nameof(IsValidPassword));
            }
        }

        public bool IsFavourite { get; private set; }
        public bool IsNew { get; private set; }
        
        public override bool CanClose
        {
            get
            {
                return IsValidName && IsValidPassword;
            }
        } 
        
        private void ShowPasswordGenerator()
        {
            var vm = new PasswordGeneratorViewModel(provider);
            vm.dialogResultRequest += GetDialogResult;
            dialogService.OpenDialog(vm);
        }
       
        private void Add()
        {
            if (CanClose)
            {
                if (Name == "") Name = "NewApp";
                dialogResult = true;
                Model = new AppModel(Name, Password, IsFavourite);
                Model.Id = id;

                dialogResultRequest?.Invoke(this, new DialogResultEventArgs(dialogResult));
            }
            else
            {
                OnPropertyChanged(nameof(CanClose));
                OnPropertyChanged(nameof(IsValidName));
                OnPropertyChanged(nameof(IsValidPassword));
                

            }
        }

        private void GetDialogResult(object? sender, DialogResultEventArgs e)
        {
            if (sender is PasswordGeneratorViewModel vm && e.DialogResult)
            {
                Password = vm.Password;
                dialogService.CloseDialog(vm);
            }
        }

        public bool IsValidName
        {
            get
            {
                if (!isChecked) return true;
                return Regex.IsMatch(Name, namePattern) && Name.Length >= 0
                    && Name.Length <= 100;
            }
        }

        public bool IsValidPassword
        {
            get
            {
                if (!isChecked) return true;
                return Regex.IsMatch(Password, passwordPattern);
            }
        }
        private bool isChecked = false;
        protected override void Close()
        {
            dialogResult = false;
            dialogResultRequest?.Invoke(this, new DialogResultEventArgs(dialogResult));
        }
        
    }
}