using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Dialogs;

namespace ViewModels.BaseClasses
{
    public abstract class DialogViewModelBase : ViewModelBase, IDialogViewModel, IDialogResultHelper
    {
        public abstract bool CanClose { get;}

        public event EventHandler<DialogResultEventArgs>? dialogResultRequest;

        public abstract void Close();

        protected void RequestClose(DialogResultEventArgs args)
        {
            dialogResultRequest?.Invoke(this, args);
        }
    }
}
