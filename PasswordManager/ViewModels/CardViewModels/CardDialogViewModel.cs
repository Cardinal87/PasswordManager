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
        private int? cvc;
        private int? month;
        private int? year;
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
            }
        }

        public int? Cvc
        {
            get => cvc;
            set
            {
                cvc = value;
                OnPropertyChanged(nameof(Cvc));
            }
        }
        public int? Month
        {
            get => month;
            set
            {
                month = value;
                OnPropertyChanged(nameof(Month));
            }

        }
        public int? Year
        {
            get => year;
            set
            {
                year = value;
                OnPropertyChanged(nameof(Year));
                OnPropertyChanged(nameof(IsValidYear));
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
            dialogResult = true;
            dialogResultRequest?.Invoke(this, new DialogResultEventArgs(dialogResult));
        }


        
        public override bool CanClose
        {
            [MemberNotNullWhen(true, nameof(Cvc))]
            [MemberNotNullWhen(true, nameof(Month))]
            [MemberNotNullWhen(true, nameof(Year))]
            get
            {
                return Owner != string.Empty &&
                       Cvc != null &&
                       Month != null &&
                       Year != null &&
                       IsValidYear;
            }
        }
        public bool IsValidYear
        {
            get
            {
                return (Year >= DateTime.Today.Year - 1 && Year <= DateTime.Today.Year + 10) ||
                       Year == null;
            }
        }


        protected override void Close()
        {
            dialogResult = false;
            if (CanClose)
            {
                if (Name == "") Name = "NewCard";
                Model = new CardModel(Number, Month.Value, Year.Value, Cvc.Value, Owner, Name, IsFavourite);
                dialogResultRequest?.Invoke(this, new DialogResultEventArgs(dialogResult));

            }
        }
    }
}
