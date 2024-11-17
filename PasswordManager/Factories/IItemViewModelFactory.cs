using Avalonia.Input.Platform;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Helpers;
using PasswordManager.Models;
using PasswordManager.ViewModels;
using PasswordManager.ViewModels.AppViewModels;
using PasswordManager.ViewModels.CardViewModels;
using PasswordManager.ViewModels.WebSiteViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Factories
{
    internal interface IItemViewModelFactory
    {
        public IClipboardService Clipboard { get; }
        public WebSiteItemViewModel CreateWebSiteItem(WebSite model, RelayCommand delete, RelayCommand change, Action<WebSiteItemViewModel> showData);
        public AppItemViewModel CreateAppItem(Models.App model, RelayCommand delete, RelayCommand change, Action<AppItemViewModel> showData);
        public CardItemViewModel CreateCardItem(Card model, RelayCommand delete, RelayCommand change, Action<AppItemViewModel> showData);
    }
}
