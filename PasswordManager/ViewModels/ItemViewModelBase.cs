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
        
        public ItemViewModelBase(string name, RelayCommand del)
        {
            Name = name;
            DeleteCommand = del;
        }

        public string Name { get; set; }
        public RelayCommand DeleteCommand { get; private set; }
        
        
    }
}
