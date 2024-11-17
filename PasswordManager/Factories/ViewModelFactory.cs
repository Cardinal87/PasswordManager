using PasswordManager.DataConnectors;
using PasswordManager.Helpers;
using PasswordManager.ViewModels;
using PasswordManager.ViewModels.AllEntriesViewModels;
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
    internal class ViewModelFactory : IViewModelFactory
    {
        public ViewModelFactory(IItemViewModelFactory itemFac, IDialogService dialogService, IDatabaseClient databaseClient)
        {
            ItemFac = itemFac;
            DialogService = dialogService;
            DatabaseClient = databaseClient;
        }

        public IItemViewModelFactory ItemFac { get; }

        public IDialogService DialogService { get; }

        public IDatabaseClient DatabaseClient { get; }

        public AllEntriesViewModel CreateAllEntriesVM(List<ItemViewModelBase> list) =>
            new AllEntriesViewModel(list);
        

        public AppViewModel CreateAppVM() =>
             new AppViewModel(DatabaseClient, DialogService, ItemFac);

        public CardViewModel CreateCardVM() =>
            new CardViewModel(DatabaseClient, DialogService, ItemFac);

        public WebSiteViewModel CreateWebSiteVM() =>
            new WebSiteViewModel(DatabaseClient, DialogService, ItemFac);
        
    }
}
