using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using PasswordManager.ViewModels.BaseClasses;
using PasswordManager.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PasswordManager.Helpers
{
    internal class DialogService : IDialogService
    {
        private List<Window?> openWindows = [];
        
        private void RegisterView(DialogViewModelBase viewmodel, Window dialog) => dialog.DataContext = viewmodel;
        
        
        public void OpenDialog(DialogViewModelBase DialogVm)
        {
            Window owner = ((IClassicDesktopStyleApplicationLifetime)Application.Current!.ApplicationLifetime!)!.MainWindow!;
            Window dialog = CreateWindow(DialogVm);
            openWindows.Add(dialog);
            RegisterView(DialogVm, dialog);
            dialog.ShowDialog(owner);
        }

        private Window CreateWindow(DialogViewModelBase viewmodel)
        {
            string name = viewmodel.GetType().FullName!.Replace( "ViewModel", "View");
            var type = Type.GetType(name);
            if (type != null)
            {
                Window win = (Window)Activator.CreateInstance(type)!;
                return win;
            }
            else throw new Exception();
        }

        public void CloseDialog(DialogViewModelBase dialogVM)
        {
            Window? win = openWindows.FirstOrDefault(x => x!.DataContext == dialogVM);
            if (win != null)
            {
                win.Close();
                openWindows.Remove(win);
                
            }
        }
        
    }
}
