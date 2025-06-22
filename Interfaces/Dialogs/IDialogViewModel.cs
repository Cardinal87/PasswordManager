

namespace Interfaces.Dialogs
{
    public interface IDialogViewModel
    {
        bool CanClose { get; }
        void Close();

    }
}
