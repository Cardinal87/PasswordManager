using PasswordManager.Helpers;
using PasswordManager.Models;
using PasswordManager.ViewModels.WebSiteViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.AllEntriesViewModels
{
    internal class AllEntriesViewModel : ViewModelBase
    {
        public AllEntriesViewModel(Func<ObservableCollection<ItemViewModelBase>> loadViewModelList)
        {
            Items = loadViewModelList.Invoke();
            LoadViewModelList = loadViewModelList;
        }
        public Func<ObservableCollection<ItemViewModelBase>> LoadViewModelList { get; private set; }
        public ObservableCollection<ItemViewModelBase> Items { get; set; }
        

        
    }
}
