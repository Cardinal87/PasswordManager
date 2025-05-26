
using CommunityToolkit.Mvvm.Input;
using Interfaces;

using Models;
using System.Diagnostics.CodeAnalysis;
using ViewModels.BaseClasses;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace ViewModels.WebSiteViewModels
{
    public partial class WebSiteItemViewModel : ItemViewModelBase
    {
        
        public WebSiteModel Model { get; private set; }
        private IClipboardService _clipBoardService;
        
        public WebSiteItemViewModel(WebSiteModel model, IClipboardService clipboardService) 
        {
            
            UpdateModel(model);
            _clipBoardService = clipboardService;

            OpenWebSiteCommand = new RelayCommand(OpenWebSite);
            CopyToClipboardCommand = new RelayCommand<string>(CopyToClipboard);
            
            
        }
        public RelayCommand OpenWebSiteCommand { get; set; }
        public RelayCommand<string> CopyToClipboardCommand { get; }
        

        
        private string login;
        private string password;
        private string webAddress;
        
        
        public string Login {
            get
            {
                return login;
            }
            [MemberNotNull(nameof(login))]
            set
            {
                login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        
        public string Password 
        {
            get
            {
                return password;
            }
            [MemberNotNull(nameof(password))]
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        
        
        
        public string WebAddress 
        {
            get
            {
                return webAddress;
            }
            [MemberNotNull(nameof(webAddress))]
            set
            {
                webAddress = value;
                OnPropertyChanged(nameof(WebAddress));
            }
        }


        private void CopyToClipboard(string? text)
        {
            if (text != null)
                _clipBoardService.SaveToClipBoard(text);
        }

        private void OpenWebSite()
        {
            if (!String.IsNullOrEmpty(WebAddress))
            {
                string url = "http://" + WebAddress + "/";
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
        }


        [MemberNotNull(nameof(webAddress))]
        [MemberNotNull(nameof(login))]
        [MemberNotNull(nameof(password))]
        [MemberNotNull(nameof(Model))]
        public void UpdateModel(WebSiteModel model)
        {
            
            Model = model;
            Id = model.Id;
            Name = model.Name;
            Password = model.Password;
            Login = model.Login;
            WebAddress = model.WebAddress;
            IsFavourite = model.IsFavourite;
        }
        
    }
}
