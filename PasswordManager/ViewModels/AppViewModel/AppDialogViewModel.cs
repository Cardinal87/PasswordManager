﻿using CommunityToolkit.Mvvm.Input;
using PasswordManager.Helpers;
using PasswordManager.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.AppViewModel
{
    internal class AppDialogViewModel : DialogViewModelBase, IDialogResultHelper
    {
        public AppDialogViewModel() 
        {
            AddCommand = new RelayCommand(Add);
            CloseCommand = new RelayCommand(Close);
            IsNew = true;
        }
        public AppDialogViewModel(Models.App model)
        {
            Name = model.Name;
            Password = model.Password;
            AddCommand = new RelayCommand(Add);
            CloseCommand = new RelayCommand(Close);
            IsFavourite = model.IsFavourite;
            IsNew = false;
        }

        public bool IsReadOnly;
        public bool dialogResult;
        private string name = "";
        private string password = "";
        public event EventHandler<DialogResultEventArgs>? dialogResultRequest;

        RelayCommand AddCommand;
        RelayCommand CloseCommand;
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
                OnPropertyChanged(nameof(password));
            }
        }

        public bool IsFavourite { get; private set; }
        public bool IsNew { get; private set; }
        protected override bool CanClose()
        {
            return true;
        }

        public void Add()
        {
            dialogResult = true;
            dialogResultRequest?.Invoke(this, new DialogResultEventArgs(dialogResult));
        }

        public override void Close()
        {
            dialogResult = false;
            if (CanClose())
            {
                dialogResultRequest?.Invoke(this, new DialogResultEventArgs(dialogResult));
            }
        }
    }
}