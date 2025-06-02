
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IDialogService
    {
        void OpenDialog(IDialogViewModel DialogVm);
        void CloseDialog(IDialogViewModel dialogVM);
        void CloseAllDialogs();
        
    }
}
