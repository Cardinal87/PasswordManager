using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.BaseClasses
{
    internal partial class ItemViewModelBase : ViewModelBase
    {
        private string? name;
        public int Id { get; protected set; }
        
        
        public string? Name 
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        

    }
}
