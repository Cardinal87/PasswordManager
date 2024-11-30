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
    internal class ItemViewModelFactory : IItemViewModelFactory
    {
        public IClipboardService Clipboard { get; }
        public ItemViewModelFactory(IClipboardService clipboard) 
        { 
            Clipboard = clipboard;
        }


        public AppItemViewModel CreateAppItem(AppModel model) =>
            new AppItemViewModel(model, Clipboard);
        

        public WebSiteItemViewModel CreateWebSiteItem(WebSiteModel model) =>
            new WebSiteItemViewModel(model, Clipboard);


        public CardItemViewModel CreateCardItem(CardModel model) =>
            new CardItemViewModel(model,Clipboard);
        
    }
}
