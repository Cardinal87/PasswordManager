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
        
        public ItemViewModelBase(string name, RelayCommand del, RelayCommand change, RelayCommand showData)
        {
            Name = name;
            DeleteCommand = del;
            ChangeCommand = change;
            ShowDataCommand = showData;
        }

        public string Name { get; set; }
        public RelayCommand DeleteCommand { get; private set; }
        public RelayCommand ChangeCommand { get; private set; }

        public RelayCommand ShowDataCommand { get; private set; }
    }
}
