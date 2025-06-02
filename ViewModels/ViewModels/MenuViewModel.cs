
using ViewModels.BaseClasses;
using System.Security.Cryptography;
using Interfaces;
using CommunityToolkit.Mvvm.Input;
using Services;
using System.Text.RegularExpressions;
using Models.AppConfiguration;


namespace ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
       
        
        public MenuViewModel(IWritableOptions<AppAuthorizationOptions> loggingOpt,
                             ITokenHandlerService tokenService) 
        {
            _loggingOpt = loggingOpt;
            _tokenService = tokenService;
            DeleteStorageCommand = new RelayCommand(DeleteStorage);
            SavePasswordCommand = new AsyncRelayCommand<string>(SavePassword);
            CheckPasswordCommand = new AsyncRelayCommand<string>(CheckPassword);
        }
        private Func<string, Task>? _startApp;
        private IWritableOptions<AppAuthorizationOptions> _loggingOpt;
        private ITokenHandlerService _tokenService;
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
            if (_startApp == null)
                throw new NullReferenceException("start up action is not set");
            
            if (!String.IsNullOrEmpty(password))
            {
                IsCorrectPass = EncodingKeysService.CompareHash(password, _loggingOpt.Value.Hash);
                if (IsCorrectPass)
                    await _tokenService.FetchToken(password);
                    await _startApp(password);
            }
        }

        private async Task SavePassword(string? password)
        {
            if (_startApp == null)
                throw new NullReferenceException("start up action is not set");

            if (!String.IsNullOrEmpty(password))
            {
                var salt = EncodingKeysService.GenerateSalt();
                var hash = EncodingKeysService.GetHash(password);
                IsCorrectPass = EncodingKeysService.IsCorrectPassword(password);
                if (IsCorrectPass)
                {
                    await _tokenService.FetchToken(password);
                    await _startApp(password);
                }
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
        
        public void SetStartAction(Func<string, Task> startApp)
        {
            _startApp = startApp;
        }
        
    }
}
