using PasswordManager.DataConnectors;
using PasswordManager.Helpers;
using PasswordManager.ViewModels.AllEntriesViewModels;
using PasswordManager.ViewModels.AppViewModels;
using PasswordManager.ViewModels.BaseClasses;
using PasswordManager.ViewModels.CardViewModels;
using PasswordManager.ViewModels.WebSiteViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Factories
{
    internal class ViewModelFactory : IViewModelFactory
    {
        public ViewModelFactory(IItemViewModelFactory itemFac, IDialogService dialogService, IContextFactory contextFactory)
        {
            ItemFac = itemFac;
            DialogService = dialogService;
            ContextFactory = contextFactory;
        }

        public IItemViewModelFactory ItemFac { get; }

        public IDialogService DialogService { get; }

        public IContextFactory ContextFactory { get; }

        public Task<AllEntriesViewModel> CreateAllEntriesVMAsync()
        {
            var vm = new AllEntriesViewModel();
            return Task.FromResult(vm);
        }
        public Task<AppViewModel> CreateAppVMAsync()
        {
            var vm = new AppViewModel(ContextFactory, DialogService, ItemFac);
            return Task.FromResult(vm);
        }

        public Task<CardViewModel> CreateCardVMAsync()
        {
            var vm = new CardViewModel(ContextFactory, DialogService, ItemFac);
            return Task.FromResult(vm);
        }

        public Task<WebSiteViewModel> CreateWebSiteVMAsync() 
        { 
            var vm = new WebSiteViewModel(ContextFactory, DialogService, ItemFac);
            return Task.FromResult(vm);
        }
        
    }
}
