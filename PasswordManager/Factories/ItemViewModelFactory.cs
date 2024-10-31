using Avalonia.Input.Platform;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Helpers;
using PasswordManager.Models;
using PasswordManager.ViewModels;
using PasswordManager.ViewModels.AppViewModels;
using PasswordManager.ViewModels.WebSiteViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Factories
{
    internal class ItemViewModelFactory : IItemViewModelFactory
    {
        public IClipboardService Clipboard { get; }
        public ItemViewModelFactory(IClipboardService clipboard) 
        { 
            Clipboard = clipboard;
        }


        public AppItemViewModel CreateAppItem(Models.App model, RelayCommand delete, RelayCommand change, Action<ItemViewModelBase> showData)
        {
            return new AppItemViewModel(model, delete, change, showData, Clipboard);
        }

        public WebSiteItemViewModel CreateWebSiteItem(WebSite model, RelayCommand delete, RelayCommand change, Action<ItemViewModelBase> showData)
        {
            return new WebSiteItemViewModel(model, Clipboard, delete, change, showData);
        }
    }
}
