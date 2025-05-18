using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Interfaces.PasswordGenerator;
using ViewModels.BaseClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace ViewModels
{
    public class PasswordGeneratorViewModel : DialogViewModelBase
    {
        
        public PasswordGeneratorViewModel(IServiceProvider provider) 
        {
            _generator = provider.GetRequiredService<IPasswordGenerator>();
            GeneratePasswordCommand = new RelayCommand(GeneratePassword);
            ConfirmCommand = new RelayCommand(Confirm);
            CancelCommand = new RelayCommand(Close);
            GeneratePassword();

        }
        private IPasswordGenerator _generator;
        public RelayCommand ConfirmCommand { get;private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand GeneratePasswordCommand { get; private set; }
        private bool dialogResult;
        private string password = "";
        private bool activateUpcase = true;
        private bool activateLowcase = true;
        private bool activateDigits = false;
        private bool activateSpecSymbs = false;
        private int length = 15;


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
            }
        }
        public override bool CanClose
        {
            get
            {
                return ActivateUpcase || ActivateLowcase ||
                    ActivateDigits || ActivateSpecSymbs;
            }
        }

        public bool ActivateUpcase
        {
            get => activateUpcase;
            set
            {
                activateUpcase = value;
                OnPropertyChanged(nameof(ActivateUpcase));
                OnPropertyChanged(nameof(CanClose));
            }
        }
        public bool ActivateLowcase
        {
            get => activateLowcase;
            set
            {
                activateLowcase = value;
                OnPropertyChanged(nameof(ActivateUpcase));
                OnPropertyChanged(nameof(CanClose));

            }
        }
        public bool ActivateDigits
        {
            get => activateDigits;
            set
            {
                activateDigits = value;
                OnPropertyChanged(nameof(ActivateDigits));
                OnPropertyChanged(nameof(CanClose));
            }
        }
        public bool ActivateSpecSymbs
        {
            get => activateSpecSymbs;
            set
            {
                activateSpecSymbs = value;
                OnPropertyChanged(nameof(ActivateUpcase));
                OnPropertyChanged(nameof(CanClose));
            }
        }
        public int Length
        {
            get => length;
            set
            {
                length = value;
                OnPropertyChanged(nameof(Length));
                OnPropertyChanged(nameof(CanClose));
            }
        }


        private void GeneratePassword()
        {

            var args = new PasswordGeneratorArgs(Length, ActivateDigits, ActivateSpecSymbs, ActivateUpcase, ActivateLowcase);
            string password = _generator.GeneratePassword(args);
            Password = password;
            
        }
        private void Confirm()
        {
            if (CanClose)
            {
                dialogResult = true;

                RequestClose(new DialogResultEventArgs(dialogResult));
            }
        }
        public override void Close()
        {
            dialogResult = false;
            RequestClose(new DialogResultEventArgs(dialogResult));
        }


    }
}
