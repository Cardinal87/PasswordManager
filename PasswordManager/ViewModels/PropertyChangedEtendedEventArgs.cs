using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels
{
    
    internal class PropertyChangedEtendedEventArgs : PropertyChangedEventArgs
    {
        public PropertyChangedEtendedEventArgs(string? propertyName, object? newValue) : base(propertyName)
        {
            NewValue = newValue;
        }
        public object? NewValue {  get; set; } 
    }
}
