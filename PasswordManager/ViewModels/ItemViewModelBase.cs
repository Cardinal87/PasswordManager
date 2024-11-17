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
        
        public int Id { get; protected set; }
        public string? Name { get; set; }
        public RelayCommand? DeleteCommand { get; protected set; }
        public RelayCommand? ChangeCommand { get; protected set; }

        public RelayCommand? ShowDataCommand { get; protected set; }
        
    }
}
