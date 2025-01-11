using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.ViewModels.Interfaces
{
    internal interface IDialogResultHelper
    {
        event EventHandler<DialogResultEventArgs> dialogResultRequest; 
    }



    internal class DialogResultEventArgs : EventArgs 
    { 
        public bool DialogResult { get; private set; }
        public DialogResultEventArgs(bool dialogResult)
        {
            DialogResult = dialogResult;
        }
    }


}
