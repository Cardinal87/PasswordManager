using CommunityToolkit.Mvvm.Input;
using PasswordManager.Helpers;
using PasswordManager.Models;
using PasswordManager.ViewModels.BaseClasses;
using PasswordManager.ViewModels.WebSiteViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.CardViewModels
{
    class CardItemViewModel : ItemViewModelBase
    {
        public CardItemViewModel(Card model, IClipboardService clipboardService, RelayCommand delete, RelayCommand change, Action<CardItemViewModel> ShowDataOfItem) 
        {
            this.clipboardService = clipboardService;
            DeleteCommand = delete;
            ChangeCommand = change;
            ShowDataCommand = new RelayCommand(() => ShowDataOfItem.Invoke(this));
            CopyToClipboardCommand = new RelayCommand<string>(CopyToClipboard);
            AddToFavouriteCommand = new RelayCommand(AddToFavourite);
            
            
            UpdateModel(model);
        
        }
        private IClipboardService clipboardService;

        private Card model;
        
        private string owner;
        private string number;
        private int cvc;
        private int month;
        private int year;
        private bool isFavourite;

        public RelayCommand<string> CopyToClipboardCommand { get; }
        public RelayCommand AddToFavouriteCommand { get; }
      

        
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


        private void CopyToClipboard(string? text)
        {
            if (text != null)
                clipboardService.SaveToClipBoard(text);
        }

        private void AddToFavourite()
        {
            if (IsFavourite) IsFavourite = false;
            else IsFavourite = true;
        }


        [MemberNotNull(nameof(owner))]
        [MemberNotNull(nameof(number))]
        [MemberNotNull(nameof(cvc))]
        [MemberNotNull(nameof(month))]
        [MemberNotNull(nameof(year))]
        [MemberNotNull(nameof(isFavourite))]
        [MemberNotNull(nameof(model))]
        public void UpdateModel(Card model)
        {
            this.model = model;
            Id = model.Id;
            Name = model.Name;
            Owner = model.Owner;
            Number = model.Number;
            Cvc = model.Cvc;
            Month = model.Month;
            Year = model.Year;
            IsFavourite = model.IsFavourite;

        }
    }
}
