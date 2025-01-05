using Microsoft.Data.Sqlite;
using PasswordManager.Factories;
using PasswordManager.ViewModels.BaseClasses;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Configuration;
using PasswordManager.Configuration.OptionExtensions;





namespace PasswordManager.ViewModels
{
    class MenuViewModel : ViewModelBase
    {

        public MenuViewModel(IWritableOptions<LoggingOptions> loggingOpt) 
        {
            _loggingOpt = loggingOpt;
            SavePasswordCommand = new AsyncRelayCommand<string>(SavePassword);
            CheckPasswordCommand = new AsyncRelayCommand<string>(CheckPassword);
        }
        private IWritableOptions<LoggingOptions> _loggingOpt;
        public AsyncRelayCommand<string> SavePasswordCommand { get; set; }
        public AsyncRelayCommand<string> CheckPasswordCommand { get; set; }

        private bool? isCorrectPass;
        public bool? IsCorrectPass
        {
            get => isCorrectPass;
            set
            {
                isCorrectPass = value;
                OnPropertyChanged(nameof(IsCorrectPass));
            }
        }

        public bool HasPassword { get => !String.IsNullOrEmpty(_loggingOpt.Value.Hash); }


        private async Task CheckPassword(string? password)
        {
            if (!String.IsNullOrEmpty(password))
            {
                
                var salt = _loggingOpt.Value.Hash.Split()[0]!;
                var hash = await GetHash(password, salt);
                IsCorrectPass = _loggingOpt.Value.Hash.Equals(hash);
                OnPropertyChanged("PasswordChecked");
            }
        }

        private async Task SavePassword(string? password)
        {
            if (!String.IsNullOrEmpty(password))
            {
                var salt = GenerateSalt();
                var hash = await GetHash(password, salt);
                _loggingOpt.Update(opt =>
                {
                    opt.Hash = hash;
                });

                OnPropertyChanged("PasswordCreated");
            }
        }


        private async Task<string> GetHash(string password, string salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string str = String.Concat(password, salt);
                var bites = Encoding.UTF8.GetBytes(str);
                var hash = await Task.Run(() => sha256.ComputeHash(bites));
                var hashString = BitConverter.ToString(hash).ToLower().Replace("-", "");
                hashString = String.Concat(salt + " ", hashString);
                return hashString;
            }
            
        }

        private string GenerateSalt()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[16];
                rng.GetBytes(salt);
                return Convert.ToBase64String(salt);
            }
        }

        
    }
}
