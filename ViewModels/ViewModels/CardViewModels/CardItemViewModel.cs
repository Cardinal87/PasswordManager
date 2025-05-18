using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using ViewModels;
using Models;
using ViewModels.BaseClasses;
using ViewModels.WebSiteViewModels;
using System;
using System.Collections.Generic;
using Interfaces;
using Models.DataConnectors;

using System.Diagnostics.CodeAnalysis;

namespace ViewModels.CardViewModels
{
    public class CardItemViewModel : ItemViewModelBase
    {
        public CardItemViewModel(CardModel model, IServiceProvider provider) 
        {
            clipboardService = provider.GetRequiredService<IClipboardService>();
            CopyToClipboardCommand = new RelayCommand<string>(CopyToClipboard);
            
            
            
            UpdateModel(model);
        
        }
        private IClipboardService clipboardService;

        public CardModel Model { get; private set; }
        
        private string owner;
        private string number;
        private string cvc;
        private string month;
        private string year;
        

        public RelayCommand<string> CopyToClipboardCommand { get; }
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

        public string Cvc 
        { 
            get => cvc;
            [MemberNotNull(nameof(cvc))]
            set
            {
                cvc = value;
                OnPropertyChanged(nameof(Cvc));
            }
        }
        public string Month
        {
            get 
            {
                return month;
            }
            [MemberNotNull(nameof(month))]
            set
            {
                month = value;
                OnPropertyChanged(nameof(Month));
            }
        
        }
        public string Year 
        { 
            get => year;
            [MemberNotNull(nameof(year))]
            set
            {
                year = value;
                OnPropertyChanged(nameof(Year));
            }
                
        }
        

        private void CopyToClipboard(string? text)
        {
            if (text != null)
                clipboardService.SaveToClipBoard(text);
        }

        

        [MemberNotNull(nameof(owner))]
        [MemberNotNull(nameof(number))]
        [MemberNotNull(nameof(cvc))]
        [MemberNotNull(nameof(month))]
        [MemberNotNull(nameof(year))]
        [MemberNotNull(nameof(Model))]
        public void UpdateModel(CardModel model)
        {
            Model = model;
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
