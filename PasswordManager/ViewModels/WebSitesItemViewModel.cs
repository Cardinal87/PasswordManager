using Avalonia;
using Avalonia.Controls;
using Avalonia.Input.Platform;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace PasswordManager.ViewModels
{
    internal partial class WebSitesItemViewModel : ViewModelBase 
    {
        private WebSite model;
        private IClipboard clipboard = Helpers.ClipBoard.GetClipBoard();
        public WebSitesItemViewModel(WebSite model)
        {
            this.model = model;
            Id = model.Id;
            Name = model.Name;
            Login = model.Login;
            Password = model.Password;
        }
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string? Login { get; private set; }
        public string Password { get; private set; }
        public bool Favourite { get; private set; }

        
        public RelayCommand<int> DeleteCommand; 

        [RelayCommand]
        public async void CopyToClipboard(string name)
        {
            if (name == "login")
            {
                await clipboard.SetTextAsync(Login);
            }
            else if (name == "password")
            {
                await clipboard.SetTextAsync(Password);
            }
        }
        

        [RelayCommand]
        public void GoToWebSite()
        {

        }

        [RelayCommand]
        public void AddToFavourite()
        {
            if (Favourite) Favourite = false;
            else Favourite = true;
        }
            
            
        
    }
}
