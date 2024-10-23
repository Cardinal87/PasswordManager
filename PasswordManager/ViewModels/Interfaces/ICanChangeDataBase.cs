using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.Interfaces
{
    internal interface ICanChangeDataBase
    {
        public event Func<ObservableCollection<ItemViewModelBase>>? OnDataBaseChanged;
    }
}
