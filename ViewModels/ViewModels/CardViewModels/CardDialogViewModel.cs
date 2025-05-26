using CommunityToolkit.Mvvm.Input;

using ViewModels.BaseClasses;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Models;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ViewModels.CardViewModels
{
    public class CardDialogViewModel : DialogViewModelBase
    {
        private const string namePattern = @"[a-zA-Z0-9._%+-]+|^$";
        private const string cvcPattern = @"^\d{3}$";
        private const string numberPattern = @"^(\d\s*){12,19}\d$";
        private const string ownerPattern = @"[a-zA-Z0-9._%+-]+|^$";

        public CardDialogViewModel() 
        { 
            AddCommand = new RelayCommand(Add);
            CloseCommand = new RelayCommand(Close);
            isFavourite = false;
            Id = 0;
            IsNew = true;
        }
        public CardDialogViewModel(CardModel model)
        {
            AddCommand = new RelayCommand(Add);
            CloseCommand = new RelayCommand(Close);
            isFavourite = model.IsFavourite;
            IsNew = false;
            Id = model.Id;
            Name = model.Name!;
            Owner = model.Owner;
            Number = model.Number;
            Cvc = model.Cvc;
            Month = model.Month;
            Year = model.Year;
            Model = model;
            
        }

        public bool dialogResult;
        
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
                month = value;
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

        public string Date
        {
            get
            {
                if (String.IsNullOrEmpty(Year)) return Month;
                return Month + "/" + Year;

            }
            set
            {
                var arr = value.Split('/');
                Month = arr[0];
                Year = arr.Length == 2 ? arr[1] : "";
                OnPropertyChanged(nameof(Date));
                OnPropertyChanged(nameof(CanClose));
                OnPropertyChanged(nameof(IsValidDate));
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

                RequestClose(new DialogResultEventArgs(dialogResult));
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
                return (int.TryParse(Year, out int intYear) && intYear >= DateTime.Today.Year - 2001 && intYear <= DateTime.Today.Year - 1900) &&
                    (int.TryParse(Month, out int intMonth) && intMonth >= 0 && intMonth <= 12);
            }
        }

        private bool isChecked = false;

        public override void Close()
        {
            dialogResult = false;
            RequestClose(new DialogResultEventArgs(dialogResult));
        }
    }
}
