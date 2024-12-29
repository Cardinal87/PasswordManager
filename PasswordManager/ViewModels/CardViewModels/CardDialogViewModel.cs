using CommunityToolkit.Mvvm.Input;
using PasswordManager.Models;
using PasswordManager.ViewModels.BaseClasses;
using PasswordManager.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.CardViewModels
{
    internal class CardDialogViewModel : DialogViewModelBase, IDialogResultHelper
    {
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
            if (CanClose)
            {
                if (Name == "") Name = "NewWebSite";
                dialogResult = true;
                Model = new CardModel(Number, Month, Year, Cvc, Owner, Name, IsFavourite);
                Model.Id = Id;
                dialogResultRequest?.Invoke(this, new DialogResultEventArgs(dialogResult));
            }
        }


        
        public override bool CanClose
        {
            [MemberNotNullWhen(true, nameof(Cvc))]
            [MemberNotNullWhen(true, nameof(Month))]
            [MemberNotNullWhen(true, nameof(Year))]
            get
            {
                return Owner != string.Empty &&
                       Cvc != string.Empty &&
                       Month != string.Empty &&
                       Year != string.Empty &&
                       IsValidDate;
            }
        }
        public bool IsValidDate
        {
            get
            {
                return (int.TryParse(Year, out int intYear) && intYear >= DateTime.Today.Year - 1 && intYear <= DateTime.Today.Year + 10) &&
                    (int.TryParse(Month, out int intMonth) && intMonth >= 0 && intMonth <= 12);
            }
        }


        protected override void Close()
        {
            dialogResult = false;
            dialogResultRequest?.Invoke(this, new DialogResultEventArgs(dialogResult));
        }
    }
}
