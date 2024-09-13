using PasswordManager.ViewModels.DialogInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Helpers
{
    internal interface IDialogService
    {
        void OpenDialog(IDialogViewModel DialogVm);
        void CloseDialog(IDialogViewModel dialogVM);
        
    }
}
