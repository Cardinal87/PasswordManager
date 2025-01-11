
using PasswordManager.ViewModels.BaseClasses;
using System.Security.Cryptography;
using System;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Configuration.OptionExtensions;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using PasswordManager.Configuration.AppConfiguration;
using System.Text.RegularExpressions;





namespace PasswordManager.ViewModels
{
    class MenuViewModel : ViewModelBase
    {
        private const string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{10,50}$";
        
        
        public MenuViewModel(IWritableOptions<LoggingOptions> loggingOpt, Func<string, Task> startApp) 
        {
            _startApp = startApp;
            _loggingOpt = loggingOpt;
            DeleteStorageCommand = new RelayCommand(DeleteStorage);
            SavePasswordCommand = new AsyncRelayCommand<string>(SavePassword);
            CheckPasswordCommand = new AsyncRelayCommand<string>(CheckPassword);
        }
        private Func<string, Task> _startApp;
        private IWritableOptions<LoggingOptions> _loggingOpt;
        public RelayCommand DeleteStorageCommand { get; private set; }
        public AsyncRelayCommand<string> SavePasswordCommand { get; private set; }
        public AsyncRelayCommand<string> CheckPasswordCommand { get; private set; }
        private bool isCorrectPass = true;

        public bool IsCorrectPass
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
                var salt = _loggingOpt.Value.Hash.Split("==")[0] + "==";
                var hash = await GetHash(password, salt);
                IsCorrectPass = _loggingOpt.Value.Hash.Equals(hash);
                if (IsCorrectPass)
                    await _startApp(password);
                    
                
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
                IsCorrectPass = Regex.IsMatch(password, passwordPattern);
                if (IsCorrectPass)
                    await _startApp(password);
            }
        }
        private void DeleteStorage()
        {
            if (File.Exists(_loggingOpt.Value.ConnectionString))
            {
                File.Delete(_loggingOpt.Value.ConnectionString);
            }
            _loggingOpt.Update(opt =>
            {
                opt.Hash = "";
            });
            OnPropertyChanged(nameof(HasPassword));
        }
        
        private async Task<string> GetHash(string password, string salt)
        {
            int iterCount = 300000;
            int keySize = 32;
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt), iterCount, HashAlgorithmName.SHA256))
            {
                var hash = await Task.Run(() => pbkdf2.GetBytes(keySize));
                var hashString = BitConverter.ToString(hash).ToLower().Replace("-", "");
                hashString = String.Concat(salt, hashString);
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
