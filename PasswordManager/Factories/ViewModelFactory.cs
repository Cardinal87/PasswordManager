using PasswordManager.DataConnectors;
using PasswordManager.Helpers;
using PasswordManager.ViewModels;
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

        public AllEntriesViewModel CreateAllEntriesVM(WebSiteViewModel webVm, CardViewModel cardVm, AppViewModel appVm)
        {
            var vm = new AllEntriesViewModel(appVm, cardVm, webVm);
            return vm;
        }
        public async Task<AppViewModel> CreateAppVMAsync()
        {
            var vm = await AppViewModel.CreateAsync(ContextFactory, DialogService, ItemFac);
            return vm;
        }

        public async Task<CardViewModel> CreateCardVMAsync()
        {
            var vm = await CardViewModel.CreateAsync(ContextFactory, DialogService, ItemFac);
            return vm;
        }


        public async Task<WebSiteViewModel> CreateWebSiteVMAsync() 
        {
            var vm = await WebSiteViewModel.CreateAsync(ContextFactory, DialogService, ItemFac);
            return vm;
        }
        
    }
}
