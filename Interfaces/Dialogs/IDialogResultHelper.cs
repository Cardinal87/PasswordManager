

namespace Interfaces.Dialogs
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
