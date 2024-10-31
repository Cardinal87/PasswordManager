using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels
{
    
    internal class PropertyChangedExtendedEventArgs : PropertyChangedEventArgs
    {
        public PropertyChangedExtendedEventArgs(string? propertyName, object? newValue) : base(propertyName)
        {
            NewValue = newValue;
        }
        public object? NewValue {  get; set; } 
    }
}
