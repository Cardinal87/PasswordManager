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
        
        public ItemViewModelBase(int id, string name, RelayCommand del, RelayCommand change, Action<ItemViewModelBase> ShowData)
        {
            Id = id;
            Name = name;
            DeleteCommand = del;
            ChangeCommand = change;
            showData = ShowData;
        }
        public int Id { get; protected set; }
        public string Name { get; set; }
        public RelayCommand DeleteCommand { get; protected set; }
        public RelayCommand ChangeCommand { get; protected set; }

        protected Action<ItemViewModelBase> showData;
        
    }
}
