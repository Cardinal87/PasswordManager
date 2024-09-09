using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using PasswordManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Helpers
{
    internal class DialogService : IDialogService
    {
        private void RegisterView(DialogViewModelBase viewmodel, Window dialog) => dialog.DataContext = viewmodel;
        
        
        public void OpenDialog(DialogViewModelBase DialogVm)
        {
            Window owner = ((IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!)!.MainWindow!;
            var dialog = CreateWindow(DialogVm);
            RegisterView(DialogVm, dialog);
            dialog.ShowDialog(owner);
        }

        private Window CreateWindow(DialogViewModelBase viewmodel)
        {
            string name = viewmodel.GetType().FullName!.Replace("ViewModel", "");
            var type = Type.GetType(name);
            if (type != null)
            {
                return (Window)Activator.CreateInstance(type)!;
            }
            else throw new Exception();
        }
    }
}
