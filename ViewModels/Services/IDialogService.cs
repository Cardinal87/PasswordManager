using ViewModels.BaseClasses;
using ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Services
{
    public interface IDialogService
    {
        void OpenDialog(DialogViewModelBase DialogVm);
        void CloseDialog(DialogViewModelBase dialogVM);
        
    }
}
