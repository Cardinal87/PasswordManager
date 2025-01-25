
using ViewModels.BaseClasses;
using System.Security.Cryptography;
using System;
using System.Text;
using ViewModels.Services;
using ViewModels.Services.AppConfiguration;
using CommunityToolkit.Mvvm.Input;


using System.Text.RegularExpressions;
using Models.DataConnectors;





namespace ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private const string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{10,50}$";
        
        
        public MenuViewModel(IWritableOptions<AppAuthorizationOptions> loggingOpt, Func<string, Task> startApp) 
        {
            _startApp = startApp;
            _loggingOpt = loggingOpt;
            DeleteStorageCommand = new RelayCommand(DeleteStorage);
            SavePasswordCommand = new AsyncRelayCommand<string>(SavePassword);
            CheckPasswordCommand = new AsyncRelayCommand<string>(CheckPassword);
        }
        private Func<string, Task> _startApp;
        private IWritableOptions<AppAuthorizationOptions> _loggingOpt;
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
                var salt = _loggingOpt.Value.Salt;
                var hash = await EncodingKeys.GetHash(password, salt);
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
                var hash = await EncodingKeys.GetHash(password, salt);
                _loggingOpt.Update(opt =>
                {
                    opt.Hash = hash;
                    opt.Salt = salt;
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
                opt.Salt = "";
            });
            OnPropertyChanged(nameof(HasPassword));
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
