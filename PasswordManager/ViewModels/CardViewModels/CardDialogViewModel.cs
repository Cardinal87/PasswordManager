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
        public CardDialogViewModel(Card model)
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
        public RelayCommand AddCommand;
        public RelayCommand CloseCommand;

        public Card? Model { get; private set; }
        private string name = "";
        private string owner = "";
        private string number = "";
        private int cvc;
        private int month;
        private int year;
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

        public int Cvc
        {
            get => cvc;
            [MemberNotNull(nameof(cvc))]
            set
            {
                cvc = value;
                OnPropertyChanged(nameof(Cvc));
            }
        }
        public int Month
        {
            get => month;
            [MemberNotNull(nameof(month))]
            set
            {
                month = value;
                OnPropertyChanged(nameof(Month));
            }

        }
        public int Year
        {
            get => year;
            [MemberNotNull(nameof(year))]
            set
            {
                year = value;
                OnPropertyChanged(nameof(Year));
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
            get
            {
                return Owner != string.Empty;
            }
        }


        protected override void Close()
        {
            dialogResult = false;
            if (CanClose)
            {
                Model = new Card(Number, Month, Year, Cvc, Owner, Name, IsFavourite);
                dialogResultRequest?.Invoke(this, new DialogResultEventArgs(dialogResult));

            }
        }
    }
}
