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
using System.Runtime.InteropServices.Marshalling;

namespace PasswordManager.ViewModels
{
    class MenuViewModel : ViewModelBase
    {

        public MenuViewModel() 
        {
            SavePasswordCommand = new AsyncRelayCommand<string>(SavePassword);
            CheckPasswordCommand = new AsyncRelayCommand<string>(CheckPassword);
        }

        private AsyncRelayCommand<string> SavePasswordCommand { get; set; }
        private AsyncRelayCommand<string> CheckPasswordCommand { get; set; }

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
        private async Task CheckPassword(string? password)
        {
            if (!String.IsNullOrEmpty(password))
            {
                var salt = Config.Default.Hash.Split('-')[1];
                var hash = await GetHash(password, salt);
                IsCorrectPass = Config.Default.Hash.Equals(hash);
                OnPropertyChanged(nameof(IsCorrectPass));
            }
        }

        private async Task SavePassword(string? password)
        {
            if (!String.IsNullOrEmpty(password))
            {
                var salt = GenerateSalt();
                var hash = await GetHash(password, salt);
                Config.Default.Hash = hash;
            }
        }


        private async Task<string> GetHash(string password, string salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string str = String.Concat(password, salt);
                var bites = Encoding.UTF8.GetBytes(str);
                var hash = await Task.Run(() => sha256.ComputeHash(bites));
                var hashString = BitConverter.ToString(hash).ToLower();
                hashString = String.Concat(salt + "-", hashString);
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
