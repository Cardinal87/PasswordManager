using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia.Controls;
using Avalonia.Controls.Templates;
using PasswordManager.ViewModels.BaseClasses;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PasswordManager.Helpers
{
    internal class ViewLocator : IDataTemplate
    {
        public Control? Build(object? param)
        {
            var name = param!.GetType().FullName!.Replace("ViewModel", "View");
            var type = Type.GetType(name);
            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }
            else
            {
                return new TextBlock { Text = "Not Found: " + name };
            }
        }
        public bool Match(object? data)
        {
            return data is ViewModelBase;
        }
    }
}
