using PasswordManager.DataConnectors;
using PasswordManager.Helpers;
using PasswordManager.ViewModels.AllEntriesViewModels;
using PasswordManager.ViewModels.AppViewModels;
using PasswordManager.ViewModels.BaseClasses;
using PasswordManager.ViewModels.CardViewModels;
using PasswordManager.ViewModels.WebSiteViewModels;
using PasswordManager.Views.AllEntriesViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Factories
{
    internal interface IViewModelFactory
    {
        public IItemViewModelFactory ItemFac { get; }
        public IDialogService DialogService { get; }
        public IContextFactory ContextFactory { get; }
        public Task<WebSiteViewModel> CreateWebSiteVMAsync();
        public Task<AppViewModel> CreateAppVMAsync();
        public Task<CardViewModel> CreateCardVMAsync();
        public Task<AllEntriesViewModel> CreateAllEntriesVMAsync();

    }
}
