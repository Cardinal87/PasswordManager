using CommunityToolkit.Mvvm.Input;
using PasswordManager.Models;
using PasswordManager.ViewModels.BaseClasses;
using PasswordManager.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.CardViewModels
{
    internal class CardDialogViewModel : DialogViewModelBase, IDialogResultHelper
    {
        private const string namePattern = @"[a-zA-Z0-9._%+-]+|^$";
        private const string cvcPattern = @"^\d{3}$";
        private const string numberPattern = @"^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$|^(\d{16})$";
        private const string ownerPattern = @"[a-zA-Z0-9._%+-]+|^$";




        public CardDialogViewModel() 
        { 
            AddCommand = new RelayCommand(Add);
            CloseCommand = new RelayCommand(Close);
            isFavourite = false;
            IsNew = true;
        }
        public CardDialogViewModel(CardModel model)
        {
            AddCommand = new RelayCommand(Add);
            CloseCommand = new RelayCommand(Close);
            isFavourite = model.IsFavourite;
            IsNew = false;
            Name = model.Name!;
            Owner = model.Owner;
            Number = model.Number;
            Cvc = model.Cvc;
            Month = model.Month;
            Year = model.Year;
            Model = model;
            
        }

        public bool dialogResult;
        
        public event EventHandler<DialogResultEventArgs>? dialogResultRequest;
        public bool IsNew { get; private set; }
        public RelayCommand AddCommand { get; private set; }
        public RelayCommand CloseCommand { get; private set; }

        public CardModel? Model { get; private set; }
        private string name = "";
        private string owner = "";
        private string number = "";
        private string cvc = "";
        private string month = "";
        private string year = "";
        private bool isFavourite;

        public int Id { get; private set; }
        public string Name {
            get => name;
            [MemberNotNull(nameof(name))]
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(CanClose));
                OnPropertyChanged(nameof(IsValidName));
            }
        }
        public string Owner
        {
            get => owner;
            [MemberNotNull(nameof(owner))]
            set
            {
                owner = value;
                OnPropertyChanged(nameof(Owner));
                OnPropertyChanged(nameof(CanClose));
                OnPropertyChanged(nameof(IsValidOwner));
            }
        }
        public string Number
        {
            get => number;
            [MemberNotNull(nameof(number))]
            set
            {
                number = value;
                OnPropertyChanged(nameof(Number));
                OnPropertyChanged(nameof(CanClose));
                OnPropertyChanged(nameof(IsValidNumber));
            }
        }

        public string Cvc
        {
            get => cvc;
            set
            {
                cvc = value;
                OnPropertyChanged(nameof(Cvc));
                OnPropertyChanged(nameof(CanClose));
                OnPropertyChanged(nameof(IsValidCvc));
            }
        }
        public string Month
        {
            get => month;
            set
            {
                string val = value;
                if (val.Length == 1) val = "0" + val;
                month = val;
                OnPropertyChanged(nameof(Month));
                OnPropertyChanged(nameof(IsValidDate));
                OnPropertyChanged(nameof(CanClose));

            }

        }
        public string Year
        {
            get => year;
            set
            {
                
                year = value;
                OnPropertyChanged(nameof(Year));
                OnPropertyChanged(nameof(IsValidDate));
                OnPropertyChanged(nameof(CanClose));

            }

        }
        public bool IsFavourite
        {
            get => isFavourite;
            [MemberNotNull(nameof(isFavourite))]
            set
            {
                isFavourite = value;
                OnPropertyChanged(nameof(IsFavourite));
            }
        }

        protected void Add()
        {
            isChecked = true;
            if (CanClose)
            {
                if (Name == "") Name = "NewWebSite";
                dialogResult = true;
                Model = new CardModel(Number, Month, Year, Cvc, Owner, Name, IsFavourite);
                Model.Id = Id;
                dialogResultRequest?.Invoke(this, new DialogResultEventArgs(dialogResult));
            }
            else
            {
                OnPropertyChanged(nameof(CanClose));
                OnPropertyChanged(nameof(IsValidDate));
                OnPropertyChanged(nameof(IsValidCvc));
                OnPropertyChanged(nameof(IsValidNumber));
                OnPropertyChanged(nameof(IsValidOwner));
                OnPropertyChanged(nameof(IsValidName));
            }
        }


        
        public override bool CanClose
        {
            get
            {
                if (!isChecked) return true;
                return IsValidOwner &&
                       IsValidCvc &&
                       Month != string.Empty &&
                       Year != string.Empty &&
                       IsValidName &&
                       IsValidNumber &&
                       IsValidDate;
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
        public bool IsValidOwner
        {
            get
            {
                if (!isChecked) return true;
                return Regex.IsMatch(Owner, ownerPattern) && Owner.Length > 0
                    && Owner.Length <= 50;
            }
        }
        
        public bool IsValidNumber
        {
            get
            {
                if (!isChecked) return true;
                return Regex.IsMatch(Number, numberPattern);
            }
        }

        public bool IsValidCvc
        {
            get
            {
                if (!isChecked) return true;
                return Regex.IsMatch(Cvc, cvcPattern);
            }
        }

        public bool IsValidDate
        {
            get
            {
                if (!isChecked) return true;
                return (int.TryParse(Year, out int intYear) && intYear >= DateTime.Today.Year - 1 && intYear <= DateTime.Today.Year + 10) &&
                    (int.TryParse(Month, out int intMonth) && intMonth >= 0 && intMonth <= 12);
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
