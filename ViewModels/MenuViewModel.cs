using ViewModels.BaseClasses;
using System.Net;
using Interfaces;
using CommunityToolkit.Mvvm.Input;
using Services;
using Services.Http;


namespace ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
       
        
        public MenuViewModel(ITokenHandlerService tokenService, HttpDatabaseManager httpDatabaseManager) 
        {
            _httpDatabaseManager = httpDatabaseManager;
            _tokenService = tokenService;
            DeleteStorageCommand = new AsyncRelayCommand(DeleteStorage);
            SavePasswordCommand = new AsyncRelayCommand<string>(SavePassword);
            CheckPasswordCommand = new AsyncRelayCommand<string>(CheckPassword);
            HasPassword = httpDatabaseManager.IsDatabaseExist();
        }
        private HttpDatabaseManager _httpDatabaseManager;
        private Func<string, Task>? _startApp;
        private ITokenHandlerService _tokenService;
        public AsyncRelayCommand DeleteStorageCommand { get; private set; }
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

        public bool HasPassword { get; private set; }


        private async Task CheckPassword(string? password)
        {
            if (_startApp == null)
                throw new NullReferenceException("start up action is not set");
            
            if (!String.IsNullOrEmpty(password))
            {
                
                var status = await _tokenService.FetchToken(password);
                if (status == HttpStatusCode.Unauthorized)
                {
                    IsCorrectPass = false;
                    return;
                }
                await _startApp(password);
                
                
            }
        }

        private async Task SavePassword(string? password)
        {
            if (_startApp == null)
                throw new NullReferenceException("start up action is not set");

            if (!String.IsNullOrEmpty(password))
            {
                IsCorrectPass = EncodingKeysService.IsStrongPassword(password);
                if (IsCorrectPass)
                {
                    await _httpDatabaseManager.CreateDatabase(password);
                    await _tokenService.FetchToken(password);
                    await _startApp(password);
                }
            }
        }
        private async Task DeleteStorage()
        {
            await _httpDatabaseManager.DeleteDatabase();
            HasPassword = false;
            OnPropertyChanged(nameof(HasPassword));
        }
        
        public void SetStartAction(Func<string, Task> startApp)
        {
            _startApp = startApp;
        }
        
    }
}
