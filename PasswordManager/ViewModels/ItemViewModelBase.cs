using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels
{
    internal partial class ItemViewModelBase : ViewModelBase
    {
        
        public ItemViewModelBase(string name, RelayCommand del, RelayCommand change, Action<ItemViewModelBase> ShowData)
        {
            Name = name;
            DeleteCommand = del;
            ChangeCommand = change;
            showData = ShowData;
        }

        public string Name { get; set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand ChangeCommand { get; private set; }

        private Action<ItemViewModelBase> showData;
    }
}
