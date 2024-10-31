using PasswordManager.DataConnectors;
using PasswordManager.Helpers;
using PasswordManager.ViewModels;
using PasswordManager.ViewModels.AllEntriesViewModels;
using PasswordManager.ViewModels.AppViewModels;
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
        public IDatabaseClient DatabaseClient { get; }
        public WebSiteViewModel CreateWebSiteVM();
        public AppViewModel CreateAppVM();
        public AllEntriesViewModel CreateAllEntriesVM(List<ItemViewModelBase> list);

    }
}
