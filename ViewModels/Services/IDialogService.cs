using PasswordManager.ViewModels.BaseClasses;
using PasswordManager.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.Services
{
    public interface IDialogService
    {
        void OpenDialog(DialogViewModelBase DialogVm);
        void CloseDialog(DialogViewModelBase dialogVM);
        
    }
}
