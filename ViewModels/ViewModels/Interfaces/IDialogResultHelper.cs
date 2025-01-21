using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Interfaces
{
    public interface IDialogResultHelper
    {
        event EventHandler<DialogResultEventArgs> dialogResultRequest; 
    }



    public class DialogResultEventArgs : EventArgs 
    { 
        public bool DialogResult { get; private set; }
        public DialogResultEventArgs(bool dialogResult)
        {
            DialogResult = dialogResult;
        }
    }


}
